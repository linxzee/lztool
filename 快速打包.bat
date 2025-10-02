@echo off
chcp 65001 >nul
echo 梨子电脑工具箱 - 快速打包脚本
echo.

REM 检查.NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 请先安装 .NET 8.0 SDK
    pause
    exit /b 1
)

echo 正在打包应用程序...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "发布包"

if %errorlevel% equ 0 (
    echo.
    echo 正在复制工具文件...
    if exist "tools" (
        xcopy "tools" "发布包\tools\" /E /I /Y /H
        if %errorlevel% equ 0 (
            echo ✓ 工具文件复制成功
        ) else (
            echo ⚠ 工具文件复制可能不完整
        )
    ) else (
        echo ⚠ tools 文件夹不存在
    )
    
    echo.
    echo ✓ 打包成功！
    echo.
    echo 发布文件位于: 发布包\
    echo 包含:
    echo   - DesktopApp.exe (主程序)
    echo   - 所有依赖文件
    echo   - tools\ (工具文件夹)
    echo   - img\ (图标资源)
    echo.
    echo 使用方法:
    echo   双击 DesktopApp.exe 即可运行
    echo   无需安装任何依赖
) else (
    echo.
    echo ✗ 打包失败，请检查错误信息
)

echo.
pause