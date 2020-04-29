module.exports = {
    transform: {
      '\\.(ts|tsx)?$': 'ts-jest',
    },
    testRegex: ".\\**\\.*?.tests.(ts|tsx)$",   // looks for your test
    moduleFileExtensions: ['ts', 'tsx', 'js'],
    coverageDirectory: "./coverage",
    coveragePathIgnorePatterns: [
        "/node_modules/",
        "/tests/",
        "/wwwroot/"
    ],
    testPathIgnorePatterns: ['/node_modules/'],
    setupFilesAfterEnv: [
        "./config/jest.setup.ts"
    ]
  };