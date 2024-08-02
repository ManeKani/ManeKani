const DEFAULT_SANS_FONT = [
  "ui-sans-serif",
  "system-ui",
  "sans-serif",
  "Apple Color Emoji",
  "Segoe UI Emoji",
  "Segoe UI Symbol",
  "Noto Color Emoji",
];

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Pages/**/*.cshtml"],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Roboto", ...DEFAULT_SANS_FONT],
        title: ["Poppins", "Roboto", ...DEFAULT_SANS_FONT],
      },
      colors: {
        text: {
          50: "rgb(var(--text-50) / <alpha-value>)",
          100: "rgb(var(--text-100) / <alpha-value>)",
          200: "rgb(var(--text-200) / <alpha-value>)",
          300: "rgb(var(--text-300) / <alpha-value>)",
          400: "rgb(var(--text-400) / <alpha-value>)",
          500: "rgb(var(--text-500) / <alpha-value>)",
          600: "rgb(var(--text-600) / <alpha-value>)",
          700: "rgb(var(--text-700) / <alpha-value>)",
          800: "rgb(var(--text-800) / <alpha-value>)",
          900: "rgb(var(--text-900) / <alpha-value>)",
          950: "rgb(var(--text-950) / <alpha-value>)",
          DEFAULT: "rgb(var(--text-100) / <alpha-value>)",
        },
        background: {
          50: "rgb(var(--background-50) / <alpha-value>)",
          100: "rgb(var(--background-100) / <alpha-value>)",
          200: "rgb(var(--background-200) / <alpha-value>)",
          300: "rgb(var(--background-300) / <alpha-value>)",
          400: "rgb(var(--background-400) / <alpha-value>)",
          500: "rgb(var(--background-500) / <alpha-value>)",
          600: "rgb(var(--background-600) / <alpha-value>)",
          700: "rgb(var(--background-700) / <alpha-value>)",
          800: "rgb(var(--background-800) / <alpha-value>)",
          900: "rgb(var(--background-900) / <alpha-value>)",
          950: "rgb(var(--background-950) / <alpha-value>)",
          DEFAULT: "rgb(var(--background-50) / <alpha-value>)",
        },
      },
    },
  },
  plugins: [require("daisyui")],
  daisyui: {
    themes: [
      {
        light: {
          primary: "#0D0C22",
          secondary: "#e0dfdc",
          accent: "#F525A2",
          neutral: "#262626",
          "base-100": "#ffffff",
        },
        dark: {
          primary: "#deddf3",
          secondary: "#23221f",
          accent: "#F525A2",
          neutral: "#6f6f6f",
          "base-100": "#0F0F0E",
          "base-200": "#171716",
          "base-300": "#222120",
        },
      },
    ],
  },
};
