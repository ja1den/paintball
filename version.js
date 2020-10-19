// Modules
const { resolve } = require('path');
const { readFileSync } = require('fs');

const { execSync } = require('child_process');

// Bundle
const settings = readFileSync(
	resolve(__dirname, 'ProjectSettings', 'ProjectSettings.asset'),
	'utf8'
);

const bundle = settings
	.match(/bundleVersion: (?<version>[\S]+)/)
	.groups.version.trim();

// Tag
const tag = execSync('git describe --tags').toString().split('-')[0].trim();

// Compare
if (tag !== bundle) {
	console.log(`E: T(${tag}) V(${bundle})`);
	process.exitCode = 1;
} else {
	console.log(`S: T(${tag}) V(${bundle})`);
}
