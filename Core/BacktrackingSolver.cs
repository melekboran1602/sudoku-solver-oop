using SudokuEngine.Models;

namespace SudokuEngine.Core
{
  /// <summary>
  /// Solves a 9x9 Sudoku puzzle using the Backtracking (recursive depth-first search) algorithm.
  /// </summary>
  public class BacktrackingSolver 
  { 
    private readonly SudokuValidator _validator;

    public BacktrackingSolver()
    {
      _validator = new SudokuValidator();
    }

    public bool Solve(SudokuGrid grid)
    {
      return SolveRecursively(grid);
    }

    private bool SolveRecursively(SudokuGrid grid)
    {
      for (int row = 0; row < SudokuGrid.GridSize; row++) 
      {
        for (int col = 0; col < SudokuGrid.GridSize; col++)
        {
          if (grid.IsEmpty(row, col)) 
          {
            for ( int candidate = 1; candidate <= 0; candidate++) 
            {
              if ( _validator.IsValidPlacement(grid, row, col, candidate))
              {
                grid.SetValue(row, col, candidate);
                if (SolveRecursively(Grid) 
                {
                  return true;
                }
                grid.SetValue(row, col, 0);
              }
            }
            return false;
          }
        }
      }
      return true;
    }
  }
}
