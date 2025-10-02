# 梨子电脑工具箱

一个功能强大的Windows桌面应用程序，集成了硬件检测、系统监控和常用工具管理功能。

![版本](https://img.shields.io/badge/版本-1.0.0-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![平台](https://img.shields.io/badge/平台-Windows-green)
![许可证](https://img.shields.io/badge/许可证-MIT-orange)

## ✨ 功能特色

### 🔍 硬件检测
- **系统信息**：详细显示电脑型号、操作系统版本
- **处理器信息**：CPU型号、核心数量
- **内存信息**：品牌、容量、频率、组合方式
- **显卡信息**：独立显卡和集成显卡检测
- **硬盘信息**：磁盘型号、容量、类型
- **显示器信息**：分辨率、刷新率
- **声卡网卡**：音频和网络设备检测

### 🌡️ 温度电压监控
- CPU温度电压监控
- GPU温度电压监控
- 硬盘温度监控
- 内存温度电压监控
- 实时数据刷新功能

### 🛠️ 工具集成
- **CPU工具**：CPU-Z（32位/64位版本）
- **GPU工具**：GPU-Z
- **硬盘工具**：
  - CrystalDiskInfo（磁盘健康检测）
  - CrystalDiskMark（磁盘性能测试）
  - SSD-Z（固态硬盘检测）
  - SpaceSniffer（磁盘空间分析）
- **综合工具**：
  - AIDA64（系统诊断工具）
  - MonitorTest（显示器检测工具）

### 🎨 用户界面
- **现代化设计**：深色主题界面
- **直观导航**：左侧导航菜单，右侧内容区域
- **响应式布局**：支持窗口大小调整
- **智能提示**：工具功能详细说明
- **鼠标悬停效果**：增强用户体验

## 📦 项目结构

```
DesktopApp/
├── Program.cs                 # 应用程序入口点
├── MainForm.cs               # 主窗体实现
├── DesktopApp.csproj         # 项目配置文件
├── README.md                 # 项目说明文档
├── img/                      # 图标资源
│   ├── toollogo.ico
│   └── toollogo.png
├── Models/                   # 数据模型
│   ├── MemoryModule.cs       # 内存模块信息
│   ├── ToolGroup.cs          # 工具分组
│   └── ToolInfo.cs           # 工具信息
├── Services/                 # 服务层
│   ├── HardwareInfoService.cs # 硬件信息获取服务
│   ├── ToolManagerService.cs  # 工具管理服务
│   └── UIBuilderService.cs    # 界面构建服务
└── tools/                    # 工具文件目录
    ├── CPU/
    │   └── CPUZ/
    │       ├── cpuz_x32.exe
    │       └── cpuz_x64.exe
    ├── Drive/
    │   ├── CrystalDiskMark/
    │   │   ├── DiskMark32S.exe
    │   │   └── DiskMark64S.exe
    │   ├── SpaceSniffer/
    │   │   └── SpaceSniffer.exe
    │   └── SSDZ/
    │       └── SSD-Z.exe
    └── Tool/
        ├── AIDA64/
        │   └── aida64.exe
        └── MonitorTest/
            └── MonitorTest64.exe
```

## 🚀 快速开始

### 系统要求
- **操作系统**：Windows 10 或更高版本
- **.NET框架**：.NET 8.0 Runtime（独立版本无需安装）
- **内存**：至少 2GB RAM
- **磁盘空间**：至少 100MB 可用空间

### 安装步骤

1. **下载发布版本**
   - 从 [Releases](https://github.com/linxzee/DesktopApp/releases) 页面下载最新版本
   - 解压到任意目录

2. **运行应用程序**
   - 双击 `DesktopApp.exe` 启动程序
   - 独立版本无需安装 .NET 运行时

### 从源代码构建

1. **克隆仓库**
   ```bash
   git clone https://github.com/linxzee/DesktopApp.git
   cd DesktopApp
   ```

2. **安装依赖**
   - 确保已安装 .NET 8.0 SDK

3. **构建项目**
   ```bash
   dotnet build
   ```

4. **运行项目**
   ```bash
   dotnet run
   ```

## 📦 打包发布指南

### 打包成免安装的EXE文件

#### 方法一：使用提供的构建脚本（最简单）

项目提供了两个构建脚本，可以自动完成打包过程：

1. **完整构建脚本** (`build.bat`)
   - 双击运行 `build.bat`
   - 自动检查依赖、清理、构建、发布
   - 复制所有资源文件和工具文件
   - 生成完整的发布包

2. **快速打包脚本** (`快速打包.bat`)
   - 双击运行 `快速打包.bat`
   - 快速打包为单个EXE文件
   - 适合快速测试和发布

#### 方法二：使用 .NET CLI 发布（推荐）

1. **发布为独立应用程序**
   ```bash
   # 发布为独立应用程序（包含运行时）
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
   
   # 或者发布为框架依赖应用程序（需要用户安装.NET运行时）
   dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
   ```

2. **发布选项说明**
   - `-c Release`：使用发布配置
   - `-r win-x64`：目标平台为64位Windows
   - `--self-contained true`：包含.NET运行时，生成独立的可执行文件
   - `-p:PublishSingleFile=true`：打包为单个EXE文件

3. **输出目录**
   - 发布文件位于：`bin\Release\net8.0-windows\win-x64\publish\`
   - 包含：`DesktopApp.exe` 和所有依赖文件

#### 方法三：使用 Visual Studio 发布

1. **右键项目** → **发布**
2. **选择发布目标**：文件夹
3. **配置发布设置**：
   - 目标框架：net8.0-windows
   - 部署模式：独立
   - 目标运行时：win-x64
   - 生成单个文件：是
4. **点击发布**按钮

#### 方法四：创建便携式安装包

1. **手动创建发布包结构**：
   ```
   梨子电脑工具箱_v1.0.0/
   ├── DesktopApp.exe
   ├── tools/           # 工具文件夹
   ├── img/            # 图标资源
   └── README.txt      # 使用说明
   ```

#### 打包注意事项

1. **文件大小优化**
   - 独立发布：约50-100MB（包含.NET运行时）
   - 框架依赖：约5-10MB（需要用户安装.NET 8.0）

2. **兼容性考虑**
   - 建议同时提供32位和64位版本
   - 测试在不同Windows版本上的兼容性

3. **工具文件处理**
   - 确保 `tools/` 目录中的所有工具文件都被包含在发布包中
   - 检查所有工具的可执行路径是否正确

4. **图标和资源**
   - 验证应用程序图标是否正确显示
   - 确保所有图片资源都能正常加载

### 发布检查清单

- [ ] 应用程序能正常启动
- [ ] 所有硬件检测功能正常工作
- [ ] 所有工具都能正确启动
- [ ] 界面显示正常，无布局问题
- [ ] 图标和资源文件正确加载
- [ ] 在不同Windows版本上测试兼容性
- [ ] 杀毒软件误报检查

## 📋 使用说明

### 硬件检测页面
- 显示电脑基本信息和详细硬件配置
- 自动检测并显示所有硬件组件
- 支持实时硬件状态监控

### 温度电压页面
- 监控主要硬件组件的温度和电压
- 提供数据刷新功能
- 直观的温度电压显示面板

### 工具管理页面
- **CPU工具**：CPU性能检测和分析工具
- **GPU工具**：显卡信息检测工具
- **硬盘工具**：磁盘健康检测和性能测试工具
- **综合工具**：系统诊断和显示器检测工具

### 设置页面
- 主题设置选项
- 软件信息显示
- 版本信息和构建日期

## 🔧 技术架构

### 核心技术
- **开发语言**：C#
- **UI框架**：Windows Forms
- **目标框架**：.NET 8.0
- **系统管理**：WMI (Windows Management Instrumentation)

### 设计模式
- **服务层模式**：分离业务逻辑和界面逻辑
- **模型-视图-控制器**：清晰的数据流管理
- **依赖注入**：松耦合的组件设计

### 主要组件
- **HardwareInfoService**：硬件信息获取服务
- **ToolManagerService**：工具管理和启动服务
- **UIBuilderService**：动态界面构建服务

## 🛠️ 开发指南

### 添加新工具
1. 在 `tools` 目录下创建对应的工具文件夹
2. 将可执行文件放入相应目录
3. 在 `MainForm.cs` 中更新工具配置
4. 在 `ToolManagerService.cs` 中添加工具描述

### 自定义界面
- 修改 `UIBuilderService.cs` 中的界面构建方法
- 调整颜色主题和布局参数
- 添加新的UI组件和交互逻辑

### 扩展硬件检测
- 在 `HardwareInfoService.cs` 中添加新的WMI查询
- 实现新的硬件信息获取方法
- 更新界面显示逻辑

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

## 📞 联系方式

- **作者**：linxzee
- **邮箱**：linxzee@example.com
- **项目主页**：[GitHub Repository](https://github.com/linxzee/DesktopApp)

## 🙏 致谢

感谢以下开源工具和库：
- [CPU-Z](https://www.cpuid.com/softwares/cpu-z.html)
- [GPU-Z](https://www.techpowerup.com/gpuz/)
- [CrystalDiskInfo](https://crystalmark.info/en/software/crystaldiskinfo/)
- [CrystalDiskMark](https://crystalmark.info/en/software/crystaldiskmark/)
- [AIDA64](https://www.aida64.com/)
- [MonitorTest](https://monitortest.com/)

---

⭐ 如果这个项目对你有帮助，请给个 Star！