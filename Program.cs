using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace julie
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the message loop with the main form
            Application.Run(new cmsForm1());
        }
    }

    public partial class cmsForm1 : Form
    {

     }
}
