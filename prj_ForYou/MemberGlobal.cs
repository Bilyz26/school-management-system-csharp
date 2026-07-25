using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Configuration;
using System.Security.Cryptography;

namespace prj_ForYou
{
    public class MemberGlobal
    {
        private static string _sqliteDbFile = "SchoolData.db";

        public static string AppDirectory
        {
            get
            {
                try
                {
                    string asmPath = typeof(MemberGlobal).Assembly.Location;
                    if (!string.IsNullOrEmpty(asmPath))
                    {
                        return Path.GetDirectoryName(asmPath);
                    }
                }
                catch { }
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string cnxstring
        {
            get
            {
                string dbPath = Path.Combine(AppDirectory, _sqliteDbFile);
                return string.Format("Data Source={0};Version=3;", dbPath);
            }
            set
            {
            }
        }

        public static void InitSQLiteDatabase()
        {
            try
            {
                string dbPath = Path.Combine(AppDirectory, _sqliteDbFile);

                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }

                using (SQLiteConnection cnx = new SQLiteConnection(cnxstring))
                {
                    cnx.Open();

                    string[] ddls = new string[]
                    {
                        "CREATE TABLE IF NOT EXISTS Annee (annee INTEGER PRIMARY KEY);",
                        "CREATE TABLE IF NOT EXISTS emp (nomemp TEXT PRIMARY KEY, tele TEXT, fonction TEXT, username TEXT, pw TEXT);",
                        "CREATE TABLE IF NOT EXISTS matier (idmat TEXT PRIMARY KEY, nomMat TEXT);",
                        "CREATE TABLE IF NOT EXISTS prof (nomprof TEXT PRIMARY KEY, teleprof TEXT, \"#idmat\" TEXT);",
                        "CREATE TABLE IF NOT EXISTS niveauMat (codeNiv TEXT PRIMARY KEY, \"#idmat\" TEXT, nomMat TEXT);",
                        "CREATE TABLE IF NOT EXISTS grp (codegrp TEXT PRIMARY KEY, \"#idmat\" TEXT, \"#codeNiv\" TEXT);",
                        "CREATE TABLE IF NOT EXISTS inscStd (\"#cin\" TEXT, qui TEXT, nom TEXT PRIMARY KEY, tele TEXT, frinsc REAL, dateD TEXT);",
                        "CREATE TABLE IF NOT EXISTS Raff (\"#nom\" TEXT, \"#codegrp\" TEXT, annee INTEGER, \"#nomprof\" TEXT, PRIMARY KEY (\"#nom\", \"#nomprof\", \"#codegrp\", annee));",
                        "CREATE TABLE IF NOT EXISTS seance (\"#codegrp\" TEXT, \"#annee\" INTEGER, \"#nomprof\" TEXT, dayy TEXT, heureD TEXT, heureF TEXT, PRIMARY KEY (\"#codegrp\", \"#annee\", \"#nomprof\", dayy));",
                        "CREATE TABLE IF NOT EXISTS pay (\"#nom\" TEXT, \"#codegrp\" TEXT, \"#nomprof\" TEXT, \"#idmat\" TEXT, \"#annee\" INTEGER, \"#codeNiv\" TEXT, datep TEXT, monthp TEXT, prix REAL, PRIMARY KEY (\"#nom\", \"#codegrp\", \"#nomprof\", \"#idmat\", monthp, \"#annee\"));"
                    };

                    foreach (string sql in ddls)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Seed default admin account if empty
                    using (SQLiteCommand checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM emp WHERE username='admin'", cnx))
                    {
                        long count = Convert.ToInt64(checkCmd.ExecuteScalar());
                        if (count == 0)
                        {
                            using (SQLiteCommand seedCmd = new SQLiteCommand("INSERT INTO emp (nomemp, tele, fonction, username, pw) VALUES ('admin', '0600000000', 'Directeur', 'admin', 'admin123')", cnx))
                            {
                                seedCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Seed default academic years if empty
                    using (SQLiteCommand checkAnnee = new SQLiteCommand("SELECT COUNT(*) FROM Annee", cnx))
                    {
                        long count = Convert.ToInt64(checkAnnee.ExecuteScalar());
                        if (count == 0)
                        {
                            using (SQLiteCommand seedAnnee = new SQLiteCommand("INSERT INTO Annee (annee) VALUES (2026), (2025), (2024)", cnx))
                            {
                                seedAnnee.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    File.WriteAllText(Path.Combine(AppDirectory, "sqlite_error.log"), ex.ToString());
                }
                catch { }
            }
        }

        public static void CreateBackupOnExit()
        {
            try
            {
                string dbPath = Path.Combine(AppDirectory, _sqliteDbFile);
                if (!File.Exists(dbPath)) return;

                string backupDir = Path.Combine(AppDirectory, "Backups");
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                string backupFileName = string.Format("SchoolData_Backup_{0:yyyyMMdd_HHmmss}.db", DateTime.Now);
                string backupPath = Path.Combine(backupDir, backupFileName);

                File.Copy(dbPath, backupPath, true);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Quotes any #columnName identifiers in the SQL string so SQLite
        /// treats them as column names instead of parameter placeholders.
        /// e.g.  #idmat  →  "#idmat"    #codegrp  →  "#codegrp"
        /// </summary>
        private static string NormalizeSql(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return sql;
            // Replace unquoted #word with "#word" (SQLite double-quote identifier)
            return System.Text.RegularExpressions.Regex.Replace(
                sql,
                @"(?<![""@:$\w])#(\w+)",
                "\"#$1\""
            );
        }

        public static DataTable rechercher(string query, params object[] parameters)
        {
            query = NormalizeSql(query);
            DataTable dtResult = new DataTable();
            using (SQLiteConnection cnx = new SQLiteConnection(cnxstring))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(query, cnx))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (object p in parameters)
                        {
                            if (p is SQLiteParameter)
                                cmd.Parameters.Add((SQLiteParameter)p);
                            else if (p is SqlParameter)
                            {
                                SqlParameter sp = (SqlParameter)p;
                                cmd.Parameters.Add(new SQLiteParameter(sp.ParameterName, sp.Value ?? DBNull.Value));
                            }
                        }
                    }
                    cnx.Open();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dtResult);
                    }
                }
            }
            return dtResult;
        }

        public static bool Insert_Edit_Delete(string query, params object[] parameters)
        {
            query = NormalizeSql(query);
            using (SQLiteConnection cnx = new SQLiteConnection(cnxstring))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(query, cnx))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (object p in parameters)
                        {
                            if (p is SQLiteParameter)
                                cmd.Parameters.Add((SQLiteParameter)p);
                            else if (p is SqlParameter)
                            {
                                SqlParameter sp = (SqlParameter)p;
                                cmd.Parameters.Add(new SQLiteParameter(sp.ParameterName, sp.Value ?? DBNull.Value));
                            }
                        }
                    }
                    cnx.Open();
                    int n = cmd.ExecuteNonQuery();
                    return n > 0;
                }
            }
        }

        public static void vider(Control parent)
        {
            if (parent == null) return;

            foreach (Control c in parent.Controls)
            {
                if (c is DataGridView)
                {
                    ((DataGridView)c).Columns.Clear();
                }
                else if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                else if (c is ComboBox)
                {
                    ((ComboBox)c).Text = string.Empty;
                }
                else if (c is DateTimePicker)
                {
                    ((DateTimePicker)c).Text = string.Empty;
                }
                else if (c is MaskedTextBox)
                {
                    ((MaskedTextBox)c).Text = string.Empty;
                }

                if (c.HasChildren)
                {
                    vider(c);
                }
            }
        }

        public static void messageBox(Form f, string m)
        {
            if (f == null) return;
            foreach (Control c in f.Controls)
            {
                if (c is Label)
                {
                    ((Label)c).Text = m;
                }
            }
            f.Show();
        }

        public static string HashPassword(string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword)) return string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static DataTable dt = new DataTable();
    }
}
