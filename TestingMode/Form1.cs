using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingMode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection SqlCon = new SqlConnection("Data Source=igleoa.database.windows.net,1433;User ID=AdminIgleOA;Password=Wxyz1234;Initial Catalog=DB_MAIN_CR_OA;");

        string imgLocation = "";

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
                txtExt.Text = Path.GetExtension(imgLocation);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            byte[] images = null;

            FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
            BinaryReader brs = new BinaryReader(Stream);
            images = brs.ReadBytes((int)Stream.Length);

            SqlCon.Open();
            var SqlCmd = new SqlCommand("[adm].[uspAddBanner]", SqlCon)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlParameter parName = new SqlParameter
            {
                ParameterName = "@BannerName",
                SqlDbType = SqlDbType.VarChar,
                Size = 100,
                Value = txtName.Text.Trim()
            };
            SqlCmd.Parameters.Add(parName);

            SqlParameter parExt = new SqlParameter
            {
                ParameterName = "@BannerExt",
                SqlDbType = SqlDbType.VarChar,
                Size = 10,
                Value = txtExt.Text.Trim()
            };
            SqlCmd.Parameters.Add(parExt);

            SqlParameter parLocation = new SqlParameter
            {
                ParameterName = "@Location",
                SqlDbType = SqlDbType.VarChar,
                Size = 100,
                Value = txtLocation.Text.Trim()
            };
            SqlCmd.Parameters.Add(parLocation);

            SqlParameter parInsert = new SqlParameter
            {
                ParameterName = "@InsertUser",
                SqlDbType = SqlDbType.VarChar,
                Size = 100,
                Value = "johmstone"
            };
            SqlCmd.Parameters.Add(parInsert);


            SqlParameter parPicture = new SqlParameter
            {
                ParameterName = "@Banner",
                Value = images
            };
            SqlCmd.Parameters.Add(parPicture);

            int N = SqlCmd.ExecuteNonQuery();

            MessageBox.Show(N.ToString() + "Data Saved Successfully...!");

            SqlCon.Close();

        }

        
    }
}
