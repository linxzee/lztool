using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DesktopApp.Models;

namespace DesktopApp.Services
{
    public class ToolManagerService
    {
        private Dictionary<string, string>? toolDescriptions;

        public ToolManagerService()
        {
            InitializeToolDescriptions();
        }

        private void InitializeToolDescriptions()
        {
            toolDescriptions = new Dictionary<string, string>
            {
                { "CPU-Z", "CPU-Z 是一款免费的系统信息检测工具，专门用于显示电脑处理器（CPU）、主板、内存和显卡等硬件的详细规格。它以精准的硬件检测和轻量级运行著称，广泛应用于硬件爱好者、超频用户和系统诊断领域。" },
                { "GPU-Z", "GPU-Z 是一款用于监测显卡信息的免费软件，它可以展示显卡的型号、制造商、核心频率、显存容量、显存频率等详细信息。GPU-Z可以读取显卡的BIOS信息，同时提供了一些高级功能。" },
                { "DiskInfo", "DiskInfo 是一款免费的硬盘驱动器（HDD）和固态硬盘（SSD）健康监控工具，由日本开发者 hiyohiyo 开发。它为用户提供详细的硬盘信息和健康状态报告。" },
                { "DiskMark", "CrystalDiskMark是一个硬盘基准测试工具，可以测试硬盘的读取和写入速度。它可以显示硬盘的随机读取和写入速度、顺序读取和写入速度以及随机4K读取和写入速度。" },
                { "SSD-Z", "SSD-Z 是一款小巧高可靠性的固态硬盘检测工具，同样也支持 HDD 机械硬盘和其它磁盘设备信息的检测。它通过读取 SSD 的固件信息显示控制器、工艺技术等。" },
                { "SpaceSniffer", "SpaceSniffer 是一款免费的磁盘空间分析工具，采用矩形树状图（Treemap）方式直观展示文件和文件夹的体积占用情况，帮助用户快速识别磁盘中占用空间较大的内容。" },
                { "AIDA64", "AIDA64 是一款专业的系统信息检测工具，提供详细的硬件和软件信息、系统诊断、基准测试和传感器监控功能。" },
                { "显示器检测", "MonitorTest是一个专门用来测试你的计算机屏幕效能的性能测试软件。它提供了有35种不同的测试项目。" }
            };
        }

        public string FormatToolDescription(string description)
        {
            if (string.IsNullOrEmpty(description)) return description;
            
            var result = new StringBuilder();
            var currentLine = new StringBuilder();
            int charCount = 0;
            
            foreach (char c in description)
            {
                currentLine.Append(c);
                charCount++;
                
                // 每20个汉字字符换行（中文字符占用2个位置）
                if (charCount >= 40 || c == '。' || c == '，' || c == '；')
                {
                    result.AppendLine(currentLine.ToString());
                    currentLine.Clear();
                    charCount = 0;
                }
            }
            
            if (currentLine.Length > 0)
            {
                result.Append(currentLine.ToString());
            }
            
            return result.ToString();
        }

        public string GetToolDescription(string toolName)
        {
            if (toolDescriptions?.ContainsKey(toolName) == true)
            {
                return FormatToolDescription(toolDescriptions[toolName]);
            }
            return "";
        }

        public void LaunchTool(string executablePath)
        {
            if (File.Exists(executablePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = executablePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法启动工具: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"工具文件不存在: {executablePath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void CreateToolTip(ToolTip toolTip)
        {
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            toolTip.BackColor = Color.FromArgb(45, 45, 48);
            toolTip.ForeColor = Color.White;
        }
    }
}