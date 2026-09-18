using System;
using System.Collections.Generic;

namespace SudokuEngine.UI
{
  /// <summary>
  /// Supported interface languages for localization.
  /// </summary>
  public enum Language
  {
    English = 1,
    Turkish = 2,
    German = 3
  }

  /// <summary>
  /// Manages multi-language string translations.
  /// </summary>
  public static class Localization 
  {
    public static Language CurrentLanguage { get; set; } = Language.English;
  
    private static readonly Dictionary<Language, Dictionary<string, string>> Translations = new Dictionary<Language, Dictionary<string, string>> 
    {
      [Language.English] = new Dictionary<string, string>
        {
          ["SelectLanguage"] = "Select Language / Dil Seçin / Sprache wählen:",
          ["InitialBoard"] = "--- Initial Sudoku Board ---",
          ["Solving"] = "Solving board using Backtracking algorithm...",
          ["SolvedSuccess"] = "Sudoku solved successfully!",
          ["NoSolution"] = "No valid solution exists for this puzzle.",
          ["ExecutionTime"] = "Elapsed Time: ",
          ["InputInstructions"] = "Please enter each row with 9 digits (use 0 or . for empty cells):",
          ["RowPrompt"] = "Row ",
          ["InvalidRowError"] = "Error: Invalid input! Each row must be exactly 9 digits (0-9 or .).",
          ["AppTitle"] = "🧩 Sudoku Solver Engine",
          ["LanguageLabel"] = "Language:",
          ["SolveButton"] = "Solve",
          ["ClearButton"] = "Clear",
          ["ResultTitle"] = "Result",
          ["InvalidBoard"] = "Invalid board! The same number cannot appear twice in any row, column, or 3x3 box.",
          ["InputInfo"] = "Please enter the numbers on the board:",
        },
       [Language.Turkish] = new Dictionary<string, string>
        {
          ["SelectLanguage"] = "Select Language / Dil Seçin / Sprache Wählen:",
          ["InitialBoard"] = "--- Başlangıç Sudoku Tahtası ---",
          ["Solving"] = "Backtracking algoritması ile çözülüyor...",
          ["SolvedSuccess"] = "Sudoku başarıyla çözüldü!",
          ["NoSolution"] = "Bu sudoku için geçerli bir çözüm bulunamadı.",
          ["ExecutionTime"] = "Geçen süre: ",
          ["InputInstructions"] = "Lütfen her satırı 9 karakter olacak şekilde girin (boşluklar için 0 veya . kullanın):",
          ["RowPrompt"] = "Satır ",
          ["InvalidRowError"] = "Hata: Geçersiz giriş! Satır tam 9 karakter olmalı (0-9 veya .).",
          ["AppTitle"] = "🧩 Sudoku Çözücü Motoru",
          ["LanguageLabel"] = "Dil:",
          ["SolveButton"] = "Çöz",
          ["ClearButton"] = "Temizle",
          ["ResultTitle"] = "Sonuç",
          ["InvalidBoard"] = "Hatalı tahta! Aynı satır, sütun veya 3x3 kutuda aynı sayı birden fazla olamaz.",
          ["InputInfo"] = "Lütfen tahtadaki sayıları giriniz:",
        },
        [Language.German] = new Dictionary<string, string>
          {
          ["SelectLanguage"] = "Select Language / Dil Seçin / Sprache Wählen:",
          ["InitialBoard"] = "--- Ursprüngliches Sudoku-Brett ---",
          ["Solving"] = "Das Sudoku wird mit Backtracking-Algorithmus gelöst...",
          ["SolvedSuccess"] = "Sudoku erfolgreich gelöst!",
          ["NoSolution"] = "Keine gültige Lösung für dieses Rätsel gefunden.",
          ["ExecutionTime"] = "Benötigte Zeit: ",
          ["InputInstructions"] = "Bitte jede Zeile mit 9 Zeichen eingeben (0 oder . für leere Felder):",
          ["RowPrompt"] = "Zeile ",
          ["InvalidRowError"] = "Fehler: Ungültige Eingabe! Jede Zeile muss genau 9 Zeichen lang sein (0-9 oder .).",
          ["AppTitle"] = "🧩 Sudoku-Löser-Engine",
          ["LanguageLabel"] = "Sprache:",
          ["SolveButton"] = "Lösen",
          ["ClearButton"] = "Zurücksetzen",
          ["ResultTitle"] = "Ergebnis",
          ["InvalidBoard"] = "Ungültiges Brett! Die gleiche Zahl darf nicht mehrfach in einer Zeile, Spalte oder 3x3-Box vorkommen.",
          ["InputInfo"] = "Bitte Zahlen auf dem Brett eingeben:",
        }
      };
      /// <summary>
      /// Retrieves localized text by key based on the current language.
      /// </summary>
      public static string Get(string key) 
      {
       if (Translations.TryGetValue(CurrentLanguage, out var langDict) && langDict.TryGetValue(key, out var text))
      {
        return text;
      }
      if (Translations[Language.English].TryGetValue(key, out var fallback))
      {
        return fallback;
      }

      return key;
    }
  }
}
