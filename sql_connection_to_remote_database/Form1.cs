using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace sql_connection_to_remote_database
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // read the db info
            try
            {
                using (StreamReader sr = new StreamReader(db_info_path))
                {
                    db_ip = sr.ReadLine();
                    db_name = sr.ReadLine();
                    db_table_name = sr.ReadLine();
                    db_account = sr.ReadLine();
                    db_pw = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            cn_str = @"Data Source=" + db_ip + ";" +
               "Network Library=DBMSSOCN;" +
               "Initial Catalog=" + db_name + ";" +
               "User ID=" + db_account + ";" +
               "Password=" + db_pw + ";";

            // test connection
            try
            {
                SqlConnection cn = new SqlConnection(cn_str);
                cn.Open();
                MessageBox.Show("connection success");
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // read some data tables from db
            try
            {
                ds = new DataSet();
                SqlDataAdapter sql_da = new SqlDataAdapter("SELECT * FROM " + db_table_name, cn_str);
                sql_da.Fill(ds, db_table_name);

                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = db_table_name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        DataSet ds;
        string db_info_path = "database.txt";
        string db_ip, db_name, db_table_name, db_account, db_pw, cn_str;
    }
}
