// Modules
const { resolve } = require('path');
const { readFileSync } = require('fs');

const { execSync } = require('child_process');

// Latest Tag
const tag = execSync('git describe --tags').toString().split('-')[0].trim();

// Bundle Version
const settings = readFileSync(
	resolve(__dirname, 'ProjectSettings', 'ProjectSettings.asset'),
	'utf8'
);
const version = settings
	.match(/bundleVersion: (?<version>[\S]+)/)
	.groups.version.trim();

// Compare
if (tag !== version) {
	console.log(`Error: T(${tag}) V(${version})`);
	process.exitCode = 1;
}
