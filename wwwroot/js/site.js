// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const THEME_STORAGE_KEY = "preferred-theme";

function getSystemTheme() {
  const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: dark)");
  return prefersDarkScheme.matches ? "dark" : "light";
}

function setTheme(theme) {
  if (theme == "auto") {
    theme = getSystemTheme();
  }

  document.documentElement.classList.remove("light", "dark");
  document.documentElement.classList.add(theme);

  document.documentElement.setAttribute("data-theme", theme);
  localStorage.setItem(THEME_STORAGE_KEY, theme);
}

let theme = localStorage.getItem(THEME_STORAGE_KEY);
if (!theme) {
  theme = "auto";
}
setTheme(theme);
