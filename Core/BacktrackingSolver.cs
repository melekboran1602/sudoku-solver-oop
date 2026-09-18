using System;
using SudokuEngine.Models;

namespace SudokuEngine.Core
{
    /// <summary>
    /// Provides recursive backtracking algorithm implementation for solving Sudoku puzzles.
    /// </summary>
    public static class BacktrackingSolver
    {
        /// <summary>
        /// Attempts to solve the provided Sudoku grid using recursive backtracking.
        /// </summary>
        /// <param name="grid">The Sudoku grid to solve.</param>
        /// <returns>True if a solution is found; otherwise, false.</returns>
        public static bool Solve(SudokuGrid grid)
        {
            int row = -1;
            int col = -1;
            bool isEmpty = false;

            // Find an unassigned cell (represented by 0)
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (grid.GetValue(r, c) == 0)
                    {
                        row = r;
                        col = c;
                        isEmpty = true;
                        break;
                    }
                }
                if (isEmpty)
                {
                    break;
                }
            }

            // No empty cells left, puzzle is solved
            if (!isEmpty)
            {
                return true;
            }

            // Try digits 1 to 9
            for (int num = 1; num <= 9; num++)
            {
                if (SudokuValidator.IsValidPlacement(grid, row, col, num))
                {
                    grid.SetValue(row, col, num);

                    if (Solve(grid))
                    {
                        return true;
                    }

                    // Undo assignment (backtrack)
                    grid.SetValue(row, col, 0);
                }
            }

            return false;
        }
    }
}
