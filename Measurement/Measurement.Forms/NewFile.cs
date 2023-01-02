using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DY.Core.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class NewFile : Form
    {

        private MeasurementData.RecipeDataItem _DataItem = null;

        public MeasurementData.RecipeDataItem DataItem
        {
            get
            {
                return _DataItem;
            }
        }

        public NewFile(MeasurementData.RecipeDataItem dataItem)
        {
            InitializeComponent();
            _DataItem = dataItem;
            if (_DataItem==null)
            {
                Text = "新建";
                strtxt_filename.Tips = "新建文件名称：";
            }
            else
            {
                Text = "编辑";
                strtxt_filename.Tips = "重新编辑名称：";
                strtxt_filename.Text = _DataItem.Name;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            string _name = strtxt_filename.Text.Trim();
            if (string.IsNullOrEmpty(_name))
            {
                MessageBoxEx.ShowErrorMessage("请输入名称！");
            }
            else
            {
                if (_DataItem==null)
                {
                    _DataItem = new MeasurementData.RecipeDataItem();
                    _DataItem = MeasurementData.Clone(MeasurementContext.Data.CurrentRecipeData);
                }
                _DataItem.LastModifyTime = DateTime.Now;
                _DataItem.Name = _name;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
