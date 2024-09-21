using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Entry : Window
    {
        public Entry()
        {
            InitializeComponent();
        }
        private void SwitchWindow(MakerConfig config, string folder)
        {
            Maker newWindow = new(config, folder);
            newWindow.Show();

            Close();
        }
        private void btnCreateMod_Click(object sender, RoutedEventArgs e)
        {
            string modName = txtModName.Text;
            string authorName = txtAuthorName.Text;
            if (!string.IsNullOrEmpty(modName) && !string.IsNullOrEmpty(authorName))
            {
                MakerConfig config = new(authorName, modName);
                MessageBox.Show($"Mod 名: {modName}\n作者名: {authorName}");
            }
        }

        private void btnReadOldMod_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();

            dialog.Multiselect = false;
            dialog.Title = "选择已有mod所在文件夹（config.json所在目录）";

            // Show open folder dialog box
            bool? result = dialog.ShowDialog();

            // Process open folder dialog box results
            if (result == true)
            {
                // Get the selected folder
                string fullPathToFolder = dialog.FolderName;
                //string folderNameOnly = dialog.SafeFolderName;

                //TODO:读取功能

                MakerConfig config = new("goldchick", "test");
             
                SwitchWindow(config, fullPathToFolder);
            }
        }
    }
}
