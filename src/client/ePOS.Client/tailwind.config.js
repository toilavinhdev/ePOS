/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js}"],
  theme: {
    extend: {},
    colors: {
      primary: "var(--primary)",
      black: "var(--black)",
      white: "var(--white)",
    },
  },
  corePlugins: {
    preflight: false,
  },
  plugins: [],
};
