using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Login
{
    public partial class Login : Form
    {
        SqlConnection con = new SqlConnection("Data Source=desktop-ngihs18;Initial Catalog=master;Integrated Security=True");
        SqlCommand cmd;
        SqlDataReader dr;
        bool log = true;
        public Login()
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Location = new Point(53, 147);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from users where usernam=@username and userpass =@userpass", con);
            cmd.Parameters.AddWithValue("@username",txtname.Text);
            cmd.Parameters.AddWithValue("@userpass",txtpassword.Text);

            con.Open();

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                button1.ForeColor = Color.Green; 
                label7.Visible = false;
                Hide();
                DashBoard next = new DashBoard();
                next.Show();
            }
            else
            {
                label7.Visible = true;
            }
            con.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (log == true)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                button2.Text = "Log in";
                button2.Location = new Point(239, 471);
                label3.Text = "Created an account?";
                label3.Location = new Point(107, 476);
                txtname.Text = "";
                txtpassword.Text = "";
                label7.Visible = false;
                log = false;
            }
            else if (log == false) 
            {
                panel1.Visible = true;
                panel2.Visible = false;
                button2.Text = "Create one";
                button2.Location = new Point(209, 471);
                label3.Text = "No account?";
                label3.Location = new Point(119, 476);
                textBox1.Text = "";
                textBox2.Text = "";
                label8.Visible = false;
                log =true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text=="")
            {
                MessageBox.Show("Please enter a new username and password");
            }
            else
            {
                cmd = new SqlCommand("select * from users where usernam=@username", con);
                cmd.Parameters.AddWithValue("@username", textBox2.Text);

                con.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    label8.Visible = true;
                    con.Close();
                }
                else
                {
                    con.Close();
                    label8.Visible = false;

                    cmd = new SqlCommand("INSERT INTO users (usernam,userpass) VALUES (@nusername,@nuserpass);", con);
                    cmd.Parameters.AddWithValue("@nusername", textBox2.Text);
                    cmd.Parameters.AddWithValue("@nuserpass", textBox1.Text);

                    con.Open();

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }
            

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
