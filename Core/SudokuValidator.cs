using System;
using SudokuEngine.Models;

namespace SudokuEngine.Core
{
    /// <summary>
    /// Enforces standard Sudoku placement rules across rows, columns, and 3x3 boxes.
    /// </summary>
    public static class SudokuValidator
    {
        /// <summary>
        /// Verifies whether placing a value at the specified coordinates violates any Sudoku rule.
        /// </summary>
        public static bool IsValidPlacement(SudokuGrid grid, int row, int col, int value)
        {
            if (value < 1 || value > 9)
            {
                return false;
            }

            return !IsInRow(grid, row, value) &&
                   !IsInColumn(grid, col, value) &&
                   !IsInSubGrid(grid, row, col, value);
        }

        /// <summary>
        /// Checks if the target value already exists in the given row.
        /// </summary>
        private static bool IsInRow(SudokuGrid grid, int row, int value)
        {
            for (int col = 0; col < SudokuGrid.GridSize; col++)
            {
                if (grid.GetValue(row, col) == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the target value already exists in the given column.
        /// </summary>
        private static bool IsInColumn(SudokuGrid grid, int col, int value)
        {
            for (int row = 0; row < SudokuGrid.GridSize; row++)
            {
                if (grid.GetValue(row, col) == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the target value exists within the corresponding 3x3 sub-grid.
        /// </summary>
        private static bool IsInSubGrid(SudokuGrid grid, int row, int col, int value)
        {
            int startRow = (row / SudokuGrid.BoxSize) * SudokuGrid.BoxSize;
            int startCol = (col / SudokuGrid.BoxSize) * SudokuGrid.BoxSize;

            for (int r = 0; r < SudokuGrid.BoxSize; r++)
            {
                for (int c = 0; c < SudokuGrid.BoxSize; c++)
                {
                    if (grid.GetValue(startRow + r, startCol + c) == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
