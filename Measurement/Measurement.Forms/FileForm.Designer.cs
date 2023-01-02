namespace LZ.CNC.Measurement.Forms
{
    partial class FileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_loadfile = new System.Windows.Forms.Button();
            this.btn_delfile = new System.Windows.Forms.Button();
            this.btn_newfile = new System.Windows.Forms.Button();
            this.dgv_filename = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_edit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_filename)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_loadfile
            // 
            this.btn_loadfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_loadfile.Location = new System.Drawing.Point(571, 193);
            this.btn_loadfile.Name = "btn_loadfile";
            this.btn_loadfile.Size = new System.Drawing.Size(75, 38);
            this.btn_loadfile.TabIndex = 8;
            this.btn_loadfile.Text = "设为加工";
            this.btn_loadfile.UseVisualStyleBackColor = true;
            this.btn_loadfile.Click += new System.EventHandler(this.btn_loadfile_Click);
            // 
            // btn_delfile
            // 
            this.btn_delfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_delfile.Location = new System.Drawing.Point(188, 530);
            this.btn_delfile.Name = "btn_delfile";
            this.btn_delfile.Size = new System.Drawing.Size(75, 38);
            this.btn_delfile.TabIndex = 9;
            this.btn_delfile.Text = "删除";
            this.btn_delfile.UseVisualStyleBackColor = true;
            this.btn_delfile.Click += new System.EventHandler(this.btn_delfile_Click_1);
            // 
            // btn_newfile
            // 
            this.btn_newfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_newfile.Location = new System.Drawing.Point(466, 193);
            this.btn_newfile.Name = "btn_newfile";
            this.btn_newfile.Size = new System.Drawing.Size(75, 38);
            this.btn_newfile.TabIndex = 10;
            this.btn_newfile.Text = "新建";
            this.btn_newfile.UseVisualStyleBackColor = true;
            this.btn_newfile.Click += new System.EventHandler(this.btn_newfile_Click);
            // 
            // dgv_filename
            // 
            this.dgv_filename.AllowUserToAddRows = false;
            this.dgv_filename.AllowUserToDeleteRows = false;
            this.dgv_filename.AllowUserToResizeColumns = false;
            this.dgv_filename.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgv_filename.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_filename.BackgroundColor = System.Drawing.Color.White;
            this.dgv_filename.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_filename.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_filename.ColumnHeadersHeight = 30;
            this.dgv_filename.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_filename.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgv_filename.Location = new System.Drawing.Point(12, 12);
            this.dgv_filename.Name = "dgv_filename";
            this.dgv_filename.ReadOnly = true;
            this.dgv_filename.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgv_filename.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_filename.RowHeadersVisible = false;
            this.dgv_filename.RowHeadersWidth = 60;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_filename.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_filename.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_filename.Size = new System.Drawing.Size(339, 512);
            this.dgv_filename.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "创建时间";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // btn_edit
            // 
            this.btn_edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_edit.Location = new System.Drawing.Point(100, 530);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(75, 38);
            this.btn_edit.TabIndex = 9;
            this.btn_edit.Text = "编辑";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Visible = false;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // FileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 579);
            this.Controls.Add(this.dgv_filename);
            this.Controls.Add(this.btn_loadfile);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.btn_delfile);
            this.Controls.Add(this.btn_newfile);
            this.Name = "FileForm";
            this.Text = "FileForm";
            this.Load += new System.EventHandler(this.FileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_filename)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btn_loadfile;
        internal System.Windows.Forms.Button btn_delfile;
        internal System.Windows.Forms.Button btn_newfile;
        internal System.Windows.Forms.DataGridView dgv_filename;
        internal System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}