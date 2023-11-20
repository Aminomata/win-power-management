namespace win_power_management
{
    public partial class Form1 : Form
    {
        private const string QUIT = "Quit";

        private IList<ToolStripMenuItem> PowerSchemesMenu { get; set; } = new List<ToolStripMenuItem>();

        public Form1()
        {
            InitializeComponent();
            SetSchemes();

            contextMenuStrip1.Items.AddRange(PowerSchemesMenu.ToArray());
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "TITLE";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void ItemClick(object? sender, EventArgs e)
        {
            var checkedList = (ToolStripMenuItem)sender;
            var checkedBox = checkedList.Text as string;

            if (checkedBox == QUIT)
            {
                Application.Exit();
                return;
            }

            PowerSchemes.SetActiveScheme(PowerSchemes._schemes[checkedBox]);
            
            this.Hide();

            SetSchemes();
        }

        private string GetSchemeName(Guid schemeGuid)
        {
            return PowerSchemes._schemes.FirstOrDefault(s => s.Value == schemeGuid).Key;
        }

        private void SetSchemes()
        {
            if (PowerSchemesMenu.Count == 0)
            {
                var schemesNames = PowerSchemes._schemes.Keys;

                PowerSchemesMenu = schemesNames
                    .Select(s => new ToolStripMenuItem(s, null, ItemClick))
                    .ToList();

                PowerSchemesMenu
                    .Add(new ToolStripMenuItem(QUIT, null, ItemClick));
            }

            var activeSchemeGuid = PowerSchemes.GetActiveScheme();

            PowerSchemesMenu = PowerSchemesMenu
                .Select(t =>
                {
                    if (t.Text == GetSchemeName(activeSchemeGuid))
                    {
                        return new ToolStripMenuItem(t.Text, Properties.Resources.Power.ToBitmap(), ItemClick);
                    }

                    return new ToolStripMenuItem(t.Text, null, ItemClick);
                })
                .ToList();

            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.AddRange(PowerSchemesMenu.ToArray());
        }
    }
}