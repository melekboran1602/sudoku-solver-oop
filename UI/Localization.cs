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
          ["ExecutionTime"] = "Elapsed Time: "
        },
      [Language.Turkish] = new Dictionary<string, string>
      {
        ["SelectLanguage"] = "Select Language / Dil Seçin / Sprache Wählen:",
        ["InitialBoard"] = "--- Başlangıç Sudoku Tahtası ---",
        ["Solving"] = "Backtracking algoritması ile çözülüyor...",
        ["SolvedSuccess"] = "Sudoku başarıyla çözüldü!",
        ["NoSolution"] = "Bu sudoku için geçerli bir çözüm bulunamadı.",
        ["ExecutionTime"] = "Geçen süre: "
      },
      [Language.German] = new Dictionary<string, string>
      {
        ["SelectLanguage"] = "Select Language / Dil Seçin / Sprache Wählen:",
        ["InitialBoard"] = "--- Ursprüngliches Sudoku-Brett ---",
        ["Solving"] = "Das Sudoku wird mit Backtracking-Algorithmus gelöst...",
        ["SolvedSuccess"] = "Sudoku erfolgreich gelöst!",
        ["NoSolution"] = "Keine gültige Lösung für dieses Rätsel gefunden.",
        ["ExecutionTime"] = "Benötigte Zeit: "
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
