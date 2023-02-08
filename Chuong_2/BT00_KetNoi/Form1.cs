using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; // Sử dụng kết nối và làm việc với Access
using System.Data.SqlClient; // sử dụng kết nối và làm việc với SQL

namespace BT00_KetNoi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnacc2003_Click(object sender, EventArgs e)
        {
            //Khai báo các đối tượng để kết nối
            // Chuỗi kết nối đến CSDL Access 2003
            string strcon = @"provider=microsoft.jet.oledb.4.0; data source=..\..\..\Data\QLSinhVien.mdb";
            OleDbConnection cnn = new OleDbConnection(strcon);
            //Mở kết nối
            cnn.Open();
            //Kiểm tra xem kết nối có mở dc ko
            if (cnn.State == ConnectionState.Open)
                MessageBox.Show(" kết nối đã mở");
            //Đóng kết nối
            cnn.Close();

        }

        private void btnacc2019_Click(object sender, EventArgs e)
        {
            //Khai báo các đối tượng để kết nối
            // Chuỗi kết nối đến CSDL Access 2007 trở về sau
            string strcon = @"provider=microsoft.ACE.oledb.12.0; data source=..\..\..\Data\QLSinhVien.mdb";
            OleDbConnection cnn = new OleDbConnection(strcon);
            //Mở kết nối
            cnn.Open();
            //Kiểm tra xem kết nối có mở dc ko
            if (cnn.State == ConnectionState.Open)
                MessageBox.Show(" kết nối đã mở");
            //Đóng kết nối
            cnn.Close();
        }

        private void btnsqlwd_Click(object sender, EventArgs e)
        {
            //Khai báo các đối tượng để kết nối
            // Chuỗi kết nối đến SQL Sever bằng use windows
            string strcon = @"server=.; database=C21TH2_LTCSDL; integrated security=true";
            SqlConnection cnn = new SqlConnection(strcon);
            //Mở kết nối
            cnn.Open();
            //Kiểm tra xem kết nối có mở dc ko
            if (cnn.State == ConnectionState.Open)
                MessageBox.Show(" kết nối đã mở");
            //Đóng kết nối
            cnn.Close();
        }

        private void btnsqlsa_Click(object sender, EventArgs e)
        {
            //Khai báo các đối tượng để kết nối
            // Chuỗi kết nối đến SQL Sever bằng use sa
            string strcon = @"server=.; database=C21TH2_LTCSDL; uid=sa; pwd=c21th";
            SqlConnection cnn = new SqlConnection(strcon);
            //Mở kết nối
            cnn.Open();
            //Kiểm tra xem kết nối có mở dc ko
            if (cnn.State == ConnectionState.Open)
                MessageBox.Show(" kết nối đã mở");
            //Đóng kết nối
            cnn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
