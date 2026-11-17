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

type WaitForAppOptions = {
    url: string;
    attempts: number;
    delayMs: number;
    timeoutMs: number;
};

type WaitForAppResult = {
    ok: boolean;
    status?: number;
    attempts: number;
    error?: string;
};

const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms));

// A single probe: yields the status code if the app answers at all, throws otherwise.
async function probeApp(url: string, timeoutMs: number): Promise<number> {
    const controller = new AbortController();
    const abortProbe = setTimeout(() => controller.abort(), timeoutMs);

    try {
        // Any response means the app is up; the dev portal answers `/` with a 302 to the
        // auth provider, so the status itself is not checked.
        const response = await fetch(url, { redirect: 'manual', signal: controller.signal });

        return response.status;
    } finally {
        clearTimeout(abortProbe);
    }
}

// Probes until the app answers, and reports the outcome rather than throwing. Attempts
// are sequential by design — each has to finish before the next begins — so this recurses
// instead of awaiting inside a loop.
async function waitUntilAppResponds(options: WaitForAppOptions, attempt = 1): Promise<WaitForAppResult> {
    const { url, attempts, delayMs, timeoutMs } = options;

    try {
        return { ok: true, status: await probeApp(url, timeoutMs), attempts: attempt };
    } catch (error) {
        const lastError = describeNetworkError(error, timeoutMs);

        if (attempt >= attempts) {
            return { ok: false, error: lastError, attempts: attempt };
        }

        await delay(delayMs);

        return waitUntilAppResponds(options, attempt + 1);
    }
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

                waitForApp(options: WaitForAppOptions) {
                    return waitUntilAppResponds(options);
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
