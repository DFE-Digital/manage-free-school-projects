export default [
  {
    parser: "@typescript-eslint/parser",
    files: [
      ".js", ".jsx", ".ts", ".tsx"
    ],
    rules: {
      "cypress/no-assigning-return-values": "error",
      "cypress/no-unnecessary-waiting": "error",
      "cypress/assertion-before-screenshot": "error",
      "cypress/no-force": "error",
      "cypress/no-async-tests": "error",
      "cypress/no-pause": "error",
      "cypress/unsafe-to-chain-command": "off",
      "@typescript-eslint/no-namespace": [
        "error",
        { "allowDeclarations": true }
      ]
    },
    plugins: [
      "cypress"
    ],
    env: {
      "cypress/globals": true,
      "node": true
    },
    extends: [
      "plugin:cypress/recommended",
      "eslint:recommended",
      "plugin:@typescript-eslint/eslint-recommended",
      "plugin:@typescript-eslint/recommended"
    ]
  }
]
