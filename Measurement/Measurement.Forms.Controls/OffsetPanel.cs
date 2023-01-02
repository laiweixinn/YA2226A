using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZ.CNC.Measurement.Core;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class OffsetPanel : UserControl
    {
        public OffsetPanel()
        {
            InitializeComponent();
        }

        public void Init()
        {

            MeasurementData.RecipeDataItem recipe = MeasurementContext.Data.CurrentRecipeData;
            MeasurementConfig config = MeasurementContext.Config;

            if (config!=null)
            {
                //nib_aendzoffset.Value = config.AEndOffsetZ;
                //nib_bendzoffset.Value = config.BEndOffsetZ;
            }

            if (recipe!=null)
            {
              

                nib_aswidth.Value = recipe.StartOffSet[0].X;
                nib_aslength.Value = recipe.StartOffSet[0].Y;
                nib_asheight.Value = recipe.StartOffSet[0].Z;

                nib_aewidth.Value = recipe.EndOffSet[0].X;
                nib_aelength.Value = recipe.EndOffSet[0].Y;
                nib_aeheight.Value = recipe.EndOffSet[0].Z;

                nib_bswidth.Value = recipe.StartOffSet[1].Y;
                nib_bslength.Value = recipe.StartOffSet[1].X;
                nib_bsheight.Value = recipe.StartOffSet[1].Z;

                nib_bewidth.Value = recipe.EndOffSet[1].Y;
                nib_belength.Value = recipe.EndOffSet[1].X;
                nib_beheight.Value = recipe.EndOffSet[1].Z;

                nib_cswidth.Value = recipe.StartOffSet[2].X;
                nib_cslength.Value = recipe.StartOffSet[2].Y;
                nib_csheight.Value = recipe.StartOffSet[2].Z;

                nib_cewidth.Value = recipe.EndOffSet[2].X;
                nib_celength.Value = recipe.EndOffSet[2].Y;
                nib_ceheight.Value = recipe.EndOffSet[2].Z;

                str_codefirstold.Text = recipe.FirstCode;
                chkex_isuvdisabled.IsCkecked = recipe.IsFirstCodeParse;
            }
        }

        public void Save()
        {
            MeasurementData.RecipeDataItem recipe = MeasurementContext.Data.CurrentRecipeData;
            MeasurementConfig config = MeasurementContext.Config;

            //config.AEndOffsetZ = nib_aendzoffset.Value;
            //config.BEndOffsetZ = nib_bendzoffset.Value;

            recipe.StartOffSet[0].X = nib_aswidth.Value;
            recipe.StartOffSet[0].Y = nib_aslength.Value;
            recipe.StartOffSet[0].Z = nib_asheight.Value;

            recipe.EndOffSet[0].X = nib_aewidth.Value;
            recipe.EndOffSet[0].Y = nib_aelength.Value;
            recipe.EndOffSet[0].Z = nib_aeheight.Value;

            recipe.StartOffSet[1].Y = nib_bswidth.Value;
            recipe.StartOffSet[1].X = nib_bslength.Value;
            recipe.StartOffSet[1].Z = nib_bsheight.Value;

            recipe.EndOffSet[1].Y = nib_bewidth.Value;
            recipe.EndOffSet[1].X = nib_belength.Value;
            recipe.EndOffSet[1].Z = nib_beheight.Value;

            recipe.StartOffSet[2].X = nib_cswidth.Value;
            recipe.StartOffSet[2].Y = nib_cslength.Value;
            recipe.StartOffSet[2].Z = nib_csheight.Value;

            recipe.EndOffSet[2].X = nib_cewidth.Value;
            recipe.EndOffSet[2].Y = nib_celength.Value;
            recipe.EndOffSet[2].Z = nib_ceheight.Value;

            recipe.FirstCode = str_codefirstold.Text;
     

        }

        public void RefreshUI()
        {
            groupBox3.Enabled = MeasurementContext.Config.IsPointLaserDisabled;          
            groupBox1.Enabled = MeasurementContext.UesrManage.LoginType != LZ.CNC.UserLevel. LoginTypes.None;
        }

    }


}
