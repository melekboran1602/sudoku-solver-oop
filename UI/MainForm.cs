using System;
using System.Drawing;
using System.Windows.Forms;
using SudokuEngine.Core;
using SudokuEngine.Models;

namespace SudokuEngine.UI
{
    public class MainForm : Form
    {
        private TextBox[,] cells = new TextBox[9, 9];
        private Button btnSolve;
        private Button btnClear;
        private ComboBox cmbLanguage;
        private Label lblLanguage;

        // Varsayılan dil: Türkçe ("tr")
        private string currentLang = "tr";

        public MainForm()
        {
            this.Size = new Size(460, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            InitializeLanguageSelector();
            InitializeGrid();
            InitializeButtons();

            ApplyLocalization();
        }

        private void InitializeLanguageSelector()
        {
            lblLanguage = new Label
            {
                Location = new Point(25, 18),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbLanguage = new ComboBox
            {
                Location = new Point(110, 16),
                Size = new Size(130, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f)
            };

            cmbLanguage.Items.Add("Türkçe");
            cmbLanguage.Items.Add("English");
            cmbLanguage.Items.Add("Deutsch");
            cmbLanguage.SelectedIndex = 0;

            cmbLanguage.SelectedIndexChanged += (s, e) =>
            {
                currentLang = cmbLanguage.SelectedIndex switch
                {
                    1 => "en",
                    2 => "de",
                    _ => "tr"
                };
                ApplyLocalization();
            };

            this.Controls.Add(lblLanguage);
            this.Controls.Add(cmbLanguage);
        }

        private void InitializeGrid()
        {
            int startX = 25;
            int startY = 60;
            int cellSize = 42;

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    int extraX = (c / 3) * 6;
                    int extraY = (r / 3) * 6;

                    var tb = new TextBox
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(startX + c * cellSize + extraX, startY + r * cellSize + extraY),
                        Font = new Font("Segoe UI", 16, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        MaxLength = 1
                    };

                    tb.KeyPress += (s, e) =>
                    {
                        if (!char.IsControl(e.KeyChar) && (e.KeyChar < '1' || e.KeyChar > '9'))
                        {
                            e.Handled = true;
                        }
                    };

                    cells[r, c] = tb;
                    this.Controls.Add(tb);
                }
            }
        }

        private void InitializeButtons()
        {
            btnSolve = new Button
            {
                Location = new Point(25, 475),
                Size = new Size(185, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSolve.Click += BtnSolve_Click;

            btnClear = new Button
            {
                Location = new Point(230, 475),
                Size = new Size(185, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += (s, e) =>
            {
                for (int r = 0; r < 9; r++)
                    for (int c = 0; c < 9; c++)
                    {
                        cells[r, c].Text = "";
                        cells[r, c].ForeColor = Color.Black;
                    }
            };

            this.Controls.Add(btnSolve);
            this.Controls.Add(btnClear);
        }

        private void ApplyLocalization()
        {
            this.Text = Localization.Get("AppTitle", currentLang);
            lblLanguage.Text = Localization.Get("LanguageLabel", currentLang);
            btnSolve.Text = Localization.Get("SolveButton", currentLang);
            btnClear.Text = Localization.Get("ClearButton", currentLang);
        }

        private void BtnSolve_Click(object? sender, EventArgs e)
        {
            var grid = new SudokuGrid();

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (int.TryParse(cells[r, c].Text, out int val) && val >= 1 && val <= 9)
                    {
                        grid.SetValue(r, c, val);
                        cells[r, c].ForeColor = Color.Black;
                    }
                    else
                    {
                        grid.SetValue(r, c, 0);
                        cells[r, c].ForeColor = Color.DodgerBlue;
                    }
                }
            }

            bool solved = BacktrackingSolver.Solve(grid);

            if (solved)
            {
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
                string msg = Localization.Get("NoSolution", currentLang);
                string title = Localization.Get("ResultTitle", currentLang);
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
