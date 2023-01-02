using DY.CNC.Core;
using DY.CNC.LeadShine.LTDMC.Core;
using System;
using System.Threading;
namespace LZ.CNC.Measurement.Core.Motions
{
    public class MeasurementAxis:LTDMCAxisBase
    {
        public MeasurementAxis(AxisTypes axistype):base(axistype)
        {          
        }

        public bool IsALM
        {
            get
            {
                bool flag;
                MeasurementIOListener oListener = Motion.IOListener as MeasurementIOListener;
                flag = (oListener == null ? false : oListener.ALM[AxisIndex - 1]);
                return flag;
            }
        }

        public bool IsELNActived
        {
            get
            {
                bool flag;
                bool alm = false;
                bool elp = false;
                bool eln = false;
                bool emg = false;
                bool org = false;
                bool slp = false;
                bool sln = false;
                bool inp = false;
                bool ez = false;
                bool rdy = false;
                bool dstp = false;
                flag = (Motion.Client.GetAxisIOStatus(AxisIndex, ref alm, ref elp, ref eln, ref emg, ref org, ref slp, ref sln, ref inp, ref ez, ref rdy, ref dstp) ? eln : false);
                return flag;
            }
        }

        public bool IsELPActived
        {
            get
            {
                bool flag;
                bool alm = false;
                bool elp = false;
                bool eln = false;
                bool emg = false;
                bool org = false;
                bool slp = false;
                bool sln = false;
                bool inp = false;
                bool ez = false;
                bool rdy = false;
                bool dstp = false;
                flag = (Motion.Client.GetAxisIOStatus(AxisIndex, ref alm, ref elp, ref eln, ref emg, ref org, ref slp, ref sln, ref inp, ref ez, ref rdy, ref dstp) ? elp : false);
                return flag;
            }
        }

        public bool IsHomeActived
        {
            get
            {
                bool flag;
                bool alm = false;
                bool elp = false;
                bool eln = false;
                bool emg = false;
                bool org = false;
                bool slp = false;
                bool sln = false;
                bool inp = false;
                bool ez = false;
                bool rdy = false;
                bool dstp = false;
                flag = (Motion.Client.GetAxisIOStatus(AxisIndex, ref alm, ref elp, ref eln, ref emg, ref org, ref slp, ref sln, ref inp, ref ez, ref rdy, ref dstp) ? org : false);
                return flag;
            }
        }

        public override bool GoHome()
        {
            bool result;
            bool flag = false;
            Motion.BeginMotion();
            if (HomeType == HomeTypes.Back)
            {
                flag = HomeMoveBack(AxisSet.StrokeLength / 3);
                if (!flag)
                {
                    Motion.EndMotion();
                    result = false;
                    return result;
                }
                else if (Motion.IsStoped)
                {
                    result = false;
                    return result;
                }
            }
            flag = MoveOutHome(AxisSet.HomeSpeed);
            if (!flag)
            {
                Motion.EndMotion();
                result = false;
            }
            else if (!Motion.IsStoped)
            {
                //  Motion.EndMotion();
                Motion.EndMotion();
                flag = base.GoHome();
                if (flag)
                {
                    Thread.Sleep(100);
                    if (!IsHomeActived)
                    {
                        result = false;
                        return result;
                    }
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool HomeMoveBack(double dist)
        {
            bool result;
            MoveDirections homeDir = AxisSet.HomeDir;
            if (!IsHomeActived)
            {
                if (homeDir == MoveDirections.Negative)
                {
                    dist = Math.Abs(dist);
                    if (IsELPActived)
                    {
                        result = true;
                        return result;
                    }
                }
                else if (homeDir == MoveDirections.Positive)
                {
                    dist = -Math.Abs(dist);
                    if (IsELNActived)
                    {
                        result = true;
                        return result;
                    }
                }
                if (Move(dist, AxisSet.HomeSpeed))
                {
                    while (true)
                    {
                        if (!Motion.IsLinked)
                        {
                            break;
                        }
                        else if (Motion.IsStoped)
                        {
                            break;
                        }
                        else if (IsMoving())
                        {
                            if (IsHomeActived)
                            {
                                bool flag = true;
                                int num = 0;
                                while (num < 3)
                                {
                                    if (IsHomeActived)
                                    {
                                        Thread.Sleep(10);
                                        num++;
                                    }
                                    else
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                if (flag)
                                {
                                    Thread.Sleep(100);
                                    StopSlowly();
                                    WaitMotionStop();
                                    break;
                                }
                            }
                            Thread.Sleep(10);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (Motion.IsStoped)
                    {
                        result = false;
                    }
                    else if (!IsHomeActived)
                    {
                        if (homeDir == MoveDirections.Negative)
                        {
                            if (IsELPActived)
                            {
                                Move(-5, AxisSet.HomeSpeed);
                                WaitMotionStop();
                                result = true;
                                return result;
                            }
                        }
                        else if (homeDir == MoveDirections.Positive)
                        {
                            if (IsELNActived)
                            {
                                Move(5, AxisSet.HomeSpeed);
                                WaitMotionStop();
                                result = true;
                                return result;
                            }
                        }
                        StopSlowly();
                        WaitMotionStop();
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else
                {
                    StopSlowly();
                    result = false;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public override bool MoveOutHome(double speed)
        {
            bool flag;
            if (IsHomeActived)
            {
                MoveDirections homeDir =AxisSet.HomeDir;
                if (homeDir == MoveDirections.Negative)
                {
                    homeDir = MoveDirections.Positive;
                }
                else if (homeDir == MoveDirections.Positive)
                {
                    homeDir = MoveDirections.Negative;
                }
                if (MoveContinue(homeDir, speed))
                {
                    while (true)
                    {
                        if (!Motion.IsLinked)
                        {
                            break;
                        }
                        else if (Motion.IsStoped)
                        {
                            break;
                        }
                        else if (IsMoving())
                        {
                            if (!IsHomeActived)
                            {
                                bool flag1 = false;
                                int num = 0;
                                while (num < 1)
                                {
                                    if (!IsHomeActived)
                                    {
                                        Thread.Sleep(100);
                                        num++;
                                    }
                                    else
                                    {
                                        flag1 = true;
                                        break;
                                    }
                                }
                                if (!flag1)
                                {
                                    StopSlowly();
                                    WaitMotionStop();
                                    break;
                                }
                            }
                            Thread.Sleep(10);
                        }
                        else
                        {
                            break;
                        }
                    }
                    StopSlowly();
                    WaitMotionStop();
                    if (!Motion.IsStoped)
                    {
                        flag = (IsHomeActived ? false : true);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    StopSlowly();
                    WaitMotionStop();
                    flag = false;
                }
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public bool HomeMoveWait()
        {
            bool result;
            if (!MoveOutHome(AxisSet.HomeSpeed))
            {
                result = false;
            }
            else
            {
                WaitMotionStop();
                MoveDirections directions = AxisSet.HomeDir;
                if (!SetMoveProfile(AxisSet.SpeedStart, AxisSet.HomeSpeed, 0, AxisSet.SpeedAcc, AxisSet.SpeedAcc))
                {
                    result = false;              
                }
                else
                {
                    int mode = 0;
                    switch (HomeMode)
                    {
                        case HomeModes.None:
                            return true;
                        case HomeModes.Once:
                            mode = 0;
                            break;
                        case HomeModes.OnceBack:
                            mode = 1;
                            break;
                        case HomeModes.Twice:
                            mode = 2;
                            break;
                        case HomeModes.OnceEZ:
                            mode = 3;
                            break;
                        case HomeModes.EZ:
                            mode = 4;
                            break;
                    }
                    if (!Motion.Client.SetHomeMode(AxisIndex, (directions == MoveDirections.Positive ? true : false), 1, mode, 0))
                    {
                        result = false;
                    }
                    else if (!Motion.Client.HomeMove(AxisIndex))
                    {
                        StopSudden();
                        result = false;
                    }
                    else if (!WaitStop())
                    {
                        GoHomeExit();
                        result = false;
                    }
                    else if (!Motion.IsStoped)
                    {
                        PositionCode = 0;
                        PositionDev = 0;
                        result = true;
                    }
                    else
                    {
                        GoHomeExit();
                        result = false;
                    }
                }
            }
            return result;
        }

        public void SetSevOFF()
        {
            Motion.Client.WriteSEVON(AxisIndex, true);
        }

        public void SetSevON()
        {
            Motion.Client.WriteSEVON(AxisIndex, false);
        }
    }
}
