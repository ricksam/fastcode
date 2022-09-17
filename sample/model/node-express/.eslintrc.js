module.exports = {
  env: {
    es6: true,
    node: true
  },
  extends: [
    'standard'
  ],
  ignorePatterns: ["*.ejs", "*.sql", "node_modules/", "src/views", "src/public", "emitter.js"],
  globals: {
    Atomics: 'readonly',
    SharedArrayBuffer: 'readonly'
  },
  parserOptions: {
    ecmaVersion: 2018
  },
  rules: {
    "space-before-function-paren": [2, "never"],
    "space-before-blocks": [2, "never"]
  }
}
