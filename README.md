# Node-Based Visual Formula Editor (CanvasTest Project)

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet)
![Language](https://img.shields.io/badge/Language-C%23-blue)

A WPF desktop application that provides a node-based, visual interface for building and composing formulas, similar to a visual Excel worksheet. Users can drag function nodes onto a canvas, connect them, and see the results, creating complex calculations in an intuitive, graphical way.
It's similar to Grasshopper for Rhino, or Unreal's Blueprints or Blender's Geometry nodes.

Define an input => plug into a node => get the output ...=> plug it in again somewhere

---

## 📸 Demo

here's what it looksk like so far
![Application Screenshot](./images/Screenshot 2025-06-17 225316.png "Screenshot of the main canvas with nodes and connections")

---

## ✨ Core Features

* **Visual Node-Based Editor:** A dynamic canvas where you can add, move, and arrange function nodes.
* **Drag & Drop Interface:** Easily drag new functions from a library onto the canvas.
* **Node Connections:** Visually connect the output of one node to the input of another to chain operations.
* **Data-Driven Function Library:** The available functions (like `SUM`, `AVERAGE`) are defined in a data model, making it easy to add new ones without changing core logic.

---

## 🛠️ I'm using;

* **Framework:** .NET 8.0
* **UI:** WPF (Windows Presentation Foundation)
* **Language:** C# 12
* **Primary Design Pattern:** MVVM (Model-View-ViewModel) ... kinda

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

## 🏛️ Architecture

This project is attempting to follow the MVVM design pattern to ensure a clean separation of concerns. (it doesn't, but that's the end goal)
* **I'm doing my best to try and make this pure MVVM. I know it allows UI changes without impacting the underlying data and models. 
* **Models:** Represent the core data of the application. The `ExcelFunction` class - defining the blueprint for functions without containing any logic related to its display.
* **Views:** The UI of the application, defined in XAML (`.xaml` files). This includes the `MainWindow`, the `Node` UserControl, and other visual elements. The View contains minimal code-behind, with its primary role being to display data and forward user input to the ViewModel via commands and data binding.
* **ViewModels:** Act as the bridge between the Model and the View. They hold the application's state and logic. For instance, the `MainViewModel` would contain the list of all nodes on the canvas, it will manage the Undo/Redo stacks, and expose `ICommand` properties that the View can bind to.

### Safety and Reliability

The project Isn't yet safe nor reliable. Use it at your own risk.

---

## 🗺️ Project Roadmap
* [ ] **Sort out the Zoom and Pan functions** the canvas is massive and you lose yourself almost immediately with how jerky and awful the zoom function is.
* [ ] **Implement the connection logic** you can't connect nodes yet i've been bogged down in UI stuff at the moment so that I can actually see what is even going on.
* [ ] **Implement Undo/Redo Manager:** Complete the `UndoRedoManager` and integrate it with all user actions.
* [ ] **Calculation Engine:** Build the logic that evaluates the node graph and computes the final results.
* [ ] **Expand Function Library:** Add more mathematical, statistical, and text functions.
* [ ] **Load Functions from File:** Modify the `ExcelFunction` service to load definitions from a JSON or XML file, making the application extensible.
* [ ] **Advanced Node Types:** Introduce nodes for constant values, data inputs, and text display, formatting etc.
* [ ] **UI/UX Polish:** Improve styling, add visual feedback for connections, and refine the user experience.
* [ ] **Save/Load Functionality:** Implement the ability to save the node canvas state to a file and load it back.

