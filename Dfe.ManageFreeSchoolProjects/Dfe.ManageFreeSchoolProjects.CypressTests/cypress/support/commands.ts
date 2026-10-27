import 'cypress-localstorage-commands';
import 'cypress-axe';
import { AuthenticationInterceptor } from '../auth/authenticationInterceptor';
import { Logger } from '../common/logger';
import { EnvUrl } from '../constants/cypressConstants';
import { Result, RuleObject } from 'axe-core';

function formatViolation(violation: Result): string {
    const nodes = violation.nodes.map((node) => node.target.join(', ')).join('\n    ');
    return (
        `\n[${violation.impact?.toUpperCase()}] ${violation.id}: ${violation.description}\n` +
        `  Help: ${violation.helpUrl}\n` +
        `  Affected nodes:\n    ${nodes}`
    );
}

function logViolations(violations: Result[]): void {
    cy.url().then((url) => {
        violations.forEach((violation) => {
            Logger.log(`A11y violation on ${url}: ${formatViolation(violation)}`);
        });
    });
}

Cypress.Commands.add('getByTestId', (id) => {
    cy.get(`[data-testid="${id}"]`);
});

Cypress.Commands.add('containsByTestId', (id) => {
    cy.get(`[data-testid*="${id}"]`);
});

Cypress.Commands.add('getById', (id) => {
    cy.get(`[id="${id}"]`);
});

Cypress.Commands.add('getByClass', (className) => {
    cy.get(`[class="${className}"]`);
});

Cypress.Commands.add('getByName', (name) => {
    cy.get(`[name="${name}"]`);
});

Cypress.Commands.add('getByRole', (role) => {
    cy.get(`[role="${role}"]`);
});

Cypress.Commands.add('getByLabelFor', (labelFor) => {
    cy.get(`[for="${labelFor}"]`);
});

// Scoped to `label` on purpose: this reads a `for` attribute, which only a label carries.
// An unscoped `cy.contains` matches any element holding the text — e.g. the dashboard's
// `<span>Presumption</span>` project-type cell — which yields no `for` and so ends up as
// `cy.get('#undefined')`. Restricting to labels also makes the command retry until the
// expected page has rendered, rather than latching onto the previous page's text.
Cypress.Commands.add('getByRadioOption', (radioText: string) => {
    cy.contains('label', radioText)
        .invoke('attr', 'for')
        .then((id) => {
            cy.get('#' + id);
        });
});

Cypress.Commands.add('assertChildList', (selector: string, values: string[]) => {
    cy.getByTestId(selector)
        .children()
        .should('have.length', values.length)
        .each((el, i) => {
            expect(el.text()).to.equal(values[i]);
        });
});

// When several Cypress jobs hit one environment the web app starts refusing connections
// and `cy.visit()` fails with ESOCKETTIMEDOUT. `login` runs in a `beforeEach`, and a
// failure there makes Cypress skip every remaining test in the spec file — the CI run of
// 2026-10-26 lost 27 tests to the timeout itself and 45 more to that cascade. Waiting for
// the app to answer first turns a saturated or cold-starting environment into a short
// delay instead of a whole file's lost results.
const appWarmUp = {
    attempts: 4,
    delayMs: 5000,
    timeoutMs: 30000,
};

// Generous enough to cover every attempt plus its backoff, so the command itself is not
// what times out — we want the task's diagnosis, not an opaque cy.task timeout.
const warmUpCommandTimeout = appWarmUp.attempts * (appWarmUp.timeoutMs + appWarmUp.delayMs) + 10000;

type WaitForAppResult = {
    ok: boolean;
    status?: number;
    attempts: number;
    error?: string;
};

Cypress.Commands.add('login', (params) => {
    cy.clearCookies();
    cy.clearLocalStorage();

    // Intercept all browser requests and add our special auth header
    // Means we don't have to use azure to authenticate
    new AuthenticationInterceptor().register(params);

    const url = Cypress.expose(EnvUrl);

    cy.task('waitForApp', { url, ...appWarmUp }, { timeout: warmUpCommandTimeout }).then((result) => {
        const { ok, attempts, error } = result as WaitForAppResult;

        // A warm-up that succeeds silently is indistinguishable from an app that answered
        // instantly, which makes it impossible to tell "the environment is coping" from
        // "the environment is drowning and the warm-up is hiding it". Log whenever it took
        // more than one attempt, so the retry count is visible evidence when deciding how
        // far `max-parallel` can be raised in cypress-tests.yml.
        if (ok && attempts > 1) {
            Logger.log(`WARM-UP: the web app needed ${attempts} attempts before it responded`);
        }

        if (!ok) {
            throw new Error(
                `The web app at ${url} did not respond after ${attempts} attempt(s). ` +
                    `Last network error: ${error}. This is an environment problem, not a test failure.`
            );
        }
    });

    cy.visit('/');
});

Cypress.Commands.add('executeAccessibilityTests', () => {
    Logger.log('Executing the command');
    const wcagStandards = ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa', 'wcag22aa'];
    const impactLevel = ['critical', 'minor', 'moderate', 'serious'];
    const continueOnFail = false;
    const ruleConfiguration: RuleObject = {
        // govuk-frontend v5.x adds aria-expanded to radio inputs with conditional
        // reveals, which is not yet permitted by the ARIA spec on the radio role.
        // Tracked upstream: https://github.com/w3c/aria/issues/1404
        'aria-allowed-attr': { enabled: false },
    };

    Logger.log('Injecting Axe and checking accessibility');
    cy.injectAxe();

    cy.checkA11y(
        undefined,
        {
            runOnly: {
                type: 'tag',
                values: wcagStandards,
            },
            rules: ruleConfiguration,
            includedImpacts: impactLevel,
        },
        logViolations,
        continueOnFail
    );
});

Cypress.Commands.add('typeFast', { prevSubject: 'element' }, (subject: JQuery<HTMLElement>, text: string) => {
    cy.wrap(subject).invoke('val', text);
});

Cypress.Commands.add('enterDate', (idPrefix: string, day: string, month: string, year: string) => {
    if (day.length > 0) {
        cy.getById(`${idPrefix}-day`).typeFast(day);
    }

    if (month.length > 0) {
        cy.getById(`${idPrefix}-month`).typeFast(month);
    }

    if (year.length > 0) {
        cy.getById(`${idPrefix}-year`).typeFast(year);
    }
});

Cypress.Commands.add('checkDate', (idPrefix: string, day: string, month: string, year: string) => {
    cy.getById(`${idPrefix}-day`).should('have.value', day);
    cy.getById(`${idPrefix}-month`).should('have.value', month);
    cy.getById(`${idPrefix}-year`).should('have.value', year);
});

Cypress.Commands.add('hasAddress', (id: string, line1: string, line2: string, line3: string) => {
    if (line1 === 'Empty') {
        cy.getByTestId(id).should('contain.text', 'Empty');

        return;
    }

    cy.getByTestId(id).find("[data-testid='address-line1']").should('contain.text', line1);
    cy.getByTestId(id).find("[data-testid='address-line2']").should('contain.text', line2);
    cy.getByTestId(id).find("[data-testid='address-line3']").should('contain.text', line3);
});
