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
    public partial class Form2 : Form
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
        OleDbCommand cmdKhoa;
        OleDbCommand cmdSinhVien;
        OleDbCommand cmdKetQua;
        int stt = -1;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Khởi tạo kết nối 
            cnn = new OleDbConnection(strcon);
            Tao_bang_table();
            Moc_noi_quan_he_cac_bang();
            Nhap_lieu_cac_bang_Access();
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
            dtpngaysinh.Value = (DateTime)rsv["NgaySinh"];
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
        private void Nhap_lieu_cac_bang_Access()
        {
            Bang_khoa();
            Bang_sinhvien();
            Bang_ketqua();
        }
        private void Bang_ketqua()
        {
            //Nhập dữ liệu cho bảng tblketqua
            //Mở kết nối
            cnn.Open();
            //2.khởi tạo đối tượng command tương ứng để đọc dữ liệu từ table KETQUA
            cmdKetQua = new OleDbCommand("select * from ketqua", cnn);
            //3.Tạo đối tượng DataReader để tiến hành đọc từng dòng dữ liệu trong table KETQUA
            OleDbDataReader rkq = cmdKetQua.ExecuteReader();
            //.4 tiến hành đọc dữ liệu với đối tượng DataReader như sau
            while (rkq.Read()) //Mỗi lần lặp thì rkh trỏ đến 1 dòng trong table KETQUA
            {
                DataRow r = tblKetqua.NewRow();
                for (int i = 0; i < rkq.FieldCount; i++)
                    r[i] = rkq[i];
                tblKetqua.Rows.Add(r);
            }
            //Dóng DataReader và đóng kết nối
            rkq.Close();
            cnn.Close();
        }

        private void Bang_sinhvien()
        {
            //Nhập dữ liệu cho bảng tblsinhvien
            //Mở kết nối
            cnn.Open();
            //2.khởi tạo đối tượng command tương ứng để đọc dữ liệu từ table SIHNVIEN
            cmdSinhVien = new OleDbCommand("select * from sinhvien", cnn);
            //3.Tạo đối tượng DataReader để tiến hành đọc từng dòng dữ liệu trong table SINHVIEN
            OleDbDataReader rsv = cmdSinhVien.ExecuteReader();
            //.4 tiến hành đọc dữ liệu với đối tượng DataReader như sau
            while (rsv.Read()) //Mỗi lần lặp thì rkh trỏ đến 1 dòng trong table SINHVIEN
            {
                DataRow r = tblSinhvien.NewRow();
                for (int i = 0; i < rsv.FieldCount; i++)
                    r[i] = rsv[i];
                tblSinhvien.Rows.Add(r);
            }
            //Dóng DataReader và đóng kết nối
            rsv.Close();
            cnn.Close();
        }

        private void Bang_khoa()
        {
            //Nhập dữ liệu cho bảng tblkhoa
            //Mở kết nối
            cnn.Open();
            //2.khởi tạo đối tượng command tương ứng để đọc dữ liệu từ table KHOA
            cmdKhoa = new OleDbCommand("select * from khoa", cnn);
            //3.Tạo đối tượng DataReader để tiến hành đọc từng dòng dữ liệu trong table KHOA
            OleDbDataReader rkh = cmdKhoa.ExecuteReader();
            //.4 tiến hành đọc dữ liệu với đối tượng DataReader như sau
            while (rkh.Read()) //Mỗi lần lặp thì rkh trỏ đến 1 dòng trong table KHOA
            {
                DataRow r = tblKhoa.NewRow();
                for (int i = 0; i < rkh.FieldCount; i++)
                    r[i] = rkh[i];
                tblKhoa.Rows.Add(r);
            }
            //Dóng DataReader và đóng kết nối
            rkh.Close();
            cnn.Close();
            
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
            foreach (Control ctl in this.Controls)
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
                //Xóa trong CSDL
                //MỞ kết nối
                cnn.Open();
                string chuoi_xoa = "delete from sinhvien where masv=@masv";
                cmdSinhVien = new OleDbCommand(chuoi_xoa, cnn);
                //Khai báo paranetter trên như sau
                cmdSinhVien.Parameters.Add("@masv", OleDbType.Char).Value = txtmasv.Text;
                int n = cmdSinhVien.ExecuteNonQuery();
                if (n > 0)
                    MessageBox.Show("Hủy sinh viên thành công");
                stt = 0;
                Gan_du_lieu(stt);
                //Đóng kết nối
                cnn.Close();
            }
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            //Mở kết nối
            cnn.Open();
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
                //2.Sửa trong CSDL
                string chuoi_sua = "update sinhvien set hosv=@hsv, tensv=@tsv, " +
                    "phai=@ph,ngaysinh=@ngs, noisinh=@ns, makh=@makh, hocbong=@hocbong where masv=@masv";
                cmdSinhVien = new OleDbCommand(chuoi_sua, cnn);
                //Khai báo paranetter trên như sau
                cmdSinhVien.Parameters.Add("@hsv", OleDbType.VarWChar).Value = txtho.Text;
                cmdSinhVien.Parameters.Add("@tsv", OleDbType.VarWChar).Value = txtten.Text;
                cmdSinhVien.Parameters.Add("@ph", OleDbType.Boolean).Value = chkgioitinh.Checked;
                cmdSinhVien.Parameters.Add("@ngs", OleDbType.Date).Value = dtpngaysinh.Value;
                cmdSinhVien.Parameters.Add("@ns", OleDbType.VarWChar).Value = txtnoisinh.Text;
                cmdSinhVien.Parameters.Add("@makh", OleDbType.Char).Value = cbokhoa.SelectedValue.ToString();
                cmdSinhVien.Parameters.Add("@hocbong", OleDbType.Double).Value = txthocbong.Text;
                cmdSinhVien.Parameters.Add("@masv", OleDbType.Char).Value = txtmasv.Text;
                int n = cmdSinhVien.ExecuteNonQuery();
                if (n > 0)
                    MessageBox.Show("Cập nhật sinh viên thành công");
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
                string chuoi_them = "insert into values(@msv,@hsv,@tsv,@ph,@ngs,@ns,@makh,@hocbong) where masv=@masv";
                cmdSinhVien = new OleDbCommand(chuoi_them, cnn);
                //Khai báo paranetter trên như sau
                cmdSinhVien.Parameters.Add("@masv", OleDbType.Char).Value = txtmasv.Text;
                cmdSinhVien.Parameters.Add("@hsv", OleDbType.VarWChar).Value = txtho.Text;
                cmdSinhVien.Parameters.Add("@tsv", OleDbType.VarWChar).Value = txtten.Text;
                cmdSinhVien.Parameters.Add("@ph", OleDbType.Boolean).Value = chkgioitinh.Checked;
                cmdSinhVien.Parameters.Add("@ngs", OleDbType.Date).Value = dtpngaysinh.Value;
                cmdSinhVien.Parameters.Add("@ns", OleDbType.VarWChar).Value = txtnoisinh.Text;
                cmdSinhVien.Parameters.Add("@makh", OleDbType.Char).Value = cbokhoa.SelectedValue.ToString();
                cmdSinhVien.Parameters.Add("@hocbong", OleDbType.Double).Value = txthocbong.Text;
                
                int n = cmdSinhVien.ExecuteNonQuery();
                if (n > 0)
                    MessageBox.Show("Thêm sinh viên thành công");
            }
            //Đóng kết nối
            cnn.Close();
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            Gan_du_lieu(stt);
        }

    }
}
