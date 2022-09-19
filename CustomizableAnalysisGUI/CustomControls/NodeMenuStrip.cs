using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public class NodeMenuStrip : ContextMenuStrip
    {
        public event Action<string> MenuItemClicked;

        public void SetPaths(IEnumerable<string> paths)
        {
            SuspendLayout();
            Items.Clear();
            var items = CreateNodeMenu(null, paths);
            Items.AddRange(items);
            ResumeLayout();
        }

        private ToolStripItem[] CreateNodeMenu(string root, IEnumerable<string> paths)
        {
            var items = new List<ToolStripItem>();
            var subPaths = new Dictionary<string, List<string>>();

            foreach (var path in paths)
            {
                var index = path.IndexOf("/");
                if (index < 0)
                {
                    var item = new ToolStripMenuItem(path);
                    var fullPath = string.IsNullOrWhiteSpace(root) ? path : $"{root}/{path}";
                    item.Click += (_, __) => MenuItemClicked?.Invoke(fullPath);
                    items.Add(item);
                    continue;
                }

                if (index + 1 >= path.Length) continue;

                var subLabel = path.Substring(0, index);
                var rest = path.Substring(index + 1);
                if (subPaths.ContainsKey(subLabel) == false)
                {
                    subPaths[subLabel] = new List<string>();
                }
                subPaths[subLabel].Add(rest);
            }

            if (items.Count == 0 && subPaths.Count == 0) return new ToolStripItem[] { };
            if (items.Count == 1 && subPaths.Count == 0) return items.ToArray();

            foreach ((var subLabel, var subPathGroup) in subPaths)
            {
                var subItems = CreateNodeMenu(string.IsNullOrWhiteSpace(root) ? subLabel : $"{root}/{subLabel}", subPathGroup);
                if (subItems is null) continue;
                var menu = new ToolStripMenuItem(subLabel);
                menu.DropDownItems.AddRange(subItems);
                items.Add(menu);
            }

            return items.ToArray();
        }
    }
}
