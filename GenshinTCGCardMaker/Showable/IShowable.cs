using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinTCGCardMaker
{
    /// <summary>
    /// 可以作为条目展示在右侧的框架中
    /// </summary>
    public interface IShowable
    {
        public string Name { get; }
        public List<string> TagsA { get; }
        public List<string> TagsB { get; }
        public string Description { get; }
    }
    /// <summary>
    /// Just 4 Test
    /// </summary>
    public class FolderItem(string filename, string path, bool isFolder) : IShowable
    {
        public string Name { get; } = filename;
        public List<string> TagsA { get; } = [isFolder ? "文件夹" : "文件"];
        public List<string> TagsB => ["1火", "2杂"];
        public string Description { get; } = path;
    }
}
