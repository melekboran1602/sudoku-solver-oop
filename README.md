# 🧩 Sudoku-Solver-OOP

A modern, clean, and localized Sudoku solver desktop application built with **C# (.NET 8)** and **Windows Forms**, powered by an optimized recursive **Backtracking algorithm**.

---

## 📸 Preview

![Sudoku Solver Interface](screenshots/app_preview.png)

---

## 🌐 About the Project

This project showcases clean architectural design and algorithmic problem-solving in modern C#.

Moving beyond basic console programs, this project implements a full desktop graphical interface adhering strictly to the **Separation of Concerns (SoC)** principle. The solution cleanly decouples core data models, validation logic, recursive backtracking algorithms, and a multi-language Windows Forms user interface.

---

## ✨ Features

- **Recursive Backtracking Engine:** Efficient depth-first search (DFS) algorithm with state restoration, resolving valid 9x9 puzzles in milliseconds.
- **Modern Pastel UI:** Custom-rendered grid lines with thin pastel blue inner lines and prominent 3x3 block separators.
- **Real-Time Input Validation:** Automatic conflict detection highlighting rule violations in pastel red across rows, columns, and 3x3 boxes before solving.
- **Focus & Selection Highlighting:** Interactive cell highlighting with a soft-blue fill upon selection.
- **Dual Clearing Options:** Dedicated controls to clear either the currently selected cell (`Clear Cell` / `Delete` key) or reset the entire board (`Clear All`).
- **Runtime Localization (i18n):** Real-time language switching across **English**, **Türkçe**, and **Deutsch** with an intuitive dropdown menu.
- **Modular OOP Architecture:** Clean division between `Core` solver logic, `Models` data structures, and the `UI` layer.

---

## 🛠️ Technologies & Concepts

- C# (.NET 8)
- Windows Forms (WinForms)
- Object-Oriented Programming (OOP)
- Separation of Concerns (SoC)
- Recursive Backtracking & Constraint Propagation
- Custom Graphics & UI Painting (`Paint` Events)
- Software Localization (i18n)
- Git & GitHub

---

## 📂 Repository Structure

```text
sudoku-solver-oop/
├── Core/
│   ├── BacktrackingSolver.cs   # Recursive solving logic & search tree traversal
│   └── SudokuValidator.cs      # Row, column, and 3x3 subgrid rule verification
├── Models/
│   └── SudokuGrid.cs           # 9x9 matrix encapsulation, state cloning & indexing
├── UI/
│   ├── MainForm.cs             # Windows Forms GUI, custom grid painting & event handlers
│   └── Localization.cs         # Key-value multi-language dictionary service
├── Program.cs                  # Application bootstrap and STAThread entry point
├── SudokuEngine.csproj         # .NET 8 WinForms project configuration
└── README.md                   # Project documentation & architecture overview
```

## 🎯 Project Goal

The main goal of this project was to strengthen algorithmic problem-solving skills and master clean architecture patterns in modern C# desktop development.

Through this project, I practiced:

- Implementing recursive search algorithms with state rollback (backtracking)
- Designing mathematical coordinate mapping for 3x3 Sudoku subgrids
- Handling Windows Forms UI events, keyboard shortcuts, and custom graphic rendering without visual artifacts
- Structuring a multi-layer solution with isolated namespaces
- Managing multi-language support dynamically without hardcoding strings in logic classes
- Maintaining a clean, commit-driven development workflow on GitHub

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Windows OS (required for Windows Forms runtime)

### Run Locally

1. Clone the repository:
   ```bash
   git clone [https://github.com/melekboran1602/sudoku-solver-oop.git](https://github.com/melekboran1602/sudoku-solver-oop.git)
   cd sudoku-solver-oop
2. Build and run the project:
   dotnet run

---

## 🎮 How to Use

1. **Select Language:** Choose your preferred language (`🌐 Language`) from the top-right button.
2. **Enter Numbers:** Click any box to type the initial digits (1–9). The selected cell will highlight in light blue.
3. **Solve:** Click **Solve** to run the backtracking engine.
   - Initial numbers remain displayed in black.
   - Solved numbers appear in vivid blue.
4. **Edit / Clear:** Use **Clear Cell** (or the `Backspace` / `Delete` key) to clear a single box, or click **Clear All** to reset the entire grid.

---

## 👩‍💻 Developer

**Melek Boran**

A software development project engineered to demonstrate clean architecture, algorithm design, and modular C# desktop development.

---

## 🤝 Acknowledgments & Collaboration

* **Architectural Guidance & Pair Programming:** Developed with the collaborative assistance of **Gemini**, utilized for architectural review, WinForms GDI+ rendering optimization, and documentation structuring.
