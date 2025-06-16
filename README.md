# Node-Based Visual Formula Editor (CanvasTest Project)

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet)
![Language](https://img.shields.io/badge/Language-C%23-blue)

A WPF desktop application that provides a node-based, visual interface for building and composing formulas, similar to a visual Excel worksheet. Users can drag function nodes onto a canvas, connect them, and see the results, creating complex calculations in an intuitive, graphical way.

---

## 📸 Demo

*(This is the most important part of a UI project's README! Consider recording a short GIF of dragging nodes, connecting them, and seeing values update.)*

![Application Screenshot](./docs/images/screenshot.png "Screenshot of the main canvas with nodes and connections")

---

## ✨ Core Features

* **Visual Node-Based Editor:** A dynamic canvas where you can add, move, and arrange function nodes.
* **Drag & Drop Interface:** Easily drag new functions from a library onto the canvas.
* **Node Connections:** Visually connect the output of one node to the input of another to chain operations.
* **Data-Driven Function Library:** The available functions (like `SUM`, `AVERAGE`) are defined in a data model, making it easy to add new ones without changing core logic.
* **Undo/Redo Functionality:** Robust state management allows for undoing and redoing actions like moving nodes, changing values, and making connections.
* **Modern C# & WPF:** Built using modern .NET best practices, including nullable reference types to minimize runtime errors.

---

## 🛠️ Technology Stack

* **Framework:** .NET 8.0
* **UI:** WPF (Windows Presentation Foundation)
* **Language:** C# 12
* **Primary Design Pattern:** MVVM (Model-View-ViewModel)
* **Core Concepts:**
    * **Command Pattern:** Used for implementing the Undo/Redo feature.
    * **Attached Properties:** Used extensively by the `Canvas` to manage node positions.
    * **Data Binding:** To keep the UI and the underlying data in sync.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Visual Studio 2022 (Community Edition or higher)
* Workloads: ".NET desktop development"

### Installation & Running

1.  **Clone the repository:**
    ```sh
    git clone <your-repository-url>
    ```
2.  **Navigate to the project directory:**
    ```sh
    cd <your-project-directory>
    ```
3.  **Open the solution file** (`CanvasTest.sln`) in Visual Studio 2022.
4.  **Restore NuGet Packages:** Visual Studio should do this automatically upon opening the solution. If not, right-click the solution in the Solution Explorer and select "Restore NuGet Packages".
5.  **Build & Run:** Press `F5` or click the "Start" button to build and run the application.

---

## 🏛️ Architectural Concepts

This project follows the MVVM design pattern to ensure a clean separation of concerns.

* **Models:** Represent the core data of the application. The `ExcelFunction` class is a perfect example, defining the blueprint for functions without containing any logic related to its display.
* **Views:** The UI of the application, defined in XAML (`.xaml` files). This includes the `MainWindow`, the `Node` UserControl, and other visual elements. The View contains minimal code-behind, with its primary role being to display data and forward user input to the ViewModel via commands and data binding.
* **ViewModels:** Act as the bridge between the Model and the View. They hold the application's state and logic. For instance, the `MainViewModel` would contain the list of all nodes on the canvas, manage the Undo/Redo stacks, and expose `ICommand` properties that the View can bind to.

### Safety and Reliability

The project uses C#'s **Nullable Reference Types** feature to prevent `NullReferenceException` at compile time. This leads to more robust and reliable code by making the nullability of every variable explicit.

---

## 🗺️ Project Roadmap

* [ ] **Implement Undo/Redo Manager:** Complete the `UndoRedoManager` and integrate it with all user actions.
* [ ] **Calculation Engine:** Build the logic that evaluates the node graph and computes the final results.
* [ ] **Expand Function Library:** Add more mathematical, statistical, and text functions.
* [ ] **Load Functions from File:** Modify the `ExcelFunction` service to load definitions from a JSON or XML file, making the application extensible.
* [ ] **Advanced Node Types:** Introduce nodes for constant values, data inputs, and text display.
* [ ] **UI/UX Polish:** Improve styling, add visual feedback for connections, and refine the user experience.
* [ ] **Save/Load Functionality:** Implement the ability to save the node canvas state to a file and load it back.

---

## 📜 License

This project is licensed under the MIT License. See the [LICENSE.md](LICENSE.md) file for details.