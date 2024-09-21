using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GenshinTCGCardMaker
{
    public partial class Maker : Window
    {
        // 用于绑定到ListBox的数据集合
        ObservableCollection<FolderItem> folderContents = new ObservableCollection<FolderItem>();
        public Maker() : this(new("goldchick", "test"), "C:\\Users\\11847\\Documents\\GenshinTCG\\GenshinTCGCardMaker\\bin\\Debug\\net8.0-windows\\genshin3_4")
        {

        }
        public Maker(MakerConfig config, string folder)
        {
            InitializeComponent();
            txtFolderContent.Text = "ModName: ";
            txtFolderContent.Inlines.Add(new Run(config.NameSpace)
            {
                Foreground = new SolidColorBrush(Colors.Green)
            });
            txtFolderContent.Inlines.Add(new Run(" | Made by "));
            txtFolderContent.Inlines.Add(new Run(config.Author)
            {
                Foreground = new SolidColorBrush(Colors.DarkCyan)
            });
            txtFolderContent.Inlines.Add(new Run(" | Version: "));
            txtFolderContent.Inlines.Add(new Run(config.Version.ToString())
            {
                Foreground = new SolidColorBrush(Colors.DeepPink)
            });

            // 将ListBox的ItemsSource绑定到folderContents集合
            listBoxFolderContent.ItemsSource = folderContents;

            DisplayFolderContentsInListNew(folder);
        }
        private void DisplayFolderContentsInListNew(string folderPath)
        {
            try
            {
                // 清空之前的内容
                folderContents.Clear();

                // 获取文件夹中的子文件夹
                string[] directories = Directory.GetDirectories(folderPath);
                foreach (var directory in directories)
                {
                    folderContents.Add(new FolderItem(System.IO.Path.GetFileName(directory), directory, true));
                }

                // 获取文件夹中的文件
                string[] files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    folderContents.Add(new FolderItem(System.IO.Path.GetFileName(file), file, false));
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show($"错误: {ex.Message}");
            }
        }
    }
}
