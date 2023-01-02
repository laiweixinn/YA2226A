namespace LZ.CNC.Measurement.Core
{
    public enum ButtonTypes
    {
        START,
        STOP,
        PAUSE,
        CONTINUE=4,
        RESET=8,
        EMG=16
    }

    public enum StationType
    {
        Left,
        Mid,
        Right
    }


    public enum WorkStatuses
    {
        Idle,
        Running,
        Stoping,
        Pending,
        Homing,
        Emg,
        Error,
        Pausing,
        Conting,
        Stopped
    }

    public enum DetectResults
    {
        OK = 0,
        W_Max = 1,
        W_Min = 2,
        H_Max = 4,
        H_Min = 8,
        GlueStart = 16,
        GlueEnd = 32
    }

    public enum DataResults
    {
        None,
        OK,
        NG
    }

    public enum SideTypes
    {
        Side1,
        Side2,
        Side3,
        Side4
    }

    public enum PhotoPoints
    {
        PointA,
        PointB,
        PointC,
        PointD
    }


    public enum CCDPoints
    {
        PointA1=1,
        PointA2,
        PointB1,
        PointB2,
        PointC1,
        PointC2
    }


    public enum Areas
    {
        AShort,
        ALong,
        B,
        CLong,
        CShort,
        DNomal,
        DSpecial
    }

    public enum RollMotorStatus
    {
        Idel,
        Working
    }

    public enum GlueSystemStatus
    {
        UnKnow,
        Ready,
        NoGlue,
        Error
    }

    public enum PhotoFailHander
    {
        None,
        Cancel,
        Conti
    }
}
