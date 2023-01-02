using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.CNC.Core;

namespace LZ.CNC.Measurement.Core.Motions
{
    public enum AxisIOTypes
    {
        None,
        SON,
        ORG,
        ELP,
        ELN,
        ALM
    }

    public class AxisIOStatusEventArgs : EventArgs
    {
        private AxisTypes _AxisType = AxisTypes.None;

        private AxisIOTypes _AxisIOType = AxisIOTypes.None;

        private bool _Status = false;

        public AxisIOTypes AxisIOType
        {
            get
            {
                return _AxisIOType;
            }
        }

        public AxisTypes AxisType
        {
            get
            {
                return _AxisType;
            }
        }

        public bool Status
        {
            get
            {
                return _Status;
            }
        }

        public AxisIOStatusEventArgs(AxisTypes axistype, AxisIOTypes axisiotype, bool status)
        {
            _AxisType = axistype;
            _AxisIOType = axisiotype;
            _Status = status;
        }
    }

    public class EmgStatusEventArgs : EventArgs
    {
        private bool _Status = false;

        public bool Status
        {
            get
            {
                return _Status;
            }
        }

        public EmgStatusEventArgs(bool status)
        {
            _Status = status;
        }
    }

}
