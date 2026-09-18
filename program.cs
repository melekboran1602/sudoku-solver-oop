using System;
using System.Windows.Forms;
using SudokuEngine.UI;

namespace SudokuEngine
{
    /// <summary>
    /// Entry point for the Sudoku solver application.
    /// Launches the Windows Forms graphical interface.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
