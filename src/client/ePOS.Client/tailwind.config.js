/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js}"],
  theme: {
    extend: {},
    colors: {
      primary: {
        DEFAULT: "var(--primary)",
        lighter: "var(--primary-lighter)",
      },
      black: "var(--black)",
      white: "var(--white)",
    },
  },
  corePlugins: {
    preflight: false,
  },
  plugins: [],
};
