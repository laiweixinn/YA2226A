using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using LZ.CNC.Measurement.Core;
using DY.Core.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FileForm : TabForm
    {
        private DataTable _Table;

        private void InitTable()
        {
            _Table = new DataTable();
            _Table.Columns.Add("Index");
            _Table.Columns.Add("Name");
            _Table.Columns.Add("LastTime");
            _Table.Columns.Add("Obj", typeof(object));

            Column1.DataPropertyName = "Index";
            Column2.DataPropertyName = "Name";
            Column3.DataPropertyName = "LastTime";
            dgv_filename.AutoGenerateColumns = false;
            dgv_filename.DataSource = _Table;
        }

        private MeasurementWorker _worker = MeasurementContext.Worker;

        public FileForm()
        {
            InitializeComponent();
        }

        private void RefreshTable()
        {
            MeasurementData data = MeasurementContext.Data;
            int i = _Table.Rows.Count - data.DateCollection.Count;
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
                    DataRow row = _Table.NewRow();
                    _Table.Rows.Add(row);
                }
            }
            for (int j = 0; j < data.DateCollection.Count; j++)
            {
                DataRow row = _Table.Rows[j];
                MeasurementData.RecipeDataItem dataItem = data.DateCollection[j];
                row["Index"] = string.Format("{0}{1}", j + 1, (dataItem == data.CurrentRecipeData) ? "[活动]" : "");
                row["Name"] = dataItem.Name;
                row["LastTime"] = dataItem.LastModifyTime.ToString();
                row["Obj"] = dataItem;
            }
        }

        private void FileForm_Load(object sender, EventArgs e)
        {
            InitTable();
            RefreshTable();
            if ((MeasurementContext.Data.CurrentRecipeData == null ? false : _Table != null))
            {
                dgv_filename.ClearSelection();
                int i = 0;
                while (i < _Table.Rows.Count)
                {
                    if (_Table.Rows[i]["Obj"] as MeasurementData.RecipeDataItem != MeasurementContext.Data.CurrentRecipeData)
                    {
                        i++;
                    }
                    else
                    {
                        dgv_filename.Rows[i].Selected = true;
                        break;
                    }
                }
            }
        }

        private void btn_newfile_Click(object sender, EventArgs e)
        {
            NewFile newFile = new NewFile(null);
            DialogResult result = newFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                MeasurementContext.Data.DateCollection.Add(newFile.DataItem);
                MeasurementContext.Data.Save();
                RefreshTable();
                string oldpath;
                string newpath;
                oldpath = Path.Combine(Application.StartupPath, "visionfile\\"+ MeasurementContext.Data.CurrentRecipeData.Name + ".sci");
                newpath = Path.Combine(Application.StartupPath, "visionfile\\" + newFile.DataItem.Name + ".sci");
              //  File.Copy(oldpath, newpath);
                MeasurementContext.OutputMessage("新建文件！");
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (dgv_filename.SelectedRows.Count>0)
            {
                DataGridViewRow gridViewRow = dgv_filename.SelectedRows[0];
                DataRowView rowView = gridViewRow.DataBoundItem as DataRowView;
                DataRow row = rowView.Row;
                MeasurementData.RecipeDataItem dataItem = row["Obj"] as MeasurementData.RecipeDataItem;
                NewFile fileForm = new NewFile(dataItem);
                DialogResult result = fileForm.ShowDialog();
                if (result==DialogResult.OK)
                {
                    MeasurementContext.Data.Save();
                    RefreshTable();
                    MeasurementContext.OutputMessage("重新命名！");
                }

            }
        }

        private void btn_loadfile_Click(object sender, EventArgs e)
        {
            if (dgv_filename.SelectedRows.Count > 0)
            {
                if (MessageBoxEx.ShowSystemQuestion("是否将当前选择项设为加工?"))
                {
                    DataGridViewRow dvr = dgv_filename.SelectedRows[0];
                    MeasurementData.RecipeDataItem _data = (dvr.DataBoundItem as DataRowView).Row["Obj"] as MeasurementData.RecipeDataItem;
                    if (MeasurementContext.Data.CurrentRecipeData != _data)
                    {
                        if (MeasurementContext.Data.CurrentRecipeData != null)
                        {
                            MeasurementStatistics.NewDefaultGroup(MeasurementContext.Data.CurrentRecipeData.Name);
                        }
                    }
                    MeasurementContext.Data.CurrentRecipeData = _data;
                    MeasurementContext.Data.Save();
                    RefreshTable();
                    MeasurementContext.OutputMessage(string.Format("设定[{0}]为加工文件！", MeasurementContext.Data.CurrentRecipeData.Name));
                    _worker.OnRecipeChanged();
                }
            }
        }

        private void btn_delfile_Click_1(object sender, EventArgs e)
        {
            if (dgv_filename.SelectedRows.Count > 0)
            {
                if (MessageBoxEx.ShowSystemQuestion("是否删除所选文件"))
                {
                    for (int i = 0; i < dgv_filename.SelectedRows.Count; i++)
                    {
                        DataGridViewRow dvr = dgv_filename.SelectedRows[i];
                        DataRowView drv = dvr.DataBoundItem as DataRowView;
                        DataRow dr = drv.Row;
                        MeasurementData.RecipeDataItem _data = dr["Obj"] as MeasurementData.RecipeDataItem;
                        if (_data.Name== MeasurementContext.Data.CurrentRecipeData.Name)
                        {
                            MeasurementContext.OutputMessage("活动配方不可删除!");
                            return;
                        }
                        MeasurementContext.Data.DateCollection.Remove(_data);
                    }
                    MeasurementContext.Data.Save();
                    RefreshTable();
                    MeasurementContext.OutputMessage("删除文件");
                }
            }
        }
    }
}
