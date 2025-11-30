using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;

namespace ShrimpCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class FileItem
    {
        public string Name { get; set; }
        public FileInfo Info { get; set; }
        public ObservableCollection<FileItem> Children { get; set; }

        public FileItem(string name, FileInfo info)
        {
            Name = name;
            Info = info;
            Children = new ObservableCollection<FileItem>();
        }
    }

    public class FileInfo
    {
        public bool IsFolder { get; set; }
        public string Path { get; set; }

        public FileInfo(bool isFolder, string path)
        {
            IsFolder = isFolder;
            Path = path;
        }
    }

    public partial class MainWindow : Window
    {
        string filePath = "";
        ArrayList omegaPath = new ArrayList();

        public MainWindow()
        {
            InitializeComponent();
            updateTitle();
        }

        private void getFilePath()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".cpp";
            dlg.Filter = "C++ (.cpp)|*.cpp| Python (.py)|*.py| Text (.txt)|*.txt| Moai (.🗿)|*.🗿";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filePath = dlg.FileName;
                updateTitle();
            }
        }

        private void fileBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            if (btn?.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            getFilePath();
            if (filePath != "")
            {
                fileNameTbox.Text = File.ReadAllText(filePath);
            }
        }

        private void setFile(string path)
        {
            filePath = path;
            updateTitle();
            if (filePath != "")
            {
                fileNameTbox.Text = File.ReadAllText(filePath);
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void save()
        {
            if (filePath != "")
            {
                File.WriteAllText(filePath, fileNameTbox.Text);
            }
        }

        private void keyDown(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                if (filePath != "")
                {
                    save();
                }
                else
                {
                    saveAs();
                }
            }
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            saveAs();
        }

        private void saveAs()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".cpp";
            dlg.Filter = "C++ (.cpp)|*.cpp| Python (.py)|*.py| Text (.txt)|*.txt| Moai (.🗿)|*.🗿";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(dlg.FileName, fileNameTbox.Text);
                filePath = dlg.FileName;
                updateTitle();
            }
            
        }

        private void deleteA()
        {
            string path = getFilePath("a.exe");
            File.Delete(path);
            System.Windows.MessageBox.Show(path);
        }

        private void newBtn_Click(object sender, RoutedEventArgs eventArgs)
        {
            savingBox(null);
        }

        private void savingBox(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Would you like to save the fruits of ur hard work, mate?", "Wanna save?", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (filePath != "")
                    {
                        save();
                    }
                    else
                    {
                        saveAs();
                    }
                    fileNameTbox.Text = "";
                    filePath = "";
                    updateTitle();
                    break;
                case MessageBoxResult.No:
                    fileNameTbox.Text = "";
                    filePath = "";
                    updateTitle();
                    break;
                case MessageBoxResult.Cancel:
                    if (e != null)
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (GetFileType(filePath))
            {
                case "cpp":
                    runCpp();
                    break;
                case "py":
                    runPython();
                    break;
                case "":
                    System.Windows.MessageBox.Show("No file selected.");
                    break;
                default:
                    System.Windows.MessageBox.Show("Running not supported for this file type.");
                    break;
            }
        }

        private void debugBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (GetFileType(filePath))
            {
                case "cpp":
                    debugCpp();
                    break;
                case "":
                    System.Windows.MessageBox.Show("No file selected.");
                    break;
                default:
                    System.Windows.MessageBox.Show("Debugging not supported for this file type.");
                    break;
            }
        }

        private void runCpp()
        {
            deleteA();
            var process = new Process();
            var process1 = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "C:\\WINDOWS\\system32\\cmd.exe",
                Arguments = ("/C  g++ " + filePath)
            };
            var startInfo1 = new ProcessStartInfo
            {
                FileName = "C:\\WINDOWS\\system32\\cmd.exe",
                Arguments = ("/K a.exe")
            };

            process.StartInfo = startInfo;
            process1.StartInfo = startInfo1;
            process.Start();
            process.WaitForExit();
            process1.Start();
        }

        private void debugCpp()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "C:\\WINDOWS\\system32\\cmd.exe",
                Arguments = ("/K  g++ " + filePath)
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        private void runPython()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "C:\\WINDOWS\\system32\\cmd.exe",
                Arguments = ("/K python " + filePath)
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        void updateTitle()
        {
            if (filePath != "")
            {
                fileBtn.Content = GetFileName(filePath);
            }
            else
            {
                fileBtn.Content = "No file selected";
            }
        }

        public string GetFileName(string fullPath)
        {
            int lastSlashIndex = fullPath.LastIndexOf('\\');
            return fullPath.Substring(lastSlashIndex + 1);
        }

        public string GetFileType(string fullPath)
        {
            int lastDotIndex = fullPath.LastIndexOf('.');
            return fullPath.Substring(lastDotIndex + 1);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            savingBox(e);
        }

        private void DarkSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            double t = DarkSlider.Value;

            BrushConverter bc = new BrushConverter();

            System.Drawing.Color background = System.Drawing.Color.FromArgb(255 - (int)t, 255 - (int)t, 255 - (int)t);
            string hexBg = background.R.ToString("X2") + background.G.ToString("X2") + background.B.ToString("X2");

            System.Drawing.Color foreground = System.Drawing.Color.FromArgb((int)t, (int)t, (int)t);
            string hexFg = foreground.R.ToString("X2") + foreground.G.ToString("X2") + foreground.B.ToString("X2");

            System.Windows.Media.Brush brushBg = (System.Windows.Media.Brush)bc.ConvertFrom("#" + hexBg);
            System.Windows.Media.Brush brushFg = (System.Windows.Media.Brush)bc.ConvertFrom("#" + hexFg);

            Okno.Background = brushBg;
            Okno.Foreground = brushFg;
            FilesTreeView.Background = brushBg;
            FilesTreeView.Foreground = brushFg;
            fileNameTbox.Background = brushBg;
            fileNameTbox.Foreground = brushFg;

        }

        private void ChooseFld()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string folderPath = dialog.SelectedPath;
                    LoadFiles(folderPath);
                    OpenFolderView();
                }
            }
        }

        private void OpenFolderView()
        {
            FilesTreeView.Width = 200;
            fileNameTbox.Margin = new Thickness(210, 27, 0, 0);
        }

        private void LoadFiles(string folderPath)
        {
            try
            {
                FileItem rootItem = new FileItem(System.IO.Path.GetFileName(folderPath), new FileInfo(true, System.IO.Path.GetFullPath(folderPath)));

                AddFolderContent(rootItem, folderPath);

                FilesTreeView.Items.Clear();
                FilesTreeView.Items.Add(rootItem);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error reading folder:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FldButtonClick(object sender, RoutedEventArgs e)
        {
            omegaPath.Clear();
            ChooseFld();
        }

        private void AddFolderContent(FileItem folderItem, string folderPath)
        {
            try
            {
                string[] subfolders = Directory.GetDirectories(folderPath);
                string[] files = Directory.GetFiles(folderPath);

                foreach (var file in files)
                {
                    omegaPath.Add(System.IO.Path.GetFullPath(file));
                    folderItem.Children.Add(new FileItem(System.IO.Path.GetFileName(file), new FileInfo(false, System.IO.Path.GetFullPath(file))));
                }

                foreach (var subfolder in subfolders)
                {
                    omegaPath.Add(System.IO.Path.GetFullPath(subfolder));
                    FileItem subfolderItem = new FileItem(System.IO.Path.GetFileName(subfolder), new FileInfo(true, System.IO.Path.GetFullPath(subfolder)));
                    folderItem.Children.Add(subfolderItem);

                    AddFolderContent(subfolderItem, subfolder);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading subfolders/files:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FurnaceOpen(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            FileInfo tag = (FileInfo)btn.Tag;
            if (tag != null)
            {
                if (tag.IsFolder)
                {
                    var parent = FindParent<TreeViewItem>(btn); ;
                    if(parent != null)
                    {
                        parent.IsExpanded = !parent.IsExpanded;
                    }
                }
                else
                {
                    savingBox(null);
                    setFile(tag.Path);
                }
                
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;

                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }

        private static string getFilePath(string fileName)
        {
            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            return System.IO.Path.Combine(Directory.GetParent(binPath).FullName, fileName);
        }
    }
}