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
using System.IO;

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
        int stt = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Khởi tạo kết nối 
            cnn = new OleDbConnection(strcon);
            Tao_bang_table();
            Moc_noi_quan_he_cac_bang();
            Nhap_lieu_cac_bang();
            stt = 0;
            Gan_du_lieu(stt);
            khoi_tao_combo();
           
        }

        private void khoi_tao_combo()
        {
            cbokhoa.DataSource = tblKhoa;
            cbokhoa.DisplayMember = "TenKH";
            cbokhoa.ValueMember = "MaKH";
        }

        public void Gan_du_lieu(int stt)
        {
            DataRow rsv = tblSinhvien.Rows[stt];
            txtmasv.Text = rsv["MaSV"].ToString();
            txtho.Text = rsv["HoSV"].ToString();
            txtten.Text = rsv["TenSV"].ToString();
            chkgioitinh.Checked = (Boolean)rsv["Phai"];
            dtpngaysinh.Value =(DateTime)rsv["NgaySinh"];
            txtnoisinh.Text = rsv["NoiSinh"].ToString();
            cbokhoa.SelectedValue = rsv["MaKH"].ToString();
            txthocbong.Text = rsv["HocBong"].ToString();
            //Hiện thị số thứ tự mẫu tin hiện hành
            lblstt.Text = (stt + 1) + "/" + (tblSinhvien.Rows.Count);
            //Tính tổng điểm trong bảng tblketqua của sinhvien đó
            txttongdiem.Text = Tongdiem(txtmasv.Text).ToString();
        }

        private double Tongdiem(string msv)
        {
            double kq = 0;
            object td = tblKetqua.Compute("sum(Diem)", "MaSV='" + msv + "'");
            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;
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
        private void Nhap_lieu_cac_bang()
        {
            Bang_khoa();
            Bang_sinhvien();
            Bang_ketqua();
        }
        private void Bang_ketqua()
        {
            string[] mang_ketqua = File.ReadAllLines(@"..\..\..\Data\KETQUA.txt");
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
            string[] mang_sinhvien = File.ReadAllLines(@"..\..\..\Data\SINHVIEN.txt");
            foreach (string chuoi_sinhvien in mang_sinhvien)
            {
                string[] mang_thanh_phan = chuoi_sinhvien.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //Tạo dòng mới datarow
                DataRow rsv = tblSinhvien.NewRow();
                for (int i = 0; i < mang_thanh_phan.Length; i++)
                {
                    rsv[i] = mang_thanh_phan[i];
                }
                tblSinhvien.Rows.Add(rsv);
            }
        }

        private void Bang_khoa()
        {
            string[] mang_Khoa = File.ReadAllLines(@"..\..\..\Data\KHOA.txt");
            foreach (string chuoi_khoa in mang_Khoa)
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

        private void btntruoc_Click(object sender, EventArgs e)
        {
            stt--;
            Gan_du_lieu(stt);
        }

        private void btnsau_Click(object sender, EventArgs e)
        {
            stt++;
            Gan_du_lieu(stt);
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtmasv.ReadOnly = false;
            foreach(Control ctl in this.Controls)
            {
                if (ctl is TextBox)
                    (ctl as TextBox).Clear();
                else if (ctl is CheckBox)
                    (ctl as CheckBox).Checked = true;
                else if (ctl is ComboBox)
                    (ctl as ComboBox).SelectedIndex = 0;
                else if (ctl is DateTimePicker)
                    (ctl as DateTimePicker).Value = new DateTime(2005, 1, 1);
                
            }
            txtmasv.Focus();
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            //Xác định dòng cần hủy ==> sử dụng hàm Find tìm theo khóa chính của Datatable
            DataRow rsv = tblSinhvien.Rows.Find(txtmasv.Text);
            //Cần kiểm tra: Nếu bảng tblsinhvien có tồn tại những dòng liên quan trong bảng tblketqua thì không cho xóa
            //ngược lại thì xóa những sinh viên không có dòng liên quan đến bảng tblketqua
            //Sử dụng hàm Getchilrow để kiểm tra những dòng có liên quan trong bảng tblketqua ? giá trị trả về của hàm này là một mảng
            DataRow[] Mang_dong_lien_quan = rsv.GetChildRows("FK_SV_KQ");
            if (Mang_dong_lien_quan.Length > 0)
                MessageBox.Show("Không xóa được do tồn tại những dòng liên quan tblketqua");
            else
            {
                rsv.Delete();
                stt = 0;
                Gan_du_lieu(stt);
            }    
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            if (txtmasv.ReadOnly)
            {
                DataRow rsv = tblSinhvien.Rows.Find(txtmasv.Text);
                rsv["HoSV"] = txtho.Text;
                rsv["TenSV"] = txtten.Text;
                rsv["Phai"] = chkgioitinh.Checked;
                rsv["NgaySinh"] = dtpngaysinh.Value;
                rsv["NoiSinh"] = txtnoisinh.Text;
                rsv["MaKH"] = cbokhoa.SelectedValue.ToString();
                rsv["HocBong"] = txthocbong.Text;
            }
            else
            {
                //Kiểm tra Khóa chính có trùng hay ko
                DataRow rsv = tblSinhvien.Rows.Find(txtmasv.Text);
                if (rsv != null)
                {
                    MessageBox.Show("Bạn đã nhập trùng MaSV. Nhập lại nhé!");
                    txtmasv.Focus();
                    return;
                }
                rsv = tblSinhvien.NewRow();
                rsv["MaSV"] = txtmasv.Text;
                rsv["HoSV"] = txtho.Text;
                rsv["TenSV"] = txtten.Text;
                rsv["Phai"] = chkgioitinh.Checked;
                rsv["NgaySinh"] = dtpngaysinh.Value;
                rsv["NoiSinh"] = txtnoisinh.Text;
                rsv["MaKH"] = cbokhoa.SelectedValue.ToString();
                rsv["HocBong"] = txthocbong.Text;
                tblSinhvien.Rows.Add(rsv);
                txtmasv.ReadOnly = true;

            }               
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            Gan_du_lieu(stt);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ghi tập tin
            //Lưu ý: tblsinhvien.rows -> tập hợp dòng (không phải là mảng ) => chuyển thành kiểu mảng => chuyển thành kiểu chuỗi
            //Thuật toán khi 1 Datatable vào tập tin
            //1.Khai báo 1 mảng chuỗi với 1 phần tử sẽ chứa một chuỗi tương ứng với 1 dòng trong datatable
            //2.Duyệt qua tập hợp Rows của Datatable và đưa từng dòng vào mảng chuỗi với hàm Join
            //3.sử dụng phương thức wriAllines để ghi mảng chuỗi vào tập tin SINHVIEN.txt
            List<string> Mang_chuoi_sinh_vien = new List<string>();
            foreach(DataRow rsv in tblSinhvien.Rows)
            {
                string chuoi_sinh_vien = string.Join("|", rsv.ItemArray);
                Mang_chuoi_sinh_vien.Add(chuoi_sinh_vien);
            }
            File.WriteAllLines(@"..\..\..\Data\SINHVIEN.txt", Mang_chuoi_sinh_vien);
        }
    }
}
