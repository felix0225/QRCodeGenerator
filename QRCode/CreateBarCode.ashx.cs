using System;
using System.IO;
using System.Web;
using ZXing;
using ZXing.OneD;

namespace QRCode
{
    /// <summary>
    /// CreateBarCode 的摘要描述
    /// </summary>
    public class CreateBarCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var strTxt = context.Request["txt"] ?? string.Empty;
            var strFName = context.Request["fn"] ?? Guid.NewGuid().ToString();
            var strW = context.Request["w"] ?? "250";   //寬
            var strH = context.Request["h"] ?? "100";   //高
            var strBg = context.Request["bg"] ?? "FFFFFF";  //背景色
            var strFg = context.Request["fg"] ?? "000000";  //文字顏色
            var pureCode = context.Request["pc"] ?? "false";  //文字顏色

            strBg = "#" + strBg;
            strFg = "#" + strFg;

            if (string.IsNullOrWhiteSpace(strTxt)) return;

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Renderer = new ZXing.Rendering.BitmapRenderer { Background = System.Drawing.ColorTranslator.FromHtml(strBg), Foreground = System.Drawing.ColorTranslator.FromHtml(strFg) },
                Options = new Code128EncodingOptions
                {
                    Height = int.Parse(strH),
                    Width = int.Parse(strW),
                    Margin = 4,                                 //去白邊
                    PureBarcode = Convert.ToBoolean(pureCode)   //是否不顯示文字
                }
            };

            var mBitmap = writer.Write(strTxt);
            var ms = new MemoryStream();
            mBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); //JPG、GIF、PNG等均可  

            var buff = ms.ToArray();

            try
            {
                var fileName = strFName + ".jpg";

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "image/jpeg"; //二進位方式

                //設定標頭檔資訊
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;  filename=" + HttpUtility.UrlEncode(fileName));
                HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "Binary");
                HttpContext.Current.Response.AddHeader("Content-Length", buff.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(buff);
                HttpContext.Current.Response.End();

            }
            catch (Exception e)
            {
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(e.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                //return false;
                return true;
            }
        }
    }
}