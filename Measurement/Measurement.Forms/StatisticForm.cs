using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class StatisticForm : Form
    {
        private DataTable _Table = null;

        private List<MeasurementStatistics.StatisticsItem> _StatisticItems = new List<MeasurementStatistics.StatisticsItem>();

        private bool _IsDelaySelectStatistics = false;

        private DateTime _DelaySelectStatisticsTime = DateTime.Now;

        private List<string> _ColumnsName = new List<string>()
        {
            "Index",
            "Name",
            "StartTime",
            "EndTime",
            "Result"
        };

        public StatisticForm()
        {
            InitializeComponent();
            InitTable();
        }

        private void InitTable()
        {
            _Table = new DataTable();
            for (int i = 0; i < _ColumnsName.Count; i++)
            {
                _Table.Columns.Add(_ColumnsName[i]);
            }
            _Table.Columns.Add("Obj", typeof(object));

            for (int i = 0; i < _ColumnsName.Count; i++)
            {
                dataGridView1.Columns[i].DataPropertyName = _ColumnsName[i];
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _Table;
        }

        private void StatisticForm_Load(object sender, EventArgs e)
        {
            MeasurementStatistics statistics = MeasurementStatistics.Load();
            if (statistics!=null)
            {
                _StatisticItems.Clear();
                for (int i = 0; i < statistics.Items.Count; i++)
                {
                    _StatisticItems.Add(statistics.Items[i]);
                }
                RefreshTable();
            }
        }

        private void RefreshTable()
        {
            int i = _Table.Rows.Count - _StatisticItems.Count;
            if (i>0)
            {
                for (int j = 0; j < i; j++)
                {
                    _Table.Rows.RemoveAt(_Table.Rows.Count - 1);
                }
            }
            else if (i<0)
            {
                i = -i;
                for (int j = 0; j < i; j++)
                {
                    _Table.Rows.Add(_Table.NewRow());
                }
            }
            for (int j = 0; j < _StatisticItems.Count; j++)
            {
                MeasurementStatistics.StatisticsItem item = _StatisticItems[j];
                DataRow dr = _Table.Rows[j];
                dr["Index"] = (j + 1).ToString();
                dr["Name"] = item.Name;
                dr["StartTime"] = item.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                dr["EndTime"] = item.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                dr["Result"] = item.Result?"OK":"NG";
                dr["Obj"] = item;
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            DelaySelectStatistics();
        }

        private void DelaySelectStatistics()
        {
            _DelaySelectStatisticsTime = DateTime.Now;
            if (!_IsDelaySelectStatistics)
            {
                _IsDelaySelectStatistics = true;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while ((DateTime.Now-_DelaySelectStatisticsTime).TotalMilliseconds<=200)
                    {
                    
                    }
                    try
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            if (dataGridView1.SelectedRows.Count>0)
                            {
                                DataGridViewRow dvr = dataGridView1.SelectedRows[0];
                                DataRowView drv = dvr.DataBoundItem as DataRowView;
                                DataRow dr = drv.Row;
                                MeasurementStatistics.StatisticsItem item = dr["Obj"] as MeasurementStatistics.StatisticsItem;
                                if (item!=null)
                                {
                                    MeasurementStatistics.StatisticsSideData data = MeasurementStatistics.StatisticsSideData.Load(item.StartTime);
                                    if (data!=null)
                                    {
                                        statisticDataPanel1.SideData = data.Sides.GetSide(0);
                                        statisticDataPanel2.SideData = data.Sides.GetSide(1);
                                        statisticDataPanel3.SideData = data.Sides.GetSide(2);
                                        statisticDataPanel4.SideData = data.Sides.GetSide(3);
                                        statisticDataPanel5.SideData = data.Sides.GetSide(4);
                                        statisticDataPanel6.SideData = data.Sides.GetSide(5);
                                        statisticDataPanel7.SideData = data.Sides.GetSide(6);
                                    }
                                    else
                                    {
                                        statisticDataPanel1.SideData = null;
                                        statisticDataPanel2.SideData = null;
                                        statisticDataPanel3.SideData = null;
                                        statisticDataPanel4.SideData = null;
                                        statisticDataPanel5.SideData = null;
                                        statisticDataPanel6.SideData = null;
                                        statisticDataPanel7.SideData = null;
                                    }
                                }
                                else
                                {
                                    statisticDataPanel1.SideData = null;
                                    statisticDataPanel2.SideData = null;
                                    statisticDataPanel3.SideData = null;
                                    statisticDataPanel4.SideData = null;
                                    statisticDataPanel5.SideData = null;
                                    statisticDataPanel6.SideData = null;
                                    statisticDataPanel7.SideData = null;
                                }
                            }
                            else
                            {
                                statisticDataPanel1.SideData = null;
                                statisticDataPanel2.SideData = null;
                                statisticDataPanel3.SideData = null;
                                statisticDataPanel4.SideData = null;
                                statisticDataPanel5.SideData = null;
                                statisticDataPanel6.SideData = null;
                                statisticDataPanel7.SideData = null;
                            }
                        }));
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                    _IsDelaySelectStatistics = false;
                });
            }
        }
    }
}
