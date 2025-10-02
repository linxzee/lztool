using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DesktopApp.Models;
using DesktopApp.Services;

namespace DesktopApp
{
    public partial class MainForm : Form
    {
        private Panel? leftPanel;
        private Panel? rightPanel;
        private Button? homeButton;
        private Button? settingsButton;
        private Panel? contentPanel;
        
        private HardwareInfoService? hardwareInfoService;
        private ToolManagerService? toolManagerService;
        private UIBuilderService? uiBuilderService;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeUI();
            ShowHomePage();
        }

        private void InitializeServices()
        {
            hardwareInfoService = new HardwareInfoService();
            toolManagerService = new ToolManagerService();
            uiBuilderService = new UIBuilderService(toolManagerService);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 窗体设置
            this.Text = "梨子电脑工具箱";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            this.MaximumSize = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            
            // 设置窗体图标
            try
            {
                if (System.IO.File.Exists("img\\toollogo.ico"))
                {
                    this.Icon = new Icon("img\\toollogo.ico");
                }
            }
            catch
            {
                // 如果图标加载失败，忽略错误
            }
            
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            // 右侧内容面板 - 先添加，确保在底层
            rightPanel = new Panel();
            rightPanel.BackColor = Color.FromArgb(37, 37, 38);
            rightPanel.Dock = DockStyle.Fill;
            this.Controls.Add(rightPanel);

            // 左侧导航面板 - 后添加，确保在顶层
            leftPanel = new Panel();
            leftPanel.BackColor = Color.FromArgb(45, 45, 48);
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 200;
            this.Controls.Add(leftPanel);

            // 导航按钮
            CreateNavigationButtons();
        }

        private void CreateNavigationButtons()
        {
            // 硬件检测按钮
            homeButton = uiBuilderService!.CreateNavButton("硬件检测", 0, leftPanel!);
            homeButton.Click += (s, e) => ShowHomePage();

            // 温度电压按钮
            var tempVoltageButton = uiBuilderService!.CreateNavButton("温度电压", 1, leftPanel!);
            tempVoltageButton.Click += (s, e) => ShowTempVoltagePage();

            // 工具分类按钮
            var cpuToolsButton = uiBuilderService!.CreateNavButton("CPU工具", 2, leftPanel!);
            cpuToolsButton.Click += (s, e) => ShowCpuToolsPage();

            var gpuToolsButton = uiBuilderService!.CreateNavButton("GPU工具", 3, leftPanel!);
            gpuToolsButton.Click += (s, e) => ShowGpuToolsPage();

            var diskToolsButton = uiBuilderService!.CreateNavButton("硬盘工具", 4, leftPanel!);
            diskToolsButton.Click += (s, e) => ShowDiskToolsPage();

            var systemToolsButton = uiBuilderService!.CreateNavButton("综合工具", 5, leftPanel!);
            systemToolsButton.Click += (s, e) => ShowSystemToolsPage();

            // 设置按钮
            settingsButton = uiBuilderService!.CreateNavButton("设置", 6, leftPanel!);
            settingsButton.Click += (s, e) => ShowSettingsPage();

            // 为导航按钮添加鼠标悬停效果
            AddNavigationButtonHoverEffects();
        }

        private void AddNavigationButtonHoverEffects()
        {
            foreach (Control control in leftPanel!.Controls)
            {
                if (control is Button button)
                {
                    button.MouseEnter += (s, e) => 
                    {
                        if (button.BackColor != Color.FromArgb(0, 122, 204))
                            button.BackColor = Color.FromArgb(62, 62, 64);
                    };
                    button.MouseLeave += (s, e) => 
                    {
                        if (button.BackColor != Color.FromArgb(0, 122, 204))
                            button.BackColor = Color.FromArgb(45, 45, 48);
                    };
                }
            }
        }

        private void ShowHomePage()
        {
            ClearContentPanel();
            SetActiveButton(homeButton!);

            // 第一格：电脑型号
            var computerModelPanel = uiBuilderService!.CreateInfoPanel("电脑型号", hardwareInfoService!.GetComputerModel(), 20, contentPanel!);
            contentPanel!.Controls.Add(computerModelPanel);

            // 第二格：系统信息
            var systemInfoPanel = uiBuilderService!.CreateInfoPanel("系统信息", hardwareInfoService!.GetSystemBasicInfo(), 80, contentPanel!);
            contentPanel!.Controls.Add(systemInfoPanel);

            // 下面大格：硬件信息
            var hardwarePanel = new Panel();
            hardwarePanel.BackColor = Color.FromArgb(45, 45, 48);
            hardwarePanel.BorderStyle = BorderStyle.FixedSingle;
            hardwarePanel.Location = new Point(20, 140);
            hardwarePanel.Size = new Size(contentPanel!.Width - 40, contentPanel.Height - 160);
            hardwarePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentPanel.Controls.Add(hardwarePanel);

            // 硬件信息文本框
            var hardwareTextBox = new TextBox();
            hardwareTextBox.Multiline = true;
            hardwareTextBox.ScrollBars = ScrollBars.Vertical;
            hardwareTextBox.ReadOnly = true;
            hardwareTextBox.BackColor = Color.FromArgb(45, 45, 48);
            hardwareTextBox.ForeColor = Color.White;
            hardwareTextBox.BorderStyle = BorderStyle.None;
            hardwareTextBox.Font = new Font("Microsoft YaHei", 10, FontStyle.Regular);
            hardwareTextBox.Location = new Point(10, 10);
            hardwareTextBox.Size = new Size(hardwarePanel.Width - 20, hardwarePanel.Height - 20);
            hardwareTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            hardwareTextBox.Text = hardwareInfoService!.GetHardwareInfo();
            hardwarePanel.Controls.Add(hardwareTextBox);
        }

        private void ShowTempVoltagePage()
        {
            ClearContentPanel();
            SetActiveButtonForTools("温度电压");

            var label = new Label();
            label.Text = "温度电压监控";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 30);
            contentPanel!.Controls.Add(label);

            // 第一行：CPU和GPU
            var cpuPanel = CreateTempVoltagePanel("CPU", "温度: 45°C\n电压: 1.2V", 80, 50);
            contentPanel!.Controls.Add(cpuPanel);

            var gpuPanel = CreateTempVoltagePanel("GPU", "温度: 65°C\n电压: 1.1V", 80, 250);
            contentPanel!.Controls.Add(gpuPanel);

            // 第二行：硬盘和内存
            var diskPanel = CreateTempVoltagePanel("硬盘", "温度: 38°C", 150, 50);
            contentPanel!.Controls.Add(diskPanel);

            var memoryPanel = CreateTempVoltagePanel("内存", "温度: 42°C\n电压: 1.35V", 150, 250);
            contentPanel!.Controls.Add(memoryPanel);

            // 刷新按钮
            var refreshButton = new Button();
            refreshButton.Text = "刷新数据";
            refreshButton.BackColor = Color.FromArgb(0, 122, 204);
            refreshButton.ForeColor = Color.White;
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Size = new Size(120, 40);
            refreshButton.Location = new Point(50, 230);
            refreshButton.Font = new Font("Microsoft YaHei", 10, FontStyle.Regular);
            refreshButton.Click += (s, e) => RefreshTempVoltageData();
            contentPanel!.Controls.Add(refreshButton);
        }

        private Panel CreateTempVoltagePanel(string component, string data, int top, int left)
        {
            var panel = new Panel();
            panel.BackColor = Color.FromArgb(45, 45, 48);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(left, top);
            panel.Size = new Size(200, 60);

            var componentLabel = new Label();
            componentLabel.Text = component;
            componentLabel.ForeColor = Color.FromArgb(0, 122, 204);
            componentLabel.Font = new Font("Microsoft YaHei", 12, FontStyle.Bold);
            componentLabel.AutoSize = true;
            componentLabel.Location = new Point(15, 15);
            panel.Controls.Add(componentLabel);

            var dataLabel = new Label();
            dataLabel.Text = data;
            dataLabel.ForeColor = Color.White;
            dataLabel.Font = new Font("Microsoft YaHei", 10, FontStyle.Regular);
            dataLabel.TextAlign = ContentAlignment.MiddleCenter;
            dataLabel.Size = new Size(120, 40);
            dataLabel.Location = new Point(40, 10);
            panel.Controls.Add(dataLabel);

            return panel;
        }

        private void RefreshTempVoltageData()
        {
            // 这里可以添加实际的温度电压数据获取逻辑
            // 目前使用模拟数据
            MessageBox.Show("温度电压数据已刷新！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowCpuToolsPage()
        {
            ClearContentPanel();
            SetActiveButtonForTools("CPU工具");

            var label = new Label();
            label.Text = "CPU工具";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 30);
            contentPanel!.Controls.Add(label);

            uiBuilderService!.CreateToolGroupButtons(new[]
            {
                new ToolGroup("CPU-Z")
                {
                    Tools = new List<ToolInfo>
                    {
                        new ToolInfo("32位版本", @"tools\CPU\CPUZ\cpuz_x32.exe"),
                        new ToolInfo("64位版本", @"tools\CPU\CPUZ\cpuz_x64.exe")
                    }
                }
            }, 80, contentPanel!);
        }

        private void ShowGpuToolsPage()
        {
            ClearContentPanel();
            SetActiveButtonForTools("GPU工具");

            var label = new Label();
            label.Text = "GPU工具";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 30);
            contentPanel!.Controls.Add(label);

            uiBuilderService!.CreateToolButtons(new[]
            {
                new ToolInfo("GPU-Z", @"tools\GPU\GPUZ\GPU-Z.exe")
            }, 80, contentPanel!);
        }

        private void ShowDiskToolsPage()
        {
            ClearContentPanel();
            SetActiveButtonForTools("硬盘工具");

            var label = new Label();
            label.Text = "硬盘工具";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 30);
            contentPanel!.Controls.Add(label);

            uiBuilderService!.CreateToolGroupButtons(new[]
            {
                new ToolGroup("DiskInfo")
                {
                    Tools = new List<ToolInfo>
                    {
                        new ToolInfo("32位版本", @"tools\Drive\CrystalDiskInfo\DiskInfo32S.exe"),
                        new ToolInfo("64位版本", @"tools\Drive\CrystalDiskInfo\DiskInfo64S.exe")
                    }
                },
                new ToolGroup("DiskMark")
                {
                    Tools = new List<ToolInfo>
                    {
                        new ToolInfo("32位版本", @"tools\Drive\CrystalDiskMark\DiskMark32S.exe"),
                        new ToolInfo("64位版本", @"tools\Drive\CrystalDiskMark\DiskMark64S.exe")
                    }
                }
            }, 80, contentPanel!);

            // 添加SSD-Z和SpaceSniffer工具按钮
            uiBuilderService!.CreateToolButtons(new[]
            {
                new ToolInfo("SSD-Z", @"tools\Drive\SSDZ\SSD-Z.exe"),
                new ToolInfo("SpaceSniffer", @"tools\Drive\SpaceSniffer\SpaceSniffer.exe")
            }, 180, contentPanel!);
        }

        private void ShowSystemToolsPage()
        {
            ClearContentPanel();
            SetActiveButtonForTools("综合工具");

            var label = new Label();
            label.Text = "综合工具";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 30);
            contentPanel!.Controls.Add(label);

            uiBuilderService!.CreateToolButtons(new[]
            {
                new ToolInfo("AIDA64", @"tools\Tool\AIDA64\aida64.exe"),
                new ToolInfo("显示器检测", @"tools\Tool\MonitorTest\MonitorTest64.exe")
            }, 80, contentPanel!);
        }

        private void ShowSettingsPage()
        {
            ClearContentPanel();
            SetActiveButton(settingsButton!);

            var label = new Label();
            label.Text = "设置页面";
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft YaHei", 16, FontStyle.Bold);
            label.AutoSize = true;
            label.Location = new Point(50, 50);
            contentPanel!.Controls.Add(label);

            // 添加设置选项
            var themeLabel = new Label();
            themeLabel.Text = "主题设置:";
            themeLabel.ForeColor = Color.White;
            themeLabel.AutoSize = true;
            themeLabel.Location = new Point(50, 100);
            contentPanel!.Controls.Add(themeLabel);

            var themeCombo = new ComboBox();
            themeCombo.Items.AddRange(new object[] { "深色主题", "浅色主题" });
            themeCombo.SelectedIndex = 0;
            themeCombo.Location = new Point(150, 97);
            themeCombo.Width = 150;
            contentPanel!.Controls.Add(themeCombo);

            // 添加软件信息
            var infoLabel = new Label();
            infoLabel.Text = "软件信息";
            infoLabel.ForeColor = Color.FromArgb(0, 122, 204);
            infoLabel.Font = new Font("Microsoft YaHei", 12, FontStyle.Bold);
            infoLabel.AutoSize = true;
            infoLabel.Location = new Point(50, 150);
            contentPanel!.Controls.Add(infoLabel);

            // 作者信息
            var authorLabel = new Label();
            authorLabel.Text = "作者: linxzee";
            authorLabel.ForeColor = Color.White;
            authorLabel.AutoSize = true;
            authorLabel.Location = new Point(70, 190);
            contentPanel!.Controls.Add(authorLabel);

            // 版本信息
            var versionLabel = new Label();
            versionLabel.Text = "版本: 20251003.1.0.0";
            versionLabel.ForeColor = Color.White;
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(70, 220);
            contentPanel!.Controls.Add(versionLabel);

            // 构建日期
            var buildDateLabel = new Label();
            buildDateLabel.Text = "构建日期: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buildDateLabel.ForeColor = Color.White;
            buildDateLabel.AutoSize = true;
            buildDateLabel.Location = new Point(70, 250);
            contentPanel!.Controls.Add(buildDateLabel);
        }

        private void ClearContentPanel()
        {
            if (contentPanel != null)
            {
                rightPanel!.Controls.Remove(contentPanel);
                contentPanel.Dispose();
            }

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(37, 37, 38);
            rightPanel!.Controls.Add(contentPanel);
        }

        private void SetActiveButton(Button activeButton)
        {
            // 重置所有按钮颜色
            homeButton!.BackColor = Color.FromArgb(45, 45, 48);
            settingsButton!.BackColor = Color.FromArgb(45, 45, 48);

            // 重置所有工具按钮颜色
            foreach (Control control in leftPanel!.Controls)
            {
                if (control is Button button && button != homeButton && button != settingsButton)
                {
                    button.BackColor = Color.FromArgb(45, 45, 48);
                }
            }

            // 设置活动按钮颜色
            activeButton!.BackColor = Color.FromArgb(0, 122, 204);
        }

        private void SetActiveButtonForTools(string buttonText)
        {
            // 重置所有按钮颜色
            homeButton!.BackColor = Color.FromArgb(45, 45, 48);
            settingsButton!.BackColor = Color.FromArgb(45, 45, 48);

            // 重置所有工具按钮颜色
            foreach (Control control in leftPanel!.Controls)
            {
                if (control is Button button && button != homeButton && button != settingsButton)
                {
                    button.BackColor = Color.FromArgb(45, 45, 48);
                }
            }

            // 设置活动工具按钮颜色
            foreach (Control control in leftPanel!.Controls)
            {
                if (control is Button button && button.Text == buttonText)
                {
                    button.BackColor = Color.FromArgb(0, 122, 204);
                    break;
                }
            }
        }
    }
}