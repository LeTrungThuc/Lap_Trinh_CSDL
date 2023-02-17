using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; //Sử dụng assecc

namespace BT01_Ngat_KetNoi
{
    public partial class Form1 : Form
    {
        //Khai báo các đối tượng để kết nối
        // Chuỗi kết nối đến CSDL Access 2003
        string strcon = @"provider=microsoft.jet.oledb.4.0; data source=..\..\..\Data\QLSinhVien.mdb";
        //khai báo đối tượng lưu trữ dữ liệu
        DataSet ds = new DataSet();
        //Khai báo đối tượng Datadapter sử dụng với 1 nguyên tắc:Cứ 1 Datatable trong Dataset
        //sẽ tương ứng với 1 table trong CSDL
        OleDbDataAdapter adpkhoa, adpsinhvien, adpketqua;
        //Khai báo đối tượng cần cập nhật dữ liệu 
        OleDbCommandBuilder cmdsinhvien;
        int stt = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Khoi_tao_du_lieu();
            Doc_du_lieu();
            stt = 0;
            Gan_du_lieu(stt);
            khoi_tao_combo();
        }

        private void Khoi_tao_du_lieu()
        {
            //1. Khởi tạo các đối tượng DataAdapter
            adpkhoa = new OleDbDataAdapter("select * from khoa", strcon);
            adpsinhvien = new OleDbDataAdapter("select * from sinhvien", strcon);
            adpketqua = new OleDbDataAdapter("select * from ketqua", strcon);
            //2.Khởi tạo đối tượng commandbuilder
            cmdsinhvien = new OleDbCommandBuilder(adpsinhvien);
        }
        private void Doc_du_lieu()
        {
            //1.Đọc dữ liệu bảng khoa
            //1.1 Sao chép cấu trúc của table KHOA vào datatable trong dataset ds
            adpkhoa.FillSchema(ds, SchemaType.Source, "KHOA");
            //1.2 Sao chép dữ liệu của table KHOA vào datatable trong dataset ds
            adpkhoa.Fill(ds, "KHOA");
            //2.Đọc dữ liệu bảng sinhvien
            adpsinhvien.FillSchema(ds, SchemaType.Source, "SINHVIEN");
            adpsinhvien.Fill(ds, "SINHVIEN");
            //2.Đọc dữ liệu bảng ketqua
            adpketqua.FillSchema(ds, SchemaType.Source, "KETQUA");
            adpketqua.Fill(ds, "KETQUA");
        }
        private void khoi_tao_combo()
        {
            cbokhoa.DataSource = ds.Tables["KHOA"];
            cbokhoa.DisplayMember = "TenKH";
            cbokhoa.ValueMember = "MaKH";
        }
        public void Gan_du_lieu(int stt)
        {
            DataTable tblSinhvien = ds.Tables["SINHVIEN"] ;
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
            object td = ds.Tables["KETQUA"].Compute("sum(Diem)", "MaSV='" + msv + "'");
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
            DataRow rsv = ds.Tables["SINHVIEN"].Rows.Find(txtmasv.Text);
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
                int n = adpsinhvien.Update(ds,"SINHVIEN");
                if (n > 0)
                    MessageBox.Show("Cập nhật sinh viên thành công");
            }
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            DataTable tblSinhvien = ds.Tables["SINHVIEN"];
            
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
            int n =adpsinhvien.Update(ds,"SINHVIEN");
            if (n > 0)
                MessageBox.Show("Cập nhật sinh viên thành công");
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            Gan_du_lieu(stt);
        }
    }
}
