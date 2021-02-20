using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace VeraContainer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region file Dialogs
        private string fileSelect_dialog()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".vhdx";
            dlg.Filter = "VHDX Files (*.vhdx)|*.vhdx|All Files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and return the value
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                return filename;
            }
            return "No file selected";
        }
        private string saveFile_dialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".vhdx";
            saveFileDialog.Filter = "VHDX Files (*.vhdx)|*.vhdx|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return "No file selected";
        }
        #endregion
        #region VHDx Operations
        private void createVHDx(string filename, int size)
        {
            string vdisk = "create vdisk file=" + "\"" + filename + "\"";
            string vsize = "maximum=" + size + " " + "type=expandable";
            string vdisk_vsize = vdisk + " " + vsize;

            Process p = new Process();                                    // new instance of Process class
            p.StartInfo.UseShellExecute = false;                          // do not start a new shell
            p.StartInfo.RedirectStandardOutput = true;                    // Redirects the on screen results
            p.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";   // executable to run
            p.StartInfo.RedirectStandardInput = true;                     // Redirects the input commands
            p.StartInfo.CreateNoWindow = true;                            // Hide the process Window
            p.Start();                                                    // Starts the process
            p.StandardInput.WriteLine(vdisk_vsize);                       // Issues commands to diskpart
            p.StandardInput.WriteLine("attach vdisk");
            p.StandardInput.WriteLine("create partition primary");
            p.StandardInput.WriteLine("assign");
            p.StandardInput.WriteLine("format fs=ntfs quick");
            p.StandardInput.WriteLine("exit");
            string output = p.StandardOutput.ReadToEnd();                 // Places the output to a variable
            p.WaitForExit();                                              // Waits for the exe to finish
            // Console.WriteLine(output);
        }
        private void createVHDx_fixed(string filename, int size)
        {
            string vdisk = "create vdisk file=" + "\"" + filename + "\"";
            string vsize = "maximum=" + size + " " + "type=fixed";
            string vdisk_vsize = vdisk + " " + vsize;

            Process p = new Process();                                    // new instance of Process class
            p.StartInfo.UseShellExecute = false;                          // do not start a new shell
            p.StartInfo.RedirectStandardOutput = true;                    // Redirects the on screen results
            p.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";   // executable to run
            p.StartInfo.RedirectStandardInput = true;                     // Redirects the input commands
            p.StartInfo.CreateNoWindow = true;                            // Hide the process Window
            p.Start();                                                    // Starts the process
            p.StandardInput.WriteLine(vdisk_vsize);                       // Issues commands to diskpart
            p.StandardInput.WriteLine("attach vdisk");
            p.StandardInput.WriteLine("create partition primary");
            p.StandardInput.WriteLine("assign");
            p.StandardInput.WriteLine("format fs=ntfs quick");
            p.StandardInput.WriteLine("exit");
            string output = p.StandardOutput.ReadToEnd();                 // Places the output to a variable
            p.WaitForExit();                                              // Waits for the exe to finish
            // Console.WriteLine(output);
        }
        private void mountVHDx(string filename, bool read_only)
        {
            string vdisk = "select vdisk file=" + "\"" + filename + "\"";
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(vdisk);
            if (read_only == false) { p.StandardInput.WriteLine("attach vdisk"); }
            else { p.StandardInput.WriteLine("attach vdisk readonly"); }
            p.StandardInput.WriteLine("assign");
            p.StandardInput.WriteLine("exit");
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            // Console.WriteLine(output);
        }
        private void unmountVHDx(string filename)
        {
            string vdisk = "select vdisk file=" + "\"" + filename + "\"";
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(vdisk);
            p.StandardInput.WriteLine("detach vdisk");
            p.StandardInput.WriteLine("exit");
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            // Console.WriteLine(output);
        }
        #endregion
        #region Buttons Hide and Show
        private void allDisable()
         {
                createVHDX_button.IsEnabled = false;
                createVHDX_button_fixed.IsEnabled = false;
                mountVHDX_button.IsEnabled = false;
                mountVHDX_read_button.IsEnabled = false;
                bitlocker_button.IsEnabled = false;
                unmountVHDX_button.IsEnabled = false;
                openFile_button.IsEnabled = false;
                saveFile_button.IsEnabled = false;
                fileSelected.IsEnabled = false;
                fileSize.IsEnabled = false;
          }
        private void allEnable()
        {
            createVHDX_button.IsEnabled = true;
            createVHDX_button_fixed.IsEnabled = true;
            mountVHDX_button.IsEnabled = true;
            mountVHDX_read_button.IsEnabled = true;
            bitlocker_button.IsEnabled = true;
            unmountVHDX_button.IsEnabled = true;
            openFile_button.IsEnabled = true;
            saveFile_button.IsEnabled = true;
            fileSelected.IsEnabled = true;
            fileSize.IsEnabled = true;
        }
        #endregion
        #region Buttons
        private void openFile_Button_Click(object sender, RoutedEventArgs e)
        {
            string filename = fileSelect_dialog();
            fileSelected.Text = filename;
        }

        private void saveFile_button_Click(object sender, RoutedEventArgs e)
        {
            string filename = saveFile_dialog();
            fileSelected.Text = filename;
        }

        private void createVHDX_button_Click(object sender, RoutedEventArgs e)
        {
            string filename = fileSelected.Text;
            if (File.Exists(filename)) { MessageBox.Show("The container is already there", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            allDisable();
            progressBar_1.IsIndeterminate = true;
            int number = 0;
            int size = 0;
            try
            {
                number = Convert.ToInt32(fileSize.Text);
                if (number < 1) { number = 100; combobox_bytes.SelectedIndex = 0; }
                if (combobox_bytes.SelectedIndex == 0)
                {
                    size = number * 1;
                }
                if (combobox_bytes.SelectedIndex == 1)
                {
                    size = number * 1000;
                }
            }
            catch
            {
                size = 100;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    createVHDx(filename, size);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Dispatcher.Invoke(new Action(() => progressBar_1.IsIndeterminate = false));
                    Application.Current.Dispatcher.Invoke(new Action(() => allEnable()));
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                progressBar_1.IsIndeterminate = false;
                allEnable();
            };      
            worker.RunWorkerAsync();
        }

        private void createVHDX_button_fixed_Click(object sender, RoutedEventArgs e)
        {
            string filename = fileSelected.Text;
            if (File.Exists(filename)) { MessageBox.Show("The container is already there", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            allDisable();
            progressBar_1.IsIndeterminate = true;
            int number = 0;
            int size = 0;
            try
            {
                number = Convert.ToInt32(fileSize.Text);
                if (number < 1) { number = 100; combobox_bytes.SelectedIndex = 0; }
                if (combobox_bytes.SelectedIndex == 0)
                {
                    size = number * 1;
                }
                if (combobox_bytes.SelectedIndex == 1)
                {
                    size = number * 1000;
                }
            }
            catch
            {
                size = 100;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    createVHDx_fixed(filename, size);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Dispatcher.Invoke(new Action(() => progressBar_1.IsIndeterminate = false));
                    Application.Current.Dispatcher.Invoke(new Action(() => allEnable()));
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                progressBar_1.IsIndeterminate = false;
                allEnable();
            };
            worker.RunWorkerAsync();
        }

        private void mountVHDX_button_Click(object sender, RoutedEventArgs e)
        {
            allDisable();
            progressBar_1.IsIndeterminate = true;
            string filename = fileSelected.Text;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    mountVHDx(filename, false);
                }
                catch
                {
                    MessageBox.Show("Can not mount the container", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Current.Dispatcher.Invoke(new Action(() => progressBar_1.IsIndeterminate = false));
                    Application.Current.Dispatcher.Invoke(new Action(() => allEnable()));
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                progressBar_1.IsIndeterminate = false;
                allEnable();
            };
            worker.RunWorkerAsync();
        }
        private void mountVHDX_read_button_Click(object sender, RoutedEventArgs e)
        {
            allDisable();
            progressBar_1.IsIndeterminate = true;
            string filename = fileSelected.Text;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    mountVHDx(filename, true);
                }
                catch
                {
                    MessageBox.Show("Can not mount the container", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Current.Dispatcher.Invoke(new Action(() => progressBar_1.IsIndeterminate = false));
                    Application.Current.Dispatcher.Invoke(new Action(() => allEnable()));
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                progressBar_1.IsIndeterminate = false;
                allEnable();
            };
            worker.RunWorkerAsync();
        }

        private void unmountVHDX_button_Click(object sender, RoutedEventArgs e)
        {
            allDisable();
            progressBar_1.IsIndeterminate = true;
            string filename = fileSelected.Text;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    unmountVHDx(filename);
                }
                catch
                {
                    MessageBox.Show("Can not eject the volume" + Environment.NewLine + "Container is in use", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Current.Dispatcher.Invoke(new Action(() => progressBar_1.IsIndeterminate = false));
                    Application.Current.Dispatcher.Invoke(new Action(() => allEnable()));
                }
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                progressBar_1.IsIndeterminate = false;
                allEnable();
            };
            worker.RunWorkerAsync();
        }
        private void bitlocker_button_Click(object sender, RoutedEventArgs e)
        {
            // control /name Microsoft.BitLockerDriveEncryption
            try
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "control";
                p.StartInfo.Arguments = "/name Microsoft.BitLockerDriveEncryption";
                p.Start();
            }
            catch { }
        }
        private void diskManagement_button_Click(object sender, RoutedEventArgs e)
        {
            // diskmgmt.msc
            try
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "diskmgmt.msc";
                p.Start();
            }
            catch { }
        }
        #endregion

    }
}
