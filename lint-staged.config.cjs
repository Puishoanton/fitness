module.exports = {
	"server/**/*.cs": () => "dotnet format server/Fitness.sln",
	"client/**/*.{ts,tsx,js,jsx}": ["eslint --fix", "prettier --write"],
	"client/**/*.{json,css}": ["prettier --write"],
	"*.{json,md}": ["prettier --write"]
};
