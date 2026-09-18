using System;
using System.Collections.Generic;

namespace SudokuEngine.UI
{
    public enum Language
    {
        English = 1,
        Turkish = 2,
        German = 3
    }

    /// <summary>
    /// Provides localized UI strings with fallback support.
    /// </summary>
    public static class Localization 
    {
        public static Language CurrentLanguage { get; set; } = Language.Turkish;
    
        private static readonly Dictionary<Language, Dictionary<string, string>> Translations = new()
        {
            [Language.English] = new Dictionary<string, string>
            {
                ["AppTitle"] = "🧩 Sudoku Solver Engine",
                ["LanguageLabel"] = "Language:",
                ["InputInfo"] = "Please enter the numbers on the board:",
                ["SolveButton"] = "Solve",
                ["ClearButton"] = "Clear All",
                ["DeleteCell"] = "Clear Cell",
                ["ResultTitle"] = "Result",
                ["NoSolution"] = "No valid solution exists for this puzzle.",
                ["InvalidBoard"] = "Invalid board! The same number cannot appear twice in any row, column, or 3x3 box."
            },
            [Language.Turkish] = new Dictionary<string, string>
            {
                ["AppTitle"] = "🧩 Sudoku Çözücü Motoru",
                ["LanguageLabel"] = "Dil:",
                ["InputInfo"] = "Lütfen tahtadaki sayıları giriniz:",
                ["SolveButton"] = "Çöz",
                ["ClearButton"] = "Tümünü Temizle",
                ["DeleteCell"] = "Hücreyi Sil",
                ["ResultTitle"] = "Sonuç",
                ["NoSolution"] = "Bu sudoku için geçerli bir çözüm bulunamadı.",
                ["InvalidBoard"] = "Hatalı tahta! Aynı satır, sütun veya 3x3 kutuda aynı sayı birden fazla olamaz."
            },
            [Language.German] = new Dictionary<string, string>
            {
                ["AppTitle"] = "🧩 Sudoku-Löser-Engine",
                ["LanguageLabel"] = "Sprache:",
                ["InputInfo"] = "Bitte Zahlen auf dem Brett eingeben:",
                ["SolveButton"] = "Lösen",
                ["ClearButton"] = "Alles löschen",
                ["DeleteCell"] = "Zelle löschen",
                ["ResultTitle"] = "Ergebnis",
                ["NoSolution"] = "Keine gültige Lösung für dieses Rätsel gefunden.",
                ["InvalidBoard"] = "Ungültiges Brett! Die gleiche Zahl darf nicht mehrfach in einer Zeile, Spalte oder 3x3-Box vorkommen."
            }
        };

        /// <summary>
        /// Gets the localized string for the specified key, falling back to English if missing.
        /// </summary>
        public static string Get(string key) 
        {
            if (Translations.TryGetValue(CurrentLanguage, out var langDict) && langDict.TryGetValue(key, out var text))
            {
                return text;
            }

            // Fallback to English if translation is missing in active language
            if (Translations[Language.English].TryGetValue(key, out var fallback))
            {
                return fallback;
            }

            return key;
        }
    }
}
