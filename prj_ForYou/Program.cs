using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prj_ForYou
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Initialize Embedded SQLite Local Database & Seed Admin
                MemberGlobal.InitSQLiteDatabase();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite_init.log"), ex.ToString());
            }

            // Register automatic backup handler on application exit/shutdown
            Application.ApplicationExit += (s, e) => MemberGlobal.CreateBackupOnExit();

            Application.Run(new frmLogIn());
        }
    }
}
