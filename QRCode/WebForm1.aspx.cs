using System;

namespace QRCode
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const string txt = "https://github.com/micjahn/ZXing.Net";
            Image1.ImageUrl = $"CreateQRCode.ashx?txt={txt}&w=458&h=458";

            var bartxt = "2017051609460011";
            Image2.ImageUrl = $"CreateBarCode.ashx?txt={bartxt}";
        }
    }
}