import eslint from '@eslint/js';
import tseslint from 'typescript-eslint';
import cypressPlugin from 'eslint-plugin-cypress/flat';
import unusedImports from 'eslint-plugin-unused-imports';
import globals from 'globals';

export default tseslint.config(
    eslint.configs.recommended,
    ...tseslint.configs.recommended,
    cypressPlugin.configs.recommended,
    {
        files: ['**/*.ts', '**/*.tsx'],
        plugins: {
            'unused-imports': unusedImports,
        },
        rules: {
            'cypress/no-assigning-return-values': 'error',
            'cypress/no-unnecessary-waiting': 'error',
            'cypress/assertion-before-screenshot': 'error',
            'cypress/no-force': 'error',
            'cypress/no-async-tests': 'error',
            'cypress/no-pause': 'error',
            'cypress/unsafe-to-chain-command': 'off',
            '@typescript-eslint/no-namespace': ['error', { allowDeclarations: true }],
            // Disable base rule, use unused-imports instead for auto-fix
            '@typescript-eslint/no-unused-vars': 'off',
            'unused-imports/no-unused-imports': 'error',
            'unused-imports/no-unused-vars': [
                'warn',
                {
                    vars: 'all',
                    varsIgnorePattern: '^_',
                    args: 'after-used',
                    argsIgnorePattern: '^_',
                },
            ],
        },
    },
    {
        files: ['**/*.js'],
        languageOptions: {
            globals: {
                ...globals.node,
            },
        },
        rules: {
            '@typescript-eslint/no-require-imports': 'off',
        },
    },
    {
        files: ['*.config.ts', '*.config.js'],
        rules: {
            '@typescript-eslint/no-require-imports': 'off',
        },
    },
    {
        ignores: ['node_modules/**', 'cypress/reports/**'],
    }
);
