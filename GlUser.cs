using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BX
{
    public partial class GLLogin : Form
    {
        public GLLogin()
        {
            InitializeComponent();
        }

        private void GLLogin_Load(object sender, EventArgs e)
        {
            string[] args = new string[2];
            args[0] = myuser.ErrInfo;
            args[1] = myuser.bm;
            object result = myuser.InvokeWebService(myuser.ServerPage, "ReaderLogin", args);
            DataSet DSRe = (DataSet)result;
            this.dataGridView1.AutoGenerateColumns = true;

            //将返回的DSRe绑定到datagridview上
            this.dataGridView1.DataSource = DSRe;

            //指定显示的datatable
            this.dataGridView1.DataMember = "admin";
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].Width = 60;
            int i;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Moccasin;
                }
            }
        }

        private void 修改资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;    //取得选中行的索引  
            UserName.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
            TrueName.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
            Dept.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GLPassWord.mydat = dataGridView1.CurrentRow;
            GLPassWord glpw = new GLPassWord();
            glpw.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] args = new string[3];
            args[0] = myuser.ErrInfo;
            args[1] = myuser.bm;
            args[2] = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            object result = myuser.InvokeWebService(myuser.ServerPage, "DelUser", args);
            MessageBox.Show(result.ToString());
            button2_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 修改_Click(object sender, EventArgs e)
        {
            string[] args = new string[5];
            args[0] = myuser.ErrInfo;
            args[1] = myuser.bm;
            args[2] = UserName.Text.Trim();
            args[3] = TrueName.Text.Trim();
            args[4] = Dept.Text.Trim();
            object result = myuser.InvokeWebService(myuser.ServerPage, "GLUserInfo", args);
            MessageBox.Show(result.ToString());
            button2_Click(sender, e);


        }

        private void GLLogin_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddUser add = new AddUser();
                add.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] args1 = new string[2];
            args1[0] = myuser.ErrInfo;
            args1[1] = myuser.bm;
            object result1 = myuser.InvokeWebService(myuser.ServerPage, "ReaderLogin", args1);
            DataSet DSRe = (DataSet)result1;
            this.dataGridView1.AutoGenerateColumns = true;

            //将返回的DSRe绑定到datagridview上
            this.dataGridView1.DataSource = DSRe;

            //指定显示的datatable
            this.dataGridView1.DataMember = "admin";
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].Width = 60;
            int i;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Moccasin;
                }
            }
        }
    }
}
