/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js}"],
  theme: {
    extend: {},
    colors: {
      primary: "var(--primary)",
      white: "var(--white)",
      black: "var(--black)",
      red: "var(--red)",
      info: "var(--info)",
      success: "var(--success)",
      warning: "var(--primary)",
    },
  },
  corePlugins: {
    preflight: false,
  },
  plugins: [],
};
