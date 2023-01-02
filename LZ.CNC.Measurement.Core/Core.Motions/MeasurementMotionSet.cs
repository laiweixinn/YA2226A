using DY.CNC.Core;
using DY.CNC.LeadShine.LTDMC.Core;
using System;
using System.IO;
using System.Windows.Forms;
namespace LZ.CNC.Measurement.Core.Motions
{
    [Serializable]
    public class MeasurementMotionSet: LTDMCMotionSetBase
    {
        public MeasurementAxisSet AxisSetX
        {
            get
            {
                return base[AxisTypes.X] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetY
        {
            get
            {
                return base[AxisTypes.Y] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetZ
        {
            get
            {
                return base[AxisTypes.Z] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetU
        {
            get
            {
                return base[AxisTypes.U] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetV
        {
            get
            {
                return base[AxisTypes.V] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetW
        {
            get
            {
                return base[AxisTypes.W] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetA
        {
            get
            {
                return base[AxisTypes.A] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetB
        {
            get
            {
                return base[AxisTypes.B] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetC
        {
            get
            {
                return base[AxisTypes.C] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetD
        {
            get
            {
                return base[AxisTypes.D] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetE
        {
            get
            {
                return base[AxisTypes.E] as MeasurementAxisSet;
            }
        }

        public MeasurementAxisSet AxisSetF
        {
            get
            {
                return base[AxisTypes.F] as MeasurementAxisSet;
            }
        }

        public MeasurementMotionSet() : base(new MeasurementAxisSet[]
            {
                new MeasurementAxisSet(AxisTypes.X),
                new MeasurementAxisSet(AxisTypes.Y),
                new MeasurementAxisSet(AxisTypes.Z),
                new MeasurementAxisSet(AxisTypes.U),
                new MeasurementAxisSet(AxisTypes.V),
                new MeasurementAxisSet(AxisTypes.W),
                new MeasurementAxisSet(AxisTypes.A),
                new MeasurementAxisSet(AxisTypes.B),
                new MeasurementAxisSet(AxisTypes.C),
                new MeasurementAxisSet(AxisTypes.D),
                new MeasurementAxisSet(AxisTypes.E),
                new MeasurementAxisSet(AxisTypes.F),
            })
        {
        }

        public  static  MeasurementMotionSet LoadMotionSet(string cardindex)
        {
            string path = Path.Combine(Application.StartupPath, string.Format("set/motionset{0}.config",cardindex));
            return Load(path) as MeasurementMotionSet;
        }

        public bool SaveMotionSet(string cardindex)
        {
            string path = Path.Combine(Application.StartupPath, string.Format("set/motionset{0}.config", cardindex));
            return Save(path);
        }

        public new static MeasurementMotionSet Load()
        {
            string path = Path.Combine(Application.StartupPath, "set/motionset.config");
            return Load(path) as MeasurementMotionSet;
        }

        public override bool Save()
        {
            string path = Path.Combine(Application.StartupPath, "set/motionset.config");
            return Save(path);
        }
    }
}
