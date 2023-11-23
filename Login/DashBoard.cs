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
using System.Windows;
using System.Xml.Linq;
using System.Deployment.Internal;
using System.Diagnostics;

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

        string time = "Week";

        bool column = false;

        bool home = false;
        bool add = false;
        bool update_need = false;

        string ID = Login.ID;
        public DashBoard()
        {
            InitializeComponent();
            

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            string eid = string.Empty;
            string owner_id = string.Empty;
            string etype = string.Empty;
            string cals = string.Empty;
            string duration = string.Empty;
            string date_ = string.Empty;
            string description_ = string.Empty;
            d1_total = 0;
            d2_total = 0;
            d3_total = 0;
            d4_total = 0;
            d5_total = 0;
            d6_total = 0;
            d7_total = 0;

            d1_act = 0;
            d2_act = 0;
            d3_act = 0;
            d4_act = 0;
            d5_act = 0;
            d6_act = 0;
            d7_act = 0;
            exercises.Clear();
            Add_Panel.Location = new System.Drawing.Point(90, 3);

            cmd = new SqlCommand("SELECT * FROM exercise_log WHERE owner_id = @user_id ORDER BY date_ ASC", con);
            cmd.Parameters.AddWithValue("user_id", ID);
            con.Open();
            dr = cmd.ExecuteReader();
            int goal = 100;

            while (dr.Read()){
                eid = dr["eid"].ToString();
                owner_id = dr["owner_id"].ToString();
                etype = dr["etype"].ToString();
                cals = dr["cals"].ToString();
                duration = dr["duration"].ToString();
                date_ = dr["date_"].ToString();
                description_ = dr["description_"].ToString();
                //MessageBox.Show($"{eid},{owner_id},{etype},{cals},{duration},{date_},{description_}");
                Exercise exercise = new Exercise(owner_id, etype, cals, duration, date_, description_);
                exercises.Add(exercise);
                //MessageBox.Show($"{date_},{Day1}");
                if (date_ == Day1.Date.ToString())
                {
                    d1_total += int.Parse(cals);
                    d1_act += int.Parse(duration);
                }
                else if (date_ == Day2.Date.ToString())
                {
                    d2_total += int.Parse(cals);
                    d2_act += int.Parse(duration);
                }
                else if (date_ == Day3.Date.ToString())
                {
                    d3_total += int.Parse(cals);
                    d3_act += int.Parse(duration);
                }
                else if (date_ == Day4.Date.ToString())
                {
                    d4_total += int.Parse(cals);
                    d4_act += int.Parse(duration);
                }
                else if (date_ == Day5.Date.ToString())
                {
                    d5_total += int.Parse(cals);
                    d5_act += int.Parse(duration);
                }
                else if (date_ == Day6.Date.ToString())
                {
                    d6_total += int.Parse(cals);
                    d6_act += int.Parse(duration);
                }
                else if (date_ == Day7.Date.ToString())
                {
                    d7_total += int.Parse(cals);
                    d7_act += int.Parse(duration);
                }
            }
            con.Close();
            //Get username using dr.Read()//
            cmd = new SqlCommand("select * from users where id=@ID", con);
            cmd.Parameters.AddWithValue("@id", ID);
            con.Open();
            dr = cmd.ExecuteReader();
            string name = string.Empty;
            dr.Read();
            name = dr["usernam"].ToString();
            con.Close();
            /////////Set Label as Name//////
            label12.Text = $"{name}'s Overview";
 
            //////////////////////////////////////////////////////////////////
            int b = 0;
            int len = exercises.Count;
            int amount = 30;
            List <double> temp_cal = new List <double>();
            List<string> temp_date = new List<string>();
            List<double> temp_time = new List<double>();
            double[] values = new double[len];
            string[] dates = new string[amount];
            double[] atime = new double[len];
            for (int i=0;i< amount; i++)
            {
                string f = Day1.AddDays(i-amount).Date.ToString("yyyy-MM-dd");
                temp_date.Add(f);
            }

            dates = temp_date.ToArray();

            while (b < amount)
            {
                int sub_days = b- amount;
                int x = 0;
                int y = 0;
                DateTime check_date = DateTime.Now.AddDays(sub_days);
                for (int i =0; i < exercises.Count; i++)
                {
                    if (exercises[i].date_.ToString() == check_date.Date.ToString())
                    {
                        
                        x += int.Parse(exercises[i].cals);
                        y += int.Parse(exercises[i].duration);
                    }
                }
                temp_cal.Add(x);
                temp_time.Add(y);
                b += 1;
            }

            values = temp_cal.ToArray();
            atime = temp_time.ToArray();

            cartesianChart3.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels =  dates ,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = System.Windows.Media.Brushes.Coral
            });
            cartesianChart3.Series = new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<double>(values) ,
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16,45,85)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1

                },
                new LineSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<double>(atime) ,
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1
                }


            };
            cartesianChart4.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = dates,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = System.Windows.Media.Brushes.Coral
            });
            cartesianChart4.Series = new LiveCharts.SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<double>(values) ,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16,45,85))

                },
                new ColumnSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<double>(atime) ,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51))
                }


            };

            ///////////////////////////////////////////////////////////////////////////////////////////////
            int ylen = exercises.Count;
            int yamount = 365;
            List<double> ytemp_cal = new List<double>();
            List<string> ytemp_date = new List<string>();
            List<double> ytemp_time = new List<double>();
            double[] yvalues = new double[ylen];
            string[] ydates = new string[yamount];
            double[] yatime = new double[ylen];
            for (int i = 0; i < amount; i++)
            {
                string f = Day1.AddDays(i-yamount).Date.ToString("yyyy-MM-dd");
                ytemp_date.Add(f);
            }

            ydates = ytemp_date.ToArray();

            while (b < yamount)
            {
                int sub_days = b - yamount;
                int x = 0;
                int y = 0;
                DateTime check_date = DateTime.Now.AddDays(sub_days);
                for (int i = 0; i < exercises.Count; i++)
                {
                    if (exercises[i].date_.ToString() == check_date.Date.ToString())
                    {

                        x += int.Parse(exercises[i].cals);
                        y += int.Parse(exercises[i].duration);
                    }
                }
                ytemp_cal.Add(x);
                ytemp_time.Add(y);
                b += 1;
            }


            yvalues = ytemp_cal.ToArray();
            yatime = ytemp_time.ToArray();

            cartesianChart5.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = ydates,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = System.Windows.Media.Brushes.Coral
            });
            cartesianChart5.Series = new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<double>(yvalues) ,
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 45, 85)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1

                },
                new LineSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<double>(yatime) ,
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51)),
                    PointGeometry  =  DefaultGeometries.Circle ,
                    PointGeometrySize = 1
                }


            };
            cartesianChart6.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = ydates,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = System.Windows.Media.Brushes.Coral
            });
            cartesianChart6.Series = new LiveCharts.SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<double>(yvalues) ,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 45, 85))

                },
                new ColumnSeries
                {
                    Title = "Active Minutes",
                    Values = new ChartValues<double>(yatime) ,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51))
                }
            };
            cartesianChart5.Pan = PanningOptions.X;
            ///////////////////////////////////////////////////////////////////////////////////////////////
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Days",
                Labels = new[] { Day7.DayOfWeek.ToString(), Day6.DayOfWeek.ToString(), Day5.DayOfWeek.ToString(), Day4.DayOfWeek.ToString(), Day3.DayOfWeek.ToString(), Day2.DayOfWeek.ToString(), Day1.DayOfWeek.ToString() },

                Foreground = System.Windows.Media.Brushes.Coral
            });
            cartesianChart1.Series = new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 45, 85)),
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



            cartesianChart2.Series = new LiveCharts.SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Calories Burnt",
                    Values = new ChartValues<int> {d7_total,d6_total,d5_total,d4_total,d3_total,d2_total,d1_total},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 45, 85))

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
            solidGauge1.HighFontSize = 11;
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
            label3.Text = Day7.DayOfWeek.ToString().Substring(0, 3);
            ///////////////////////////////////////////
            //////////////////////dos////////////////////////
            solidGauge2.Uses360Mode = true;
            solidGauge2.From = 0;
            solidGauge2.To = goal;
            solidGauge2.HighFontSize = 11;
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
            label4.Text = Day6.DayOfWeek.ToString().Substring(0, 3);
            ///////////////////////////////////////////
            //////////////////////3////////////////////////
            solidGauge3.Uses360Mode = true;
            solidGauge3.From = 0;
            solidGauge3.To = goal;
            solidGauge3.HighFontSize = 11;
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
            label5.Text = Day5.DayOfWeek.ToString().Substring(0, 3);
            ///////////////////////////////////////////
            //////////////////////4////////////////////////
            solidGauge4.Uses360Mode = true;
            solidGauge4.From = 0;
            solidGauge4.To = goal;
            solidGauge4.HighFontSize = 11;
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
            label6.Text = Day4.DayOfWeek.ToString().Substring(0, 3);
            ///////////////////////////////////////////
            //////////////////////5////////////////////////
            solidGauge5.Uses360Mode = true;
            solidGauge5.From = 0;
            solidGauge5.To = goal;
            solidGauge5.HighFontSize = 11;
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
            label7.Text = Day3.DayOfWeek.ToString().Substring(0,3);
            ///////////////////////////////////////////
            //////////////////////6////////////////////////
            solidGauge6.Uses360Mode = true;
            solidGauge6.From = 0;
            solidGauge6.To = goal;
            solidGauge6.HighFontSize = 11;
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
            label8.Text = Day2.DayOfWeek.ToString().Substring(0, 3);
            ///////////////////////////////////////////
            //////////////////////7////////////////////////
            solidGauge7.Uses360Mode = true;
            solidGauge7.From = 0;
            solidGauge7.To = goal;
            solidGauge7.HighFontSize = 1;
            if (d1_total >= goal * 1.2)
            {
                solidGauge7.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge7.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                label13.ForeColor = System.Drawing.Color.FromArgb(0, 207, 4, 203);

            }
            else if (d1_total >= goal)
            {
                solidGauge7.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge7.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                label13.ForeColor = System.Drawing.Color.FromArgb(0, 6, 204, 16);
            }
            else
            {
                solidGauge7.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge7.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                
            }
            solidGauge7.Value = d1_total;
            ///////////////////////////////////////////
            //////////////////////8////////////////////////
            solidGauge8.Uses360Mode = true;
            solidGauge8.From = 0;
            solidGauge8.To = goal/4 ;
            solidGauge8.HighFontSize = 1;
            if (d1_total >= goal/4 * 1.2)
            {
                solidGauge8.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                solidGauge8.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(207, 4, 203));
                label14.ForeColor = System.Drawing.Color.FromArgb(0, 207, 4, 203);

            }
            else if (d1_total >= goal / 4)
            {
                solidGauge8.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                solidGauge8.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 204, 16));
                label13.ForeColor = System.Drawing.Color.FromArgb(0, 6, 204, 16);
            }
            else
            {
                solidGauge8.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));
                solidGauge8.Base.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51));

            }
            solidGauge8.Value = d1_act;
            ///////////////////////////////////////////
            label13.Text = $"{d1_total} / {goal}";
            label14.Text = $"{d1_act} / {goal/4}";
            ////////////////Dog nut ting//////////////
            List<string> exercise_types = new List<string>();
            string ty = string.Empty ;
            string ct = string.Empty ;
            cmd = new SqlCommand("SELECT etype,Count(eid) FROM exercise_log WHERE owner_id = @user_id GROUP BY etype Order by COUNT(eid) DESC;", con);
            cmd.Parameters.AddWithValue("user_id", ID);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ty = dr["etype"].ToString();
                exercise_types.Add(ty);
                ct = dr[1].ToString();
                exercise_types.Add(ct);
            }
            con.Close();
            while (exercise_types.Count() < 6)
            {
                exercise_types.Add("N/A");
                exercise_types.Add("0");
            }
            pieChart1.InnerRadius = 30;
            Func<ChartPoint, string> labelPoint = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            pieChart1.Series = new LiveCharts.SeriesCollection
            {
                new PieSeries
                {
                    Title = $"{exercise_types[0]}",
                    Values = new ChartValues<int> {int.Parse(exercise_types[1])},
                    //PushOut = 15,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 102, 51)),
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(153, 153, 153)),
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = $"{exercise_types[2]}",
                    Values = new ChartValues<int> {int.Parse(exercise_types[3])},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240,127,117)),
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(153, 153, 153)),
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = $"{exercise_types[4]}",
                    Values = new ChartValues<double> {int.Parse(exercise_types[5])},
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 27, 57)),
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(153, 153, 153)),
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };
            //reminder label//
            int total_ds = d1_total+d2_total+d3_total+d4_total+d5_total+d6_total+d7_total;
            if(total_ds >= goal*8)
            {
                label22.Text = $"Well done you smashed your weekly goal {total_ds}/{goal*7} !";
            }
            else if (total_ds >= goal * 7)
            {
                label22.Text = $"Phew you just managed to hit your weekly goal!\n                         You can relax now :) ";
            }
            else if (total_ds >= goal * 6)
            {
                label22.Text = $"Your just under your weekly goal,\n be sure to check out some workouts below to help you hit it !";
            }
            else
            {
                label22.Text = $"Staying active can make significant improvments in your \n                        mental and phyisical wellbeing.\n                              Be sure to keep active! ";
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (open == false)
            {
                button10.Image = System.Drawing.Image.FromFile("C:\\Users\\Andrew\\source\\repos\\Login\\Login\\Resources\\menu-4-48 (1)_flipped.png");
                button10.Location = new System.Drawing.Point(63, 92);
                pictureBox2.Size = new System.Drawing.Size(120, 21);
                pictureBox2.Location = new System.Drawing.Point(33, -4);
                flowLayoutPanel1.Size = new System.Drawing.Size(202, 542);
                pictureBox3.Location = new System.Drawing.Point(57, -7);
                open = true;
            }
            else
            {
                button10.Image = System.Drawing.Image.FromFile("C:\\Users\\Andrew\\source\\repos\\Login\\Login\\Resources\\menu-4-48 (1).png");
                button10.Location = new System.Drawing.Point(10, 92);
                flowLayoutPanel1.Size = new System.Drawing.Size(90, 542);
                pictureBox2.Size = new System.Drawing.Size(58, 21);
                pictureBox2.Location = new System.Drawing.Point(15, -4);
                pictureBox3.Location = new System.Drawing.Point(0, -7);
                open = false;

            }
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to log out ?","Log out", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Hide();
                Login next = new Login();
                next.Show();
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            column = true;
            cartesianChart1.Visible = false; cartesianChart2.Visible = false; cartesianChart3.Visible = false; cartesianChart4.Visible = false; cartesianChart5.Visible = false; cartesianChart6.Visible = false;
            switch (time)
            {
                case "Week":
                    if (column == true)
                    {
                        cartesianChart2.Visible = true;
                    }
                    else
                    {
                        cartesianChart1.Visible = true;
                    }

                    break;
                case "Month":
                    if (column == true)
                    {
                        cartesianChart4.Visible = true;
                    }
                    else
                    {
                        cartesianChart3.Visible = true;
                    }
                    break;
                case "Year":

                    if (column == true)
                    {
                        cartesianChart6.Visible = true;
                    }
                    else
                    {
                        cartesianChart5.Visible = true;
                    }


                    break;
            };
        }

        private void button14_Click(object sender, EventArgs e)
        {
            column = false;
            cartesianChart1.Visible = false; cartesianChart2.Visible = false; cartesianChart3.Visible = false; cartesianChart4.Visible = false; cartesianChart5.Visible =false; cartesianChart6.Visible =false;
            switch (time)
            {
                case "Week":
                    if (column == true)
                    {
                        cartesianChart2.Visible = true;
                    }
                    else
                    {
                        cartesianChart1.Visible = true;
                    }

                    break;
                case "Month":
                    if (column == true)
                    {
                        cartesianChart4.Visible = true;
                    }
                    else
                    {
                        cartesianChart3.Visible = true;
                    }
                    break;
                case "Year":

                    if (column == true)
                    {
                        cartesianChart6.Visible = true;
                    }
                    else
                    {
                        cartesianChart5.Visible = true;
                    }

                    break;
            };
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //right week month year 
            cartesianChart1.Visible = false; cartesianChart2.Visible = false; cartesianChart3.Visible = false; cartesianChart4.Visible = false; cartesianChart5.Visible = false; cartesianChart6.Visible = false;
            switch (time)
            {
                case "Week":
                    label9.Text = "Month";
                    time = "Month";
                    if (column == true)
                    {
                        cartesianChart4.Visible = true;
                    }
                    else
                    {
                        cartesianChart3.Visible = true;
                    }
                    
                    break;
                case "Month":
                    label9.Text = "Year";
                    time = "Year";
                    if (column == true)
                    {
                        cartesianChart6.Visible = true;
                    }
                    else
                    {
                        cartesianChart5.Visible = true;
                    }

                    break;
                case "Year":
                    label9.Text = "Week";
                    time = "Week";
                    if (column == true)
                    {
                        cartesianChart2.Visible = true;
                    }
                    else
                    {
                        cartesianChart1.Visible = true;
                    }
                    
                    break;
            };
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //left week year month
            cartesianChart1.Visible = false; cartesianChart2.Visible = false; cartesianChart3.Visible = false; cartesianChart4.Visible = false; cartesianChart5.Visible = false; cartesianChart6.Visible = false;
            switch (time)
            {
                case "Week":
                    label9.Text = "Year";
                    time = "Year";
                    if (column == true)
                    {
                        cartesianChart6.Visible = true;
                    }
                    else
                    {
                        cartesianChart5.Visible = true;
                    }

                    break;
                case "Month":
                    label9.Text = "Week";
                    time = "Week";
                    if (column == true)
                    {
                        cartesianChart2.Visible = true;
                    }
                    else
                    {
                        cartesianChart1.Visible = true;
                    }
                    break;
                case "Year":
                    label9.Text = "Month";
                    time = "Month";
                    if (column == true)
                    {
                        cartesianChart4.Visible = true;
                    }
                    else
                    {
                        cartesianChart3.Visible = true;
                    }
                    break;
            };
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (home == false)
            {
                home_panel.Visible = true;
                Add_Panel.Visible = false;
                home = true;
                add = false;
                Activity_timer.Stop();
                if (update_need == true)
                {
                    update_need = false;
                    this.Controls.Clear();
                    this.InitializeComponent();
                    DashBoard_Load(this, null);
                    home_panel.Visible=false;
                    home_panel.Visible=true;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //size = 788, 581 // location of home_panel 90, 3
            if (add == false)
            {
                Add_Panel.Visible = true;
                home_panel.Visible = false;
                add = true;
                home = false;
                Activity_timer.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime tdate = dateTimePicker1.Value.Date;
                string ndate = tdate.ToString("yyyy-MM-dd");
                cmd = new SqlCommand("INSERT INTO exercise_log (owner_id,etype,cals,duration,date_,description_) VALUES (@oid,@etype,@cals,@dur,@date,@description);", con);
                cmd.Parameters.AddWithValue("@oid", int.Parse(ID));
                cmd.Parameters.AddWithValue("@etype", comboBox2.Text.ToString());
                cmd.Parameters.AddWithValue("@cals", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@dur", int.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@date", ndate);
                cmd.Parameters.AddWithValue("@description", comboBox1.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                update_need = true;
                comboBox2.Text = string.Empty;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                comboBox1.Text = string.Empty;
                dateTimePicker1.ResetText();

            }
            catch 
            {
                System.Windows.MessageBox.Show("Enter valid responces");
            }

        }

        private void Activity_timer_Tick(object sender, EventArgs e)
        {
            //intensity label//
            if (comboBox1.Text == "Low" | comboBox1.Text == "Medium" | comboBox1.Text == "High")
            {
                label20.Text = "Intensity";
            }
            else
            {
                label20.Text = "Intensity*";
            }
            //Sport label//
            if (comboBox2.Text == string.Empty)
            {
                label1.Text = "Type of Exercise*";
            }
            else
            {
                label1.Text = "Type of Exercise";
            }
            //Cals label//
            try
            {
                int x = int.Parse(textBox1.Text);
                label17.Text = "Calories Burnt";

            }
            catch
            {
                label17.Text = "Calories Burnt*";
            }
            //duration label//
            try
            {
                int y = int.Parse(textBox2.Text);
                label18.Text = "Duration";

            }
            catch
            {
                label18.Text = "Duration*";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.sport-fitness-advisor.com/dumbbellexercises.html");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.healthline.com/health/exercise-fitness/proper-running-form");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.ultrafootball.com/blogs/ultra-mag/best-solo-football-drills");
        }
    }
}
