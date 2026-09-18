using System;

namespace SudokuEngine.Models
{
    /// <summary>
    /// Represents the 9x9 Sudoku grid state and encapsulates board manipulations.
    /// </summary>
    public class SudokuGrid
    {
        public const int GridSize = 9;
        public const int BoxSize = 3;

        private readonly int[,] _matrix;

        /// <summary>
        /// Initializes a new empty 9x9 Sudoku grid.
        /// </summary>
        public SudokuGrid()
        {
            _matrix = new int[GridSize, GridSize];
        }

        /// <summary>
        /// Initializes a new grid by cloning the supplied 9x9 matrix.
        /// </summary>
        public SudokuGrid(int[,] initialMatrix)
        {
            if (initialMatrix.GetLength(0) != GridSize || initialMatrix.GetLength(1) != GridSize)
            {
                throw new ArgumentException($"Matrix must be exactly {GridSize}x{GridSize}.");
            }

            _matrix = new int[GridSize, GridSize];
            Array.Copy(initialMatrix, _matrix, initialMatrix.Length);
        }

        /// <summary>
        /// Gets the digit at the specified row and column.
        /// </summary>
        public int GetValue(int row, int col)
        {
            ValidateCoordinates(row, col);
            return _matrix[row, col];
        }

        /// <summary>
        /// Sets the digit at the specified coordinates (0 denotes an empty cell).
        /// </summary>
        public void SetValue(int row, int col, int value)
        {
            ValidateCoordinates(row, col);

            if (value < 0 || value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 9 (0 represents empty).");
            }

            _matrix[row, col] = value;
        }

        /// <summary>
        /// Determines whether the specified cell contains 0.
        /// </summary>
        public bool IsEmpty(int row, int col)
        {
            return GetValue(row, col) == 0;
        }

        /// <summary>
        /// Creates a deep copy of the current grid state.
        /// </summary>
        public SudokuGrid Clone()
        {
            var clonedGrid = new SudokuGrid();
            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    clonedGrid.SetValue(r, c, this._matrix[r, c]);
                }
            }
            return clonedGrid;
        }

        /// <summary>
        /// Ensures row and column indices reside within the 0 to 8 range.
        /// </summary>
        private static void ValidateCoordinates(int row, int col)
        {
            if (row < 0 || row >= GridSize || col < 0 || col >= GridSize)
            {
                throw new IndexOutOfRangeException($"Coordinates ({row}, {col}) are out of bounds for a {GridSize}x{GridSize} grid.");
            }
        }
    }
}
