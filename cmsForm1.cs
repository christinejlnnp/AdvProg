using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace julie
{
    public partial class cmsForm1 : Form
    {
        private System.Windows.Forms.TextBox textBox4;
        private ContextMenuStrip eventContextMenu;
        private Label selectedLabel;
        private DateTime currentTargetDate = DateTime.Now;

        public cmsForm1()
        {
            InitializeComponent();
            InitializeEventContextMenu();

            panel1.AutoScroll = true;
            panel1.Controls.Add(tableLayoutPanel1);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.MinimumSize = new Size(0, 1200);

            try
            {
                this.pictureBox1.Image = Image.FromFile(@"C:\Users\julie\source\repos\christinejlnnp\AdvProg\account.png");
                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch { }

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "Day", "Week", "Month", "Year", "Schedule" });
            comboBox1.SelectedIndex = 1;

            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, tableLayoutPanel1, new object[] { true });

            this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }

        private void InitializeEventContextMenu()
        {
            eventContextMenu = new ContextMenuStrip();
            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit Event");
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete Event");
            editItem.Click += EditEvent_Click;
            deleteItem.Click += DeleteEvent_Click;
            eventContextMenu.Items.Add(editItem);
            eventContextMenu.Items.Add(deleteItem);
        }

        private void EventLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                selectedLabel = sender as Label;
                eventContextMenu.Show(selectedLabel, e.Location);
            }
        }

        private void EditEvent_Click(object sender, EventArgs e)
        {
            if (selectedLabel != null && selectedLabel.Tag != null)
            {
                string fullDetails = selectedLabel.Tag.ToString();
                string[] parts = fullDetails.Split('|');
                if (parts.Length > 1)
                {
                    textBoxDate.Text = parts[0].Trim();
                    string[] timeAndTitle = parts[1].Split(':');
                    textBoxTime.Text = (timeAndTitle[0] + ":" + timeAndTitle[1]).Trim();
                    textBox1.Text = timeAndTitle.Last().Trim();
                    DeleteEvent_Click(sender, e);
                    MessageBox.Show("Details loaded. Modify and click 'Add' to save changes.");
                }
            }
        }

        private void DeleteEvent_Click(object sender, EventArgs e)
        {
            if (selectedLabel != null)
            {
                listBox1.Items.Remove(selectedLabel.Tag.ToString());
                RefreshCurrentView();
            }
        }

        private void RefreshCurrentView()
        {
            RefreshGridToDate(currentTargetDate);
        }

        private void SetupDayView(DateTime date)
        {
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85f));
            tableLayoutPanel1.RowCount = 24;

            string[] hours = { "12 am", "1 am", "2 am", "3 am", "4 am", "5 am", "6 am", "7 am", "8 am", "9 am", "10 am", "11 am", "12 pm", "1 pm", "2 pm", "3 pm", "4 pm", "5 pm", "6 pm", "7 pm", "8 pm", "9 pm", "10 pm", "11 pm" };
            string dateMatch = date.ToString("MM/dd/yyyy");

            for (int i = 0; i < 24; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
                tableLayoutPanel1.Controls.Add(new Label { Text = hours[i], TextAlign = ContentAlignment.TopRight, Dock = DockStyle.Fill, ForeColor = Color.Gray }, 0, i);

                foreach (var item in listBox1.Items)
                {
                    string entry = item.ToString();
                    if (entry.StartsWith(dateMatch))
                    {
                        string[] parts = entry.Split('|');
                        if (parts.Length > 1)
                        {
                            string timePart = parts[1].Split(':')[0].Trim() + ":" + parts[1].Split(':')[1].Substring(0, 5).Trim();
                            if (DateTime.TryParse(timePart, out DateTime t) && t.Hour == i)
                            {
                                Label lbl = CreateEventLabel(entry.Split(':').Last().Trim(), entry);
                                tableLayoutPanel1.Controls.Add(lbl, 1, i);
                            }
                        }
                    }
                }
            }
        }

        private Label CreateEventLabel(string text, string tag)
        {
            Label lbl = new Label
            {
                Text = text,
                BackColor = Color.LightSkyBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = tag
            };
            lbl.Click += EventLabel_Click;
            lbl.MouseDown += EventLabel_MouseDown;
            return lbl;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCurrentView();
        }

        private void RefreshGridToDate(DateTime targetDate)
        {
            if (tableLayoutPanel1 == null) return;
            currentTargetDate = targetDate;
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            string view = comboBox1.SelectedItem.ToString();
            if (view == "Day") SetupDayView(targetDate);
            else if (view == "Week") SetupWeekView(targetDate);
            else if (view == "Month") SetupMonthView(targetDate);
            else if (view == "Year") SetupYearView(targetDate.Year);

            tableLayoutPanel1.ResumeLayout();
        }

        private void SetupWeekView(DateTime date)
        {
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            for (int i = 0; i < 7; i++) tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.85f));
            tableLayoutPanel1.RowCount = 24;

            string[] hours = { "12 am", "1 am", "2 am", "3 am", "4 am", "5 am", "6 am", "7 am", "8 am", "9 am", "10 am", "11 am", "12 pm", "1 pm", "2 pm", "3 pm", "4 pm", "5 pm", "6 pm", "7 pm", "8 pm", "9 pm", "10 pm", "11 pm" };
            for (int i = 0; i < 24; i++)
                tableLayoutPanel1.Controls.Add(new Label { Text = hours[i], TextAlign = ContentAlignment.TopRight, Dock = DockStyle.Fill, ForeColor = Color.Gray }, 0, i);

            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            foreach (var item in listBox1.Items)
            {
                string entry = item.ToString();
                string[] parts = entry.Split('|');
                if (parts.Length > 1 && DateTime.TryParse(parts[0].Trim(), out DateTime d))
                {
                    if (d >= startOfWeek && d < endOfWeek)
                    {
                        string timePart = parts[1].Split(':')[0].Trim() + ":" + parts[1].Split(':')[1].Substring(0, 5).Trim();
                        if (DateTime.TryParse(timePart, out DateTime t))
                        {
                            Label lbl = CreateEventLabel(entry.Split(':').Last().Trim(), entry);
                            tableLayoutPanel1.Controls.Add(lbl, (int)d.DayOfWeek + 1, t.Hour);
                        }
                    }
                }
            }
        }

        private void SetupMonthView(DateTime date)
        {
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.RowCount = 6;
            for (int i = 0; i < 7; i++) tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28f));
            for (int i = 0; i < 6; i++) tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66f));

            DateTime firstDay = new DateTime(date.Year, date.Month, 1);
            int startColumn = (int)firstDay.DayOfWeek;

            for (int day = 1; day <= DateTime.DaysInMonth(date.Year, date.Month); day++)
            {
                int col = (startColumn + day - 1) % 7;
                int row = (startColumn + day - 1) / 7;

                Panel dayBox = new Panel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
                dayBox.Controls.Add(new Label { Text = day.ToString(), Dock = DockStyle.Top, Height = 20, Font = new Font("Segoe UI", 8, FontStyle.Bold) });

                // FIX: Standardize match string to look exactly like the ListBox format (MM/dd/yyyy)
                DateTime currentLoopDate = new DateTime(date.Year, date.Month, day);
                string dateMatch = currentLoopDate.ToString("MM/dd/yyyy");

                foreach (var item in listBox1.Items)
                {
                    string entry = item.ToString();
                    if (entry.StartsWith(dateMatch))
                    {
                        Label ev = new Label
                        {
                            Text = "• " + entry.Split(':').Last().Trim(),
                            Dock = DockStyle.Top,
                            ForeColor = Color.MediumPurple,
                            AutoSize = true,
                            Tag = entry // Important for search
                        };
                        ev.MouseDown += EventLabel_MouseDown;
                        dayBox.Controls.Add(ev);
                    }
                }
                tableLayoutPanel1.Controls.Add(dayBox, col, row);
            }
        }

        private void SetupYearView(int year)
        {
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.RowCount = 4;
            for (int i = 0; i < 3; i++) tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            for (int i = 0; i < 4; i++) tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            for (int m = 1; m <= 12; m++)
                tableLayoutPanel1.Controls.Add(new MonthCalendar { SelectionStart = new DateTime(year, m, 1), MaxSelectionCount = 1 });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.TryParse(textBoxDate.Text, out DateTime evDate) && DateTime.TryParse(textBoxTime.Text, out DateTime evTime))
            {
                string entry = $"{evDate:MM/dd/yyyy} | {evTime:hh:mm tt}: {textBox1.Text}";
                listBox1.Items.Add(entry);
                RefreshCurrentView();
                btnClear_Click(sender, e);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = textBox3.Text.ToLower().Trim();
                if (string.IsNullOrEmpty(searchTerm)) return;

                string foundEntry = "";
                foreach (var item in listBox1.Items)
                {
                    if (item.ToString().ToLower().Contains(searchTerm))
                    {
                        foundEntry = item.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(foundEntry))
                {
                    string dateStr = foundEntry.Split('|')[0].Trim();
                    if (DateTime.TryParse(dateStr, out DateTime targetDate))
                    {
                        // Update view to the correct month/date
                        RefreshGridToDate(targetDate);

                        // Highlight the item in the refreshed grid
                        foreach (Control ctrl in tableLayoutPanel1.Controls)
                        {
                            if (ctrl is Label lbl && lbl.Tag != null && lbl.Tag.ToString().ToLower().Contains(searchTerm))
                            {
                                lbl.BackColor = Color.Yellow;
                                panel1.ScrollControlIntoView(lbl);
                            }
                            if (ctrl is Panel p)
                            {
                                foreach (Control sub in p.Controls)
                                {
                                    if (sub is Label sl && sl.Tag != null && sl.Tag.ToString().ToLower().Contains(searchTerm))
                                    {
                                        sl.BackColor = Color.Yellow;
                                        panel1.ScrollControlIntoView(p);
                                    }
                                }
                            }
                        }
                    }
                }
                else { MessageBox.Show("Event not found."); }
                e.SuppressKeyPress = true;
            }
        }

        // --- PRESERVED METHODS ---
        private void EventLabel_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { UpdateGridHeaders(DateTime.Now); }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            textBoxDate.Text = e.Start.ToString("MM/dd/yyyy");
            // Automatically refresh grid when user clicks a new date on the calendar
            RefreshGridToDate(e.Start);
        }

        private void UpdateGridHeaders(DateTime selectedDate) { }
        private void pictureBox1_Click_2(object sender, EventArgs e) { contextMenuStrip1.Show(pictureBox1, new Point(0, pictureBox1.Height)); }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) { if (MessageBox.Show("Logout?", "Exit", MessageBoxButtons.YesNo) == DialogResult.Yes) this.Close(); }
        private void btnClear_Click(object sender, EventArgs e) { textBox1.Clear(); textBox2.Clear(); textBoxTime.Clear(); textBox1.Focus(); }

        // --- EMPTY HANDLERS (PRESERVED) ---
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void lblEventsSelectedDate_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void lblSun_Click(object sender, EventArgs e) { }
        private void lblMon_Click(object sender, EventArgs e) { }
        private void lbl11pm_Click(object sender, EventArgs e) { }
        private void lbl10pm_Click(object sender, EventArgs e) { }
        private void lblTues_Click(object sender, EventArgs e) { }
        private void lblWed_Click(object sender, EventArgs e) { }
        private void lblThurs_Click(object sender, EventArgs e) { }
        private void lblFri_Click(object sender, EventArgs e) { }
        private void lblSat_Click(object sender, EventArgs e) { }
        private void lbl12am_Click(object sender, EventArgs e) { }
        private void lbl1am_Click(object sender, EventArgs e) { }
        private void lbl3am_Click(object sender, EventArgs e) { }
        private void lbl2am_Click(object sender, EventArgs e) { }
        private void lbl4am_Click(object sender, EventArgs e) { }
        private void lbl5am_Click(object sender, EventArgs e) { }
        private void lbl6am_Click(object sender, EventArgs e) { }
        private void lbl7am_Click(object sender, EventArgs e) { }
        private void lbl8am_Click(object sender, EventArgs e) { }
        private void lbl9am_Click(object sender, EventArgs e) { }
        private void lbl10am_Click(object sender, EventArgs e) { }
        private void lbl11am_Click(object sender, EventArgs e) { }
        private void lbl12pm_Click(object sender, EventArgs e) { }
        private void lbl1pm_Click(object sender, EventArgs e) { }
        private void lbl2pm_Click(object sender, EventArgs e) { }
        private void lbl3pm_Click(object sender, EventArgs e) { }
        private void lbl4pm_Click(object sender, EventArgs e) { }
        private void lbl5pm_Click(object sender, EventArgs e) { }
        private void lbl6pm_Click(object sender, EventArgs e) { }
        private void lbl7pm_Click(object sender, EventArgs e) { }
        private void lbl9pm_Click(object sender, EventArgs e) { }
        private void lbl8pm_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBoxDate_TextChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) { }
        private void cmsForm1_Load(object sender, EventArgs e) { }
        private void SetupWeekView() { SetupWeekView(DateTime.Now); }
    }
}