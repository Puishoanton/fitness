module.exports = {
	extends: ["@commitlint/config-conventional"],
	rules: {
		"scope-enum": [2, "always", ["backend", "frontend", "infra", "monitoring", "db"]],
		"scope-empty": [2, "never"]
	}
};
