using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms.PageUC
{
    public class BasePageUC:UserControl
    {
        private bool _IsValueChanged;

        public event EventHandler ValueChanged;

        private bool _IsInited;

        private bool _IsSaved;

        public bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }

        public bool IsInited
        {
            get
            {
                return _IsInited;
            }
            set
            {
                _IsInited = value;
            }
        }

        public bool IsValueChanged
        {
            get
            {
                return _IsValueChanged;
            }
            set
            {
                _IsValueChanged = true;
            }
        }

        protected void OnValuedChanged()
        {
            if (ValueChanged!=null)
            {
                ValueChanged(this, null);
            }
        }

        public virtual void Save()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void RefreshUI()
        {

        }
    }
}
