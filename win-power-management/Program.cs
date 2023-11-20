using IWshRuntimeLibrary;

namespace win_power_management
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            AddStartup();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        static void AddStartup()
        {
            WshShell wshShell = new WshShell();
            IWshShortcut shortcut;
            string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            shortcut = (IWshShortcut)wshShell
                .CreateShortcut(startUpFolderPath + "\\" +Application.ProductName + ".lnk");

            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = "Launch";
            // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
            shortcut.Save();
        }
    }
}