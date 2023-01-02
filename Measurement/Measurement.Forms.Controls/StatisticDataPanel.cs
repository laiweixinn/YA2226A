using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class StatisticDataPanel : UserControl
    {
        private DataTable _Table = null;

        private List<string> _ColumnsName = new List<string>()
        {
            "Index",
            "H1",
            "H2",
            "H3",
            "H4",
            "W",
            "H",
            "Res"
        };

        private MeasurementStatistics.StatisticsSideItem _SideData = null;

        private MeasurementStatistics.StatisticsSideDataCollection _Datas = null;

        public MeasurementStatistics.StatisticsSideItem SideData
        {
            get
            {
                return _SideData;
            }
            set
            {
                _SideData = value;
                if (_SideData!=null)
                {
                    Datas = _SideData.Datas;
                    label1.Text = string.Format("结果：{0}", _SideData.Result ? "OK" : "NG");
                }
                else
                {
                    Datas = null;
                    label1.Text = "结果：无数据";
                }
            }
        }

        public MeasurementStatistics.StatisticsSideDataCollection Datas
        {
            get
            {
                return _Datas;
            }
            set
            {
                _Datas = value;
                RefreshTable();

            }
        }

        public StatisticDataPanel()
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
        
        private void RefreshTable()
        {
            MeasurementStatistics.StatisticsSideDataCollection datas = _Datas;
            if (_Datas==null)
            {
                _Table.Rows.Clear();
            }
            else
            {
                int i =  datas.Count- _Table.Rows.Count;
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        _Table.Rows.Add(_Table.NewRow());
                    }
                }
                else if (i < 0)
                {
                    i = -i;
                    for (int j = 0; j < i; j++)
                    {
                        _Table.Rows.RemoveAt(_Table.Rows.Count - 1);
                    }
                }

                for (int j = 0; j < _Table.Rows.Count; j++)
                {
                    MeasurementStatistics.StatisticsSideDataItem data = datas[j];
                    DataRow dr = _Table.Rows[j];
                    dr["Index"] = (j+1).ToString();
                    dr["H1"] = ((!data.H1Invalid)?data.H1.ToString("0.###"):"--");
                    dr["H2"] = ((!data.H2Invalid) ? data.H2.ToString("0.###") : "--");
                    dr["H3"] = ((!data.H3Invalid) ? data.H3.ToString("0.###") : "--");
                    dr["H4"] = ((!data.H4Invalid) ? data.H4.ToString("0.###") : "--");
                    dr["W"] = ((!data.DInvalid) ? data.D.ToString("0.###") : "--");
                    dr["H"] = ((!data.GHInvalid) ? data.GH.ToString("0.###") : "--");
                    dr["Res"] = data.ResultString;
                    dr["Obj"] = data;
                }
            }
        }
    }
}
