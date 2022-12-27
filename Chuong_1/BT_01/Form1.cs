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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

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