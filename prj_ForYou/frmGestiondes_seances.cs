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

namespace prj_ForYou
{
    public partial class frmGestiondes_seances : Form
    {
        public frmGestiondes_seances()
        {
            InitializeComponent();
            hideAllGbDays();
        }
       
        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void hideAllGbDays()
        {

            gbLundi.Hide();
            gbMardi.Hide();
            gbMercredi.Hide();
            gbJeudi.Hide();
            gbVendredi.Hide();
            gbSamedi.Hide();
            gbDimanche.Hide();
        }

        private void cbLundi_CheckedChanged(object sender, EventArgs e)
        {
           
            if(cbLundi.Checked==true)
            {
                gbLundi.Show();
            }
            else
            {
                gbLundi.Hide();
            }
        }

        private void cbMardi_CheckedChanged(object sender, EventArgs e)
        {
          
            if (cbMardi.Checked == true)
            {
                gbMardi.Show();
            }
            else
            {
                gbMardi.Hide();
            }
        }

        private void cbMercredi_CheckedChanged(object sender, EventArgs e)
        {
           
            if (cbMercredi.Checked == true)
            {
                gbMercredi.Show();
            }
            else
            {
                gbMercredi.Hide();
            }
        }

        private void cbJeudi_CheckedChanged(object sender, EventArgs e)
        {
           
            if (cbJeudi.Checked == true)
            {
                gbJeudi.Show();
            }
            else
            {
                gbJeudi.Hide();
            }
        }

        private void cbVendredi_CheckedChanged(object sender, EventArgs e)
        {
           
            if (cbVendredi.Checked == true)
            {
                gbVendredi.Show();
            }
            else
            {
                gbVendredi.Hide();
            }
        }

        private void cbSamedi_CheckedChanged(object sender, EventArgs e)
        {
           
            if (cbSamedi.Checked == true)
            {
                gbSamedi.Show();
            }
            else
            {
                gbSamedi.Hide();
            }
        }

        private void cbDimanche_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cbDimanche.Checked == true)
            {
                gbDimanche.Show();
            }
            else
            {
                gbDimanche.Hide();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        SqlDataAdapter da_matier = new SqlDataAdapter("select*from matier ", MemberGlobal.cnxstring);
        DataSet Ds = new DataSet();
        SqlDataAdapter da_Annee = new SqlDataAdapter("select*from Annee", MemberGlobal.cnxstring);

        private void frmGestiondes_seances_Load(object sender, EventArgs e)
        {
            da_matier.Fill(Ds, "m");
            cmbmatier.SelectedIndexChanged -= new EventHandler(cmbmatier_SelectedIndexChanged);
            cmbmatier.DataSource = Ds.Tables["m"];
            cmbmatier.ValueMember = "idmat";
            cmbmatier.SelectedIndexChanged += new EventHandler(cmbmatier_SelectedIndexChanged);
            Ds.Tables.Add("n");
            Ds.Tables.Add("p");
            Ds.Tables.Add("g");
            da_Annee.Fill(Ds, "a");
            cmbannee.SelectedIndexChanged -= new EventHandler(cmbannee_SelectedIndexChanged);
            cmbannee.DataSource = Ds.Tables["a"];
            cmbannee.ValueMember = "annee";
            cmbannee.SelectedIndexChanged += new EventHandler(cmbannee_SelectedIndexChanged);

        }
        SqlDataAdapter da_Niv = new SqlDataAdapter("", MemberGlobal.cnxstring);
        SqlDataAdapter da_prof = new SqlDataAdapter("", MemberGlobal.cnxstring);
        private void cmbmatier_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ds.Tables["n"].Clear();
            da_Niv.SelectCommand.CommandText = "select*from niveauMat where #idmat= " + "'" + cmbmatier.Text + "'";
            da_Niv.Fill(Ds, "n");

            if (Ds.Tables["n"].Rows.Count != 0)
            {



                cmbniveau.SelectedIndexChanged -= new EventHandler(cmbniveau_SelectedIndexChanged);
                cmbniveau.DataSource = Ds.Tables["n"];
                cmbniveau.ValueMember = "nomMat";
                cmbniveau.SelectedIndexChanged += new EventHandler(cmbniveau_SelectedIndexChanged);
            }
            Ds.Tables["p"].Clear();
            da_prof.SelectCommand.CommandText = "select*from prof  where #idmat= " + "'" + cmbmatier.Text + "'";
            da_prof.Fill(Ds, "p");
            if (Ds.Tables["p"].Rows.Count != 0)
            {
                cmbnomprof.SelectedIndexChanged -= new EventHandler(cmbnomprof_SelectedIndexChanged);
                cmbnomprof.DataSource = Ds.Tables["p"];
                cmbnomprof.ValueMember = "nomprof";
                cmbnomprof.SelectedIndexChanged += new EventHandler(cmbnomprof_SelectedIndexChanged);

            }
        }
        SqlDataAdapter da_grp = new SqlDataAdapter("", MemberGlobal.cnxstring);

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ds.Tables["g"].Clear();
            da_grp.SelectCommand.CommandText = string.Format("select distinct codegrp from grp inner join Raff on codegrp=#codegrp where #idmat='{0}'and #codeNiv='{1}' and Raff.#nomprof='{2}' and  Raff.annee='{3}' ", cmbmatier.Text, cmbniveau.Text,cmbnomprof.Text,cmbannee.Text); /*"select*from grp where #idmat=' "+cmbmatier.Text+"'and #codeNiv='"+cmbniveau.Text+"'";*/

            da_grp.Fill(Ds, "g");

            if (Ds.Tables["g"].Rows.Count != 0)
            {

                cmbnomgrp.SelectedIndexChanged -= new EventHandler(cmbnomgrp_SelectedIndexChanged);
                cmbnomgrp.DataSource = Ds.Tables["g"];
                cmbnomgrp.ValueMember = "codegrp";
                cmbnomgrp.SelectedIndexChanged += new EventHandler(cmbnomgrp_SelectedIndexChanged);
            }
        }

        private void cmbnomprof_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ds.Tables["g"].Clear();
            da_grp.SelectCommand.CommandText = string.Format("select distinct codegrp from grp inner join Raff on codegrp=#codegrp where #idmat='{0}'and #codeNiv='{1}' and Raff.#nomprof='{2}' and  Raff.annee='{3}' ", cmbmatier.Text, cmbniveau.Text, cmbnomprof.Text, cmbannee.Text); 

            da_grp.Fill(Ds, "g");

            if (Ds.Tables["g"].Rows.Count != 0)
            {

                cmbnomgrp.SelectedIndexChanged -= new EventHandler(cmbnomgrp_SelectedIndexChanged);
                cmbnomgrp.DataSource = Ds.Tables["g"];
                cmbnomgrp.ValueMember = "codegrp";
                cmbnomgrp.SelectedIndexChanged += new EventHandler(cmbnomgrp_SelectedIndexChanged);
            }
        }

        private void cmbnomgrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(string.Format(" select count(#nom) from Raff where #nomprof='{0}'and #codegrp='{1}' and annee='{2}'", cmbnomprof.Text, cmbnomgrp.Text, cmbannee.Text));
            lblnombrEleve.Text = dt.Rows[0][0].ToString();
        }

        private void cmbannee_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ds.Tables["g"].Clear();
            da_grp.SelectCommand.CommandText = string.Format("select distinct codegrp from grp inner join Raff on codegrp=#codegrp where #idmat='{0}'and #codeNiv='{1}' and Raff.#nomprof='{2}' and  Raff.annee='{3}' ", cmbmatier.Text, cmbniveau.Text, cmbnomprof.Text, cmbannee.Text); /*"select*from grp where #idmat=' "+cmbmatier.Text+"'and #codeNiv='"+cmbniveau.Text+"'";*/

            da_grp.Fill(Ds, "g");

            if (Ds.Tables["g"].Rows.Count != 0)
            {

                cmbnomgrp.SelectedIndexChanged -= new EventHandler(cmbnomgrp_SelectedIndexChanged);
                cmbnomgrp.DataSource = Ds.Tables["g"];
                cmbnomgrp.ValueMember = "codegrp";
                cmbnomgrp.SelectedIndexChanged += new EventHandler(cmbnomgrp_SelectedIndexChanged);
            }
        }


        List<string> lstQuery = new List<string>();
        bool i;
        List<string> lstdaysDelete = new List<string>();
        int cmptInsert = 0;
        //int cmptCb = 0;


        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (cmbannee.Text != "" && cmbmatier.Text != "" && cmbniveau.Text != "" && cmbnomgrp.Text != "" && cmbnomprof.Text != "")
            {
                if (cbLundi.Checked == true)
                {

                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbLundi.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string lundi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbLundi.Text, mtxtHDLundi.Text, mtxtHFLundi.Text);
                        lstQuery.Add(lundi);
                        lstdaysDelete.Add(cbLundi.Text);

                    }

                }

                if (cbMardi.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbMardi.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string mardi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbMardi.Text, mtxtHDMardi.Text, mtxtHFMardi.Text);
                        lstQuery.Add(mardi);
                        lstdaysDelete.Add(cbMardi.Text);

                    }

                }

                if (cbMercredi.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbMercredi.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string mercredi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbMercredi.Text, mtxtHDMercredi.Text, mtxtHFMercredi.Text);
                        lstQuery.Add(mercredi);
                        lstdaysDelete.Add(cbMercredi.Text);

                    }

                }

                if (cbJeudi.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbJeudi.Text));
                    if (dt.Rows.Count == 0)
                    {
                        string jeudi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbJeudi.Text, mtxtHDJeudi.Text, mtxtHFJeudi.Text);
                        lstQuery.Add(jeudi);
                        lstdaysDelete.Add(cbJeudi.Text);

                    }

                }

                if (cbVendredi.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbVendredi.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string vendredi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbVendredi.Text, mtxtHDVendredi.Text, mtxtHFVendredi.Text);
                        lstQuery.Add(vendredi);
                        lstdaysDelete.Add(cbVendredi.Text);

                    }

                }

                if (cbSamedi.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbSamedi.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string samedi = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbSamedi.Text, mtxtHDSamedi.Text, mtxtHFSamedi.Text);
                        lstQuery.Add(samedi);
                        lstdaysDelete.Add(cbSamedi.Text);

                    }

                }

                if (cbDimanche.Checked == true)
                {
                    DataTable dt = MemberGlobal.rechercher(string.Format("select * from seance where #codegrp='{0}'and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbDimanche.Text));
                    if (dt.Rows.Count == 0)
                    {

                        string dimanche = string.Format("insert into seance values('{0}',{1},'{2}','{3}','{4}','{5}')", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, cbDimanche.Text, mtxtHDimanche.Text, mtxtHFDimanche.Text);
                        lstQuery.Add(dimanche);
                        lstdaysDelete.Add(cbDimanche.Text);

                    }

                }

                //foreach(Control cb in this.Controls )
                //{
                //    if(cb is CheckBox)
                //    {
                //        if (((CheckBox)cb).Checked == true)
                //        {
                //            cmptCb++;
                //            lstdaysDelete.Add(cb.Text);

                //        }
                //    }

                //}


                foreach (string s in lstQuery)
                {
                    i = false;
                   


                    i = MemberGlobal.Insert_Edit_Delete(s);

                    if (i == true)
                    {
                        cmptInsert++;

                    }


                }
               

                if (lstdaysDelete.Count  == cmptInsert && i == true)
                {
                    
                    MemberGlobal.messageBox(new frmMssageboxSucces(), "l'emploi du temps été ajouter avec succès");
                    lstdaysDelete.Clear();
                    lstQuery.Clear();
                    cmptInsert = 0;

                }
                else
                {
                    foreach (string s in lstdaysDelete)
                    {

                        MemberGlobal.Insert_Edit_Delete(string.Format("delete from seance where #codegrp='{0}' and #annee='{1}' and #nomprof='{2}' and dayy='{3}' ", cmbnomgrp.Text, cmbannee.Text, cmbnomprof.Text, s));
                    }
                   
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "Echoué d'ajouter l'emploi a ce groupe");
                    lstdaysDelete.Clear();
                    lstQuery.Clear();
                    cmptInsert = 0;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnG_Click(object sender, EventArgs e)
        {
            frmModif_Delete_Seance f = new frmModif_Delete_Seance();
            f.Show();
        }
    }
}
