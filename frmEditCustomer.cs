using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class frmEditCustomer : Form
    {
        long idCustomer = 0;
        public frmEditCustomer(long cidCustomer)
        {
            InitializeComponent();
            this.idCustomer = cidCustomer;

            this.SetTheme();
            this.btnSave.Enabled = false;
        }
        public void SetTheme()
        {
            this.BackColor = Globals.gDialogBackgroundColor;
            this.groupBox2.BackColor = Globals.gDialogBackgroundColor;
            this.pictBoxCustomer.BackColor = Globals.gDialogBackgroundColor;
        }
        public long customerid
        {
            get { return this.idCustomer; }
            set { this.idCustomer = value; }
        }
        private void txtCustomerEmail_KeyDown(object sender, KeyEventArgs e)
        {
            this.btnSave.Enabled = true; ;
            if (e.KeyCode == Keys.Enter)
            {
                if (this.GetNextControl(ActiveControl, true) != null)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; // PUT THE DING OFF
                    this.GetNextControl(ActiveControl, true).Focus();
                }
            }
        }

        private void frmEditCustomer_Load(object sender, EventArgs e)
        {
            this.RefreshData();
            this.btnSave.Enabled = false;
            this.txtCustomerEmail.Focus();
        }
        private void RefreshData()
        {
            String cname = "", cemail = "", cadd = "", ccon = "";

            // load photo here
            this.GetCustomerPhotofromField();

            this.GetProfile(this.idCustomer, ref cname, ref cemail, ref cadd, ref ccon);

            this.txtCustomerName.Text = cname;
            this.txtCustomerEmail.Text = cemail;
            this.txtCustomerAddress.Text = cadd;
            this.txtCustomerContactNo.Text = ccon;

            this.btnSave.Enabled = false;
        }
        private void GetProfile(long cidcustomer,
            ref String ccustomername, ref String ccustomeremail, ref String ccustomeraddress,
            ref String ccustomercontact)
        {
            if (Globals.glOpenSqlConn())
            {
                SqlCommand cmd = new SqlCommand("spGetCustomerProfile",
                    Globals.sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cidCustomer", cidcustomer);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ccustomername = reader["nameCustomer"].ToString();
                    ccustomeremail = reader["emailCustomer"].ToString();
                    ccustomeraddress = reader["addressCustomer"].ToString();
                    ccustomercontact = reader["contactCustomer"].ToString();

                }
                else csMessageBox.Show("No such User ID:" + this.idCustomer.ToString() + " is found!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Globals.glCloseSqlConn();
        }
        void GetCustomerPhotofromField()
        {
            if (Globals.glOpenSqlConn())
            {
                String qrystr = "Select isnull(photoCustomer,'') as photo from dbo.customers where idCustomer=" + this.idCustomer.ToString();
                SqlCommand cmd = new SqlCommand(qrystr, Globals.sqlconn);

                SqlDataAdapter UserAdapter = new SqlDataAdapter(cmd);

                DataTable UserTable = new DataTable();
                UserAdapter.Fill(UserTable);

                if (UserTable.Rows[0][0] != null)
                {

                    //byte[] UserImg = (byte[])UserTable.Rows[0][0];
                    byte[] UserImg = (byte[])UserTable.Rows[0][0];
                    MemoryStream imgstream = new MemoryStream(UserImg);

                    if (imgstream.Length > 0)
                        this.pictBoxCustomer.Image = Image.FromStream(imgstream);
                }
                UserAdapter.Dispose();
            }
            Globals.glCloseSqlConn();
        }
        private void UpdateCustomer()
        {
            if (Globals.glOpenSqlConn())
            {
                SqlCommand cmd = new SqlCommand("spUpdateCustomer", Globals.sqlconn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cidCustomer", this.idCustomer);
                cmd.Parameters.AddWithValue("@cname", this.txtCustomerName.Text);

                cmd.Parameters.AddWithValue("@cemail", this.txtCustomerEmail.Text);
                cmd.Parameters.AddWithValue("@cadd", this.txtCustomerAddress.Text);
                cmd.Parameters.AddWithValue("@ccon", this.txtCustomerContactNo.Text);
                cmd.ExecuteNonQuery();
            }
            Globals.glCloseSqlConn();
        }
    }
}
