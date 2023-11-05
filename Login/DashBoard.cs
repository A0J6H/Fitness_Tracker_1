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
using Login.Properties;
using LiveCharts.WinForms;
using System.Windows.Media;

namespace Login
{
    public partial class DashBoard : Form
    {
        SqlConnection con = new SqlConnection("Data Source=desktop-ngihs18;Initial Catalog=master;Integrated Security=True");
        SqlCommand cmd;
        SqlDataReader dr;
        bool open = false;
        //Days for the graph 
        DateTime Day1 = DateTime.Now.AddDays(0);
        DateTime Day2 = DateTime.Now.AddDays(-1);
        DateTime Day3 = DateTime.Now.AddDays(-2);
        DateTime Day4 = DateTime.Now.AddDays(-3);
        DateTime Day5 = DateTime.Now.AddDays(-4);
        DateTime Day6 = DateTime.Now.AddDays(-5);
        DateTime Day7 = DateTime.Now.AddDays(-6);


        int d1_total = 0;
        int d2_total = 0;
        int d3_total = 0;
        int d4_total = 0;
        int d5_total = 0;
        int d6_total = 0;
        int d7_total = 0;

        int d1_act = 0;
        int d2_act = 0;
        int d3_act = 0;
        int d4_act = 0;
        int d5_act = 0;
        int d6_act = 0;
        int d7_act = 0;

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
                    d1_total += int.Parse(cals);
                }
                else if (date_ == Day2.Date.ToString())
                {
                    d2_total += int.Parse(cals);
                }
                else if (date_ == Day3.Date.ToString())
                {
                    d3_total += int.Parse(cals);
                }
                else if (date_ == Day4.Date.ToString())
                {
                    d4_total += int.Parse(cals);
                }
                else if (date_ == Day5.Date.ToString())
                {
                    d5_total += int.Parse(cals);
                }
                else if (date_ == Day6.Date.ToString())
                {
                    d6_total += int.Parse(cals);
                }
                else if (date_ == Day7.Date.ToString())
                {
                    d7_total += int.Parse(cals);
                }
                /////////////////////////////////////////////
                if (date_ == Day1.Date.ToString())
                {
                    d1_act += int.Parse(duration);
                }
                else if (date_ == Day2.Date.ToString())
                {
                    d2_act += int.Parse(duration);
                }
                else if (date_ == Day3.Date.ToString())
                {
                    d3_act += int.Parse(duration);
                }
                else if (date_ == Day4.Date.ToString())
                {
                    d4_act += int.Parse(duration);
                }
                else if (date_ == Day5.Date.ToString())
                {
                    d5_act += int.Parse(duration);
                }
                else if (date_ == Day6.Date.ToString())
                {
                    d6_act += int.Parse(duration);
                }
                else if (date_ == Day7.Date.ToString())
                {
                    d7_act += int.Parse(duration);
                }
            }
            con.Close();

            cartesianChart1.Series = new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 27, 57)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1

                },
                new LineSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<int> {d7_act,d6_act,d5_act,d4_act,d3_act,d2_act,d1_act},
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1
                }
            };
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = new[] {Day7.DayOfWeek.ToString(),Day6.DayOfWeek.ToString(),Day5.DayOfWeek.ToString(),Day4.DayOfWeek.ToString(),Day3.DayOfWeek.ToString(),Day2.DayOfWeek.ToString(),Day1.DayOfWeek.ToString()},
                
                Foreground = System.Windows.Media.Brushes.Coral
            });

            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Foreground = System.Windows.Media.Brushes.Coral
                //LabelFormatter = value => value.ToString("C")
            });

            cartesianChart2.Series = new LiveCharts.SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 27, 57))

                },
                new ColumnSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<int> {d7_act,d6_act,d5_act,d4_act,d3_act,d2_act,d1_act},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51))
                }
            };
            cartesianChart2.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = new[] { Day7.DayOfWeek.ToString(), Day6.DayOfWeek.ToString(), Day5.DayOfWeek.ToString(), Day4.DayOfWeek.ToString(), Day3.DayOfWeek.ToString(), Day2.DayOfWeek.ToString(), Day1.DayOfWeek.ToString() },

                Foreground = System.Windows.Media.Brushes.Coral
            });

            cartesianChart2.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Foreground = System.Windows.Media.Brushes.Coral
                //LabelFormatter = value => value.ToString("C")
            });
            ///////////////////uno////////////////////////
            solidGauge1.Uses360Mode = true;
            solidGauge1.From = 0;
            solidGauge1.To = goal;
            if (d7_total >= goal * 1.2)
            {
                solidGauge1.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge1.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d7_total >= goal)
            {
                solidGauge1.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge1.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge1.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge1.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge1.Value = d7_total;
            label3.Text = Day7.DayOfWeek.ToString();
            ///////////////////////////////////////////
            //////////////////////dos////////////////////////
            solidGauge2.Uses360Mode = true;
            solidGauge2.From = 0;
            solidGauge2.To = goal;
            if (d6_total >= goal * 1.2)
            {
                solidGauge2.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge2.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d6_total >= goal)
            {
                solidGauge2.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge2.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge2.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge2.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge2.Value = d6_total;
            label4.Text = Day6.DayOfWeek.ToString();
            ///////////////////////////////////////////
            //////////////////////3////////////////////////
            solidGauge3.Uses360Mode = true;
            solidGauge3.From = 0;
            solidGauge3.To = goal;
            if (d5_total >= goal * 1.2)
            {
                solidGauge3.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge3.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d5_total >= goal)
            {
                solidGauge3.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge3.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge3.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge3.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge3.Value = d5_total;
            label5.Text = Day5.DayOfWeek.ToString();
            ///////////////////////////////////////////
            //////////////////////4////////////////////////
            solidGauge4.Uses360Mode = true;
            solidGauge4.From = 0;
            solidGauge4.To = goal;
            if (d4_total >= goal * 1.2)
            {
                solidGauge4.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge4.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d4_total >= goal)
            {
                solidGauge4.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge4.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge4.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge4.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge4.Value = d4_total;
            label6.Text = Day4.DayOfWeek.ToString();
            ///////////////////////////////////////////
            //////////////////////5////////////////////////
            solidGauge5.Uses360Mode = true;
            solidGauge5.From = 0;
            solidGauge5.To = goal;
            if (d3_total >= goal * 1.2)
            {
                solidGauge5.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge5.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d3_total >= goal)
            {
                solidGauge5.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge5.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge5.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge5.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge5.Value = d3_total;
            label7.Text = Day3.DayOfWeek.ToString();
            ///////////////////////////////////////////
            //////////////////////6////////////////////////
            solidGauge6.Uses360Mode = true;
            solidGauge6.From = 0;
            solidGauge6.To = goal;
            if (d2_total >= goal * 1.2)
            {
                solidGauge6.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge6.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));

            }
            else if (d2_total >= goal)
            {
                solidGauge6.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge6.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
            }
            else
            {
                solidGauge6.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge6.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
            }
            solidGauge6.Value = d2_total;
            label8.Text = Day2.DayOfWeek.ToString();
            ///////////////////////////////////////////

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (open == false)
            {
                button10.Image = System.Drawing.Image.FromFile("C:\\Users\\Andrew\\source\\repos\\Login\\Login\\Resources\\menu-4-48 (1)_flipped.png");
                button10.Location = new Point(53, 92);
                pictureBox2.Size = new Size(110, 21);
                pictureBox2.Location = new Point(33, -4);
                flowLayoutPanel1.Size = new Size(178, 542);
                pictureBox3.Location = new Point(42, -7);
                open = true;
            }
            else
            {
                button10.Image = System.Drawing.Image.FromFile("C:\\Users\\Andrew\\source\\repos\\Login\\Login\\Resources\\menu-4-48 (1).png");
                button10.Location = new Point(10, 92);
                flowLayoutPanel1.Size = new Size(90, 542);
                pictureBox2.Size = new Size(58, 21);
                pictureBox2.Location = new Point(15, -4);
                pictureBox3.Location = new Point(0, -7);
                open = false;

            }
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to log out ?","Log out", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Hide();
                Login next = new Login();
                next.Show();
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            cartesianChart2.Visible = true; cartesianChart1.Visible = false;

            cartesianChart2.Series = new LiveCharts.SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 27, 57))

                },
                new ColumnSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<int> {d7_act,d6_act,d5_act,d4_act,d3_act,d2_act,d1_act},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51))
                }
            };
        }

        private void button14_Click(object sender, EventArgs e)
        {
            cartesianChart2.Visible = false; cartesianChart1.Visible = true;

            cartesianChart1.Series = new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 27, 57)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1

                },
                new LineSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<int> {d7_act,d6_act,d5_act,d4_act,d3_act,d2_act,d1_act},
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1
                }
            };
        }
    }
}
