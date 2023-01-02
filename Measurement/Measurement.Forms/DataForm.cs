using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;
using System.IO;
using DY.Core.Configs;


namespace LZ.CNC.Measurement.Forms
{
    public partial class DataForm : Form
    {
        private List<DatasCollections.DetectDataItem> _DetectDataItems = null;
        private BindingList<DatasCollections.DetectDataItem> _BdingDetectDataItems = null;

        public DataForm()
        {
            InitializeComponent();
        }

        DatasCollections detect;
        private void Rebding()
        {
            RefreshData();
            if (_DetectDataItems != null)
            {
                _BdingDetectDataItems = new BindingList<DatasCollections.DetectDataItem>(_DetectDataItems);
                dgv_detectdatas.DataSource = _BdingDetectDataItems;

                dgv_detectdatas.Columns[0].HeaderText = "时间";
                dgv_detectdatas.Columns[1].HeaderText = "产品";
                dgv_detectdatas.Columns[2].HeaderText = "平台";
                dgv_detectdatas.Columns[3].HeaderText = "X/Y1";
                dgv_detectdatas.Columns[4].HeaderText = "Y/Y2";
                dgv_detectdatas.Columns[5].HeaderText = "结果";
                dgv_detectdatas.Columns[6].HeaderText = "NG原因";
                dgv_detectdatas.Columns[7].HeaderText = "拉力/N";

                dgv_detectdatas.Columns[0].FillWeight = 120;
                dgv_detectdatas.Columns[1].FillWeight = 120;
                dgv_detectdatas.Columns[2].FillWeight = 60;
                dgv_detectdatas.Columns[3].FillWeight = 60;
                dgv_detectdatas.Columns[4].FillWeight = 60;
                dgv_detectdatas.Columns[5].FillWeight = 60;
                dgv_detectdatas.Columns[6].FillWeight = 120;
                dgv_detectdatas.Columns[7].FillWeight = 60;
            }
            
        }

        private void RefreshData()
        {

            double ngnum = 0.0;
            int allnum = _DetectDataItems.Count;
            foreach (DatasCollections.DetectDataItem item in _DetectDataItems)
            {
                if (item.Result == "NG")
                {
                    ngnum++;
                }
            }

            lbl_allnum.Text = allnum.ToString();
            lbl_ngnum.Text = ngnum.ToString();

            if (allnum > 0)
            {
                double tem = (allnum - ngnum) / allnum;
                lbl_percent.Text = string.Format("{0}%", (tem * 100).ToString("0.00"));
            }
            else
            {
                lbl_percent.Text = "无记录";
            }


        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            try
            {
                detect = DatasCollections.Load();
                if (detect == null)
                {
                    detect = new DatasCollections();
                    detect.Save();
                    return;
                }

                _DetectDataItems = detect.DetectDatas;
                Rebding();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                string selectname = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd")+".dds";
                string path = Path.Combine(ConfigBase.GetApplicationPath("detectdatas"), selectname);
                detect = ConfigBase.Load(path) as DatasCollections;
                _DetectDataItems = detect.DetectDatas;

               
                Rebding();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
