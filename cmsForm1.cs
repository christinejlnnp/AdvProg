using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace julie
{
    public partial class cmsForm1 : Form
    {
        private System.Windows.Forms.TextBox textBox4;

        public cmsForm1()
        {
            InitializeComponent();

            textBox4 = this.Controls.Find("textBoxTime", true).FirstOrDefault() as TextBox;

            try
            {
                this.pictureBox1.Image = Image.FromFile(@"C:\Users\julie\source\repos\christinejlnnp\AdvProg\account.png");
                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch { /* Handle missing file if necessary */ }
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "Day", "Week", "Month", "Year", "Schedule" });
            comboBox1.SelectedIndex = 1; // Default to Week

            // 3. Fix Flickering (Double Buffering)
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, tableLayoutPanel1, new object[] { true });

            // 4. Force Grid Lines to be Visible
            this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Set initial dates on load
            UpdateGridHeaders(DateTime.Now);
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Automatically fills your Date textbox when you click the calendar
            textBoxDate.Text = e.Start.ToString("MM/dd/yyyy");
        }
        private void UpdateGridHeaders(DateTime selectedDate)
        {
            // Find the Sunday of the selected week
            int diff = (7 + (selectedDate.DayOfWeek - DayOfWeek.Sunday)) % 7;
            DateTime startOfWeek = selectedDate.AddDays(-1 * diff);

            // Update Labels with Day + Date
            lblSun.Text = "Sun\n" + startOfWeek.ToString("MM/dd");
            lblMon.Text = "Mon\n" + startOfWeek.AddDays(1).ToString("MM/dd");
            lblTues.Text = "Tue\n" + startOfWeek.AddDays(2).ToString("MM/dd");
            lblWed.Text = "Wed\n" + startOfWeek.AddDays(3).ToString("MM/dd");
            lblThurs.Text = "Thu\n" + startOfWeek.AddDays(4).ToString("MM/dd");
            lblFri.Text = "Fri\n" + startOfWeek.AddDays(5).ToString("MM/dd");
            lblSat.Text = "Sat\n" + startOfWeek.AddDays(6).ToString("MM/dd");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableLayoutPanel1 == null) return;

            string selectedView = comboBox1.SelectedItem.ToString();
            tableLayoutPanel1.SuspendLayout();

            // Reset Columns to Standard Width
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles[i].SizeType = SizeType.Percent;
                tableLayoutPanel1.ColumnStyles[i].Width = (i == 0) ? 10f : 12.8f;
            }

            // Reset Rows to Standard Height
            for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles[i].SizeType = SizeType.Absolute;
                tableLayoutPanel1.RowStyles[i].Height = 25f;
            }

            if (selectedView == "Day")
            {
                // Hide all days except Column 1
                for (int i = 2; i < 8; i++) tableLayoutPanel1.ColumnStyles[i].Width = 0;
                tableLayoutPanel1.ColumnStyles[1].Width = 90f;
            }
            else if (selectedView == "Month" || selectedView == "Year")
            {
                // Hide the hour rows to show only day headers
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                    tableLayoutPanel1.RowStyles[i].Height = 0;
            }

            tableLayoutPanel1.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eventTitle = textBox1.Text;
            if (string.IsNullOrWhiteSpace(eventTitle))
            {
                MessageBox.Show("Please enter an event title.");
                return;
            }

            // Use the real textbox name
            var timeBox = textBoxTime; // ensure designer control name used
            if (timeBox == null)
            {
                MessageBox.Show("Internal error: time textbox not found.");
                return;
            }

            if (!DateTime.TryParse(textBoxDate.Text, out DateTime eventDate))
            {
                MessageBox.Show("Invalid date. Use MM/dd/yyyy.");
                return;
            }

            DateTime eventTime = DateTime.Parse(textBoxTime.Text);

            int column = (int)eventDate.DayOfWeek + 1;
            int row = eventTime.Hour + 1;

            var eventLabel = new Label
            {
                Text = eventTitle,
                BackColor = Color.LightSkyBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            tableLayoutPanel1.Controls.Add(eventLabel, column, row);
            listBox1.Items.Add($"{eventTime:hh:mm tt}: {eventTitle}");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void lblEventsSelectedDate_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void lblSun_Click(object sender, EventArgs e)
        {

        }

        private void lblMon_Click(object sender, EventArgs e)
        {

        }

        private void lbl11pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl10pm_Click(object sender, EventArgs e)
        {

        }

        private void lblTues_Click(object sender, EventArgs e)
        {

        }

        private void lblWed_Click(object sender, EventArgs e)
        {

        }

        private void lblThurs_Click(object sender, EventArgs e)
        {

        }

        private void lblFri_Click(object sender, EventArgs e)
        {

        }

        private void lblSat_Click(object sender, EventArgs e)
        {

        }

        private void lbl12am_Click(object sender, EventArgs e)
        {

        }

        private void lbl1am_Click(object sender, EventArgs e)
        {

        }

        private void lbl3am_Click(object sender, EventArgs e)
        {

        }

        private void lbl2am_Click(object sender, EventArgs e)
        {

        }

        private void lbl4am_Click(object sender, EventArgs e)
        {

        }

        private void lbl5am_Click(object sender, EventArgs e)
        {

        }

        private void lbl6am_Click(object sender, EventArgs e)
        {

        }

        private void lbl7am_Click(object sender, EventArgs e)
        {

        }

        private void lbl8am_Click(object sender, EventArgs e)
        {

        }

        private void lbl9am_Click(object sender, EventArgs e)
        {

        }

        private void lbl10am_Click(object sender, EventArgs e)
        {

        }

        private void lbl11am_Click(object sender, EventArgs e)
        {

        }

        private void lbl12pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl1pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl2pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl3pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl4pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl5pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl6pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl7pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl9pm_Click(object sender, EventArgs e)
        {

        }

        private void lbl8pm_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
