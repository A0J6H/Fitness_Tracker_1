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
                    Values = new ChartValues<int> {d1_total,d2_total,d3_total,d4_total,d5_total,d6_total,d7_total},
                    Stroke = System.Windows.Media.Brushes.DarkBlue,
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1

                },
                new LineSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<int> {d1_act,d2_act,d3_act,d4_act,d5_act,d6_act,d7_act},
                    Stroke = System.Windows.Media.Brushes.Coral,
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1
                },
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double> {5, 2, 8, 3,4},
                //
                //    PointGeometry = null ,
                //    PointGeometrySize = 15
                //}
            };
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = new[] {Day1.DayOfWeek.ToString(),Day2.DayOfWeek.ToString(),Day3.DayOfWeek.ToString(),Day4.DayOfWeek.ToString(),Day5.DayOfWeek.ToString(),Day6.DayOfWeek.ToString(),Day7.DayOfWeek.ToString()},
                Foreground = System.Windows.Media.Brushes.Coral
            });

            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Cals",
                Foreground = System.Windows.Media.Brushes.Coral
                //LabelFormatter = value => value.ToString("C")
            });

            solidGauge1.Uses360Mode = true;
            solidGauge1.From = 0;
            solidGauge1.To = 100;
            solidGauge1.Value = 50;


        }
    }
}
