using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BX
{
    public partial class GLPassWord : Form
    {
        public GLPassWord()
        {
            InitializeComponent();
        }
        public static DataGridViewRow mydat = new DataGridViewRow();
        private void GLPassWord_Load(object sender, EventArgs e)
        {
            UserName.Text = mydat.Cells[1].Value.ToString().Trim();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (NPassWord1.Text.Trim() != "")
            {
                if (NPassWord1.Text.Trim() == NPassWord2.Text.Trim())
                {

                    String NPassWordMD5 = myuser.myMD5(NPassWord1.Text);
                    string[] args = new string[4];
                    args[0] = myuser.ErrInfo;
                    args[1] = myuser.bm;
                    args[2] = UserName.Text.Trim();
                    args[3] = NPassWordMD5;
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GLChangePassword", args);
                    string str = result.ToString();
                    MessageBox.Show(str);
                    if ("修改密码成功！" == str)
                    {
                        this.Close();
                    }

                }
                else
                {
                    MessageBox.Show("两次输入的新密码不一致！");
                }
            }
            else
                MessageBox.Show("不能为空！");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
