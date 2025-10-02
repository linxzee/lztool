@echo off
echo ========================================
echo   梨子电脑工具箱 - 构建脚本
echo ========================================
echo.

REM 检查是否安装了 .NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 未检测到 .NET SDK，请先安装 .NET 8.0 SDK
    echo 下载地址: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo 检测到 .NET SDK 版本:
dotnet --version
echo.

REM 设置变量
set CONFIGURATION=Release
set RUNTIME=win-x64
set OUTPUT_DIR=发布包
set VERSION=1.0.0

echo 构建配置:
echo   配置: %CONFIGURATION%
echo   运行时: %RUNTIME%
echo   输出目录: %OUTPUT_DIR%
echo   版本: %VERSION%
echo.

REM 清理之前的构建
echo [1/5] 清理之前的构建...
if exist "bin\%CONFIGURATION%" (
    rmdir /s /q "bin\%CONFIGURATION%"
)
if exist "obj\%CONFIGURATION%" (
    rmdir /s /q "obj\%CONFIGURATION%"
)
if exist "%OUTPUT_DIR%" (
    rmdir /s /q "%OUTPUT_DIR%"
)

REM 恢复 NuGet 包
echo [2/5] 恢复 NuGet 包...
dotnet restore
if %errorlevel% neq 0 (
    echo 错误: NuGet 包恢复失败
    pause
    exit /b 1
)

REM 构建项目
echo [3/5] 构建项目...
dotnet build -c %CONFIGURATION% --no-restore
if %errorlevel% neq 0 (
    echo 错误: 项目构建失败
    pause
    exit /b 1
)

REM 发布为独立应用程序
echo [4/5] 发布为独立应用程序...
dotnet publish -c %CONFIGURATION% -r %RUNTIME% --self-contained true -p:PublishSingleFile=true -o "%OUTPUT_DIR%"
if %errorlevel% neq 0 (
    echo 错误: 发布失败
    pause
    exit /b 1
)

REM 复制必要的资源文件
echo [5/5] 复制资源文件...
if not exist "%OUTPUT_DIR%\img" mkdir "%OUTPUT_DIR%\img"
if not exist "%OUTPUT_DIR%\tools" mkdir "%OUTPUT_DIR%\tools"

REM 复制图标文件
if exist "img\toollogo.ico" (
    copy "img\toollogo.ico" "%OUTPUT_DIR%\img\"
)
if exist "img\toollogo.png" (
    copy "img\toollogo.png" "%OUTPUT_DIR%\img\"
)

REM 复制工具文件
if exist "tools" (
    echo 正在复制工具文件...
    xcopy "tools" "%OUTPUT_DIR%\tools\" /E /I /Y /H
    if %errorlevel% equ 0 (
        echo 工具文件复制成功
    ) else (
        echo 警告: 工具文件复制可能不完整
    )
) else (
    echo 警告: tools 文件夹不存在
)

REM 创建版本信息文件
echo 创建版本信息...
echo 梨子电脑工具箱 %VERSION% > "%OUTPUT_DIR%\版本信息.txt"
echo 构建时间: %date% %time% >> "%OUTPUT_DIR%\版本信息.txt"
echo 目标平台: %RUNTIME% >> "%OUTPUT_DIR%\版本信息.txt"
echo 构建类型: 独立应用程序 >> "%OUTPUT_DIR%\版本信息.txt"
echo. >> "%OUTPUT_DIR%\版本信息.txt"
echo 使用说明: >> "%OUTPUT_DIR%\版本信息.txt"
echo 1. 双击 DesktopApp.exe 启动程序 >> "%OUTPUT_DIR%\版本信息.txt"
echo 2. 确保 tools 文件夹中的所有工具文件都存在 >> "%OUTPUT_DIR%\版本信息.txt"
echo 3. 程序会自动检测硬件信息并管理工具 >> "%OUTPUT_DIR%\版本信息.txt"

echo.
echo ========================================
echo   构建完成！
echo ========================================
echo.
echo 发布文件位于: %OUTPUT_DIR%\
echo 主要文件:
echo   - DesktopApp.exe (主程序)
echo   - img\ (图标资源)
echo   - tools\ (工具文件)
echo   - 版本信息.txt
echo.
echo 使用方法:
echo   1. 将整个 %OUTPUT_DIR% 文件夹复制到目标电脑
echo   2. 双击 DesktopApp.exe 即可运行
echo   3. 无需安装 .NET 运行时
echo.
echo 文件大小信息:
for %%F in ("%OUTPUT_DIR%\DesktopApp.exe") do (
    for /f "tokens=3" %%A in ('dir "%%F" ^| find "%%~nxF"') do (
        echo   主程序: %%A
    )
)
echo.
pause