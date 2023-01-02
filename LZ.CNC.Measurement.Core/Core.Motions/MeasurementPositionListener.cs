using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.CNC.LeadShine.LTDMC.Core;
namespace LZ.CNC.Measurement.Core.Motions
{
    public class MeasurementPositionListener:LTDMCPositionListen
    {
        public MeasurementPositionListener(MeasurementMotion motion):base(motion)
        {
        }
    }
}
