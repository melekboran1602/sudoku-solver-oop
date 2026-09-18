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
        private Label lblInfo;
        private Button btnSolve;
        private Button btnClearAll;
        private Button btnClearCell;
        private Button btnLangMenu;
        private ContextMenuStrip langMenu;

        private TextBox? activeCell = null;

        // Renk Paleti
        private readonly Color bgForm = Color.FromArgb(248, 250, 252);
        private readonly Color cellDefaultBg = Color.White;
        private readonly Color cellFocusBg = Color.FromArgb(224, 242, 254);     // Seçilince tam dolgu açık mavi
        private readonly Color cellErrorBg = Color.FromArgb(254, 202, 202);     // Kural ihlalinde pastel kırmızı
        private readonly Color thinLineColor = Color.FromArgb(191, 219, 254);    // Hücreler arası pastel mavi (1px)
        private readonly Color blockLineColor = Color.FromArgb(30, 41, 59);      // 3x3 ve dış çerçeve mat siyah (2px)
        private readonly Color textColorInitial = Color.FromArgb(15, 23, 42);    // Kullanıcı sayıları (siyah)
        private readonly Color textColorSolved = Color.FromArgb(2, 132, 199);    // Çözülen sayılar (mavi)

        public MainForm()
        {
            this.Size = new Size(480, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = bgForm;

            InitializeTopBar();
            InitializeBoard();
            InitializeActionButtons();

            ApplyLocalization();

            // İlk açılışta rastgele bir hücre seçili/mavi başlamasın
            this.Shown += (s, e) => btnSolve.Focus();
        }

        private void InitializeTopBar()
        {
            // Sol Taraf: "Lütfen sayıları giriniz" mesajı
            lblInfo = new Label
            {
                Location = new Point(35, 18),
                Size = new Size(265, 34),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Sağ Taraf: Dünya simgeli dil seçimi
            langMenu = new ContextMenuStrip();
            langMenu.Items.Add("🇹🇷 Türkçe", null, (s, e) => SwitchLanguage(Language.Turkish));
            langMenu.Items.Add("🇬🇧 English", null, (s, e) => SwitchLanguage(Language.English));
            langMenu.Items.Add("🇩🇪 Deutsch", null, (s, e) => SwitchLanguage(Language.German));

            btnLangMenu = new Button
            {
                Location = new Point(305, 18),
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

        private void SwitchLanguage(Language lang)
        {
            Localization.CurrentLanguage = lang;
            ApplyLocalization();
        }

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

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
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

                    tb.Enter += (s, e) =>
                    {
                        activeCell = tb;
                        tb.BackColor = cellFocusBg;
                    };

                    tb.Leave += (s, e) =>
                    {
                        if (tb.BackColor != cellErrorBg)
                        {
                            tb.BackColor = cellDefaultBg;
                        }
                    };

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

            // Çizgilerin üst üste binip siyah leke yapmasını engelleyen çizim
            boardPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using var thinPen = new Pen(thinLineColor, 1);
                using var thickPen = new Pen(blockLineColor, 2);

                // 1. İnce pastel mavi çizgiler
                for (int i = 1; i < 9; i++)
                {
                    if (i % 3 != 0)
                    {
                        int pos = i * cellSize;
                        g.DrawLine(thinPen, pos, 0, pos, totalSize);
                        g.DrawLine(thinPen, 0, pos, totalSize, pos);
                    }
                }

                // 2. 3x3 blokları ayıran mat siyah çizgiler
                for (int i = 3; i < 9; i += 3)
                {
                    int pos = i * cellSize;
                    g.DrawLine(thickPen, pos, 0, pos, totalSize);
                    g.DrawLine(thickPen, 0, pos, totalSize, pos);
                }

                // 3. Dış çerçeve
                g.DrawRectangle(thickPen, 1, 1, totalSize, totalSize);
            };

            this.Controls.Add(boardPanel);
        }

        private void InitializeActionButtons()
        {
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

        private void BtnSolve_Click(object? sender, EventArgs e)
        {
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    cells[r, c].BackColor = cellDefaultBg;

            var grid = new SudokuGrid();
            bool hasFormatError = false;

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

            if (hasFormatError)
            {
                MessageBox.Show(Localization.Get("InvalidBoard"), Localization.Get("ResultTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                MessageBox.Show(Localization.Get("NoSolution"), Localization.Get("ResultTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
