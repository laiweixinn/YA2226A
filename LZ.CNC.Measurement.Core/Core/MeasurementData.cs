using DY.Core.Configs;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace LZ.CNC.Measurement.Core
{
    [Serializable]
    public class MeasurementData : ConfigBase
    {
        private RecipeDateCollection _DateCollection = new RecipeDateCollection();

        private RecipeDataItem _CurrentRecipeData;
        public RecipeDateCollection DateCollection
        {
            get
            {
                return _DateCollection;
            }
        }

        public RecipeDataItem CurrentRecipeData
        {
            get
            {
                return _CurrentRecipeData;
            }
            set
            {
                _CurrentRecipeData = value;
            }
        }

        [Serializable]
        public class RecipeDateCollection : CollectionBase
        {
            public RecipeDataItem this[int index]
            {
                get
                {
                    return InnerList[index] as RecipeDataItem;
                }
            }

            public void Add(RecipeDataItem dataItem)
            {
                InnerList.Add(dataItem);
            }

            public void Remove(RecipeDataItem dataItem)
            {
                InnerList.Remove(dataItem);
            }

            public void IndexOf(RecipeDataItem dataItem)
            {
                InnerList.IndexOf(dataItem);
            }

            public RecipeDateCollection()
            {
            }
        }

        public static RecipeDataItem Clone(RecipeDataItem recipe)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, recipe);
            memoryStream.Position = 0;
            return (RecipeDataItem)binaryFormatter.Deserialize(memoryStream);
        }

        [Serializable]
        public class RecipeDataItem
        {
            private SMDataItemCollection _Smdatas = new SMDataItemCollection();
            public SMDataItemCollection SMdatas
            {
                get
                {
                    if (_Smdatas == null)
                    {
                        _Smdatas = new SMDataItemCollection();
                    }
                    if(_Smdatas.Count<19)
                    {
                        for(int i=_Smdatas.Count;i<19;i++)
                        {
                            _Smdatas.Add(new SMDataItem());
                        }
                    }
                    return _Smdatas;
                }
            }

            #region 撕膜工位位置保存
            private List<SMLocation> _smposition;

            public List<SMLocation> SMPosition
            {
                get
                {
                    if (_smposition == null)
                    {
                        _smposition = new List<SMLocation>();
                    }
                    if (_smposition.Count < 3)
                    {
                        for (int i =_smposition.Count; i < 3; i++)
                        {
                            _smposition.Add(new SMLocation());
                        }
                    }
                    return _smposition;
                }
            }

            #endregion

            #region //工作模式
            private bool _xy_model;
            public bool XY_Model
            {
                get
                {
                    return _xy_model;
                }
                set
                {
                    _xy_model = value;
                }
            }


            private int _WorkModel=0;
            public int WorkModel
            {
                get
                {
                    return _WorkModel;
                }
                set
                {
                    _WorkModel = value;
                }
            }



            #endregion

            #region BEND AOI
            private double _AOIX1;
            private double _AOIX2;
            private double _AOIY1;
            private double _AOIY2;

            private double _AOIX1Offset;
            private double _AOIX2Offset;
            private double _AOIY1Offset;
            private double _AOIY2Offset;

        

            public double AOIX1
            {
                get
                {
                    return _AOIX1;
                }
                set
                {
                    _AOIX1 = value;
                }
            }

            public double AOIY2
            {
                get
                {
                    return _AOIY2;
                }
                set
                {
                    _AOIY2 = value;
                }
            }

            public double AOIX2
            {
                get
                {
                    return _AOIX2;
                }
                set
                {
                    _AOIX2 = value;
                }
            }


            public double AOIY1
            {
                get
                {
                    return _AOIY1;
                }
                set
                {
                    _AOIY1 = value;
                }
            }

            public double AOIX1Offset
            {
                get
                {
                    return _AOIX1Offset;
                }
                set
                {
                    _AOIX1Offset = value;
                }
            }

            public double AOIY2Offset
            {
                get
                {
                    return _AOIY2Offset;
                }
                set
                {
                    _AOIY2Offset = value;
                }
            }

            public double AOIX2Offset
            {
                get
                {
                    return _AOIX2Offset;
                }
                set
                {
                    _AOIX2Offset = value;
                }
            }


            public double AOIY1Offset
            {
                get
                {
                    return _AOIY1Offset;
                }
                set
                {
                    _AOIY1Offset = value;
                }
            }















            private double _MidAOIX1;
            private double _MidAOIX2;
            private double _MidAOIY1;
            private double _MidAOIY2;

            private double _MidAOIX1Offset;
            private double _MidAOIX2Offset;
            private double _MidAOIY1Offset;
            private double _MidAOIY2Offset;



            public double MidAOIX1
            {
                get
                {
                    return _MidAOIX1;
                }
                set
                {
                    _MidAOIX1 = value;
                }
            }

            public double MidAOIY2
            {
                get
                {
                    return _MidAOIY2;
                }
                set
                {
                    _MidAOIY2 = value;
                }
            }

            public double MidAOIX2
            {
                get
                {
                    return _MidAOIX2;
                }
                set
                {
                    _MidAOIX2 = value;
                }
            }


            public double MidAOIY1
            {
                get
                {
                    return _MidAOIY1;
                }
                set
                {
                    _MidAOIY1 = value;
                }
            }

            public double MidAOIX1Offset
            {
                get
                {
                    return _MidAOIX1Offset;
                }
                set
                {
                    _MidAOIX1Offset = value;
                }
            }

            public double MidAOIY2Offset
            {
                get
                {
                    return _MidAOIY2Offset;
                }
                set
                {
                    _MidAOIY2Offset = value;
                }
            }

            public double MidAOIX2Offset
            {
                get
                {
                    return _MidAOIX2Offset;
                }
                set
                {
                    _MidAOIX2Offset = value;
                }
            }


            public double MidAOIY1Offset
            {
                get
                {
                    return _MidAOIY1Offset;
                }
                set
                {
                    _MidAOIY1Offset = value;
                }
            }





            private double _RightAOIX1;
            private double _RightAOIX2;
            private double _RightAOIY1;
            private double _RightAOIY2;

            private double _RightAOIX1Offset;
            private double _RightAOIX2Offset;
            private double _RightAOIY1Offset;
            private double _RightAOIY2Offset;



            public double RightAOIX1
            {
                get
                {
                    return _RightAOIX1;
                }
                set
                {
                    _RightAOIX1 = value;
                }
            }

            public double RightAOIY2
            {
                get
                {
                    return _RightAOIY2;
                }
                set
                {
                    _RightAOIY2 = value;
                }
            }

            public double RightAOIX2
            {
                get
                {
                    return _RightAOIX2;
                }
                set
                {
                    _RightAOIX2 = value;
                }
            }


            public double RightAOIY1
            {
                get
                {
                    return _RightAOIY1;
                }
                set
                {
                    _RightAOIY1 = value;
                }
            }

            public double RightAOIX1Offset
            {
                get
                {
                    return _RightAOIX1Offset;
                }
                set
                {
                    _RightAOIX1Offset = value;
                }
            }

            public double RightAOIY2Offset
            {
                get
                {
                    return _RightAOIY2Offset;
                }
                set
                {
                    _RightAOIY2Offset = value;
                }
            }

            public double RightAOIX2Offset
            {
                get
                {
                    return _RightAOIX2Offset;
                }
                set
                {
                    _RightAOIX2Offset = value;
                }
            }


            public double RightAOIY1Offset
            {
                get
                {
                    return _RightAOIY1Offset;
                }
                set
                {
                    _RightAOIY1Offset = value;
                }
            }

            private double _LoadCell1Limit;
            public double LoadCell1Limit
            {
                get
                {
                    return _LoadCell1Limit;
                }
                set
                {
                    _LoadCell1Limit = value;
                }
            }


            private double _LoadCell2Limit;
            public double LoadCell2Limit
            {
                get
                {
                    return _LoadCell2Limit;
                }
                set
                {
                    _LoadCell2Limit = value;
                }
            }


            private double _LoadCell3Limit;
            public double LoadCell3Limit
            {
                get
                {
                    return _LoadCell3Limit;
                }
                set
                {
                    _LoadCell3Limit = value;
                }
            }



            private int _CapacityStartHour;
            public int CapacityStartHour
            {
                get
                {
                    return _CapacityStartHour;
                }
                set
                {
                    _CapacityStartHour = value;
                }
            }

            private int _CapacityStartMinute;
            public int CapacityStartMinute
            {
                get
                {
                    return _CapacityStartMinute;
                }
                set
                {
                    _CapacityStartMinute = value;
                }
            }


            public DateTime _CapacityTime;
            public DateTime CapacityTime
            {
                get
                {
                    return _CapacityTime;
                }
                set
                {
                    _CapacityTime = value;
                }
            }

            private int _HourTargetCapacity;

            /// <summary>
            /// 小时目标产能
            /// </summary>         
            public int HourTargetCapacity
            {
                get
                {
                    return _HourTargetCapacity;
                }
                set
                {
                    _HourTargetCapacity = value;
                }
            }

            private int _DayTargetCapacity;
            /// <summary>
            /// 天目标产能
            /// </summary>
            public int DayTargetCapacity
            {
                get
                {
                    return _DayTargetCapacity;
                }
                set
                {
                    _DayTargetCapacity = value;
                }
            }
            #endregion

            #region 
            private List<BendParameter> _BendPara;

            public List<BendParameter> BendPara
            {
                 get
                {
                    if(_BendPara==null)
                    {
                        _BendPara = new List<BendParameter>();
                    }
                    if(_BendPara.Count<6)
                    {
                        for(int i=_BendPara.Count;i<6;i++)
                        {
                            _BendPara.Add(new BendParameter());
                        }                       
                    }
                    return _BendPara;
                }

            }

#endregion

            #region 上料位置
            private double _loadxpos;
            public double LoadXpos
            {
                get
                {
                    return _loadxpos;
                }
                set
                {
                    _loadxpos = value;
                }
            }

            private double _loadypos;
            public double LoadYpos
            {
                get
                {
                    return _loadypos;
                }
                set
                {
                    _loadypos = value;
                }
            }

            private double _loadzpos;
            public double LoadZpos
            {
                get
                {
                    return _loadzpos;
                }
                set
                {
                    _loadzpos = value;
                }
            }


            private double _loadzwaitpos;
            public double LoadZWaitPos
            {
                get
                {
                    return _loadzwaitpos;
                }
                set
                {
                    _loadzwaitpos = value;
                }
            }

            private double _loadywaitpos;
            public double LoadYWaitPos
            {
                get
                {
                    return _loadywaitpos;
                }
                set
                {
                    _loadywaitpos = value;
                }
            }

            private double _loadxwaitpos;
            public double LoadXWaitPos
            {
                get
                {
                    return _loadxwaitpos;
                }
                set
                {
                    _loadxwaitpos = value;
                }
            }


            private double _qrcodepos;
            public double QrCodePos
            {
                get
                {
                    return _qrcodepos;
                }
                set
                {
                    _qrcodepos = value;
                }
            }


            #endregion

            #region LeftBendPT
            private double _leftbend_ccd_dwy;
            public double LeftBend_CCD_DWY
            {
                get
                {
                    return _leftbend_ccd_dwy;
                }
                set
                {
                    _leftbend_ccd_dwy = value;
                }
            }


            private double _leftbendR_Ypos;
            public double LeftBendR_Ypos
            {
                get
                {
                    return _leftbendR_Ypos;
                }
                set
                {
                    _leftbendR_Ypos = value;
                }
            }


            private double _leftbend_loadX;
            public double LeftBend_LoadX
            {
                get
                {
                    return _leftbend_loadX;
                }
                set
                {
                    _leftbend_loadX = value;
                }
            }


            private double _leftbend_loadY;
            public double LeftBend_LoadY
            {
                get
                {
                    return _leftbend_loadY;
                }
                set
                {
                    _leftbend_loadY = value;
                }
            }

      
            private double _leftbend_loadZ;
            public double LeftBend_LoadZ
            {
                get
                {
                    return _leftbend_loadZ;
                }
                set
                {
                    _leftbend_loadZ = value;
                }
            }



            private double _leftbend_DWX_safepos;
            public double LeftBend_DWX_SafePos
            {
                get
                {
                    return _leftbend_DWX_safepos;
                }
                set
                {
                    _leftbend_DWX_safepos = value;
                }
            }


            private double _leftbend_DWY_Safepos;
            public double LeftBend_DWY_SafePos
            {
                get
                {
                    return _leftbend_DWY_Safepos;
                }
                set
                {
                    _leftbend_DWY_Safepos = value;
                }
            }



            private double _leftbend_dww_safepos;
            public double LeftBend_DWW_SafePos
            {
                get
                {
                    return _leftbend_dww_safepos;
                }
                set
                {
                    _leftbend_dww_safepos = value;
                }
            }


            private double _leftbend_dwr_safepos;
            public double LeftBend_DWR_SafePos
            {
                get
                {
                    return _leftbend_dwr_safepos;
                }
                set
                {
                    _leftbend_dwr_safepos = value;
                }
            }



            private double _leftbend_dwy_workpos;
            public double LeftBend_DWY_WorkPos
            {
                get
                {
                    return _leftbend_dwy_workpos;
                }
                set
                {
                    _leftbend_dwy_workpos = value;
                }
            }


            private double _leftbend_dwx_workpos;
            public double LeftBend_DWX_WorkPos
            {
                get
                {
                    return _leftbend_dwx_workpos;
                }

                set
                {
                    _leftbend_dwx_workpos = value;
                }
            }

            private double _leftbend_dww_workpos;
            public double LeftBend_DWW_WorkPos
            {
                get
                {
                    return _leftbend_dww_workpos;
                }
                set
                {
                    _leftbend_dww_workpos = value;
                }
            }





            private double _leftbend_dwr_workpos;
            public double LeftBend_DWR_WorkPos
            {
                get
                {
                    return _leftbend_dwr_workpos;
                }
                set
                {
                    _leftbend_dwr_workpos = value;
                }
            }


            private double _leftbend_CCDPOSY;
            public double LeftBend_CCDPos_Y
            {
                get
                {
                    return _leftbend_CCDPOSY;
                }
                set
                {
                    _leftbend_CCDPOSY = value;
                }
            }


            private double _leftbend_CCDPOSY2;
            public double LeftBend_CCDPos_Y2
            {
                get
                {
                    return _leftbend_CCDPOSY2;
                }
                set
                {
                    _leftbend_CCDPOSY2 = value;
                }
            }


            private double _leftbend_PressPosX;
            public double LeftBend_PressPt_X
            {
                get
                {
                    return _leftbend_PressPosX;
                }
                set
                {
                    _leftbend_PressPosX = value;
                }
            }

            private double _leftbend_PressPosY;
            public double LeftBend_PressPt_Y
            {
                get
                {
                    return _leftbend_PressPosY;
                }
                set
                {
                    _leftbend_PressPosY = value;
                }
            }

            private double _leftbend_ccdposx;
            public double LeftBend_CCDPos_X
            {
                get
                {
                    return _leftbend_ccdposx;
                }

                set
                {
                    _leftbend_ccdposx = value;
                }
            }


            #region 左下料取NG料位置
            private double _leftbend_NGdischargeX;
            public double LeftBend_NGDischarge_x
            {
                get
                {
                    return _leftbend_NGdischargeX;
                }
                set
                {
                    _leftbend_NGdischargeX = value;
                }
            }


            private double _leftbend_NGdischargey;
            public double LeftBend_NGDischarge_Y
            {
                get
                {
                    return _leftbend_NGdischargey;
                }

                set
                {
                    _leftbend_NGdischargey = value;
                }
            }

            private double _leftbend_NGdischargez;
            public double LeftBend_NGDischarge_Z
            {
                get
                {
                    return _leftbend_NGdischargez;
                }
                set
                {
                    _leftbend_NGdischargez = value;
                }
            }



            #endregion


            #region 中折弯NG取料位
            private double _Midbend_NGdischargeX;
            public double MidBend_NGDischarge_X
            {
                get
                {
                    return _Midbend_NGdischargeX;
                }
                set
                {
                    _Midbend_NGdischargeX = value;
                }
            }


            private double _Midbend_NGdischargey;
            public double MidBend_NGDischarge_Y
            {
                get
                {
                    return _Midbend_NGdischargey;
                }

                set
                {
                    _Midbend_NGdischargey = value;
                }
            }

            private double _Midbend_NGdischargez;
            public double MidBend_NGDischarge_Z
            {
                get
                {
                    return _Midbend_NGdischargez;
                }
                set
                {
                    _Midbend_NGdischargez = value;
                }
            }

            #endregion

            #region 右折弯NG取料位
            private double _Rightbend_NGdischargeX;
            public double RightBend_NGDischarge_X
            {
                get
                {
                    return _Rightbend_NGdischargeX;
                }
                set
                {
                    _Rightbend_NGdischargeX = value;
                }
            }


            private double _Rightbend_NGdischargey;
            public double RightBend_NGDischarge_Y
            {
                get
                {
                    return _Rightbend_NGdischargey;
                }

                set
                {
                    _Rightbend_NGdischargey = value;
                }
            }

            private double _Rightbend_NGdischargez;
            public double RightBend_NGDischarge_Z
            {
                get
                {
                    return _Rightbend_NGdischargez;
                }
                set
                {
                    _Rightbend_NGdischargez = value;
                }
            }

            #endregion






            private double _leftbend_dischargeX;
            public double LeftBend_Discharge_x
            {
                get
                {
                    return _leftbend_dischargeX;
                }
                set
                {
                    _leftbend_dischargeX = value;
                }
            }


            private double _leftbend_dischargey;
            public double LeftBend_Discharge_Y
            {
                get
                {
                    return _leftbend_dischargey;
                }

                set
                {
                    _leftbend_dischargey = value;
                }
            }

            private double _leftbend_dischargez;
            public double LeftBend_Discharge_Z
            {
                get
                {
                    return _leftbend_dischargez;
                }
                set
                {
                    _leftbend_dischargez = value;
                }
            }


            private double _leftbend_Y_safepos;
            public double LeftBend_Y_SafePos
            {
                get
                {
                    return _leftbend_Y_safepos;
                }
                set
                {
                    _leftbend_Y_safepos = value;
                }
            }


            private double _leftbend_Y_workpos;
            public double LeftBend_Y_WorkPos
            {
                get
                {
                    return _leftbend_Y_workpos;
                }
                set
                {
                    _leftbend_Y_workpos = value;
                }
            }



            private double _leftbend_dww_basePos;

            public double LeftBend_DWW_BasePos
            {
                get
                {
                    return _leftbend_dww_basePos;
                }
                set
                {
                    _leftbend_dww_basePos = value;
                }

            }

            #endregion

            #region MidBendPT
            private double _midbend_ccd_dwy;
            public double MidBend_CCD_DWY
            {
                get
                {
                    return _midbend_ccd_dwy;
                }
                set
                {
                    _midbend_ccd_dwy = value;
                }
            }


            private double _midbendR_Ypos;
            public double MidBendR_Ypos
            {
                get
                {
                    return _midbendR_Ypos;
                }
                set
                {
                    _midbendR_Ypos = value;
                }
            }

            private double _midbend_PressPosX;
            public double MidBend_PressPt_X
            {
                get
                {
                    return _midbend_PressPosX;
                }
                set
                {
                    _midbend_PressPosX = value;
                }
            }

            private double _midbend_PressPosY;
            public double MidBend_PressPt_Y
            {
                get
                {
                    return _midbend_PressPosY;
                }
                set
                {
                    _midbend_PressPosY = value;
                }
            }

            private double _midbend_loadX;
            public double MidBend_LoadX
            {
                get
                {
                    return _midbend_loadX;
                }
                set
                {
                    _midbend_loadX = value;
                }
            }


            private double _midbend_loadY;
            public double MidBend_LoadY
            {
                get
                {
                    return _midbend_loadY;
                }
                set
                {
                    _midbend_loadY = value;
                }
            }


            private double _midbend_loadZ;
            public double MidBend_LoadZ
            {
                get
                {
                    return _midbend_loadZ;
                }
                set
                {
                    _midbend_loadZ = value;
                }
            }



            private double _midbend_DWX_safepos;
            public double MidBend_DWX_SafePos
            {
                get
                {
                    return _midbend_DWX_safepos;
                }
                set
                {
                    _midbend_DWX_safepos = value;
                }
            }


            private double _midbend_DWY_Safepos;
            public double MidBend_DWY_SafePos
            {
                get
                {
                    return _midbend_DWY_Safepos;
                }
                set
                {
                    _midbend_DWY_Safepos = value;
                }
            }



            private double _midbend_dww_safepos;
            public double MidBend_DWW_SafePos
            {
                get
                {
                    return _midbend_dww_safepos;
                }
                set
                {
                    _midbend_dww_safepos = value;
                }
            }


            private double _midbend_dwr_safepos;
            public double MidBend_DWR_SafePos
            {
                get
                {
                    return _midbend_dwr_safepos;
                }
                set
                {
                    _midbend_dwr_safepos = value;
                }
            }



            private double _midbend_dwy_workpos;
            public double MidBend_DWY_WorkPos
            {
                get
                {
                    return _midbend_dwy_workpos;
                }
                set
                {
                    _midbend_dwy_workpos = value;
                }
            }


            private double _midbend_dwx_workpos;
            public double MidBend_DWX_WorkPos
            {
                get
                {
                    return _midbend_dwx_workpos;
                }

                set
                {
                    _midbend_dwx_workpos = value;
                }
            }


            private double _midbend_dww_workpos;
            public double MidBend_DWW_WorkPos
            {
                get
                {
                    return _midbend_dww_workpos;
                }
                set
                {
                    _midbend_dww_workpos = value;
                }
            }


            private double _midbend_dwr_workpos;
            public double MidBend_DWR_WorkPos
            {
                get
                {
                    return _midbend_dwr_workpos;
                }
                set
                {
                    _midbend_dwr_workpos = value;
                }
            }


            private double _midbend_CCDPOSY;
            public double MidBend_CCDPos_Y
            {
                get
                {
                    return _midbend_CCDPOSY;
                }
                set
                {
                    _midbend_CCDPOSY = value;
                }
            }
            private double _midbend_CCDPOSY2;
            public double MidBend_CCDPos_Y2
            {
                get
                {
                    return _midbend_CCDPOSY2;
                }
                set
                {
                    _midbend_CCDPOSY2 = value;
                }
            }

            private double _midbend_ccdposx;
            public double MidBend_CCDPos_X
            {
                get
                {
                    return _midbend_ccdposx;
                }

                set
                {
                    _midbend_ccdposx = value;
                }
            }


            private double _midbend_dischargeX;
            public double MidBend_Discharge_X
            {
                get
                {
                    return _midbend_dischargeX;
                }
                set
                {
                    _midbend_dischargeX = value;
                }
            }


            private double _midbend_dischargey;
            public double MidBend_Discharge_Y
            {
                get
                {
                    return _midbend_dischargey;
                }

                set
                {
                    _midbend_dischargey = value;
                }
            }


            private double _midbend_dischargez;
            public double MidBend_Discharge_Z
            {
                get
                {
                    return _midbend_dischargez;
                }
                set
                {
                    _midbend_dischargez = value;
                }
            }


            private double _midbend_Y_safepos;
            public double MidBend_Y_SafePos
            {
                get
                {
                    return _midbend_Y_safepos;
                }
                set
                {
                    _midbend_Y_safepos = value;
                }
            }


            private double _midbend_Y_workpos;
            public double MidBend_Y_WorkPos
            {
                get
                {
                    return _midbend_Y_workpos;
                }
                set
                {
                    _midbend_Y_workpos = value;
                }
            }


            private double _midbend_dww_basePos;
            public double MidBend_DWW_BasePos
            {
                get
                {
                    return _leftbend_dww_basePos;
                }
                set
                {
                    _leftbend_dww_basePos = value;
                }

            }




            #endregion

            #region RightBendPT
            private double _rightbend_ccd_dwy;
            public double RightBend_CCD_DWY
            {
                get
                {
                    return _rightbend_ccd_dwy;
                }
                set
                {
                    _rightbend_ccd_dwy = value;
                }
            }


            private double _rightbendR_Ypos;
            public double RightBendR_Ypos
            {
                get
                {
                    return _rightbendR_Ypos;
                }
                set
                {
                    _rightbendR_Ypos = value;
                }
            }


            private double _rightbend_PressPosX;
            public double RightBend_PressPt_X
            {
                get
                {
                    return _rightbend_PressPosX;
                }
                set
                {
                    _rightbend_PressPosX = value;
                }
            }

            private double _rightbend_PressPosY;
            public double RightBend_PressPt_Y
            {
                get
                {
                    return _rightbend_PressPosY;
                }
                set
                {
                    _rightbend_PressPosY = value;
                }
            }

            private double _rightbend_loadX;
            public double RightBend_LoadX
            {
                get
                {
                    return _rightbend_loadX;
                }
                set
                {
                    _rightbend_loadX = value;
                }
            }


            private double _rightbend_loadY;
            public double RightBend_LoadY
            {
                get
                {
                    return _rightbend_loadY;
                }
                set
                {
                    _rightbend_loadY = value;
                }
            }


            private double _rightbend_loadZ;
            public double RightBend_LoadZ
            {
                get
                {
                    return _rightbend_loadZ;
                }
                set
                {
                    _rightbend_loadZ = value;
                }
            }



            private double _rightbend_DWX_safepos;
            public double RightBend_DWX_SafePos
            {
                get
                {
                    return _rightbend_DWX_safepos;
                }
                set
                {
                    _rightbend_DWX_safepos = value;
                }
            }


            private double _rightbend_DWY_Safepos;
            public double RightBend_DWY_SafePos
            {
                get
                {
                    return _rightbend_DWY_Safepos;
                }
                set
                {
                    _rightbend_DWY_Safepos = value;
                }
            }



            private double _rightbend_dww_safepos;
            public double RightBend_DWW_SafePos
            {
                get
                {
                    return _rightbend_dww_safepos;
                }
                set
                {
                    _rightbend_dww_safepos = value;
                }
            }


            private double _rightbend_dwr_safepos;
            public double RightBend_DWR_SafePos
            {
                get
                {
                    return _rightbend_dwr_safepos;
                }
                set
                {
                    _rightbend_dwr_safepos = value;
                }
            }



            private double _rightbend_dwy_workpos;
            public double RightBend_DWY_WorkPos
            {
                get
                {
                    return _rightbend_dwy_workpos;
                }
                set
                {
                    _rightbend_dwy_workpos = value;
                }
            }


            private double _rightbend_dwx_workpos;
            public double RightBend_DWX_WorkPos
            {
                get
                {
                    return _rightbend_dwx_workpos;
                }

                set
                {
                    _rightbend_dwx_workpos = value;
                }
            }


            private double _rightbend_dww_workpos;
            public double RightBend_DWW_WorkPos
            {
                get
                {
                    return _rightbend_dww_workpos;
                }
                set
                {
                    _rightbend_dww_workpos = value;
                }
            }


            private double _rightbend_dwr_workpos;
            public double RightBend_DWR_WorkPos
            {
                get
                {
                    return _rightbend_dwr_workpos;
                }
                set
                {
                    _rightbend_dwr_workpos = value;
                }
            }


            private double _rightbend_CCDPOSY;
            public double RightBend_CCDPos_Y
            {
                get
                {
                    return _rightbend_CCDPOSY;
                }
                set
                {
                    _rightbend_CCDPOSY = value;
                }
            }

            private double _rightbend_CCDPOSY2;
            public double RightBend_CCDPos_Y2
            {
                get
                {
                    return _rightbend_CCDPOSY2;
                }
                set
                {
                    _rightbend_CCDPOSY2 = value;
                }
            }
            private double _rightbend_ccdposx;
            public double RightBend_CCDPos_X
            {
                get
                {
                    return _rightbend_ccdposx;
                }

                set
                {
                    _rightbend_ccdposx = value;
                }
            }


            private double _rightbend_dischargeX;
            public double RightBend_Discharge_X
            {
                get
                {
                    return _rightbend_dischargeX;
                }
                set
                {
                    _rightbend_dischargeX = value;
                }
            }


            private double _rightbend_dischargey;
            public double RightBend_Discharge_Y
            {
                get
                {
                    return _rightbend_dischargey;
                }

                set
                {
                    _rightbend_dischargey = value;
                }
            }


            private double _rightbend_dischargez;
            public double RightBend_Discharge_Z
            {
                get
                {
                    return _rightbend_dischargez;
                }
                set
                {
                    _rightbend_dischargez = value;
                }
            }


            private double _rightbend_Y_safepos;
            public double RightBend_Y_SafePos
            {
                get
                {
                    return _rightbend_Y_safepos;
                }
                set
                {
                    _rightbend_Y_safepos = value;
                }
            }


            private double _rightbend_Y_workpos;
            public double RightBend_Y_WorkPos
            {
                get
                {
                    return _rightbend_Y_workpos;
                }
                set
                {
                    _rightbend_Y_workpos = value;
                }
            }



            private double _rightbend_dww_basePos;

            public double RightBend_DWW_BasePos
            {
                get
                {
                    return _rightbend_dww_basePos;
                }
                set
                {
                    _rightbend_dww_basePos = value;
                }

            }
            #endregion

            #region 对位保护左工位上下限位置           
            private double _Leftbend_Xlowlimit;
            public double Leftbend_Xlowlimit
            {
                get
                {
                    return _Leftbend_Xlowlimit;
                }
                set
                {
                    _Leftbend_Xlowlimit = value;
                }
            }

            private double _Leftbend_Ylowlimit;
            public double Leftbend_Ylowlimit
            {
                get
                {
                    return _Leftbend_Ylowlimit;
                }
                set
                {
                    _Leftbend_Ylowlimit = value;
                }
            }


            private double _Leftbend_Wlowlimit;
            public double Leftbend_Wlowlimit
            {
                get
                {
                    return _Leftbend_Wlowlimit;
                }
                set
                {
                    _Leftbend_Wlowlimit = value;
                }
            }


            private double _Leftbend_XUpperlimit;
            public double Leftbend_XUpperlimit
            {
                get
                {
                    return _Leftbend_XUpperlimit;
                }
                set
                {
                    _Leftbend_XUpperlimit = value;
                }
            }

            private double _Leftbend_YUpperlimit;
            public double Leftbend_YUpperlimit
            {
                get
                {
                    return _Leftbend_YUpperlimit;
                }
                set
                {
                    _Leftbend_YUpperlimit = value;
                }
            }

            private double _Leftbend_WUpperlimit;
            public double Leftbend_WUpperlimit
            {
                get
                {
                    return _Leftbend_WUpperlimit;
                }
                set
                {
                    _Leftbend_WUpperlimit = value;
                }
            }

            #endregion

            #region 对位保护中工位上下限位置           
            private double _Midbend_Xlowlimit;
            public double Midbend_Xlowlimit
            {
                get
                {
                    return _Midbend_Xlowlimit;
                }
                set
                {
                    _Midbend_Xlowlimit = value;
                }
            }

            private double _Midbend_Ylowlimit;
            public double Midbend_Ylowlimit
            {
                get
                {
                    return _Midbend_Ylowlimit;
                }
                set
                {
                    _Midbend_Ylowlimit = value;
                }
            }


            private double _Midbend_Wlowlimit;
            public double Midbend_Wlowlimit
            {
                get
                {
                    return _Midbend_Wlowlimit;
                }
                set
                {
                    _Midbend_Wlowlimit = value;
                }
            }


            private double _Midbend_XUpperlimit;
            public double Midbend_XUpperlimit
            {
                get
                {
                    return _Midbend_XUpperlimit;
                }
                set
                {
                    _Midbend_XUpperlimit = value;
                }
            }

            private double _Midbend_YUpperlimit;
            public double Midbend_YUpperlimit
            {
                get
                {
                    return _Midbend_YUpperlimit;
                }
                set
                {
                    _Midbend_YUpperlimit = value;
                }
            }

            private double _Midbend_WUpperlimit;
            public double Midbend_WUpperlimit
            {
                get
                {
                    return _Midbend_WUpperlimit;
                }
                set
                {
                    _Midbend_WUpperlimit = value;
                }
            }

            #endregion

            #region 对位保护右工位上下限位置           
            private double _Rightbend_Xlowlimit;
            public double Rightbend_Xlowlimit
            {
                get
                {
                    return _Rightbend_Xlowlimit;
                }
                set
                {
                    _Rightbend_Xlowlimit = value;
                }
            }

            private double _Rightbend_Ylowlimit;
            public double Rightbend_Ylowlimit
            {
                get
                {
                    return _Rightbend_Ylowlimit;
                }
                set
                {
                    _Rightbend_Ylowlimit = value;
                }
            }


            private double _Rightbend_Wlowlimit;
            public double Rightbend_Wlowlimit
            {
                get
                {
                    return _Rightbend_Wlowlimit;
                }
                set
                {
                    _Rightbend_Wlowlimit = value;
                }
            }


            private double _Rightbend_XUpperlimit;
            public double Rightbend_XUpperlimit
            {
                get
                {
                    return _Rightbend_XUpperlimit;
                }
                set
                {
                    _Rightbend_XUpperlimit = value;
                }
            }

            private double _Rightbend_YUpperlimit;
            public double Rightbend_YUpperlimit
            {
                get
                {
                    return _Rightbend_YUpperlimit;
                }
                set
                {
                    _Rightbend_YUpperlimit = value;
                }
            }

            private double _Rightbend_WUpperlimit;
            public double Rightbend_WUpperlimit
            {
                get
                {
                    return _Rightbend_WUpperlimit;
                }
                set
                {
                    _Rightbend_WUpperlimit = value;
                }
            }

            #endregion 

            private double _leftzb_speed;
            public double LeftZB_Speed
            {
                get 
                {
                    return _leftzb_speed;
                }
                set
                {
                    _leftzb_speed = value;
                }
            }

            private double _midzb_speed;
            public double MidZB_Speed
            {
                get
                {
                    return _midzb_speed;
                }
                set
                {
                    _midzb_speed = value;
                }
            }


            private double _rightzb_speed;
            public double RightZB_Speed
            {
                get
                {
                    return _rightzb_speed;
                }
                set
                {
                    _rightzb_speed = value;
                }
            }

            private double _leftyb_time;
            public double LeftYB_Time
            {
                get
                {
                    return _leftyb_time;
                }
                set
                {
                    _leftyb_time = value;
                }
            }

            private double _midyb_time;
            public double MidYB_Time
            {
                get
                {
                    return _midyb_time;
                }
                set
                {
                    _midyb_time = value;
                }
            }

            private double _rightyb_time;
            public double RightYB_Time
            {
                get
                {
                    return _rightyb_time;
                }
                set
                {
                    _rightyb_time = value;
                }
            }

            private double _bend1adjust_speed;
            public double Bend1adjust_Speed
            {
                get
                {
                    return _bend1adjust_speed;
                }
                set
                {
                    _bend1adjust_speed = value;
                }
            }


            private double _bend2adjust_speed;
            public double Bend2adjust_Speed
            {
                get
                {
                    return _bend2adjust_speed;
                }
                set
                {
                    _bend2adjust_speed = value;
                }
            }



            private double _bend3adjust_speed;
            public double Bend3adjust_Speed
            {
                get
                {
                    return _bend3adjust_speed;
                }
                set
                {
                    _bend3adjust_speed = value;
                }
            }

            #region Transfer
            private double _transferZsafepos;
            public double TransferZSafePos
            {
                get
                {
                    return _transferZsafepos;
                }
                set
                {
                    _transferZsafepos = value;
                }

            }

            private double _transferXsafepos;
            public double TransferXSafePos
            {
                get
                {
                    return _transferXsafepos;
                }
                set
                {
                    _transferXsafepos = value;
                }
            }


            private double _transferXNGAPos;

            public double TransferX_NGA_Pos
            {
                get
                {
                    return _transferXNGAPos;
                }
                set
                {
                    _transferXNGAPos = value;
                }
            }


            private double _transferZNGApos;
            public double TransferZ_NGA_Pos
            {
                get
                {
                    return _transferZNGApos;
                }
                set
                {
                    _transferZNGApos = value;
                }
            }


            private double _transferXNGBPos;

            public double TransferX_NGB_Pos
            {
                get
                {
                    return _transferXNGBPos;
                }
                set
                {
                    _transferXNGBPos = value;
                }
            }


            private double _transferZNGBpos;
            public double TransferZ_NGB_Pos
            {
                get
                {
                    return _transferZNGBpos;
                }
                set
                {
                    _transferZNGBpos = value;
                }
            }

            private double _transferXNGCPos;

            public double TransferX_NGC_Pos
            {
                get
                {
                    return _transferXNGCPos;
                }
                set
                {
                    _transferXNGCPos = value;
                }
            }


            private double _transferZNGCpos;
            public double TransferZ_NGC_Pos
            {
                get
                {
                    return _transferZNGCpos;
                }
                set
                {
                    _transferZNGCpos = value;
                }
            }



            #endregion

            #region Discharge
            private double _dischargeZsafepos;
            public double DischargeZSafePos
            {
                get
                {
                    return _dischargeZsafepos;
                }
                set
                {
                    _dischargeZsafepos = value;
                }
            }


            private double _dischargeXsafepos;
            public double DischargeXSafePos
            {
                get
                {
                    return _dischargeXsafepos;
                }
                set
                {
                    _dischargeXsafepos = value;
                }
            }


            private double _dischargezokpullpos;
            public double DischargeZ_OK_PullPos
            {
                get
                {
                    return _dischargezokpullpos;
                }
                set
                {
                    _dischargezokpullpos = value;
                }
            }



            private double _dischargexokpullpos;

            public double DischargeX_OK_PullPos
            {
                get
                {
                    return _dischargexokpullpos;
                }
                set
                {
                    _dischargexokpullpos = value;
                }
            }


            private double _dischargezNGpullpos;
            public double DischargeZ_NG_PullPos
            {
                get
                {
                    return _dischargezNGpullpos;
                }
                set
                {
                    _dischargezNGpullpos = value;
                }
            }



            private double _dischargexNGpullpos;

            public double DischargeX_NG_PullPos
            {
                get
                {
                    return _dischargexNGpullpos;
                }
                set
                {
                    _dischargexNGpullpos = value;
                }
            }


            #endregion
            #region //ZGH20220912增加撕膜X位安全位
            private double _leftsmXsafepos;
            public double LeftSM_XSafePos
            {
                get
                {
                    return _leftsmXsafepos;
                }
                set
                {
                    _leftsmXsafepos = value;
                }
            }
            private double _midsmXsafepos;
            public double MidSM_XSafePos
            {
                get
                {
                    return _midsmXsafepos;
                }
                set
                {
                    _midsmXsafepos = value;
                }
            }
            private double _rightsmXsafepos;
            public double RightSM_XSafePos
            {
                get
                {
                    return _rightsmXsafepos;
                }
                set
                {
                    _rightsmXsafepos = value;
                }
            }
            #endregion

            private string _Name;

            private bool _IsFirstCodeParse;

            public bool IsFirstCodeParse
            {
                get
                {
                    return _IsFirstCodeParse;                  
                }
                set
                {
                    _IsFirstCodeParse = value;
                }
            }

            private string _FirstCode;

            public string FirstCode
            {
                get
                {
                    return _FirstCode;
                }
                set
                {
                    _FirstCode = value;
                }
            }

            #region W/A/B/Y轴位置

            private double _LightAWorkPos;

            private double _LightBWorkPos;

            private double _PlatWorkPos;

            public double LightAWorkPos
            {
                get
                {
                    return _LightAWorkPos;
                }
                set
                {
                    _LightAWorkPos = value;
                }
            }

            public double LightBWorkPos
            {
                get
                {
                    return _LightBWorkPos;
                }
                set
                {
                    _LightBWorkPos = value;
                }
            }

            public double PlatWorkPos
            {
                get
                {
                    return _PlatWorkPos;
                }
                set
                {
                    if (_PlatWorkPos != value)
                    {
                        OnStageWidthChanged();
                    }
                    _PlatWorkPos = value;
                }
            }

            #endregion

            #region 提前开关胶距离

            private List<XYZPoint> _StartOffSet;

            private List<XYZPoint> _EndOffSet;

            public List<XYZPoint> StartOffSet
            {
                get
                {
                    if (_StartOffSet == null)
                    {
                        _StartOffSet = new List<XYZPoint>();
                    }
                    if (_StartOffSet.Count < 6)
                    {
                        for (int i = _StartOffSet.Count; i < 6; i++)
                        {
                            _StartOffSet.Add(new XYZPoint());
                        }
                    }
                    return _StartOffSet;
                }
            }

            public List<XYZPoint> EndOffSet
            {
                get
                {
                    if (_EndOffSet == null)
                    {
                        _EndOffSet = new List<XYZPoint>();
                    }
                    if (_EndOffSet.Count < 6)
                    {
                        for (int i = _EndOffSet.Count; i < 6; i++)
                        {
                            _EndOffSet.Add(new XYZPoint());
                        }
                    }
                    return _EndOffSet;
                }
            }

            private double _ACSLong;

            public double ACSLong
            {
                get
                {
                    return _ACSLong;
                }
                set
                {
                    _ACSLong = value;
                }
            }

            #endregion

            private List<double> _BaseOffsetZ;

            public List<double> BaseOffsetZ
            {
                get
                {
                    if (_BaseOffsetZ == null)
                    {
                        _BaseOffsetZ = new List<double>();
                    }
                    if (_BaseOffsetZ.Count < 4)
                    {
                        _BaseOffsetZ.Add(0);
                    }
                    return _BaseOffsetZ;
                }
            }

            private DateTime _LastModifyTime;

            private PhotoPosCollection _photoDatas = new PhotoPosCollection();

            private GlueRangeClooection _GlueRanges = null;
          
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    _Name = value;
                }
            }

            public DateTime LastModifyTime
            {
                get
                {
                    return _LastModifyTime;
                }
                set
                {
                    _LastModifyTime = value;
                }
            }

            public PhotoPosCollection photoDatas
            {
                get
                {
                    if (_photoDatas.Count < 7)
                    {
                        for (int i = _photoDatas.Count; i < 7; i++)
                        {
                            _photoDatas.Add(new PhotoPosItem());
                        }
                    }
                    return _photoDatas;
                }
            }

            public GlueRangeClooection GlueRanges
            {
                get
                {
                    if (_GlueRanges == null)
                    {
                        _GlueRanges = new GlueRangeClooection();
                    }
                    if (_GlueRanges.Count < 7)
                    {
                        for (int i = _GlueRanges.Count; i < 7; i++)
                        {
                            _GlueRanges.Add(new GlueRange());
                        }
                    }
                    return _GlueRanges;
                }
            }
           
            [field: NonSerialized]
            public event EventHandler StageWidthChanged;

            private void OnStageWidthChanged()
            {
                if (StageWidthChanged != null)
                {
                    StageWidthChanged(this, null);
                }
            }
        }

        [Serializable]
        public class SMDataItem
        {
            private bool _SMEnabled;
            public bool SMEnabled
            {
                get
                {
                    return _SMEnabled;
                }
                set
                {
                    _SMEnabled = value;
                }
            }


            private bool _TearRllCLDEnabled;

            public bool TearRllCLDEnabled
            {
                get
                {
                    return _TearRllCLDEnabled;
                }
                set
                {
                    _TearRllCLDEnabled = value;
                }
            }


            private double _SMStartX;
            public double SMStartX
            {
                get
                {
                    return _SMStartX;
                }
                set
                {
                    _SMStartX = value;
                }
            }


            private double _SMStartY;
            public double SMStartY
            {
                get
                {
                    return _SMStartY;
                }
                set
                {
                    _SMStartY = value;
                }
            }


            private double _SMStartZ;
            public double SMStartZ
            {
                get
                {
                    return _SMStartZ;
                }
                set
                {
                    _SMStartZ = value;
                }
            }


            private double _SMEndX;
            public double SMEndX
            {
                get
                {
                    return _SMEndX;
                }
                set
                {
                    _SMEndX = value;
                }
            }


            private double _SMEndY;
            public double SMEndY
            {
                get
                {
                    return _SMEndY;
                }
                set
                {
                    _SMEndY = value;
                }
            }


            private double _SMEndZ;
            public double SMEndZ
            {
                get
                {
                    return _SMEndZ;
                }
                set
                {
                    _SMEndZ = value;
                }
            }

            private double _SMDist;
            public double SMDist
            {
                get
                {
                    return _SMDist;
                }
                set
                {
                    _SMDist = value;
                }
            }

            private double _GlueDist;
            public double GlueDist
            {
                get
                {
                    return _GlueDist;
                }
                set
                {
                    _GlueDist = value;
                }
            }

            private double _GlueStickDist;
            public double GlueStickDist
            {
                get
                {
                    return _GlueStickDist;
                }
                set
                {
                    _GlueStickDist = value;
                }
            }

            private double _GlueStickSpd;
            public double  GlueStickSpd
            {
                get
                {
                    return _GlueStickSpd;
                }

                set
                {
                    _GlueStickSpd = value;
                }
            }
        }

        [Serializable]
        public class SMDataItemCollection : CollectionBase
        {

            public SMDataItem this[int index]
            {
                get
                {
                    return InnerList[index] as SMDataItem;
                }
            }

            public void Add(SMDataItem item)
            {
                InnerList.Add(item);
            }

            public void Remove(SMDataItem item)
            {
                InnerList.Remove(item);
            }

            public int IndexOf(SMDataItem item)
            {
                return InnerList.IndexOf(item);
            }
        }

        [Serializable]
        public class PhotoPosCollection : CollectionBase
        {
            public PhotoPosItem this[int index]
            {
                get
                {
                    return InnerList[index] as PhotoPosItem;
                }
            }

            public void Add(PhotoPosItem item)
            {
                InnerList.Add(item);
            }

            public void Remove(PhotoPosItem item)
            {
                InnerList.Remove(item);
            }

            public int IndexOf(PhotoPosItem item)
            {
                return InnerList.IndexOf(item);
            }
        }

        [Serializable]
        public class PhotoPosItem
        {
            private double _X = 0.0;

            private double _Y = 0.0;

            private double _Z = 0.0;

            private double _U = 0.0;

            private double _V = 0.0;

            public double X
            {
                get
                {
                    return _X;
                }
                set
                {
                    _X = value;
                }
            }

            public double Y
            {
                get
                {
                    return _Y;
                }
                set
                {
                    _Y = value;
                }
            }

            public double Z
            {
                get
                {
                    return _Z;
                }
                set
                {
                    _Z = value;
                }
            }

            public double U
            {
                get
                {
                    return _U;
                }
                set
                {
                    _U = value;
                }
            }

            public double V
            {
                get
                {
                    return _V;
                }
                set
                {
                    _V = value;
                }
            }
        }

        [Serializable]
        public class GlueRangeClooection : CollectionBase
        {
            private bool _AoiEnabled;

            public bool AoiEnabled
            {
                get
                {
                    return _AoiEnabled;
                }
                set
                {
                    _AoiEnabled = value;
                }
            }

            public GlueRange this[int index]
            {
                get
                {
                    return InnerList[index] as GlueRange;
                }
            }

            public void Add(GlueRange item)
            {
                InnerList.Add(item);
            }

            public void Remove(GlueRange item)
            {
                InnerList.Remove(item);
            }

            public int IndexOf(GlueRange item)
            {
                return InnerList.IndexOf(item);
            }
        }

        [Serializable]
        public class GlueRange
        {
            private double _WMax = 1.0;

            private double _WMin = 0.5;

            private double _HMax = 0.5;

            private double _HMin = 0.2;

            private double _GlueSpeed = 55;

            private string _LaserFileName = "B";

           // private VermesSet _Vermes;

            private bool _IsGlueEnabled;

            private bool _IsDetectEnabled;

            public double WMax
            {
                get
                {
                    return _WMax;
                }
                set
                {
                    _WMax = value;
                }
            }

            public double WMin
            {
                get
                {
                    return _WMin;
                }
                set
                {
                    _WMin = value;
                }
            }

            public double HMax
            {
                get
                {
                    return _HMax;
                }
                set
                {
                    _HMax = value;
                }
            }

            public double HMin
            {
                get
                {
                    return _HMin;
                }
                set
                {
                    _HMin = value;
                }
            }

            public double GlueSpeed
            {
                get
                {
                    return _GlueSpeed;
                }
                set
                {
                    _GlueSpeed = value;
                }
            }

            public string LaserFileName
            {
                get
                {
                    return _LaserFileName;
                }
                set
                {
                    _LaserFileName = value;
                }
            }

           

            public bool IsGlueEnabled
            {
                get
                {
                    return _IsGlueEnabled;
                }
                set
                {
                    _IsGlueEnabled = value;
                }
            }

            public bool IsDetectEnabled
            {
                get
                {
                    return _IsDetectEnabled;
                }
                set
                {
                    _IsDetectEnabled = value;
                }
            }
        }

        [Serializable]
        public class VermesSet
        {
            private double _Raising = 0.5;

            private double _OpenTime = 0.5;

            private double _Falling = 0.1;

            private int _NeedleLift = 50;

            private int _NumOfPulse = 0;

            private double _DelayTime = 1.2;

            public double Raising
            {
                get
                {
                    return _Raising;
                }
                set
                {
                    _Raising = value;
                }
            }

            public double OpenTime
            {
                get
                {
                    return _OpenTime;
                }
                set
                {
                    _OpenTime = value;
                }
            }

            public double Falling
            {
                get
                {
                    return _Falling;
                }
                set
                {
                    _Falling = value;
                }
            }

            public int NeedleLift
            {
                get
                {
                    return _NeedleLift;
                }
                set
                {
                    _NeedleLift = value;
                }
            }

            public int NumOfPulse
            {
                get
                {
                    return _NumOfPulse;
                }
                set
                {
                    _NumOfPulse = value;
                }
            }

            public double DelayTime
            {
                get
                {
                    return _DelayTime;
                }
                set
                {
                    _DelayTime = value;
                }
            }
        }
        
        public MeasurementData()
        {

        }

        public static new MeasurementData Load()
        {
            string path = Path.Combine(Application.StartupPath, "set/data.config");
            return Load(path) as MeasurementData;
        }

        public override bool Save()
        {
            bool find = false;
            int i = 0;
            while (i < _DateCollection.Count)
            {
                if (_DateCollection[i] != _CurrentRecipeData)
                {
                    i++;
                }
                else
                {
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                _CurrentRecipeData = null;
            }
            string path = Path.Combine(Application.StartupPath, "set/data.config");
            return Save(path);
        }
    }
}

