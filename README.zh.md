# Careful MVVM · CarefulDemo

**简体中文** | [English](README.en.md)

基于 **C# / WPF / .NET Framework 4.7.2** 的模块化桌面应用框架与示例项目，包含 MVVM 基础设施、自定义控件库，以及一个集成停靠布局、属性编辑和图像处理活动的可视化流程设计器。

本仓库适合用于学习 WPF 模块化开发、理解 MVVM 框架实现，以及参考桌面设计器和自定义控件的组织方式。

> 当前主要演示入口为 `Demo1/ModularityIDEDemo`。`Demo2/ModularityNavigationDemo` 目前仅包含基础窗口，尚未实现完整的导航演示。本文依据仓库源码整理；构建步骤需在安装相应组件的 Windows 环境中执行。

## 目录

- [项目概览](#项目概览)
- [功能介绍](#功能介绍)
- [项目结构](#项目结构)
- [快速开始](#快速开始)
- [流程设计器使用](#流程设计器使用)
- [开发与扩展](#开发与扩展)
- [常见问题](#常见问题)
- [当前状态与边界](#当前状态与边界)
- [参与贡献](#参与贡献)
- [许可证](#许可证)

## 项目概览

CarefulDemo 将桌面应用的基础能力、界面控件和业务示例拆分为独立项目：

- **框架层**：负责依赖注入、属性通知、命令、消息通信、模块加载、区域管理和对话框服务。
- **控件层**：提供停靠布局、设计画布、属性面板以及常见输入和数据展示控件。
- **示例层**：通过流程设计器展示框架与控件的组合使用，并将视图、视图模型、数据模型分别组织为程序集。

主要技术为 C#、XAML、WPF 和 .NET Framework 4.7.2。解决方案使用传统 `.csproj` 格式，包含 Visual Studio 2019 的版本信息，第三方依赖主要通过本地 DLL 引用。

## 功能介绍

### MVVM 基础设施

`Careful.Core.Mvvm` 提供：

- `ViewModelBase`、`NotifyPropertyChanged` 和 `ObservableObject` 等属性通知基础类型。
- `RelayCommand`、`DelegateCommand`、`CompositeCommand` 等命令实现。
- `ViewModelLocator` / `ViewModelLocationProvider`，用于关联 View 与 ViewModel。
- 事件转命令、交互请求、弹窗行为和绑定扩展。
- 窗口关闭、清理及 UI 调度相关接口与辅助类型。

### 依赖注入与模块化

- `Careful.Core` 提供 `CarefulIoc` 容器、注册与解析接口、对话框服务、文件日志和通用扩展。
- `Careful.BootstrapperApplication` 提供应用启动与初始化流程。
- `Careful.Module.Core` 提供模块目录、模块加载与初始化、依赖关系处理，以及区域管理和区域导航相关实现。
- 模块通过 `IModule` 组织注册和初始化逻辑，主窗口通过命名 Region 承载业务视图。

### 消息、校验与权限绑定

- **消息通信**：`Careful.Core.MessageFrame` 包含事件聚合器、发布订阅事件和 Messenger 消息机制。
- **属性校验**：`Careful.Core.PropertyValidation` 包含校验规则、错误容器和 WPF 校验相关行为。
- **权限绑定**：`Careful.Core.AuthorizationManagement` 提供权限提供者，以及将权限结果映射到控件可用性、可见性的扩展。
- **通用工具**：`Careful.Core.Tool` 包含 HTTP、JSON、XML、配置、图像和打印等辅助代码。

权限绑定主要服务于界面状态控制，业务系统仍需自行设计身份认证和业务操作授权。

### 自定义控件库

`ControlLibrary/ControlResource` 编译为 `Careful.Controls`，主要包括：

- **桌面布局**：`DockingManager` 停靠布局、基础窗口、工具箱。
- **流程编辑**：`DesignerCanvas`、活动节点、连接器和连线相关组件。
- **属性与数据展示**：`PropertyGrid`、扩展 DataGrid、树形表格、TreeListView、分页控件。
- **输入控件**：数字输入框、日期时间选择器、多选下拉框，以及文本框、按钮、列表和开关扩展。
- **视觉反馈**：消息框、环形进度、扩展进度条和动画选项卡。

默认控件主题位于 [DefaultTheme.xaml](ControlLibrary/ControlResource/Themes/DefaultTheme.xaml)。

### 流程设计器示例

主示例将以下界面组合在同一个窗口中：

- 顶部工具栏：流程文件和画布编辑相关操作。
- 左侧工作流列表：展示和选择流程。
- 中央画布：放置活动、编辑连接关系。
- 右侧活动库：提供可添加的活动类型。
- 右侧属性面板：编辑当前选中对象的属性。

示例提供“导入图像”“图像取反”“导出图像”三种活动，演示输入输出属性、数据校验与执行方法的组织方式。

## 项目结构

```text
CarefulDemo/
├── CarefulDemo.sln                  # Visual Studio 解决方案
├── ControlLibrary/
│   └── ControlResource/             # Careful.Controls 自定义控件库
├── MVVMCareful/
│   ├── Careful.BootstrapperApplication/ # 应用引导与初始化
│   ├── Careful.Core/                # 容器、对话框、日志与基础能力
│   ├── Careful.Core.Mvvm/           # 属性通知、命令与 ViewModel 支持
│   ├── Careful.Core.MessageFrame/   # 事件聚合与消息通信
│   ├── Careful.Core.PropertyValidation/ # 属性校验
│   ├── Careful.Core.AuthorizationManagement/ # 权限与界面状态绑定
│   ├── Careful.Core.Tool/           # 通用工具
│   ├── Careful.Module.Core/         # 模块、区域与导航
│   └── LanguagePackResource/        # cn / en 语言资源目录
├── Demo1/
│   ├── ModularityIDEDemo/           # 流程设计器启动项目
│   ├── Activities/                  # 三种图像处理活动及其样式
│   └── Modules/
│       ├── Module.Test.Models/      # 流程与活动数据模型
│       ├── Module.Test.ViewModels/  # 视图模型及命令
│       └── Module.Test.Views/       # 视图与模块入口
├── Demo2/
│   └── ModularityNavigationDemo/    # 导航示例基础窗口
├── ThemeLibrary/
│   └── ThemeResources/              # 主题资源项目骨架
├── Libs/                           # 本地依赖 DLL
├── Output/                         # 示例 Debug 输出目录
└── LICENSE                         # MPL-2.0 许可证
```

## 快速开始

### 1. 准备环境

- Windows。
- 建议使用 Visual Studio 2019 或 2022，并安装 **“.NET 桌面开发”**工作负载。
- 安装 **.NET Framework 4.7.2 Developer Pack / Targeting Pack**。仅安装运行时不能替代编译所需的目标框架引用程序集。
- 如使用命令行构建，请打开 Visual Studio Developer PowerShell 或 Developer Command Prompt。

### 2. 获取源码

```powershell
git clone https://github.com/daixin0/CarefulDemo.git
cd CarefulDemo
```

### 3. 检查本地依赖

仓库的 `Libs/` 目录包含：

- `JetBrains.Annotations.dll`
- `Microsoft.Expression.Interactions.dll`
- `Microsoft.Practices.ServiceLocation.dll`
- `System.Windows.Interactivity.dll`

部分项目中的 `HintPath` 与根目录 `Libs/` 的实际相对位置不一致。若出现引用缺失，请在 Visual Studio 的“引用”中重新指向根目录 `Libs/` 下对应的 DLL，或修正项目文件中的相对路径。

建议重点检查：

- `Careful.Core.Mvvm` 中的 ServiceLocation 和 Interactivity 引用；从该项目目录访问根目录依赖应使用 `../../Libs/`。
- `Careful.Core.PropertyValidation` 中的 Interactivity 引用；现有路径指向 `../Output/`，可改为根目录 `../../Libs/` 下对应文件。
- `Module.Test.Views` 中的 Interactions 和 Interactivity 引用；从该项目目录访问根目录依赖应使用 `../../../Libs/`。

此类缺失引用需要核对本地路径，单独执行 NuGet 还原通常不能修复。

### 4. 编译并启动

1. 使用 Visual Studio 打开 [CarefulDemo.sln](CarefulDemo.sln)。
2. 选择 `Debug` 和 `Any CPU` 配置。
3. 将 **ModularityIDEDemo** 设为启动项目。
4. 生成解决方案，处理目标框架或依赖引用错误。
5. 按 `F5` 调试运行，或按 `Ctrl + F5` 直接运行。

也可以在完成依赖检查后执行：

```powershell
msbuild .\CarefulDemo.sln /m /p:Configuration=Debug /p:Platform="Any CPU"
.\Output\ModularityIDEDemo.exe
```

两个演示程序的 Debug 配置均指向仓库根目录 `Output/`；Release 配置使用各自项目的 `bin/Release/`。请以所选配置的实际生成位置为准。

## 流程设计器使用

以下步骤用于体验示例中的图像处理流程：

1. 启动 `ModularityIDEDemo`，新建一个流程，启用设计画布。
2. 从活动库向画布添加“导入图像”“图像取反”“导出图像”活动。
3. 连接前一活动的图像输出与后一活动的图像输入，形成处理链：

   ```text
   导入图像 → 图像取反 → 导出图像
   ```

4. 选中导入活动，在属性面板设置 `ImagePath`，指向存在的图像文件。
5. 选中导出活动，设置 `SavePath`，使用可写路径并确保父目录存在。
6. 使用流程检查与执行操作，查看提示结果及导出的图像。
7. 使用保存和打开操作保存、重新载入流程文件。

图像取反活动通过逐像素处理 RGB 通道实现，保留原 Alpha 值，适合作为活动扩展示例。处理大图时应自行评估性能。

相关源码：

- [主设计器视图](Demo1/Modules/Module.Test.Views/DesignerMainView.xaml)
- [流程状态与命令](Demo1/Modules/Module.Test.ViewModels/DesignerMainViewModel.cs)
- [画布命令实现](ControlLibrary/ControlResource/DesignerCanvasControl/Designer/DesignerCanvas.Commands.cs)
- [图像活动](Demo1/Activities)

## 开发与扩展

### 创建 ViewModel

继承 `ViewModelBase`，通过 `Set` 更新属性，使用 `RelayCommand` 暴露操作。以下为最小代码示例：

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
            parameter => Message = "内容已更新");
    }
}
```

在视图中设置对应的 `DataContext`，即可绑定 `Message` 和 `UpdateMessageCommand`。如使用自动关联，在 XAML 中声明 `Careful.Core.Mvvm.ViewModel` 命名空间，并设置 `ViewModelLocator.AutoWireViewModel="True"`。

自动关联需要符合框架的类型解析约定。仓库中的典型对应关系为 `Module.Test.Views.DesignerMainView` 与 `Module.Test.ViewModels.DesignerMainViewModel`；自定义命名或程序集结构时，应检查 [ViewModelLocationProvider.cs](MVVMCareful/Careful.Core.Mvvm/ViewModel/ViewModelLocationProvider.cs) 中的解析和注册逻辑。

### 添加业务模块

参考 [DesignerModule.cs](Demo1/Modules/Module.Test.Views/DesignerModule.cs)：

1. 创建实现 `IModule` 的模块类型。
2. 在 `RegisterTypes(IContainerRegistry)` 中注册所需类型。
3. 在 `OnInitialized(IContainerProvider)` 中解析 `IRegionManager`，将业务视图注册到目标区域。
4. 在应用 `Bootstrapper.ConfigureModuleCatalog` 中通过 `moduleCatalog.AddModule(typeof(...))` 添加模块。
5. 在主窗口中使用 `RegionManager.RegionName` 声明同名区域。

现有示例将 `DesignerMainView` 注册到主窗口的 `ContentRegion`，可据此替换或增加业务视图。

### 添加自定义流程活动

参考 [ImportImageActivity.cs](Demo1/Activities/ImportImageActivity.cs) 等现有实现：

1. 继承 `Careful.Controls.DesignerCanvasControl.ActivityItem.Activity`。
2. 为活动添加 `ActivityVision` 特性。
3. 使用 `InputAttribute`、`OutputAttribute` 标记输入输出属性。
4. 重写 `ValidateData()`，检查活动所需数据。
5. 重写 `RunProc()`，实现活动执行逻辑。
6. 在 `ActivitiesStyle.xaml` 中添加对应样式，并继承基础 `Activity` 样式。
7. 在 `ActivityListViewModel` 的活动集合中添加活动名称和类型。

新增文件时注意：本仓库使用传统项目格式，直接在文件系统创建的源码或 XAML 文件可能需要显式加入 `.csproj`。通过 Visual Studio 添加可自动维护项目条目。

### 引入默认资源

主示例在 `App.xaml` 中合并以下资源，可作为接入控件库与活动样式的参考：

```xml
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/Careful.Controls;component/Themes/DefaultTheme.xaml" />
        <ResourceDictionary Source="/Activities;component/ActivitiesStyle.xaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

其中 `Activities` 是 Demo1 的活动程序集；仅使用通用控件时，按实际需要引入相应资源。

## 常见问题

### 提示找不到 .NET Framework 4.7.2 引用程序集

安装对应 Developer Pack / Targeting Pack，并确认 Visual Studio 已安装 .NET 桌面开发组件，然后重新打开解决方案。

### 提示无法解析 Interactivity、Interactions 或 ServiceLocation

检查“快速开始”中的本地依赖说明，确认 DLL 存在，并检查当前项目的相对引用路径。`System.Windows.Interactivity` 是本项目引用的旧版程序集，不应仅凭名称将其直接替换成其他行为库。

### 启动后只有一个空窗口

检查启动项目是否为 `ModularityNavigationDemo`。当前完整度更高的界面示例是 `ModularityIDEDemo`。

### 自定义控件样式未生效

检查 `App.xaml` 是否合并默认主题、资源 URI 中的程序集名是否正确，以及新增 XAML 是否已加入项目并使用正确的生成操作。

### ViewModel 没有自动关联

确认启用了 `AutoWireViewModel`，检查 View、ViewModel 的命名空间、类型名和程序集名是否满足定位约定，并确认构造函数需要的依赖已经注册。

### 图像活动校验或执行失败

检查输入图像是否存在、活动输入输出是否正确连接、导出路径是否可写，以及父目录是否已创建。具体执行及异常处理行为以活动和画布实现为准。

## 当前状态与边界

- 项目面向 Windows WPF 与 .NET Framework 4.7.2；仓库未提供现代 .NET 或跨平台运行方案。
- Demo1 包含流程设计器实现；Demo2 目前是导航演示的基础窗口。
- `ThemeResources` 目前主要是项目骨架，实际默认控件样式位于 `Careful.Controls`。
- 仓库包含 `cn` / `en` 语言资源目录，不能据此推定所有界面都已完成国际化或支持运行时切换。
- 部分依赖使用需要核对的相对路径，新环境首次构建可能需要调整引用。
- `Module.Test.*` 是示例业务模块名称，并非自动化测试项目；当前仓库未发现独立的自动化测试工程。
- 本文未提供编译通过或界面运行验证结论。用于实际业务前，建议针对流程持久化、活动执行、异常处理及资源释放补充验证。

## 参与贡献

欢迎通过 [Issues](https://github.com/daixin0/CarefulDemo/issues) 反馈问题，或提交 Pull Request 改进代码与文档。

提交问题时，建议提供 Windows 与 Visual Studio 版本、构建配置、启动项目、复现步骤，以及完整的错误信息。提交界面或流程行为修改时，请说明验证方式和结果；新增活动或控件时，请同时补充必要的资源与使用说明。

## 许可证

本项目采用 **Mozilla Public License 2.0（MPL-2.0）**，完整条款见 [LICENSE](LICENSE)。第三方依赖及保留的第三方代码声明应遵循各自的许可证要求。
