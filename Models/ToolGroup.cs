using System.Collections.Generic;

namespace DesktopApp.Models
{
    public class ToolGroup
    {
        public string GroupName { get; set; } = "";
        public List<ToolInfo> Tools { get; set; } = new List<ToolInfo>();

        public ToolGroup(string groupName)
        {
            GroupName = groupName;
        }
    }
}