using Microsoft.Win32;

namespace win_power_management
{
    public partial class Form1 : Form
    {
        private readonly IDictionary<string, Guid> schemes = PowerSchemes.GetAll();

        public Form1()
        {
            InitializeComponent();
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

            var schemesNames = schemes.Keys;
            checkedListBox1.Items.AddRange(schemesNames.ToArray());

            checkedListBox1.CheckOnClick = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "TITLE";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
                if (i != e.Index) checkedListBox1.SetItemChecked(i, false);

            var checkedList = (CheckedListBox)sender;
            var checkedBox = checkedList.SelectedItem as string;

            PowerSchemes.SetActiveScheme(schemes[checkedBox]);
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (SystemInformation.PowerStatus.BatteryChargeStatus)
            {
                case System.Windows.Forms.BatteryChargeStatus.Low:
                    MessageBox.Show("Battery is running low.", "Low Battery", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case System.Windows.Forms.BatteryChargeStatus.Critical:
                    MessageBox.Show("Battery is critcally low.", "Critical Battery", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                default:
                    // Battery is okay.
                    break;
            }
        }
    }
}