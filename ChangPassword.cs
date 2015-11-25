using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BX
{
    public partial class ChangeP : Form
    {
        public ChangeP()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            UserName.Text = myuser.username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(NPassWord1.Text.Trim()==NPassWord2.Text.Trim())
            {
                if(NPassWord1.TextLength>=8)
                {
                    if (PasswordStrength(NPassWord1.Text.Trim())== 1)
                    {
                       
                        String OPassWordMD5 = myuser.myMD5(OPassWord.Text);
                        String NPassWordMD5 = myuser.myMD5(NPassWord1.Text);
                        string[] args = new string[4];
                        args[0] = myuser.ErrInfo;
                        args[1] = myuser.username;
                        args[2] = OPassWordMD5;
                        args[3] = NPassWordMD5;
                        object result = myuser.InvokeWebService(myuser.ServerPage, "ChangePassword", args);
                        string str = result.ToString();
                        MessageBox.Show(str);
                        if ("修改密码成功！" == str)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("密码必须使用字母、数字或符号组成！");
                    }
                }
                else
                {
                    MessageBox.Show("密码长度不能小于8位！");
                }
            }
            else
            {
                MessageBox.Show("两次输入的新密码不一致！");
            }
        }
        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        private static int PasswordStrength(string password)
        {
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }

            if (iLtt == 0 && iSym == 0) return 0; //纯数字密码
            if (iNum == 0 && iLtt == 0) return 0; //纯符号密码
            if (iNum == 0 && iSym == 0) return 0; //纯字母密码
            if (iLtt == 0) return 1; //数字和符号构成的密码
            if (iSym == 0) return 1; //数字和字母构成的密码
            if (iNum == 0) return 1; //字母和符号构成的密码
            return 1; //由数字、字母、符号构成的密码
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
