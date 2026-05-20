import tseslint from "typescript-eslint";

export default tseslint.config(
	{
		ignores: ["node_modules/**", "server/**", ".expo/**", "dist/**"]
	},
	tseslint.configs.recommended
);
