using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using Azure;
using System.Drawing;

namespace FlavoursServicesWorkerRole.Controllers
{

    /// <summary>
    /// Storages
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MenuModelController : ApiController
    {
        [HttpGet]
        [Route("api/MenuModel/Font/{fontUri}")]

        public object GetFontData(string fontUri)
        {
            var tablesAccount = FlavourBusinessManager.FlavourBusinessManagerApp.TablesAccount;

            Azure.Data.Tables.TableClient table = tablesAccount.GetTableClient("FontDataEntry");
            Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "FontDataEntry"));
            bool fontDataEntryTable_exist = queryTableResults.Count() > 0;
            if (!fontDataEntryTable_exist)
                table.CreateIfNotExists();

            var fontDataEntry = (from theFontData in table.Query<FontDataEntry>()
                                 where theFontData.RowKey==fontUri
                                 select theFontData).FirstOrDefault();

            if (fontDataEntry == null)
                return null;
             
            var fontData = new UIBaseEx.FontData();
            fontData.Overline=fontDataEntry.Overline;
            fontData.Underline =fontDataEntry.Underline;
            fontData.FontSpacing =fontDataEntry.FontSpacing;
            fontData.ShadowColor =fontDataEntry.ShadowColor;
            fontData.BlurRadius =fontDataEntry.BlurRadius;
            fontData.ShadowYOffset =fontDataEntry.ShadowYOffset;
            fontData.ShadowXOffset =fontDataEntry.ShadowXOffset;
            fontData.StrokeThickness =fontDataEntry.StrokeThickness;
            fontData.StrokeFill =fontDataEntry.StrokeFill;
            fontData.FontSize =fontDataEntry.FontSize;
            fontData.FontWeight =fontDataEntry.FontWeight;
            fontData.FontFamilyName =fontDataEntry.FontFamilyName;
            fontData.FontStyle =fontDataEntry.FontStyle;
            fontData.Foreground =fontDataEntry.Foreground;
            fontData.Stroke =fontDataEntry.Stroke;
            fontData.AllCaps =fontDataEntry.AllCaps;
            fontData.Shadow =fontDataEntry.Shadow;

            return fontData;
        }

        [HttpPost]
        [Route("api/MenuModel/Font")]

        public string PostFontData([FromBody] UIBaseEx.FontData font)
        {

            var tablesAccount = FlavourBusinessManager.FlavourBusinessManagerApp.TablesAccount;

            Azure.Data.Tables.TableClient table = tablesAccount.GetTableClient("FontDataEntry");
            Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "FontDataEntry"));
            bool fontDataEntryTable_exist = queryTableResults.Count() > 0;
            if (!fontDataEntryTable_exist)
                table.CreateIfNotExists();

            var fontDataEntries = (from fontData in table.Query<FontDataEntry>()
                                   where fontData.FontFamilyName == font.FontFamilyName&&fontData.FontSize == font.FontSize&&fontData.AllCaps == font.AllCaps&&fontData.FontStyle == font.FontStyle&&fontData.FontWeight == font.FontWeight
                                   select fontData).ToList();

            var fontDataEntry = fontDataEntries.Where(x => font==x).FirstOrDefault();

            if (fontDataEntry!=null)
                return fontDataEntry.RowKey;
            else
            {
                fontDataEntry=new FontDataEntry();
                fontDataEntry.RowKey=Guid.NewGuid().ToString("N"); ;
                fontDataEntry.PartitionKey="AAA";
                fontDataEntry.Overline=font.Overline;
                fontDataEntry.Underline =font.Underline;
                fontDataEntry.FontSpacing =font.FontSpacing;
                fontDataEntry.ShadowColor =font.ShadowColor;
                fontDataEntry.BlurRadius =font.BlurRadius;
                fontDataEntry.ShadowYOffset =font.ShadowYOffset;
                fontDataEntry.ShadowXOffset =font.ShadowXOffset;
                fontDataEntry.StrokeThickness =font.StrokeThickness;
                fontDataEntry.StrokeFill =font.StrokeFill;
                fontDataEntry.FontSize =font.FontSize;
                fontDataEntry.FontWeight =font.FontWeight;
                fontDataEntry.FontFamilyName =font.FontFamilyName;
                fontDataEntry.FontStyle =font.FontStyle;
                fontDataEntry.Foreground =font.Foreground;
                fontDataEntry.Stroke =font.Stroke;
                fontDataEntry.AllCaps =font.AllCaps;
                fontDataEntry.Shadow =font.Shadow;
                var result = table.AddEntity(fontDataEntry);
                return fontDataEntry.RowKey;
            }
        }
    }

    public class FontDataEntry : Azure.Data.Tables.ITableEntity
    {
        public bool Underline { get; set; }
        public bool Overline { get; set; }
        public double FontSpacing { get; set; }
        public string ShadowColor { get; set; }
        public double BlurRadius { get; set; }
        public double ShadowYOffset { get; set; }
        public double ShadowXOffset { get; set; }
        public double StrokeThickness { get; set; }
        public string StrokeFill { get; set; }
        public double FontSize { get; set; }
        public string FontWeight { get; set; }
        public string FontFamilyName { get; set; }
        public string FontStyle { get; set; }
        public string Foreground { get; set; }

        public bool Stroke { get; set; }
        public bool AllCaps { get; set; }
        public bool Shadow { get; set; }

        public static bool operator ==(FontDataEntry left, FontDataEntry right)
        {
            if (!(left is object) && !(right is object)) return true;

            if (!(left is object) ||!(right is object)) return false;


            if (left.AllCaps == right.AllCaps &&
                left.FontFamilyName == right.FontFamilyName &&
                left.FontSize == right.FontSize &&
                left.FontSpacing == right.FontSpacing &&
                left.FontStyle == right.FontStyle &&
                left.FontWeight == right.FontWeight &&
                left.Foreground == right.Foreground &&
                left.Shadow == right.Shadow &&
                left.Stroke == right.Stroke &&
                left.ShadowColor == right.ShadowColor &&
                left.ShadowXOffset == right.ShadowXOffset &&
                left.ShadowYOffset == right.ShadowYOffset &&
                left.StrokeFill == left.StrokeFill &&
                left.StrokeThickness == left.StrokeThickness &&
                left.BlurRadius == right.BlurRadius)
                return true;
            else
                return false;
        }

        public static bool operator !=(FontDataEntry left, FontDataEntry right)
        {
            return !(left == right);
        }

        public static bool operator ==(UIBaseEx.FontData left, FontDataEntry right)
        {
            if (left.AllCaps == right.AllCaps &&
                left.FontFamilyName == right.FontFamilyName &&
                left.FontSize == right.FontSize &&
                left.FontSpacing == right.FontSpacing &&
                left.FontStyle == right.FontStyle &&
                left.FontWeight == right.FontWeight &&
                left.Foreground == right.Foreground &&
                left.Shadow == right.Shadow &&
                left.Stroke == right.Stroke &&
                left.ShadowColor == right.ShadowColor &&
                left.ShadowXOffset == right.ShadowXOffset &&
                left.ShadowYOffset == right.ShadowYOffset &&
                left.StrokeFill == left.StrokeFill &&
                left.StrokeThickness == left.StrokeThickness &&
                left.BlurRadius == right.BlurRadius)
                return true;
            else
                return false;
        }

        public static bool operator !=(UIBaseEx.FontData left, FontDataEntry right)
        {
            return !(left == right);
        }


        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

}
