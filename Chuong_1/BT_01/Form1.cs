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

namespace BT_01
{
    public partial class Form1 : Form
    {
        //Tạo  DataSet
        DataSet ds = new DataSet();
        //khai báo các datatable tương ứng với các bảng có chưa dữ liệu
        DataTable tblKhoa = new DataTable("KHOA");
        DataTable tblSinhvien = new DataTable("SINHVIEN");
        DataTable tblKetqua = new DataTable("KETQUA");
        int stt = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Tạo cấu trúc cho các datatable
            Tao_bang_table();
            Moc_noi_quan_he_cac_bang();
            Nhap_lieu_cac_bang();
            khoi_tao_combo();
            btndau.PerformClick();
        }

        private void khoi_tao_combo()
        {
            cbokhoa.DataSource = tblKhoa;
            cbokhoa.DisplayMember = "TenKH";
            cbokhoa.ValueMember = "MaKH";
        }

        private void Nhap_lieu_cac_bang()
        {
            Bang_khoa();
            Bang_sinhvien();
            Bang_ketqua();
        }
        private void Gan_du_lieu(int stt)
        {
            DataRow rsv = tblSinhvien.Rows[stt];
            txtmasv.Text = rsv["MaSV"].ToString();
            txthosv.Text = rsv["HoSV"].ToString();
            txttensv.Text = rsv["TenSV"].ToString();
            chkphai.Checked = (Boolean)rsv["Phai"];
            txtngaysinh.Text = rsv["NgaySinh"].ToString();
            txtnoisinh.Text = rsv["NoiSinh"].ToString();
            cbokhoa.SelectedValue = rsv["MaKH"].ToString();
            txthocbong.Text = rsv["HocBong"].ToString();
        }

        private void Bang_ketqua()
        {
            string[] mang_ketqua = File.ReadAllLines(@"..\..\Data\KETQUA.txt");
            foreach (string chuoi_ketqua in mang_ketqua)
            {
                string[] mang_thanh_phan = chuoi_ketqua.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //Tạo dòng mới datarow
                DataRow rkq = tblKetqua.NewRow();
                for (int i = 0; i < mang_thanh_phan.Length; i++)
                {
                    rkq[i] = mang_thanh_phan[i];
                }
                tblKetqua.Rows.Add(rkq);
            }
        }

        private void Bang_sinhvien()
        {
            string[] mang_sinhvien = File.ReadAllLines(@"..\..\Data\SINHVIEN.txt");
            foreach(string chuoi_sinhvien in mang_sinhvien)
            {
                string[] mang_thanh_phan = chuoi_sinhvien.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //Tạo dòng mới datarow
                DataRow rsv = tblSinhvien.NewRow();
                for(int i = 0; i < mang_thanh_phan.Length; i++)
                {
                    rsv[i] = mang_thanh_phan[i];
                }
                tblSinhvien.Rows.Add(rsv);
            }
        }

        private void Bang_khoa()
        {
            string[] mang_Khoa = File.ReadAllLines(@"..\..\Data\KHOA.txt");
            foreach(string chuoi_khoa in mang_Khoa)
            {
                string[] mang_thanh_phan = chuoi_khoa.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //Tạo một dòng mới có cấu trúc là datarow
                DataRow rkh = tblKhoa.NewRow();
                rkh[0] = mang_thanh_phan[0];
                rkh[1] = mang_thanh_phan[1];
                //Thêm dòng tạo này vào tblkhoa
                tblKhoa.Rows.Add(rkh);
            }
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
            tblKetqua.PrimaryKey = new DataColumn[] { tblKetqua.Columns["MaSV"],tblKetqua.Columns["MaMH"] };
            //Thêm các datatable vào dataset
           /* ds.Tables.Add(tblKhoa);
            ds.Tables.Add(tblSinhvien);
            ds.Tables.Add(tblKetqua);*/
            //Thêm các datatable vào dataset cách 2
            ds.Tables.AddRange(new DataTable[] { tblKhoa, tblKetqua, tblSinhvien });

        }

        private void button4_Click(object sender, EventArgs e)
        {
            stt = tblSinhvien.Rows.Count - 1;
            Gan_du_lieu(stt);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stt++;
            Gan_du_lieu(stt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stt--;
            Gan_du_lieu(stt);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stt=0;
            Gan_du_lieu(stt);
        }

        
    }
}
//Thành phần DataTable
//1. Dùng để lưu trữ dữ liệu của một table trong bộ nhớ
//2. Tạo một đối tượng DataTable:new DataTable("<Tên DataTable>");
//3. Tạo ra cột Datacolum: <biến DataTable>.columns.add("<Tên cột>", <Kiểu dữ liệu>);
//4. Tạo khóa chính cho DataTable => pimerykey => là mảng các Datacolumn
//5. Thêm các DataTable vào DataSet
//6. Móc mối quan hệ giữa các DataTable