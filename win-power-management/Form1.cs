using Microsoft.Win32;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.PropertyGridInternal;

namespace win_power_management
{
    public partial class Form1 : Form
    {
        private readonly IDictionary<string, Guid> _schemes = PowerSchemes.GetAll();
        private const string QUIT = "Quit";

        private IList<ToolStripMenuItem> PowerSchemesMenu { get; set; }

        public Form1()
        {
            InitializeComponent();

            var schemesNames = _schemes.Keys;

            PowerSchemesMenu = schemesNames
                .Select(s => new ToolStripMenuItem(s, null, ItemClick))
                .ToList();

            PowerSchemesMenu
                .Add(new ToolStripMenuItem(QUIT, null, ItemClick));

            contextMenuStrip1.Items.AddRange(PowerSchemesMenu.ToArray());

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "TITLE";
            this.Hide();
            Visible = false;
            SetVisibleCore(false);
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

        private void ItemClick(object? sender, EventArgs e)
        {
            //for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            //    if (i != e.Index) checkedListBox1.SetItemChecked(i, false);
            var checkedList = (ToolStripMenuItem)sender;
            var checkedBox = checkedList.Text as string;

            if (checkedBox == QUIT)
            {
                Application.Exit();
                return;
            }

            PowerSchemes.SetActiveScheme(_schemes[checkedBox]);
            
            PowerSchemesMenu = PowerSchemesMenu
                .Select(t =>
                    {
                        if (t.Text == checkedBox)
                        {
                            return new ToolStripMenuItem(t.Text, Properties.Resources.Power.ToBitmap(), ItemClick);
                        }

                        return new ToolStripMenuItem(t.Text, null, ItemClick);
                    })
                .ToList();

            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.AddRange(PowerSchemesMenu.ToArray());

            this.Hide();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}