# 贡献指南

感谢你考虑为梨子电脑工具箱项目做出贡献！本指南将帮助你了解如何参与项目开发。

## 开发环境设置

### 系统要求
- Windows 10 或更高版本
- .NET 8.0 SDK
- Visual Studio 2022 或 Visual Studio Code

### 设置步骤
1. Fork 本仓库
2. 克隆你的 fork：
   ```bash
   git clone https://github.com/你的用户名/DesktopApp.git
   cd DesktopApp
   ```
3. 安装依赖：
   ```bash
   dotnet restore
   ```
4. 构建项目：
   ```bash
   dotnet build
   ```

## 代码规范

### C# 编码规范
- 使用 PascalCase 命名类和方法
- 使用 camelCase 命名变量和参数
- 使用有意义的命名
- 添加必要的 XML 注释
- 遵循 C# 编码约定

### 项目结构
- `Models/`：数据模型类
- `Services/`：业务逻辑服务
- `img/`：图标和图片资源
- `tools/`：第三方工具文件

### 提交信息规范
使用约定式提交格式：
- `feat:` 新功能
- `fix:` 修复bug
- `docs:` 文档更新
- `style:` 代码格式调整
- `refactor:` 代码重构
- `test:` 测试相关
- `chore:` 构建过程或辅助工具变动

## 添加新功能

### 添加新工具
1. 在 `tools/` 目录下创建对应的工具文件夹
2. 将可执行文件放入相应目录
3. 在 `MainForm.cs` 中更新工具配置
4. 在 `ToolManagerService.cs` 中添加工具描述

### 扩展硬件检测
1. 在 `HardwareInfoService.cs` 中添加新的WMI查询
2. 实现新的硬件信息获取方法
3. 更新界面显示逻辑

### 界面改进
1. 修改 `UIBuilderService.cs` 中的界面构建方法
2. 调整颜色主题和布局参数
3. 添加新的UI组件和交互逻辑

## 测试

### 运行测试
```bash
dotnet test
```

### 手动测试清单
- [ ] 应用程序能正常启动
- [ ] 所有硬件检测功能正常工作
- [ ] 所有工具都能正确启动
- [ ] 界面显示正常，无布局问题
- [ ] 在不同Windows版本上测试兼容性

## 提交 Pull Request

1. 创建功能分支：
   ```bash
   git checkout -b feature/你的功能名称
   ```

2. 提交更改：
   ```bash
   git add .
   git commit -m "feat: 添加新功能描述"
   ```

3. 推送到你的 fork：
   ```bash
   git push origin feature/你的功能名称
   ```

4. 在 GitHub 上创建 Pull Request

### Pull Request 检查清单
- [ ] 代码遵循项目规范
- [ ] 添加了必要的测试
- [ ] 更新了相关文档
- [ ] 代码通过所有测试
- [ ] 提交信息清晰明确

## 报告问题

### Bug 报告
使用 [Bug 报告模板](.github/ISSUE_TEMPLATE/bug_report.md) 报告问题，包括：
- 问题描述
- 重现步骤
- 环境信息
- 错误日志（如果有）

### 功能请求
使用 [功能请求模板](.github/ISSUE_TEMPLATE/feature_request.md) 提出新功能建议，包括：
- 功能描述
- 使用场景
- 预期效果

## 许可证

通过提交代码，你同意你的贡献将在 MIT 许可证下发布。

## 联系方式

如有问题，可以通过以下方式联系：
- 创建 Issue
- 发送邮件到：linxzee@example.com

感谢你的贡献！🎉