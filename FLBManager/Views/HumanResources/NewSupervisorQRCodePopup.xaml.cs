using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FLBManager.Views.HumanResources
{
    /// <summary>
    /// Interaction logic for NewSupervisorQRCodePopup.xaml
    /// </summary>
    /// <MetaDataID>{97671b58-c585-4994-8fd6-3aed5bacdad7}</MetaDataID>
    public partial class NewUserQRCodePopup : StyleableWindow.Window
    {
        /// <MetaDataID>{27fd605e-efd5-49d1-8d98-eac89013fb64}</MetaDataID>
        public NewUserQRCodePopup(string title, string prompt)
        {
            InitializeComponent();

            TitleText = title;
            Prompt = prompt;
            DataContext = this;
        }

        /// <MetaDataID>{e947a9dd-d49e-4dda-b1fd-bba63eea1d2d}</MetaDataID>
        public string TitleText { get; set; }

        /// <MetaDataID>{43ecb575-8426-44e6-8fd2-570246582140}</MetaDataID>
        public string Prompt { get; set; }

        /// <MetaDataID>{126dc12d-4270-4e07-9331-6a56e176f56f}</MetaDataID>
        public string CodeValue { get; set; }

        /// <MetaDataID>{2e52d165-fca2-414a-acd8-5fc50b62cba6}</MetaDataID>
        public string Color { get; set; } = "#000000";

        /// <MetaDataID>{8425e3ac-ad9a-4b23-80a3-0b23b0bcb3a2}</MetaDataID>
        BitmapImage QRCodeBitmapImage;
        /// <MetaDataID>{85062b88-2c18-4ba1-9c25-443dbc17fc87}</MetaDataID>
        public BitmapImage QRCodeImage
        {
            get
            {
                if (QRCodeBitmapImage == null)
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(CodeValue, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    var qrCodeImage = qrCode.GetGraphic(20, Color, "#FFFFFF", true);
                    System.IO.MemoryStream ms = new MemoryStream();
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    QRCodeBitmapImage = new BitmapImage();
                    QRCodeBitmapImage.BeginInit();
                    QRCodeBitmapImage.StreamSource = ms;
                    QRCodeBitmapImage.EndInit();
                }

                return QRCodeBitmapImage;

            }

        }


    }
}
