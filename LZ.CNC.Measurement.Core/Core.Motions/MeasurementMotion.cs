using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DY.CNC.Core;
using DY.CNC.LeadShine.LTDMC.Core;
using DY.CNC.LeadShine.LTDMC.Base;

namespace LZ.CNC.Measurement.Core.Motions
{
    public class MeasurementMotion: LTDMCMotionBase
    {

        public int CardIndex = 0;
        public MeasurementAxis AxisX
        {
            get
            {
                return base[AxisTypes.X] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisY
        {
            get
            {
                return base[AxisTypes.Y] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisZ
        {
            get
            {
                return base[AxisTypes.Z] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisU
        {
            get
            {
                return base[AxisTypes.U] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisV
        {
            get
            {
                return base[AxisTypes.V] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisW
        {
            get
            {
                return base[AxisTypes.W] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisA
        {
            get
            {
                return base[AxisTypes.A] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisB
        {
            get
            {
                return base[AxisTypes.B] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisC
        {
            get
            {
                return base[AxisTypes.C] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisD
        {
            get
            {
                return base[AxisTypes.D] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisE
        {
            get
            {
                return base[AxisTypes.E] as MeasurementAxis;
            }
        }

        public MeasurementAxis AxisF
        {
            get
            {
                return base[AxisTypes.F] as MeasurementAxis;
            }
        }

        public new MeasurementMotionSet MotionSet
        {
            get
            {
                return base.MotionSet as MeasurementMotionSet;
            }
        }

        public new MeasurementIOListener IOListener
        {
            get
            {
                return base.IOListener as MeasurementIOListener;
            }
        }

        public new MeasurementPositionListener PositionListener
        {
            get
            {
                return base.PositionListener as MeasurementPositionListener;
            }
        }

        public MeasurementMotion(int cardtype,int cardindex):base(cardtype,cardindex,new MeasurementAxis[]
        {
            new MeasurementAxis(AxisTypes.X),
            new MeasurementAxis(AxisTypes.Y),
            new MeasurementAxis(AxisTypes.Z),
            new MeasurementAxis(AxisTypes.U),
            new MeasurementAxis(AxisTypes.V),
            new MeasurementAxis(AxisTypes.W),
            new MeasurementAxis(AxisTypes.A),
            new MeasurementAxis(AxisTypes.B),
            new MeasurementAxis(AxisTypes.C),
            new MeasurementAxis(AxisTypes.D),
            new MeasurementAxis(AxisTypes.E),
            new MeasurementAxis(AxisTypes.F)
        },null)
        {
            base.MotionSet = MeasurementMotionSet.LoadMotionSet(Id.ToString());
            CardIndex= cardindex;
            if (MotionSet==null)
            {
                base.MotionSet = new MeasurementMotionSet();
            }
            base.PositionListener = new MeasurementPositionListener(this);
            base.IOListener = new MeasurementIOListener(this);
        }

        public string GetAxisName(AxisTypes axistype)
        {
            string axisname;
            MeasurementAxis axis = base[axistype] as MeasurementAxis;
            if (axis == null)
            {
                axisname = null;
            }
            else
            {
                axisname = axis.AxisSet.AxisName;
            }
            return axisname;
        }


        public void SetSevON()
        {
            for (int i = 1; i < 9; i++)
            {
                Client.WriteSEVON(i,false);
            }

            //foreach (AxisBase axis in Axises)
            //{
            //    Client.WriteSEVON(axis.AxisIndex, false);
            //}
        }

        public void SetSevOFF()
        {
            for(int i=1;i<9;i++)
            {
                Client.WriteSEVON(i,true);
            }
        }
       

        public override bool GoHome(AxisBase[] axises)
        {
            bool flag;
            BeginMotion();
            int i = 0;
            while (true)
            {
                if (i < axises.Length)
                {
                    MeasurementAxis axis = axises[i] as MeasurementAxis;
                    if (axis.HomeType == HomeTypes.Back)
                    {
                        if (!axis.HomeMoveBack(axis.AxisSet.StrokeLength / 3))
                        {
                            EndMotion();
                            flag = false;
                            break;
                        }
                    }
                    if (IsStoped)
                    {
                        flag = false;
                        break;
                    }
                    else if (!axis.MoveOutHome(axis.AxisSet.HomeSpeed))
                    {
                        EndMotion();
                        flag = false;
                        break;
                    }
                    else if (!IsStoped)
                    {
                        i++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                else
                {
                    EndMotion();                   
                    flag = base.GoHome(axises);
                    break;
                }
            } 
            return flag;
        }

        public override bool GoHome()
        {
            MeasurementAxis axis;
            bool flag;
            BeginMotion();
            AxisBase[] axises = new AxisBase[] {AxisX,AxisY, AxisZ, AxisU,AxisV,AxisW,AxisA };
            if (!GoHome(axises))
            {
                flag = false;
            }
            else
            {
                int i = 0;
                while (i<axises.Length)
                {
                    axis = axises[i] as MeasurementAxis;
                    if (axis.IsHomeActived)
                    {
                        axis.PositionDev = 0;
                        axis.PositionCode = 0;
                        i++;
                    }
                    else
                    {
                        flag = false;
                        return flag;
                    }
                   
                }
                flag = true;
               
            }
            return flag;
        }

        public override bool UpdateConfig()
        {
            bool res = false;
            foreach (AxisBase axis in Axises)
            {
                int mode = 0;
                if ((!Client.GetEncoderMode(axis.AxisIndex, ref mode) ? false : mode != 3))
                {
                    Client.SetEncoderMode(axis.AxisIndex, 3);
                }
            }
            res = base.UpdateConfig();
            foreach (AxisBase axise in Axises)
            {
                Client.SetALMMode(axise.AxisIndex, true, true);
            }
            foreach (AxisBase item in Axises)
            {
                Client.SetElMode(item.AxisIndex, true, true,0);
            }
            
            AxisA.SetALMMode(true, false);
            AxisB.SetALMMode(true, false);
            AxisC.SetALMMode(true, false);
            AxisD.SetALMMode(true, false);
            AxisE.SetALMMode(true, false);
            AxisF.SetALMMode(true, false);
           
            AxisX.SetELMode(true, false);
            AxisY.SetELMode(true, false);
            AxisZ.SetELMode(true, false);
            AxisU.SetALMMode(true, false);
            AxisV.SetELMode(true, false);
            AxisW.SetELMode(true, false);

            MeasurementConfig.ConfigIO emgio = MeasurementContext.Config.EStopIOIn;
            res = (!emgio.IsValid ? SetEMG(15, false, false) : SetEMG(emgio.IO, true, emgio.Status));
            res = Client.SetSpecialInputFilter(0.01);
            return true;
        }

        protected override bool SavePosition()
        {
            bool result;
            if (AxisCount <= 0)
            {
                result = true;
            }
            else
            {
                if (IsLinked)
                {
                    if (MotionSet.LastPosDevEnabled)
                    {
                        int lastPosDev = 0;
                        foreach (AxisBase current in Axises)
                        {
                            if (current.GetPositionDevByPulse(ref lastPosDev))
                            {
                                current.AxisSet.LastPosDev = lastPosDev;
                            }
                        }
                        foreach (AxisBase current in Axises)
                        {
                            if (current.GetPositionCodeByPulse(ref lastPosDev))
                            {
                                current.AxisSet.LastPosCode = lastPosDev;
                            }
                        }
                        MotionSet.LastPosDevValid = true;
                        MotionSet.SaveMotionSet(Id.ToString());
                    }
                }
                result = true;
            }
            return result;
        }

        protected override bool RestorePosition()
        {
            bool result;
            if (AxisCount <= 0)
            {
                result = true;
            }
            else
            {
                if (!IsLinked)
                {
                    result = false;
                }
                else
                {
                    if (MotionSet.LastPosDevEnabled && MotionSet.LastPosDevValid)
                    {
                        foreach (AxisBase current in Axises)
                        {
                            if (!current.SetPositionDevByPulse(current.AxisSet.LastPosDev))
                            {
                                result = false;
                                return result;
                            }
                            if (!current.SetPositionCodeByPulse(current.AxisSet.LastPosDev))
                            {
                                result = false;
                                return result;
                            }
                        }
                        if (IsLinked)
                        {
                            MotionSet.LastPosDevValid = false;
                            MotionSet.SaveMotionSet(Id.ToString());
                        }
                    }
                    else
                    {
                        foreach (AxisBase current in Axises)
                        {
                            if (!current.SetPositionDevByPulse(0))
                            {
                                result = false;
                                return result;
                            }
                            if (!current.SetPositionCodeByPulse(0))
                            {
                                result = false;
                                return result;
                            }
                        }
                    }
                    result = true;
                }
            }
            return result;
        }
    }
 }
