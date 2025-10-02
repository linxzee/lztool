using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using DesktopApp.Models;

namespace DesktopApp.Services
{
    public class HardwareInfoService
    {
        public string GetComputerModel()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return $"{obj["Manufacturer"]} {obj["Model"]}";
                    }
                }
            }
            catch
            {
                return "未知型号";
            }
            return "未知型号";
        }

        public string GetSystemBasicInfo()
        {
            try
            {
                var osVersion = Environment.OSVersion;
                var is64Bit = Environment.Is64BitOperatingSystem ? "64位" : "32位";
                var windowsVersion = GetWindowsVersionName(osVersion);
                
                return $"{windowsVersion} {is64Bit}";
            }
            catch
            {
                return $"{Environment.OSVersion} ({(Environment.Is64BitOperatingSystem ? "64位" : "32位")})";
            }
        }

        public string GetHardwareInfo()
        {
            var sb = new StringBuilder();
            
            try
            {
                // 处理器
                sb.AppendLine($"处理器：{GetProcessorInfo()}");
                sb.AppendLine();

                // 主板
                sb.AppendLine($"主板：{GetMotherboardInfo()}");
                sb.AppendLine();

                // 内存总量
                sb.AppendLine($"内存总量：{GetMemoryTotalInfo()}");
                sb.AppendLine();

                // 显卡
                sb.AppendLine($"显卡：{GetGraphicsCardInfo()}");
                sb.AppendLine();

                // 显示器
                sb.AppendLine($"显示器：{GetDisplayInfo()}");
                sb.AppendLine();

                // 磁盘
                sb.AppendLine($"磁盘：{GetDiskSummaryInfo()}");
                sb.AppendLine();

                // 声卡
                sb.AppendLine($"声卡：{GetSoundCardInfo()}");
                sb.AppendLine();

                // 网卡
                sb.AppendLine($"网卡：{GetNetworkCardInfo()}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"获取硬件信息时出错：{ex.Message}");
            }

            return sb.ToString();
        }

        private string GetProcessorInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var processorName = obj["Name"]?.ToString()?.Trim() ?? "未知处理器";
                        var coreCount = obj["NumberOfCores"]?.ToString() ?? "未知";
                        return $"{processorName} ({coreCount}核)";
                    }
                }
            }
            catch
            {
                return "未知处理器";
            }
            return "未知处理器";
        }

        private string GetMotherboardInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return $"{obj["Manufacturer"]} {obj["Product"]}";
                    }
                }
            }
            catch
            {
                return "未知主板";
            }
            return "未知主板";
        }

        private string GetMemoryTotalInfo()
        {
            try
            {
                var sb = new StringBuilder();
                var memoryModules = new List<MemoryModule>();
                
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var module = new MemoryModule
                        {
                            Manufacturer = obj["Manufacturer"]?.ToString() ?? "未知品牌",
                            Capacity = Convert.ToDouble(obj["Capacity"]),
                            Speed = obj["Speed"]?.ToString() ?? "未知",
                            PartNumber = obj["PartNumber"]?.ToString() ?? ""
                        };
                        memoryModules.Add(module);
                    }
                }

                if (memoryModules.Count > 0)
                {
                    var totalCapacity = memoryModules.Sum(m => m.Capacity);
                    var brands = memoryModules.Select(m => m.Manufacturer).Distinct();
                    var speeds = memoryModules.Select(m => m.Speed).Distinct();
                    
                    sb.Append($"{string.Join("/", brands)} ");
                    sb.Append($"{Math.Round(totalCapacity / 1024 / 1024 / 1024, 1)}GB ");
                    sb.Append($"{string.Join("/", speeds)}MHz ");
                    
                    if (memoryModules.Count > 1)
                    {
                        var moduleSizes = memoryModules.Select(m => $"{Math.Round(m.Capacity / 1024 / 1024 / 1024, 1)}GB");
                        sb.Append($"({string.Join("+", moduleSizes)}组合)");
                    }
                }
                else
                {
                    // 备用方法获取总内存
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            var totalMemory = Convert.ToDouble(obj["TotalPhysicalMemory"]);
                            sb.Append($"{Math.Round(totalMemory / 1024 / 1024 / 1024, 1)}GB");
                        }
                    }
                }
                
                return sb.ToString();
            }
            catch
            {
                return "未知内存";
            }
        }

        private string GetGraphicsCardInfo()
        {
            try
            {
                var graphicsCards = new List<string>();
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var name = obj["Name"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(name) && !name.Contains("Microsoft"))
                        {
                            graphicsCards.Add(name);
                        }
                    }
                }
                return graphicsCards.Count > 0 ? string.Join("\r\n", graphicsCards) : "集成显卡";
            }
            catch
            {
                return "未知显卡";
            }
        }

        private string GetDisplayInfo()
        {
            try
            {
                var sb = new StringBuilder();
                var displays = new List<string>();
                
                // 获取显示器信息
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var name = obj["Name"]?.ToString() ?? "未知品牌";
                        var screenWidth = obj["ScreenWidth"]?.ToString();
                        var screenHeight = obj["ScreenHeight"]?.ToString();
                        
                        var displayInfo = $"{name}";
                        if (!string.IsNullOrEmpty(screenWidth) && !string.IsNullOrEmpty(screenHeight))
                        {
                            displayInfo += $" {screenWidth}x{screenHeight}";
                        }
                        displays.Add(displayInfo);
                    }
                }

                // 如果WMI获取不到，使用Screen类获取基本信息
                if (displays.Count == 0)
                {
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        var refreshRate = GetDisplayRefreshRate(screen);
                        var displayInfo = $"显示器 {screen.Bounds.Width}x{screen.Bounds.Height}";
                        if (refreshRate > 0)
                        {
                            displayInfo += $" {refreshRate}Hz";
                        }
                        displays.Add(displayInfo);
                    }
                }

                return string.Join(", ", displays);
            }
            catch
            {
                var primaryScreen = Screen.PrimaryScreen;
                if (primaryScreen != null)
                {
                    return $"{primaryScreen.Bounds.Width}x{primaryScreen.Bounds.Height}";
                }
                return "未知分辨率";
            }
        }

        private int GetDisplayRefreshRate(Screen screen)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var refreshRate = obj["CurrentRefreshRate"]?.ToString();
                        if (!string.IsNullOrEmpty(refreshRate) && int.TryParse(refreshRate, out int rate) && rate > 0)
                        {
                            return rate;
                        }
                    }
                }
            }
            catch
            {
                // 忽略错误
            }
            return 0;
        }

        private string GetDiskSummaryInfo()
        {
            try
            {
                var sb = new StringBuilder();
                var disks = new List<string>();
                
                // 获取物理磁盘信息
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var model = obj["Model"]?.ToString()?.Trim() ?? "未知型号";
                        var size = Convert.ToDouble(obj["Size"]);
                        var diskInfo = $"{model} {Math.Round(size / 1024 / 1024 / 1024, 1)}GB";
                        disks.Add(diskInfo);
                    }
                }

                if (disks.Count > 0)
                {
                    sb.Append(string.Join("\r\n", disks));
                }
                else
                {
                    // 备用方法：获取逻辑磁盘信息
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType=3"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            var driveLetter = obj["DeviceID"]?.ToString() ?? "未知";
                            var size = Convert.ToDouble(obj["Size"]);
                            var diskInfo = $"{driveLetter} {Math.Round(size / 1024 / 1024 / 1024, 1)}GB";
                            disks.Add(diskInfo);
                        }
                    }
                    sb.Append(string.Join("\r\n", disks));
                }
                
                return sb.ToString();
            }
            catch
            {
                return "未知磁盘";
            }
        }

        private string GetSoundCardInfo()
        {
            try
            {
                var soundCards = new List<string>();
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var name = obj["Name"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(name))
                        {
                            soundCards.Add(name);
                        }
                    }
                }
                return soundCards.Count > 0 ? string.Join("\r\n", soundCards) : "集成声卡";
            }
            catch
            {
                return "集成声卡";
            }
        }

        private string GetNetworkCardInfo()
        {
            try
            {
                var networkCards = new List<string>();
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var name = obj["Name"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(name))
                        {
                            networkCards.Add(name);
                        }
                    }
                }
                return networkCards.Count > 0 ? string.Join("\r\n", networkCards) : "未知网卡";
            }
            catch
            {
                return "未知网卡";
            }
        }

        private string GetWindowsVersionName(OperatingSystem osVersion)
        {
            var version = osVersion.Version;
            
            // Windows 11
            if (version.Major == 10 && version.Build >= 22000)
                return $"Windows 11 {version.Build}";
            
            // Windows 10
            if (version.Major == 10 && version.Build >= 10240)
                return $"Windows 10 {version.Build}";
            
            // Windows 8.1
            if (version.Major == 6 && version.Minor == 3)
                return "Windows 8.1";
            
            // Windows 8
            if (version.Major == 6 && version.Minor == 2)
                return "Windows 8";
            
            // Windows 7
            if (version.Major == 6 && version.Minor == 1)
                return "Windows 7";
            
            // Windows Vista
            if (version.Major == 6 && version.Minor == 0)
                return "Windows Vista";
            
            // Windows XP
            if (version.Major == 5 && version.Minor == 1)
                return "Windows XP";
            
            // Windows 2000
            if (version.Major == 5 && version.Minor == 0)
                return "Windows 2000";
            
            // 其他版本
            return $"Windows {version.Major}.{version.Minor} Build {version.Build}";
        }
    }
}