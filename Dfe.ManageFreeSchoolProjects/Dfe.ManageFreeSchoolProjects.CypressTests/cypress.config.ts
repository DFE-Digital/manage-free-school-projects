import { defineConfig } from 'cypress';
import { generateZapReport } from 'cypress/plugins/generateZapReport';

type NestedError = {
    name?: string;
    code?: string;
    message?: string;
    cause?: unknown;
    errors?: unknown[];
};

// `fetch` reports every network problem as a bare "fetch failed" and buries the useful
// part — ECONNREFUSED, ENOTFOUND, ETIMEDOUT — in `cause`, or in `cause.errors` when a
// host resolves to several addresses. Dig the code out so the failure message names what
// actually happened.
function describeNetworkError(error: unknown, timeoutMs: number): string {
    const top = error as NestedError;

    if (top?.name === 'AbortError') {
        return `no response within ${timeoutMs}ms`;
    }

    const findCode = (value: unknown, depth = 0): string | undefined => {
        if (!value || depth > 4) {
            return undefined;
        }

        const node = value as NestedError;

        if (node.code) {
            return node.code;
        }

        for (const nested of node.errors ?? []) {
            const found = findCode(nested, depth + 1);

            if (found) {
                return found;
            }
        }

        return findCode(node.cause, depth + 1);
    };

    // Prefer the cause's message over the top-level one: `fetch` always says "fetch
    // failed", while the cause says something useful like "bad port".
    const causeMessage = (top?.cause as NestedError | undefined)?.message;

    return findCode(top) ?? causeMessage ?? top?.message ?? String(error);
}

export default defineConfig({
    allowCypressEnv: false,
    defaultCommandTimeout: 45000,
    pageLoadTimeout: 45000,
    watchForFileChanges: false,
    chromeWebSecurity: false,
    video: false,
    retries: {
        // Two retries hide flakiness: a test that only passes on attempt 3 still reports
        // green. Run with FLAKE_HUNT=1 to disable retries and surface those tests.
        runMode: process.env.FLAKE_HUNT ? 0 : 2,
    },
    userAgent: 'ManageFreeSchoolProjects/1.0 Cypress',
    reporter: 'cypress-multi-reporters',
    reporterOptions: {
        reporterEnabled: 'mochawesome',
        mochawesomeReporterOptions: {
            reportDir: 'cypress/reports/mocha',
            quite: true,
            overwrite: false,
            html: false,
            json: true,
        },
    },
    e2e: {
        // We've imported your old cypress plugins here.
        // You may want to clean this up later by importing these.
        setupNodeEvents(on, config) {
            on('before:run', () => {
                // Map cypress env vars to process env vars for usage outside of Cypress run environment
                process.env = config.env;
            });
            on('after:run', async () => {
                if (process.env.zapReport) {
                    await generateZapReport();
                }
            });

            on('task', {
                log(message) {
                    console.log(message);

                    return null;
                },

                // Probes the web app until it answers, and reports rather than throws.
                // Runs in Node on purpose: a failed `cy.visit()` cannot be caught from a
                // test, so the retry has to live somewhere a real try/catch works.
                async waitForApp({ url, attempts, delayMs, timeoutMs }) {
                    let lastError = 'none';

                    for (let attempt = 1; attempt <= attempts; attempt++) {
                        const controller = new AbortController();
                        const abort = setTimeout(() => controller.abort(), timeoutMs);

                        try {
                            // Any response at all means the app is up; the dev portal
                            // answers `/` with a 302 to the auth provider.
                            const response = await fetch(url, {
                                redirect: 'manual',
                                signal: controller.signal,
                            });

                            return { ok: true, status: response.status, attempts: attempt };
                        } catch (error) {
                            lastError = describeNetworkError(error, timeoutMs);

                            if (attempt < attempts) {
                                await new Promise((resolve) => setTimeout(resolve, delayMs));
                            }
                        } finally {
                            clearTimeout(abort);
                        }
                    }

                    return { ok: false, error: lastError, attempts };
                },
            });
            require('@cypress/grep/plugin').plugin(config);
            config.baseUrl = config.env.url;
            config.expose = {
                url: config.env.url,
                api: config.env.api,
                username: config.env.username,
            };

            return config;
        },
    },
    redirectionLimit: 30,
});
