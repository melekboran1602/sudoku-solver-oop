using System;
using System.Drawing;
using System.Windows.Forms;
using SudokuEngine.Core;
using SudokuEngine.Models;

namespace SudokuEngine.UI
{
    /// <summary>
    /// Represents the main graphical user interface for the Sudoku Solver application.
    /// Provides interactive board manipulation, real-time input validation, and multi-language support.
    /// </summary>
    public class MainForm : Form
    {
        // UI Controls
        private TextBox[,] cells = new TextBox[9, 9];
        private Panel boardPanel;
        private Label lblInfo;
        private Button btnSolve;
        private Button btnClearAll;
        private Button btnClearCell;
        private Button btnLangMenu;
        private ContextMenuStrip langMenu;

        // Tracks the currently focused cell for targeted operations
        private TextBox? activeCell = null;

        // Modern Pastel Color Palette
        private readonly Color bgForm = Color.FromArgb(248, 250, 252);
        private readonly Color cellDefaultBg = Color.White;
        private readonly Color cellFocusBg = Color.FromArgb(224, 242, 254);     // Full soft-blue fill on focus
        private readonly Color cellErrorBg = Color.FromArgb(254, 202, 202);     // Pastel red for rule violations
        private readonly Color thinLineColor = Color.FromArgb(191, 219, 254);    // Subtle pastel blue between standard cells (1px)
        private readonly Color blockLineColor = Color.FromArgb(30, 41, 59);      // Matte black for 3x3 blocks and outer border (2px)
        private readonly Color textColorInitial = Color.FromArgb(15, 23, 42);    // User-entered initial digits (black)
        private readonly Color textColorSolved = Color.FromArgb(2, 132, 199);    // Solver-computed digits (vibrant blue)

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // Configure form properties
            this.Size = new Size(480, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = bgForm;

            // Initialize UI components
            InitializeTopBar();
            InitializeBoard();
            InitializeActionButtons();

            // Apply initial localized strings
            ApplyLocalization();

            // Prevent any cell from being highlighted automatically on initial startup
            this.Shown += (s, e) => btnSolve.Focus();
        }

        /// <summary>
        /// Initializes the top section containing the user prompt label and the language selection menu.
        /// </summary>
        private void InitializeTopBar()
        {
            // Left side: User prompt label (supports multi-line if needed)
            lblInfo = new Label
            {
                Location = new Point(35, 12),
                Size = new Size(260, 42),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Right side: Context menu for language selection
            langMenu = new ContextMenuStrip();
            langMenu.Items.Add("🇹🇷 Türkçe", null, (s, e) => SwitchLanguage(Language.Turkish));
            langMenu.Items.Add("🇬🇧 English", null, (s, e) => SwitchLanguage(Language.English));
            langMenu.Items.Add("🇩🇪 Deutsch", null, (s, e) => SwitchLanguage(Language.German));

            // Language dropdown button with world icon
            btnLangMenu = new Button
            {
                Location = new Point(305, 16),
                Size = new Size(110, 34),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnLangMenu.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnLangMenu.Click += (s, e) => langMenu.Show(btnLangMenu, new Point(0, btnLangMenu.Height));

            this.Controls.Add(lblInfo);
            this.Controls.Add(btnLangMenu);
        }

        /// <summary>
        /// Updates the current application language and refreshes all UI texts.
        /// </summary>
        /// <param name="lang">The target language enum.</param>
        private void SwitchLanguage(Language lang)
        {
            Localization.CurrentLanguage = lang;
            ApplyLocalization();
        }

        /// <summary>
        /// Initializes the 9x9 Sudoku board panel and its child cell textboxes.
        /// </summary>
        private void InitializeBoard()
        {
            int cellSize = 42;
            int totalSize = cellSize * 9;

            boardPanel = new Panel
            {
                Location = new Point(35, 65),
                Size = new Size(totalSize + 2, totalSize + 2),
                BackColor = cellDefaultBg
            };

            // Populate the 9x9 grid with customized multiline TextBoxes
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    // Multiline = true allows custom height to fully fill the cell square without vertical margins
                    var tb = new TextBox
                    {
                        Location = new Point(c * cellSize + 2, r * cellSize + 2),
                        Width = cellSize - 3,
                        Height = cellSize - 3,
                        Multiline = true,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        MaxLength = 1,
                        BorderStyle = BorderStyle.None,
                        BackColor = cellDefaultBg,
                        ForeColor = textColorInitial,
                        Tag = new Point(r, c)
                    };

                    // Highlight cell on focus
                    tb.Enter += (s, e) =>
                    {
                        activeCell = tb;
                        tb.BackColor = cellFocusBg;
                    };

                    // Restore default background on leave unless flagged with a validation error
                    tb.Leave += (s, e) =>
                    {
                        if (tb.BackColor != cellErrorBg)
                        {
                            tb.BackColor = cellDefaultBg;
                        }
                    };

                    // Handle Backspace and Delete keys to quickly clear the cell
                    tb.KeyDown += (s, e) =>
                    {
                        if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                        {
                            tb.Text = "";
                            tb.ForeColor = textColorInitial;
                            tb.BackColor = cellFocusBg;
                            e.Handled = true;
                        }
                    };

                    // Restrict input strictly to digits 1 through 9
                    tb.KeyPress += (s, e) =>
                    {
                        if (!char.IsControl(e.KeyChar) && (e.KeyChar < '1' || e.KeyChar > '9'))
                        {
                            e.Handled = true;
                        }
                    };

                    cells[r, c] = tb;
                    boardPanel.Controls.Add(tb);
                }
            }

            // Custom painting for crisp grid lines avoiding overlapping artifacts
            boardPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using var thinPen = new Pen(thinLineColor, 1);
                using var thickPen = new Pen(blockLineColor, 2);

                // 1. Draw subtle pastel blue grid lines for internal cells
                for (int i = 1; i < 9; i++)
                {
                    if (i % 3 != 0)
                    {
                        int pos = i * cellSize;
                        g.DrawLine(thinPen, pos, 0, pos, totalSize);
                        g.DrawLine(thinPen, 0, pos, totalSize, pos);
                    }
                }

                // 2. Draw prominent black separator lines for 3x3 blocks
                for (int i = 3; i < 9; i += 3)
                {
                    int pos = i * cellSize;
                    g.DrawLine(thickPen, pos, 0, pos, totalSize);
                    g.DrawLine(thickPen, 0, pos, totalSize, pos);
                }

                // 3. Draw outer boundary frame
                g.DrawRectangle(thickPen, 1, 1, totalSize, totalSize);
            };

            this.Controls.Add(boardPanel);
        }

        /// <summary>
        /// Initializes the primary action buttons: Solve, Clear Cell, and Clear All.
        /// </summary>
        private void InitializeActionButtons()
        {
            // Solve button
            btnSolve = new Button
            {
                Location = new Point(35, 480),
                Size = new Size(185, 45),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(2, 132, 199),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSolve.FlatAppearance.BorderSize = 0;
            btnSolve.Click += BtnSolve_Click;

            // Clear Active Cell button
            btnClearCell = new Button
            {
                Location = new Point(230, 480),
                Size = new Size(185, 45),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClearCell.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnClearCell.Click += (s, e) =>
            {
                if (activeCell != null)
                {
                    activeCell.Text = "";
                    activeCell.ForeColor = textColorInitial;
                    activeCell.BackColor = cellFocusBg;
                }
            };

            // Clear Entire Board button
            btnClearAll = new Button
            {
                Location = new Point(35, 535),
                Size = new Size(380, 40),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(100, 116, 139),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClearAll.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnClearAll.Click += (s, e) =>
            {
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        cells[r, c].Text = "";
                        cells[r, c].ForeColor = textColorInitial;
                        cells[r, c].BackColor = cellDefaultBg;
                    }
                }
            };

            this.Controls.Add(btnSolve);
            this.Controls.Add(btnClearCell);
            this.Controls.Add(btnClearAll);
        }

        /// <summary>
        /// Updates all dynamic UI texts according to the selected language.
        /// </summary>
        private void ApplyLocalization()
        {
            this.Text = Localization.Get("AppTitle");
            lblInfo.Text = Localization.Get("InputInfo");
            btnLangMenu.Text = "🌐 " + (Localization.CurrentLanguage switch
            {
                Language.Turkish => "Dil",
                Language.German => "Sprache",
                _ => "Language"
            });
            btnSolve.Text = Localization.Get("SolveButton");
            btnClearCell.Text = Localization.Get("DeleteCell");
            btnClearAll.Text = Localization.Get("ClearButton");
        }

        /// <summary>
        /// Validates board inputs and executes the backtracking solver algorithm.
        /// </summary>
        private void BtnSolve_Click(object? sender, EventArgs e)
        {
            // Reset previous error indicators
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    cells[r, c].BackColor = cellDefaultBg;

            var grid = new SudokuGrid();
            bool hasFormatError = false;

            // 1. Validate initial board configuration and detect conflicting duplicate numbers
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (int.TryParse(cells[r, c].Text, out int val) && val >= 1 && val <= 9)
                    {
                        if (!SudokuValidator.IsValidPlacement(grid, r, c, val))
                        {
                            cells[r, c].BackColor = cellErrorBg;
                            hasFormatError = true;
                        }
                        else
                        {
                            grid.SetValue(r, c, val);
                            cells[r, c].ForeColor = textColorInitial;
                        }
                    }
                    else
                    {
                        grid.SetValue(r, c, 0);
                        cells[r, c].ForeColor = textColorSolved;
                    }
                }
            }

            // Halt execution if starting configuration violates standard Sudoku rules
            if (hasFormatError)
            {
                MessageBox.Show(Localization.Get("InvalidBoard"), Localization.Get("ResultTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Execute recursive backtracking solver
            bool solved = BacktrackingSolver.Solve(grid);

            if (solved)
            {
                // Populate solved values onto the visual board
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        cells[r, c].Text = grid.GetValue(r, c).ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show(Localization.Get("NoSolution"), Localization.Get("ResultTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
