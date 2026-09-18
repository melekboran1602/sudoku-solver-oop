# 🧩 Sudoku-Solver-OOP

A clean, modular, and localized Sudoku solver engine developed in C# (.NET) featuring interactive coordinate input, constraint-pruning backtracking algorithm, and runtime multi-language support.

---

## 🌐 About the Project

This project was built as an advanced algorithmic and architectural practice to move beyond basic console exercises into production-grade C# design patterns.

The primary focus was implementing the **Recursive Backtracking** algorithm while strictly adhering to the **Separation of Concerns (SoC)** principle. Instead of putting all logic into a single monolithic file, the engine is cleanly split into independent domain models, rule validators, backtracking solvers, and a localized user interface layer.

---

## ✨ Features

- **Recursive Backtracking Engine:** Depth-first tree traversal with constraint-based backtracking to find valid puzzle solutions.
- **Strict Rule Validation:** Robust sub-methods ensuring zero row, column, or 3x3 subgrid rule violations.
- **Interactive Coordinate Input:** Place puzzle hints dynamically using coordinate notation (<Row> <Col> <Value>) instead of typing full 81-character strings.
- **Dynamic Board Re-rendering:** Instant visual board refresh with UTF-8 grid borders after every placed digit.
- **Runtime Localization (i18n):** Seamless UI switching across English, Turkish, and German with dictionary fallback safety.
- **Execution Benchmarking:** Accurate solver performance tracking using high-resolution System.Diagnostics.Stopwatch.
- **Modular OOP Architecture:** Decoupled Models, Core solver logic, and UI rendering modules.

---

## 🛠️ Technologies & Concepts

- C# (.NET 8 / Core)
- Object-Oriented Programming (OOP)
- Separation of Concerns (SoC)
- Recursive Backtracking & Constraint Propagation
- Software Localization (i18n)
- Git & GitHub

---

## 📂 Repository Structure

```text
SudokuEngine/
├── Core/
│   └── BacktrackingSolver.cs   # Recursive solving logic & empty cell detection
├── Models/
│   ├── SudokuGrid.cs           # 9x9 matrix encapsulation & coordinate helpers
│   └── SudokuValidator.cs      # Row, column, and 3x3 subgrid rule validation
├── UI/
│   ├── ConsoleRenderer.cs      # UTF-8 grid rendering with 3x3 block borders
│   └── Localization.cs         # Key-value multi-language dictionary service
├── .gitignore                  # Build artifact exclusion rules
├── LICENSE                     # MIT License
├── README.md                   # Project documentation & architecture overview
└── Program.cs                  # Interactive orchestration loop & CLI entry point
```

## 🎯 Project Goal

The main goal of this project was to strengthen algorithmic problem-solving skills and master clean architecture patterns in modern C#.

Through this project, I practiced:

- Writing recursive search algorithms with state restoration (backtracking)
- Designing mathematical coordinate mapping for 3x3 Sudoku subgrids
- Structuring a multi-layer console project with distinct namespaces
- Managing multi-language support without hardcoding strings in core logic
- Measuring runtime performance and algorithmic efficiency in milliseconds
- Maintaining a clean, commit-driven development workflow on GitHub

---

## 🚀 Getting Started

Ensure you have the .NET SDK installed on your system.

### Run Locally

1. Clone the repository:
   ```bash
   git clone [https://github.com/melekboran1602/Sudoku-Solver-OOP.git](https://github.com/melekboran1602/Sudoku-Solver-OOP.git)
   cd Sudoku-Solver-OOP
   ```

2. Build and run the project:
   dotnet run

---

## 🎮 How to Use

1. **Select Language:** Choose your preferred display language (1: English, 2: Türkçe, 3: Deutsch).
2. **Set Numbers:** Enter cell coordinates and value separated by spaces:
   <Row> <Col> <Value>
   Example: 1 3 5 (Places 5 at Row 1, Column 3)
3. **Solve:** Type 0 and hit Enter to trigger the backtracking solver.

---

## 👩‍💻 Developer

**Melek Boran**

A software development project engineered to demonstrate clean architecture, algorithm design, and modular C# development.
