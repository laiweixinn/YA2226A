using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace LZ.CNC.Measurement.Forms
{
    public class TabForm : Form
    {
        public TabForm ()
        {
            InitializeComponent();
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
            }
        }

        public void InvokeTabSelectChanged() => OnTabSelectChanged();
        public void InvokeTabClose() => OnTabClose();
        public void InvokeLoginChanged() => OnLoginChanged();

        protected virtual void OnTabSelectChanged()
        {

        }
        protected virtual void OnTabClose()
        {

        }
        protected virtual void OnLoginChanged()
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(496, 391);
            this.Name = "TabForm";
            this.ResumeLayout(false);

        }
    }
}
