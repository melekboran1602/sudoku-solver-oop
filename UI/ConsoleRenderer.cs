using System;
using SudokuEngine.Models;

namespace SudokuEngine.UI
{
  /// <summary>
  /// Handles formatted console rendering for the Sudoku grid and UI components.
  /// </summary>
  public static class ConsoleRenderer
  {
    /// <summary>
    /// Prints the 9x9 Sudoku grid to the console with box borders.
    /// </summary>
    public static void RenderGrid (SudokuGrid grid)
    {
      for (int row = 0; row < SudokuGrid.GridSize; row++)
      {
        if (row % 3 == 0 && row != 0)
        {
          Console.WriteLine("------+-------+------");
        }
        for (int col = 0; col < SudokuGrid.GridSize; col++)
        {
          if (col % 3 == 0 && col !=0)
          {
            Console.Write("| ");
          }
          int val = grid.GetValue(row, col);

          if (val == 0)
          {
            Console.Write(". ");
          }
          else 
          {
            Console.Write(val + " ");
          }
        }
        Console.WriteLine();
      }
    }
  }
}
