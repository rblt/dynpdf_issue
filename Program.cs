using ceTe.DynamicPDF.PageElements.Forms;
using ceTe.DynamicPDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace DynamicPDF_TSA_Issue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Document document = new Document();
            Page page = new Page();

            // Create  add Signature Form Field
            Signature signature = new Signature("SigField", 10, 10, 250, 100);
            page.Elements.Add(signature);
            document.Pages.Add(page);
            Certificate certificate = new Certificate(@"cert\ec_certificate_384.pfx", "");

            // Create TimestampServer
            TimestampServer timestampServer = new TimestampServer("https://rfc3161.ai.moda");
            document.Sign("SigField", certificate, timestampServer);
            document.Draw("signed.pdf"); // throws System.NotSupportedException
        }
    }
}
