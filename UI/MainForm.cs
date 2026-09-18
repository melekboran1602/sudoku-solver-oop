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
        private Panel boardPanel;
        private Button btnSolve;
        private Button btnClear;
        private ComboBox cmbLanguage;
        private Label lblLanguage;

        public MainForm()
        {
            // Form ayarları
            this.Size = new Size(490, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250); // Göz yormayan yumuşak açık gri zemin

            // Varsayılan dil
            Localization.CurrentLanguage = Language.Turkish;

            InitializeLanguageSelector();
            InitializeGrid();
            InitializeButtons();

            ApplyLocalization();
        }

        private void InitializeLanguageSelector()
        {
            // Genişliği 110 yaparak "Language:" yazısının kesilmesini önledik
            lblLanguage = new Label
            {
                Location = new Point(30, 20),
                Size = new Size(110, 28),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 60, 70),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbLanguage = new ComboBox
            {
                Location = new Point(145, 20),
                Size = new Size(130, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.White
            };

            cmbLanguage.Items.Add("Türkçe");
            cmbLanguage.Items.Add("English");
            cmbLanguage.Items.Add("Deutsch");
            cmbLanguage.SelectedIndex = 0;

            cmbLanguage.SelectedIndexChanged += (s, e) =>
            {
                Localization.CurrentLanguage = cmbLanguage.SelectedIndex switch
                {
                    1 => Language.English,
                    2 => Language.German,
                    _ => Language.Turkish
                };
                ApplyLocalization();
            };

            this.Controls.Add(lblLanguage);
            this.Controls.Add(cmbLanguage);
        }

        private void InitializeGrid()
        {
            int cellSize = 42;
            int thinGap = 1;   // Normal hücreler arası ince çizgi
            int thickGap = 4;  // 3x3 bloklar arası kalın çizgi
            int padding = 4;   // Tahtanın dış çerçeve kalınlığı

            // Tahtanın toplam genişlik ve yüksekliğini hesapla
            int boardSize = (padding * 2) + (cellSize * 9) + (thinGap * 6) + (thickGap * 2);

            // Arka plandaki ana Sudoku tahta kutusu (Çizgiler bu rengin aradan görünmesiyle oluşur)
            boardPanel = new Panel
            {
                Location = new Point(30, 65),
                Size = new Size(boardSize, boardSize),
                BackColor = Color.FromArgb(33, 37, 41) // Koyu grafit/siyah çerçeve çizgileri
            };

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    // 3x3 bloklara göre piksel konumunu hesapla
                    int posX = padding + (c * cellSize) + ((c - (c / 3)) * thinGap) + ((c / 3) * thickGap);
                    int posY = padding + (r * cellSize) + ((r - (r / 3)) * thinGap) + ((r / 3) * thickGap);

                    var tb = new TextBox
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(posX, posY),
                        Font = new Font("Segoe UI", 16, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        MaxLength = 1,
                        BorderStyle = BorderStyle.None, // Keskin çerçeveyi kaldırıp yumuşattık
                        BackColor = Color.FromArgb(254, 254, 254), // Gözü dinlendiren kırık beyaz
                        ForeColor = Color.FromArgb(30, 30, 30)
                    };

                    // Sadece 1-9 arası sayılara izin ver
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

            this.Controls.Add(boardPanel);
        }

        private void InitializeButtons()
        {
            btnSolve = new Button
            {
                Location = new Point(30, 520),
                Size = new Size(195, 48),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96), // Şık zümrüt yeşili
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSolve.FlatAppearance.BorderSize = 0;
            btnSolve.Click += BtnSolve_Click;

            btnClear = new Button
            {
                Location = new Point(245, 520),
                Size = new Size(195, 48),
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                BackColor = Color.FromArgb(231, 76, 60), // Şık soft kırmızı
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (s, e) =>
            {
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        cells[r, c].Text = "";
                        cells[r, c].ForeColor = Color.FromArgb(30, 30, 30);
                    }
                }
            };

            this.Controls.Add(btnSolve);
            this.Controls.Add(btnClear);
        }

        private void ApplyLocalization()
        {
            this.Text = Localization.Get("AppTitle");
            lblLanguage.Text = Localization.Get("LanguageLabel");
            btnSolve.Text = Localization.Get("SolveButton");
            btnClear.Text = Localization.Get("ClearButton");
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
                        cells[r, c].ForeColor = Color.FromArgb(30, 30, 30); // Kullanıcının girdiği sayılar koyu gri/siyah
                    }
                    else
                    {
                        grid.SetValue(r, c, 0);
                        cells[r, c].ForeColor = Color.FromArgb(41, 128, 185); // Algoritmanın bulduğu sayılar güzel bir mavi
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
                string msg = Localization.Get("NoSolution");
                string title = Localization.Get("ResultTitle");
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
