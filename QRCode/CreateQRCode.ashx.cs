using System;
using System.Web;
using System.IO;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace QRCode
{
    /// <summary>
    /// CreateQRCode 的摘要描述
    /// </summary>
    public class CreateQRCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var strTxt = context.Request["txt"] ?? string.Empty;
            var strFName = context.Request["fn"] ?? Guid.NewGuid().ToString();
            var strW = context.Request["w"] ?? "150";   //寬
            var strH = context.Request["h"] ?? "150";   //高
            var strBG = context.Request["bg"] ?? "FFFFFF";  //背景色
            var strFG = context.Request["fg"] ?? "000000";  //文字顏色

            strBG = "#" + strBG;
            strFG = "#" + strFG;

            if (string.IsNullOrWhiteSpace(strTxt)) return;

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new ZXing.Rendering.BitmapRenderer { Background = System.Drawing.ColorTranslator.FromHtml(strBG), Foreground = System.Drawing.ColorTranslator.FromHtml(strFG) },
                Options = new QrCodeEncodingOptions
                {
                    Height = int.Parse(strH),
                    Width = int.Parse(strW),
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    ErrorCorrection = ErrorCorrectionLevel.H,
                    Margin = 1,
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
                return false;
            }
        }
    }
}