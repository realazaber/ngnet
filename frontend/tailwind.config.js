/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/app/**/*.{html,ts}"],
  darkMode: "selector",
  theme: {
    extend: {
      colors: {
        primary: "#DE002D",
        secondary: "#5632d4",
        accent: "#261b23",
        darkbg: "#131414",
      },
    },
  },
  plugins: [],
};
