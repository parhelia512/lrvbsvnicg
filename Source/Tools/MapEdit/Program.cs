using System;
using System.Windows.Forms;

namespace MapEdit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread()]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
           

            Application.Run(new MainForm());

        }
    }
}

