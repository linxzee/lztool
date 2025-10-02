using System.Drawing;
using System.Windows.Forms;
using DesktopApp.Models;
using DesktopApp.Services;

namespace DesktopApp.Services
{
    public class UIBuilderService
    {
        private ToolManagerService toolManager;

        public UIBuilderService(ToolManagerService toolManager)
        {
            this.toolManager = toolManager;
        }

        public Button CreateNavButton(string text, int index, Panel leftPanel)
        {
            var button = new Button();
            button.Text = text;
            button.BackColor = Color.FromArgb(45, 45, 48);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Height = 50;
            button.Width = leftPanel.Width - 10;
            button.Location = new Point(5, 20 + index * 60);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(20, 0, 0, 0);
            button.Font = new Font("Microsoft YaHei", 10, FontStyle.Regular);
            
            leftPanel.Controls.Add(button);
            return button;
        }

        public Panel CreateInfoPanel(string title, string content, int top, Panel contentPanel)
        {
            var panel = new Panel();
            panel.BackColor = Color.FromArgb(45, 45, 48);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(20, top);
            panel.Size = new Size(contentPanel.Width - 40, 50);

            var titleLabel = new Label();
            titleLabel.Text = title + ":";
            titleLabel.ForeColor = Color.LightGray;
            titleLabel.Font = new Font("Microsoft YaHei", 9, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(10, 15);
            panel.Controls.Add(titleLabel);

            var contentLabel = new Label();
            contentLabel.Text = content;
            contentLabel.ForeColor = Color.White;
            contentLabel.Font = new Font("Microsoft YaHei", 9, FontStyle.Regular);
            contentLabel.AutoSize = true;
            contentLabel.Location = new Point(80, 15);
            panel.Controls.Add(contentLabel);

            return panel;
        }

        public void CreateToolButtons(ToolInfo[] tools, int startTop, Panel contentPanel)
        {
            int buttonTop = startTop;
            
            // 创建ToolTip控件
            var toolTip = new ToolTip();
            toolManager.CreateToolTip(toolTip);
            
            for (int i = 0; i < tools.Length; i++)
            {
                var tool = tools[i];
                var toolButton = new Button();
                toolButton.Text = tool.Name;
                toolButton.BackColor = Color.FromArgb(62, 62, 64);
                toolButton.ForeColor = Color.White;
                toolButton.FlatStyle = FlatStyle.Flat;
                toolButton.FlatAppearance.BorderSize = 0;
                toolButton.Size = new Size(150, 35);
                toolButton.Font = new Font("Microsoft YaHei", 9, FontStyle.Regular);
                toolButton.Tag = tool.ExecutablePath;
                
                // 计算位置：三行显示，保持对齐
                int buttonLeft;
                int column = i % 3; // 0, 1, 2 对应三列
                switch (column)
                {
                    case 0:
                        buttonLeft = 50;   // 第一列
                        break;
                    case 1:
                        buttonLeft = 220;  // 第二列
                        break;
                    case 2:
                        buttonLeft = 390;  // 第三列
                        break;
                    default:
                        buttonLeft = 50;
                        break;
                }
                
                // 计算行高
                int row = i / 3;
                buttonTop = startTop + row * 45;
                
                toolButton.Location = new Point(buttonLeft, buttonTop);
                
                // 设置工具提示（自动换行）
                var description = toolManager.GetToolDescription(tool.Name);
                if (!string.IsNullOrEmpty(description))
                {
                    toolTip.SetToolTip(toolButton, description);
                }
                
                toolButton.Click += (s, e) =>
                {
                    var path = (toolButton.Tag as string) ?? "";
                    toolManager.LaunchTool(path);
                };

                // 鼠标悬停效果
                toolButton.MouseEnter += (s, e) =>
                {
                    toolButton.BackColor = Color.FromArgb(82, 82, 84);
                };
                toolButton.MouseLeave += (s, e) =>
                {
                    toolButton.BackColor = Color.FromArgb(62, 62, 64);
                };

                contentPanel.Controls.Add(toolButton);
            }
        }

        public void CreateToolGroupButtons(ToolGroup[] toolGroups, int startTop, Panel contentPanel)
        {
            int buttonTop = startTop;
            
            // 创建ToolTip控件
            var toolTip = new ToolTip();
            toolManager.CreateToolTip(toolTip);
            
            for (int i = 0; i < toolGroups.Length; i++)
            {
                var group = toolGroups[i];
                // 创建分组按钮
                var groupButton = new Button();
                groupButton.Text = group.GroupName;
                groupButton.BackColor = Color.FromArgb(62, 62, 64);
                groupButton.ForeColor = Color.White;
                groupButton.FlatStyle = FlatStyle.Flat;
                groupButton.FlatAppearance.BorderSize = 0;
                groupButton.Size = new Size(150, 35);
                groupButton.Font = new Font("Microsoft YaHei", 9, FontStyle.Regular);
                
                // 计算位置：三行显示，保持对齐
                int buttonLeft;
                int column = i % 3; // 0, 1, 2 对应三列
                switch (column)
                {
                    case 0:
                        buttonLeft = 50;   // 第一列
                        break;
                    case 1:
                        buttonLeft = 220;  // 第二列
                        break;
                    case 2:
                        buttonLeft = 390;  // 第三列
                        break;
                    default:
                        buttonLeft = 50;
                        break;
                }
                
                // 计算行高
                int row = i / 3;
                buttonTop = startTop + row * 45;
                
                groupButton.Location = new Point(buttonLeft, buttonTop);
                
                // 设置工具提示（自动换行）
                var description = toolManager.GetToolDescription(group.GroupName);
                if (!string.IsNullOrEmpty(description))
                {
                    toolTip.SetToolTip(groupButton, description);
                }
                
                // 创建上下文菜单
                var contextMenu = new ContextMenuStrip();
                foreach (var tool in group.Tools)
                {
                    var menuItem = new ToolStripMenuItem(tool.Name);
                    menuItem.Tag = tool.ExecutablePath;
                    menuItem.Click += (s, e) =>
                    {
                        var path = (menuItem.Tag as string) ?? "";
                        toolManager.LaunchTool(path);
                    };
                    contextMenu.Items.Add(menuItem);
                }
                
                groupButton.Click += (s, e) =>
                {
                    contextMenu.Show(groupButton, new Point(0, groupButton.Height));
                };

                // 鼠标悬停效果
                groupButton.MouseEnter += (s, e) =>
                {
                    groupButton.BackColor = Color.FromArgb(82, 82, 84);
                };
                groupButton.MouseLeave += (s, e) =>
                {
                    groupButton.BackColor = Color.FromArgb(62, 62, 64);
                };

                contentPanel.Controls.Add(groupButton);
            }
        }

        public void CreateToolCategory(Panel parentPanel, string categoryName, int top, ToolInfo[] tools)
        {
            // 分类标题
            var categoryLabel = new Label();
            categoryLabel.Text = categoryName;
            categoryLabel.ForeColor = Color.FromArgb(0, 122, 204);
            categoryLabel.Font = new Font("Microsoft YaHei", 12, FontStyle.Bold);
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(20, top);
            parentPanel.Controls.Add(categoryLabel);

            // 工具按钮
            int buttonTop = top + 30;
            foreach (var tool in tools)
            {
                var toolButton = new Button();
                toolButton.Text = tool.Name;
                toolButton.BackColor = Color.FromArgb(62, 62, 64);
                toolButton.ForeColor = Color.White;
                toolButton.FlatStyle = FlatStyle.Flat;
                toolButton.FlatAppearance.BorderSize = 0;
                toolButton.Size = new Size(180, 35);
                toolButton.Location = new Point(20, buttonTop);
                toolButton.Font = new Font("Microsoft YaHei", 9, FontStyle.Regular);
                toolButton.Tag = tool.ExecutablePath;
                
                toolButton.Click += (s, e) =>
                {
                    var path = (toolButton.Tag as string) ?? "";
                    toolManager.LaunchTool(path);
                };

                // 鼠标悬停效果
                toolButton.MouseEnter += (s, e) =>
                {
                    toolButton.BackColor = Color.FromArgb(82, 82, 84);
                };
                toolButton.MouseLeave += (s, e) =>
                {
                    toolButton.BackColor = Color.FromArgb(62, 62, 64);
                };

                parentPanel.Controls.Add(toolButton);
                buttonTop += 45;
            }
        }
    }
}