namespace DesktopApp.Models
{
    public class ToolInfo
    {
        public string Name { get; set; } = "";
        public string ExecutablePath { get; set; } = "";

        public ToolInfo(string name, string executablePath)
        {
            Name = name;
            ExecutablePath = executablePath;
        }
    }
}