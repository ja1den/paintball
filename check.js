// Modules
const { resolve } = require('path');
const { readFileSync } = require('fs');

const { execSync } = require('child_process');

// Latest Tag
const tag = execSync('git describe --tags').toString().split('-')[0];

// Bundle Version
const settings = readFileSync(
	resolve(__dirname, 'ProjectSettings', 'ProjectSettings.asset'),
	'utf8'
);
const version = settings.match(/bundleVersion: (?<version>[\S]+)/).groups
	.version;

// Compare
if (tag !== version) process.exitCode = 1;
console.log(`T: ${tag}\nV: ${version}`);
