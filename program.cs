using System;
using System.Diagnostics;
using System.Text;
using SudokuEngine.Models;
using SudokuEngine.Core;
using SudokuEngine.UI;

namespace SudokuEngine
{
    /// <summary>
    /// Entry point for the Sudoku solver console application.
    /// </summary>
    internal class Program 
    {
        /// <summary>
        /// Main application loop handling language selection, board rendering, and solving.
        /// </summary>
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            Console.WriteLine(Localization.Get("SelectLanguage"));
            
            Console.WriteLine("1 - English");
            Console.WriteLine("2 - Türkçe");
            Console.WriteLine("3 - Deutsch");
            Console.Write("> ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "2":
                    Localization.CurrentLanguage = Language.Turkish;
                    break;
                case "3":
                    Localization.CurrentLanguage = Language.German;
                    break;
                default:
                    Localization.CurrentLanguage = Language.English;
                    break;
            }

            Console.WriteLine();

            SudokuGrid grid = SetupGridInteractively();

            Console.WriteLine(Localization.Get("Solving"));

            Stopwatch stopwatch = Stopwatch.StartNew();

            bool solved = BacktrackingSolver.Solve(grid);

            stopwatch.Stop();

            if (solved)
            {
                Console.WriteLine(Localization.Get("SolvedSuccess"));
                ConsoleRenderer.RenderGrid(grid);
            }
            else
            {
                Console.WriteLine(Localization.Get("NoSolution"));
            }

            Console.WriteLine(Localization.Get("ExecutionTime") + stopwatch.ElapsedMilliseconds + " ms");
        }

        /// <summary>
        /// Allows the user to interactively place numbers on the board using coordinates.
        /// </summary>
        private static SudokuGrid SetupGridInteractively()
        {
            SudokuGrid grid = new SudokuGrid();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(Localization.Get("InitialBoard"));
                ConsoleRenderer.RenderGrid(grid);
                Console.WriteLine();
                Console.WriteLine(Localization.Get("InputHelp"));
                Console.Write("> ");

                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) || input == "0")
                {
                    break;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 3 &&
                    int.TryParse(parts[0], out int row) &&
                    int.TryParse(parts[1], out int col) &&
                    int.TryParse(parts[2], out int val))
                {
                    if (row >= 1 && row <= 9 && col >= 1 && col <= 9 && val >= 1 && val <= 9)
                    {
                        grid.SetValue(row - 1, col - 1, val);
                        continue;
                    }
                }

                Console.WriteLine(Localization.Get("InvalidFormat"));
                Console.WriteLine("Enter...");
                Console.ReadKey();
            }

            Console.Clear();
            Console.WriteLine(Localization.Get("InitialBoard"));
            ConsoleRenderer.RenderGrid(grid);
            Console.WriteLine();

            return grid;
        }
    }
}
