using System;

namespace QRCode
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const string txt = "https://github.com/micjahn/ZXing.Net";
            Image1.ImageUrl = $"CreateQRCode.ashx?txt={txt}";
        }
    }
}