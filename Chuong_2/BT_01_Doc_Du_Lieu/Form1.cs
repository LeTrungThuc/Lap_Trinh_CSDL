using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace BT_01_Doc_Du_Lieu
{
    public partial class Form1 : Form
    {
        //Khai báo các đối tượng để kết nối
        // Chuỗi kết nối đến CSDL Access 2003
        string strcon = @"provider=microsoft.jet.oledb.4.0; data source=..\..\..\Data\QLSinhVien.mdb";
        OleDbConnection cnn;
        DataSet ds = new DataSet();
        //khai báo các datatable tương ứng với các bảng có chua dữ liệu
        DataTable tblKhoa = new DataTable("KHOA");
        DataTable tblSinhvien = new DataTable("SINHVIEN");
        DataTable tblKetqua = new DataTable("KETQUA");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tao_bang_table();
            Moc_noi_quan_he_cac_bang();
        }
        private void Moc_noi_quan_he_cac_bang()
        {
            // Tạo mối quan hệ giữa tblkhoa và tblsinhvien
            ds.Relations.Add("FK_KH_SV", ds.Tables["KHOA"].Columns["MaKH"], ds.Tables["SINHVIEN"].Columns["MaKH"], true);
            //Tạo mối quan hệ giữa tblsinhvien và tblketqua
            ds.Relations.Add("FK_SV_KQ", ds.Tables["SINHVIEN"].Columns["MaSV"], ds.Tables["KETQUA"].Columns["MasV"], true);
            //Loại bỏ Cascade delete trong mối quan hệ
            ds.Relations["FK_KH_SV"].ChildKeyConstraint.DeleteRule = Rule.None;
            ds.Relations["FK_SV_KQ"].ChildKeyConstraint.DeleteRule = Rule.None;
        }
        private void Tao_bang_table()
        {
            //Tạo cấu trúc bảng tblkhoa
            tblKhoa.Columns.Add("MaKH", typeof(string));
            tblKhoa.Columns.Add("TenKH", typeof(string));
            //Khóa chính của tblkhoa
            tblKhoa.PrimaryKey = new DataColumn[] { tblKhoa.Columns["MaKH"] };
            //Tạo cấu trúc bảng tblsinhvien
            tblSinhvien.Columns.Add("MaSV", typeof(string));
            tblSinhvien.Columns.Add("HoSV", typeof(string));
            tblSinhvien.Columns.Add("TenSV", typeof(string));
            tblSinhvien.Columns.Add("Phai", typeof(Boolean));
            tblSinhvien.Columns.Add("NgaySinh", typeof(DateTime));
            tblSinhvien.Columns.Add("NoiSinh", typeof(string));
            tblSinhvien.Columns.Add("MaKH", typeof(string));
            tblSinhvien.Columns.Add("HocBong", typeof(double));
            //Khóa cính của tblsinhvien
            tblSinhvien.PrimaryKey = new DataColumn[] { tblSinhvien.Columns["MaSV"] };
            //Tạo cấu trúc bảng tblketqua
            tblKetqua.Columns.Add("MaSV", typeof(string));
            tblKetqua.Columns.Add("MaMH", typeof(string));
            tblKetqua.Columns.Add("Diem", typeof(Single));
            //Khóa chính của tblketqua
            tblKetqua.PrimaryKey = new DataColumn[] { tblKetqua.Columns["MaSV"], tblKetqua.Columns["MaMH"] };
            //Thêm các datatable vào dataset
            /* ds.Tables.Add(tblKhoa);
             ds.Tables.Add(tblSinhvien);
             ds.Tables.Add(tblKetqua);*/
            //Thêm các datatable vào dataset cách 2
            ds.Tables.AddRange(new DataTable[] { tblKhoa, tblKetqua, tblSinhvien });

        }
    }
}
