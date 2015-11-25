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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UserName.Text.Trim() != "" || TrueName.Text.Trim() != "" || NPassWord1.Text.Trim() != "" || NPassWord2.Text.Trim() != "" || Dept.Text.Trim() != "")
            {
                if (NPassWord1.Text.Trim() == NPassWord2.Text.Trim())
                {
                    String NPassWordMD5 = myuser.myMD5(NPassWord1.Text);
                    string[] args = new string[6];
                    args[0] = myuser.ErrInfo;
                    args[1] = myuser.bm;
                    args[2] = UserName.Text.Trim();
                    args[3] = TrueName.Text.Trim();
                    args[4] = NPassWordMD5;
                    args[5] = Dept.Text.Trim();
                    object result = myuser.InvokeWebService(myuser.ServerPage, "AddUserInfo", args);
                    string str = result.ToString();
                    MessageBox.Show(str);
                }
                else
                    MessageBox.Show("两次输入的密码不一致！");
            }
            else
                MessageBox.Show("都不能为空！");
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            ActiveControl = UserName;
        }
    }
}
