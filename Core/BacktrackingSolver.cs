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
        /// Solves the Sudoku grid using recursive depth-first search (DFS) with backtracking.
        /// </summary>
        /// <param name="grid">The Sudoku grid state to be solved.</param>
        /// <returns>True if a complete valid solution is found; otherwise, false.</returns>
        public static bool Solve(SudokuGrid grid)
        {
            int row = -1;
            int col = -1;
            bool isEmpty = false;

            // Locate the next unassigned cell (represented by 0)
            for (int r = 0; r < SudokuGrid.GridSize; r++)
            {
                for (int c = 0; c < SudokuGrid.GridSize; c++)
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

            // Base case: All cells are filled, solution found
            if (!isEmpty)
            {
                return true;
            }

            // Attempt candidates 1 through 9
            for (int num = 1; num <= 9; num++)
            {
                if (SudokuValidator.IsValidPlacement(grid, row, col, num))
                {
                    grid.SetValue(row, col, num);

                    // Recurse to solve remaining cells
                    if (Solve(grid))
                    {
                        return true;
                    }

                    // Undo placement (backtrack)
                    grid.SetValue(row, col, 0);
                }
            }

            // Trigger backtracking to previous recursion layer
            return false;
        }
    }
}
