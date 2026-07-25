using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace prj_ForYou
{
    public class MemberGlobal
    {
        private static string _cnxstring = @"data source =.\sqlexpress;initial catalog=DB_Support_School;integrated security=true";

        public static string cnxstring
        {
            get
            {
                try
                {
                    var settings = ConfigurationManager.ConnectionStrings["prj_ForYou.Properties.Settings.DB_Support_SchoolConnectionString"];
                    if (settings != null && !string.IsNullOrWhiteSpace(settings.ConnectionString))
                    {
                        return settings.ConnectionString;
                    }
                    var defaultSettings = ConfigurationManager.ConnectionStrings["DB_Support_School"];
                    if (defaultSettings != null && !string.IsNullOrWhiteSpace(defaultSettings.ConnectionString))
                    {
                        return defaultSettings.ConnectionString;
                    }
                }
                catch
                {
                    // Fallback if ConfigurationManager is uninitialized
                }
                return _cnxstring;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _cnxstring = value;
                }
            }
        }

        public static DataTable rechercher(string query, params SqlParameter[] parameters)
        {
            DataTable b = new DataTable();
            using (SqlConnection cnx = new SqlConnection(cnxstring))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cnx.Open();
                    using (SqlDataReader dtr = cmd.ExecuteReader())
                    {
                        if (dtr.HasRows)
                        {
                            b.Load(dtr);
                        }
                    }
                }
            }
            return b;
        }

        public static bool Insert_Edit_Delete(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection cnx = new SqlConnection(cnxstring))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
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
