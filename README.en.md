# Careful MVVM · CarefulDemo

[简体中文](README.md) | **English**

A modular desktop application framework and sample project built with **C# / WPF / .NET Framework 4.7.2**. It includes MVVM infrastructure, a custom control library, and a visual workflow designer with docking layouts, property editing, and image processing activities.

This repository is useful for learning modular WPF development, exploring MVVM framework implementations, and studying how desktop designers and custom controls are organized.

> The main demo entry point is `Demo1/ModularityIDEDemo`. `Demo2/ModularityNavigationDemo` currently contains only a basic window, without a complete navigation demo. This document is based on the repository source code; build instructions require Windows with the appropriate development components installed.

## Contents

- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Using the Workflow Designer](#using-the-workflow-designer)
- [Development and Extension](#development-and-extension)
- [Troubleshooting](#troubleshooting)
- [Current Status and Limitations](#current-status-and-limitations)
- [Contributing](#contributing)
- [License](#license)

## Overview

CarefulDemo separates application infrastructure, UI controls, and sample functionality into individual projects:

- **Framework layer**: dependency injection, property notifications, commands, messaging, module loading, region management, and dialog services.
- **Control layer**: docking layouts, a designer canvas, a property panel, and common input and data display controls.
- **Sample layer**: a workflow designer demonstrating how the framework and controls work together, with views, view models, and data models organized into separate assemblies.

The main technologies are C#, XAML, WPF, and .NET Framework 4.7.2. The solution uses the traditional `.csproj` format, includes Visual Studio 2019 version metadata, and primarily references third-party dependencies through local DLL files.

## Features

### MVVM Infrastructure

`Careful.Core.Mvvm` provides:

- Property notification base types, including `ViewModelBase`, `NotifyPropertyChanged`, and `ObservableObject`.
- Command implementations such as `RelayCommand`, `DelegateCommand`, and `CompositeCommand`.
- `ViewModelLocator` / `ViewModelLocationProvider` for associating views with view models.
- Event-to-command support, interaction requests, popup behaviors, and binding extensions.
- Interfaces and helpers for window closing, cleanup, and UI dispatching.

### Dependency Injection and Modularity

- `Careful.Core` provides the `CarefulIoc` container, registration and resolution interfaces, dialog services, file logging, and general extensions.
- `Careful.BootstrapperApplication` provides application startup and initialization.
- `Careful.Module.Core` includes module catalogs, module loading and initialization, dependency handling, region management, and region navigation implementations.
- Modules use `IModule` to organize registration and initialization, while named regions in the main window host application views.

### Messaging, Validation, and Authorization Bindings

- **Messaging**: `Careful.Core.MessageFrame` includes an event aggregator, publish/subscribe events, and a Messenger implementation.
- **Property validation**: `Careful.Core.PropertyValidation` includes validation rules, error containers, and WPF validation behaviors.
- **Authorization bindings**: `Careful.Core.AuthorizationManagement` provides authorization providers and extensions that map authorization results to control availability and visibility.
- **Utilities**: `Careful.Core.Tool` contains helpers for HTTP, JSON, XML, configuration, images, and printing.

Authorization bindings primarily control UI state. Applications still need to implement their own authentication and authorization for business operations.

### Custom Control Library

`ControlLibrary/ControlResource` builds the `Careful.Controls` assembly, which includes:

- **Desktop layouts**: `DockingManager`, a base window, and a toolbox.
- **Workflow editing**: `DesignerCanvas`, activity nodes, connectors, and connection components.
- **Properties and data display**: `PropertyGrid`, extended DataGrid controls, tree grids, TreeListView, and paging controls.
- **Input controls**: numeric input, date/time selection, multi-select combo boxes, and extensions for text boxes, buttons, lists, and toggles.
- **Visual feedback**: message boxes, progress rings, extended progress bars, and animated tabs.

The default control theme is defined in [DefaultTheme.xaml](ControlLibrary/ControlResource/Themes/DefaultTheme.xaml).

### Workflow Designer Demo

The main demo combines the following UI areas in a single window:

- Top toolbar: workflow file operations and canvas editing commands.
- Left workflow list: display and select workflows.
- Central canvas: place activities and edit connections.
- Right activity library: available activity types.
- Right property panel: edit the selected object's properties.

Three sample activities—Import Image, Invert Image, and Export Image—demonstrate input/output properties, data validation, and execution methods.

## Project Structure

```text
CarefulDemo/
├── CarefulDemo.sln                  # Visual Studio solution
├── ControlLibrary/
│   └── ControlResource/             # Careful.Controls custom control library
├── MVVMCareful/
│   ├── Careful.BootstrapperApplication/ # Application bootstrapping
│   ├── Careful.Core/                # Container, dialogs, logging, and core utilities
│   ├── Careful.Core.Mvvm/           # Notifications, commands, and ViewModel support
│   ├── Careful.Core.MessageFrame/   # Event aggregation and messaging
│   ├── Careful.Core.PropertyValidation/ # Property validation
│   ├── Careful.Core.AuthorizationManagement/ # Authorization and UI state bindings
│   ├── Careful.Core.Tool/           # General utilities
│   ├── Careful.Module.Core/         # Modules, regions, and navigation
│   └── LanguagePackResource/        # cn / en language resource directories
├── Demo1/
│   ├── ModularityIDEDemo/           # Workflow designer startup project
│   ├── Activities/                  # Three image activities and their styles
│   └── Modules/
│       ├── Module.Test.Models/      # Workflow and activity data models
│       ├── Module.Test.ViewModels/  # View models and commands
│       └── Module.Test.Views/       # Views and module entry point
├── Demo2/
│   └── ModularityNavigationDemo/    # Basic window for the navigation demo
├── ThemeLibrary/
│   └── ThemeResources/              # Theme resource project skeleton
├── Libs/                           # Local dependency DLLs
├── Output/                         # Demo Debug output directory
└── LICENSE                         # MPL-2.0 license
```

## Getting Started

### 1. Prerequisites

- Windows.
- Visual Studio 2019 or 2022 is recommended, with the **.NET desktop development** workload installed.
- **.NET Framework 4.7.2 Developer Pack / Targeting Pack**. Installing the runtime alone does not provide the reference assemblies required for compilation.
- For command-line builds, open Visual Studio Developer PowerShell or Developer Command Prompt.

### 2. Get the Source

```powershell
git clone https://github.com/daixin0/CarefulDemo.git
cd CarefulDemo
```

### 3. Check Local Dependencies

The repository's `Libs/` directory contains:

- `JetBrains.Annotations.dll`
- `Microsoft.Expression.Interactions.dll`
- `Microsoft.Practices.ServiceLocation.dll`
- `System.Windows.Interactivity.dll`

Some project `HintPath` values do not match the actual relative location of the root `Libs/` directory. If references are missing, update them in Visual Studio to point to the corresponding DLLs in the root `Libs/` directory, or correct the relative paths in the project files.

Pay particular attention to:

- ServiceLocation and Interactivity references in `Careful.Core.Mvvm`: use `../../Libs/` to reach the root dependency directory from this project.
- The Interactivity reference in `Careful.Core.PropertyValidation`: the existing path points to `../Output/`; it can be updated to the corresponding file under `../../Libs/`.
- Interactions and Interactivity references in `Module.Test.Views`: use `../../../Libs/` to reach the root dependency directory from this project.

These missing references require checking local paths. NuGet restore alone will generally not resolve them.

### 4. Build and Run

1. Open [CarefulDemo.sln](CarefulDemo.sln) in Visual Studio.
2. Select the `Debug` configuration and `Any CPU` platform.
3. Set **ModularityIDEDemo** as the startup project.
4. Build the solution and resolve any target framework or dependency reference errors.
5. Press `F5` to debug, or `Ctrl + F5` to run without debugging.

After checking dependencies, you can also run:

```powershell
msbuild .\CarefulDemo.sln /m /p:Configuration=Debug /p:Platform="Any CPU"
.\Output\ModularityIDEDemo.exe
```

Both demo applications use the root `Output/` directory for Debug builds. Release builds use each project's own `bin/Release/` directory. Check the actual output location for the selected configuration.

## Using the Workflow Designer

To explore the sample image processing workflow:

1. Start `ModularityIDEDemo` and create a new workflow to enable the designer canvas.
2. Add Import Image (`导入图像`), Invert Image (`图像取反`), and Export Image (`导出图像`) activities from the activity library.
3. Connect each activity's image output to the next activity's image input:

   ```text
   Import Image → Invert Image → Export Image
   ```

4. Select the import activity and set `ImagePath` in the property panel to an existing image file.
5. Select the export activity and set `SavePath` to a writable location, ensuring that its parent directory exists.
6. Use the workflow check and execution operations, then inspect the result messages and exported image.
7. Use the save and open operations to save and reload workflow files.

The invert activity processes RGB channels pixel by pixel while preserving the original alpha value. It serves as an example of activity extension; evaluate performance when processing large images.

Related source files:

- [Main designer view](Demo1/Modules/Module.Test.Views/DesignerMainView.xaml)
- [Workflow state and commands](Demo1/Modules/Module.Test.ViewModels/DesignerMainViewModel.cs)
- [Canvas command implementation](ControlLibrary/ControlResource/DesignerCanvasControl/Designer/DesignerCanvas.Commands.cs)
- [Image activities](Demo1/Activities)

## Development and Extension

### Create a ViewModel

Inherit from `ViewModelBase`, update properties through `Set`, and expose actions with `RelayCommand`. A minimal example:

```csharp
using Careful.Core.Mvvm.Command;
using Careful.Core.Mvvm.ViewModel;
using System.Windows.Input;

public class ExampleViewModel : ViewModelBase
{
    private string _message = "Hello, Careful MVVM";

    public string Message
    {
        get { return _message; }
        set { Set(ref _message, value); }
    }

    public ICommand UpdateMessageCommand { get; }

    public ExampleViewModel()
    {
        UpdateMessageCommand = new RelayCommand(
            parameter => Message = "Content updated");
    }
}
```

Set the view's `DataContext` to the corresponding view model to bind `Message` and `UpdateMessageCommand`. For automatic association, declare the `Careful.Core.Mvvm.ViewModel` namespace in XAML and set `ViewModelLocator.AutoWireViewModel="True"`.

Automatic association depends on the framework's type resolution conventions. A typical pair in this repository is `Module.Test.Views.DesignerMainView` and `Module.Test.ViewModels.DesignerMainViewModel`. If you use different naming or assembly structures, check the resolution and registration logic in [ViewModelLocationProvider.cs](MVVMCareful/Careful.Core.Mvvm/ViewModel/ViewModelLocationProvider.cs).

### Add an Application Module

Use [DesignerModule.cs](Demo1/Modules/Module.Test.Views/DesignerModule.cs) as a reference:

1. Create a module type that implements `IModule`.
2. Register required types in `RegisterTypes(IContainerRegistry)`.
3. Resolve `IRegionManager` in `OnInitialized(IContainerProvider)` and register the application view with the target region.
4. Add the module through `moduleCatalog.AddModule(typeof(...))` in the application's `Bootstrapper.ConfigureModuleCatalog` method.
5. Declare a region with the same name in the main window using `RegionManager.RegionName`.

The existing demo registers `DesignerMainView` with the main window's `ContentRegion`. Follow this pattern to replace or add application views.

### Add a Custom Workflow Activity

Refer to existing implementations such as [ImportImageActivity.cs](Demo1/Activities/ImportImageActivity.cs):

1. Inherit from `Careful.Controls.DesignerCanvasControl.ActivityItem.Activity`.
2. Add the `ActivityVision` attribute to the activity.
3. Mark input and output properties with `InputAttribute` and `OutputAttribute`.
4. Override `ValidateData()` to check the activity's required data.
5. Override `RunProc()` to implement execution logic.
6. Add a corresponding style to `ActivitiesStyle.xaml`, based on the base `Activity` style.
7. Add the activity name and type to the activity collection in `ActivityListViewModel`.

When adding files, remember that this repository uses the traditional project format. Source or XAML files created directly in the file system may need to be explicitly included in `.csproj`. Adding them through Visual Studio maintains these project entries automatically.

### Include Default Resources

The main demo merges the following resources in `App.xaml`. Use this as a reference when integrating the control library and activity styles:

```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/Careful.Controls;component/Themes/DefaultTheme.xaml" />
        <ResourceDictionary Source="/Activities;component/ActivitiesStyle.xaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

`Activities` is the activity assembly in Demo1. When using only general controls, include resources according to your application's needs.

## Troubleshooting

### Missing .NET Framework 4.7.2 Reference Assemblies

Install the corresponding Developer Pack / Targeting Pack, ensure that Visual Studio has the .NET desktop development components installed, and reopen the solution.

### Unresolved Interactivity, Interactions, or ServiceLocation References

Review the local dependency instructions under Getting Started. Confirm that the DLLs exist and check each project's relative reference paths. This project references the legacy `System.Windows.Interactivity` assembly; do not assume another behaviors library is a direct replacement based on its name.

### Only an Empty Window Appears

Check whether the startup project is `ModularityNavigationDemo`. The more complete UI demo is currently `ModularityIDEDemo`.

### Custom Control Styles Are Not Applied

Check that `App.xaml` merges the default theme, that resource URIs use the correct assembly names, and that new XAML files are included in the project with the appropriate build action.

### ViewModel Association Does Not Work

Confirm that `AutoWireViewModel` is enabled. Check that view and view model namespaces, type names, and assembly names follow the locator conventions, and ensure that constructor dependencies have been registered.

### Image Activity Validation or Execution Fails

Check that the input image exists, activity inputs and outputs are connected correctly, the export path is writable, and the parent directory exists. Refer to the activity and canvas implementations for exact execution and exception handling behavior.

## Current Status and Limitations

- The project targets Windows WPF and .NET Framework 4.7.2. The repository does not provide a modern .NET or cross-platform setup.
- Demo1 contains the workflow designer implementation; Demo2 currently provides a basic window for a navigation demo.
- `ThemeResources` is primarily a project skeleton. The actual default control styles are in `Careful.Controls`.
- The repository contains `cn` / `en` language resource directories. Their presence does not establish that all UI elements are localized or support runtime language switching.
- Some dependencies use relative paths that need checking. A first build in a new environment may require reference adjustments.
- `Module.Test.*` names refer to sample application modules, not automated test projects. No separate automated test project was found in the current repository.
- This document does not claim a verified successful build or UI run. Before using the code in an application, validate workflow persistence, activity execution, exception handling, and resource disposal.

## Contributing

Report problems through [Issues](https://github.com/daixin0/CarefulDemo/issues), or submit a pull request to improve the code and documentation.

When reporting a problem, include your Windows and Visual Studio versions, build configuration, startup project, reproduction steps, and full error details. For UI or workflow behavior changes, describe how you validated the change and the results. Include the necessary resources and usage instructions when adding an activity or control.

## License

This project is licensed under the **Mozilla Public License 2.0 (MPL-2.0)**. See [LICENSE](LICENSE) for the full terms. Third-party dependencies and retained third-party code notices remain subject to their respective license requirements.
