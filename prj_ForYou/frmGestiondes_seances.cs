using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

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
        // DataTables stored as instance fields (replaces SqlDataAdapter + DataSet)
        private DataTable dtMatier = new DataTable();
        private DataTable dtAnnee  = new DataTable();
        private DataTable dtNiv    = new DataTable();
        private DataTable dtProf   = new DataTable();
        private DataTable dtGrp    = new DataTable();

        private void frmGestiondes_seances_Load(object sender, EventArgs e)
        {
            dtMatier = MemberGlobal.rechercher("select * from matier");
            if (dtMatier.Rows.Count > 0)
            {
                cmbmatier.SelectedIndexChanged -= new EventHandler(cmbmatier_SelectedIndexChanged);
                cmbmatier.DataSource = dtMatier;
                cmbmatier.ValueMember = "idmat";
                cmbmatier.SelectedIndexChanged += new EventHandler(cmbmatier_SelectedIndexChanged);
            }

            dtAnnee = MemberGlobal.rechercher("select * from Annee");
            if (dtAnnee.Rows.Count > 0)
            {
                cmbannee.SelectedIndexChanged -= new EventHandler(cmbannee_SelectedIndexChanged);
                cmbannee.DataSource = dtAnnee;
                cmbannee.ValueMember = "annee";
                cmbannee.SelectedIndexChanged += new EventHandler(cmbannee_SelectedIndexChanged);
            }
        }
        private void cmbmatier_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtNiv = MemberGlobal.rechercher("select * from niveauMat where #idmat=@idmat",
                new System.Data.SqlClient.SqlParameter("@idmat", cmbmatier.Text));
            if (dtNiv.Rows.Count > 0)
            {
                cmbniveau.SelectedIndexChanged -= new EventHandler(cmbniveau_SelectedIndexChanged);
                cmbniveau.DataSource = dtNiv;
                cmbniveau.ValueMember = "nomMat";
                cmbniveau.SelectedIndexChanged += new EventHandler(cmbniveau_SelectedIndexChanged);
            }

            dtProf = MemberGlobal.rechercher("select * from prof where #idmat=@idmat",
                new System.Data.SqlClient.SqlParameter("@idmat", cmbmatier.Text));
            if (dtProf.Rows.Count > 0)
            {
                cmbnomprof.SelectedIndexChanged -= new EventHandler(cmbnomprof_SelectedIndexChanged);
                cmbnomprof.DataSource = dtProf;
                cmbnomprof.ValueMember = "nomprof";
                cmbnomprof.SelectedIndexChanged += new EventHandler(cmbnomprof_SelectedIndexChanged);
            }
        }
        private void LoadGroupCombo()
        {
            dtGrp = MemberGlobal.rechercher(
                "select distinct codegrp from grp inner join Raff on codegrp=#codegrp " +
                "where #idmat=@idmat and #codeNiv=@codeNiv and Raff.#nomprof=@prof and Raff.annee=@annee",
                new System.Data.SqlClient.SqlParameter("@idmat",   cmbmatier.Text),
                new System.Data.SqlClient.SqlParameter("@codeNiv", cmbniveau.Text),
                new System.Data.SqlClient.SqlParameter("@prof",    cmbnomprof.Text),
                new System.Data.SqlClient.SqlParameter("@annee",   cmbannee.Text));

            if (dtGrp.Rows.Count > 0)
            {
                cmbnomgrp.SelectedIndexChanged -= new EventHandler(cmbnomgrp_SelectedIndexChanged);
                cmbnomgrp.DataSource = dtGrp;
                cmbnomgrp.ValueMember = "codegrp";
                cmbnomgrp.SelectedIndexChanged += new EventHandler(cmbnomgrp_SelectedIndexChanged);
            }
        }

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
        }

        private void cmbnomprof_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
        }

        private void cmbnomgrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select count(#nom) from Raff where #nomprof=@prof and #codegrp=@grp and annee=@annee",
                new System.Data.SqlClient.SqlParameter("@prof",  cmbnomprof.Text),
                new System.Data.SqlClient.SqlParameter("@grp",   cmbnomgrp.Text),
                new System.Data.SqlClient.SqlParameter("@annee", cmbannee.Text));
            if (dt.Rows.Count > 0)
                lblnombrEleve.Text = dt.Rows[0][0].ToString();
        }

        private void cmbannee_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
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
