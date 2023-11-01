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
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Forms.DataVisualization.Charting;

namespace Login
{
    public partial class DashBoard : Form
    {
        SqlConnection con = new SqlConnection("Data Source=desktop-ngihs18;Initial Catalog=master;Integrated Security=True");
        SqlCommand cmd;
        SqlDataReader dr;

        List<Exercise> exercises = new List<Exercise>();
        public DashBoard()
        {
            InitializeComponent();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            cmd = new SqlCommand("SELECT * FROM exercise_log WHERE owner_id = 1 ",con);
            con.Open();
            dr = cmd.ExecuteReader();
            int goal = 100;
            //Days for the graph 
            DateTime Day1 = DateTime.Now.AddDays(0);
            DateTime Day2 = DateTime.Now.AddDays(-1);
            DateTime Day3 = DateTime.Now.AddDays(-2);
            DateTime Day4 = DateTime.Now.AddDays(-3);
            DateTime Day5 = DateTime.Now.AddDays(-4);
            DateTime Day6 = DateTime.Now.AddDays(-5);
            DateTime Day7 = DateTime.Now.AddDays(-6);
            d1.Text = Day1.DayOfWeek.ToString();
            d2.Text = Day2.DayOfWeek.ToString();
            d3.Text = Day3.DayOfWeek.ToString();
            d4.Text = Day4.DayOfWeek.ToString();
            d5.Text = Day5.DayOfWeek.ToString();
            d6.Text = Day6.DayOfWeek.ToString();
            d7.Text = Day7.DayOfWeek.ToString();
            //

            double d1_total = 0;
            double d2_total = 0;
            double d3_total = 0;
            double d4_total = 0;
            double d5_total = 0;
            double d6_total = 0;
            double d7_total = 0;

            while (dr.Read()){
                string eid = dr["eid"].ToString();
                string owner_id = dr["owner_id"].ToString();
                string etype = dr["etype"].ToString();
                string cals = dr["cals"].ToString();
                string duration = dr["duration"].ToString();
                string date_ = dr["date_"].ToString();
                string description_ = dr["description_"].ToString();
                //MessageBox.Show($"{eid},{owner_id},{etype},{cals},{duration},{date_},{description_}");
                Exercise exercise = new Exercise(owner_id, etype, cals, duration, date_, description_);
                exercises.Add(exercise);
                //MessageBox.Show($"{date_},{Day1}");
                if (date_ == Day1.Date.ToString())
                {
                    d1_total = d1_total + int.Parse(cals);
                }
                else if (date_ == Day2.Date.ToString())
                {
                    d2_total = d2_total + int.Parse(cals);
                }
                else if (date_ == Day3.Date.ToString())
                {
                    d3_total = d3_total + int.Parse(cals);
                }
                else if (date_ == Day4.Date.ToString())
                {
                    d4_total = d4_total + int.Parse(cals);
                }
                else if (date_ == Day5.Date.ToString())
                {
                    d5_total = d5_total + int.Parse(cals);
                }
                else if (date_ == Day6.Date.ToString())
                {
                    d6_total = d6_total + int.Parse(cals);
                }
                else if (date_ == Day7.Date.ToString())
                {
                    d7_total = d7_total + int.Parse(cals);
                }
            }
            con.Close();



            d1_total = d1_total / goal;
            d2_total = d2_total / goal;
            d3_total = d3_total / goal;
            d4_total = d4_total / goal;
            d5_total = d5_total / goal;
            d6_total = d6_total / goal;
            d7_total = d7_total / goal;
            if  (d1_total >= goal/100)
            {
                button7.BackColor = Color.Purple;
            }
            else if (d1_total >= 0.75)
            {
                button7.BackColor = Color.Green;
                d1_total = Math.Round(d1_total * button7.Height);
                button7.Size = new Size(31, Convert.ToInt32(d1_total));
            }
            else
            {
                button7.BackColor = Color.Coral;
                d1_total = Math.Round(d1_total * button7.Height);
                button7.Size = new Size(31, Convert.ToInt32(d1_total));
            }

            if (d2_total >= goal / 100)
            {
                button6.BackColor = Color.Purple;
            }
            else if (d2_total >= 0.75)
            {
                button6.BackColor = Color.Green;
                d2_total = Math.Round(d2_total * button6.Height);
                button6.Size = new Size(31, Convert.ToInt32(d2_total));
            }
            else
            {
                button6.BackColor = Color.Coral;
                d2_total = Math.Round(d2_total * button6.Height);
                button6.Size = new Size(31, Convert.ToInt32(d2_total));
            }

            if (d3_total >= goal / 100)
            {
                button5.BackColor = Color.Purple;
            }
            else if (d3_total >= 0.75)
            {
                button5.BackColor = Color.Green;
                d3_total = Math.Round(d3_total * button5.Height);
                button5.Size = new Size(31, Convert.ToInt32(d3_total));
            }
            else
            {
                button5.BackColor = Color.Coral;
                d3_total = Math.Round(d3_total * button5.Height);
                button5.Size = new Size(31, Convert.ToInt32(d3_total));
            }

            if (d4_total >= goal / 100)
            {
                button4.BackColor = Color.Purple;
            }
            else if (d4_total >= 0.75)
            {
                button4.BackColor = Color.Green;
                d4_total = Math.Round(d1_total * button4.Height);
                button4.Size = new Size(31, Convert.ToInt32(d4_total));
            }
            else
            {
                MessageBox.Show($"{d4_total}");
                button4.BackColor = Color.Coral;
                d4_total = Math.Round(d4_total * button4.Height);
                button4.Size = new Size(31, Convert.ToInt32(d4_total));
            }

            if (d5_total >= goal / 100)
            {
                button3.BackColor = Color.Purple;
            }
            else if (d5_total >= 0.75)
            {
                button3.BackColor = Color.Green;
                d5_total = Math.Round(d5_total * button3.Height);
                button3.Size = new Size(31, Convert.ToInt32(d5_total));
            }
            else
            {
                button3.BackColor = Color.Coral;
                d5_total = Math.Round(d5_total * button3.Height);
                button3.Size = new Size(31, Convert.ToInt32(d5_total));
            }

            if (d6_total >= goal / 100)
            {
                button2.BackColor = Color.Purple;
            }
            else if (d6_total >= 0.75)
            {
                button2.BackColor = Color.Green;
                d6_total = Math.Round(d6_total * button2.Height);
                button2.Size = new Size(31, Convert.ToInt32(d6_total));
            }
            else
            {
                button2.BackColor = Color.Coral;
                d6_total = Math.Round(d6_total * button2.Height);
                button2.Size = new Size(31, Convert.ToInt32(d6_total));
            }

            if (d7_total >= goal / 100)
            {
                button1.BackColor = Color.Purple;
            }
            else if (d7_total >= 0.75)
            {
                button1.BackColor = Color.Green;
                d7_total = Math.Round(d7_total * button1.Height);
                button1.Size = new Size(31, Convert.ToInt32(d7_total));
            }
            else
            {
                button1.BackColor = Color.Coral;
                d7_total = Math.Round(d7_total * button1.Height);
                button1.Size = new Size(31, Convert.ToInt32(d7_total));
            }



        }
    }
}
