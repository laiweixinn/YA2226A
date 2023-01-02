using DY.CNC.Core;
using DY.CNC.LeadShine.LTDMC.Core;
using System;
namespace LZ.CNC.Measurement.Core.Motions
{
    [Serializable]
    public class MeasurementAxisSet:LTDMCAxisSetBase
    {
        public MeasurementAxisSet(AxisTypes axistype):base(axistype)
        {
        }
    }
}
