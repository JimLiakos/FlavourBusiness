using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace FlavourBusinessToolKit
{

    /// <MetaDataID>{76e3f211-761c-43a8-a4f9-136b421be41e}</MetaDataID>
    public interface IRestaurantMenuPublisher
    {
        /// <MetaDataID>{c0a613f7-9a93-4112-b74f-5908dc824e83}</MetaDataID>
        void PublishMenu(string serverStorageFolder,string menuResourcesPrefix, string previousVersionServerStorageFolder, IFileManager fileManager, string menuName);
    }

    /// <MetaDataID>{acb01dc6-8fb8-4b99-bd3a-da38154f7f64}</MetaDataID>
    public class SvgUtilities
    {
        public static void SetColor(XDocument accebtImageDoc, string colorCode)
        {
            var styleText = accebtImageDoc.Root.Descendants("{http://www.w3.org/2000/svg}style").ToArray()[0].Nodes().OfType<System.Xml.Linq.XCData>().ToArray()[0].Value;
            ExCSS.Parser parser = new ExCSS.Parser();

            var styleSheet = parser.Parse(styleText);

            var svgColorDefinitionStyle = (from styleRule in styleSheet.StyleRules
                                           where styleRule.Value == ".st0"
                                           select styleRule).FirstOrDefault();
            var colorProperty = (from styleProperty in svgColorDefinitionStyle.Declarations.Properties
                                 where styleProperty.Name == "fill" || styleProperty.Name == "stroke"
                                 select styleProperty).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(colorCode) && colorCode != "none")
            {
                Color color = (Color)ColorConverter.ConvertFromString(colorCode);
                colorProperty.Term = new ExCSS.HtmlColor(color.A, color.R, color.G, color.B);
                accebtImageDoc.Root.Descendants("{http://www.w3.org/2000/svg}style").ToArray()[0].Nodes().OfType<System.Xml.Linq.XCData>().ToArray()[0].Value = styleSheet.ToString();
            }

        }

    }

    /// <MetaDataID>{a5fdce26-f38e-49eb-a169-cfd683a6c994}</MetaDataID>
    public class FlavourBusinessManager
    {
        public bool PublishMenu(string credentialKey, string serverStorageFolder, FlavourBusinessFacade.OrganizationStorageRef storageRef)
        {
            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
            //var serverStorageFileName = storageRef.StorageUrl userFolder + "/" + serverStorageFolder + "/" + fileName;
            RawStorageData rawStorageData = new RawStorageData(storageRef, null);
            string rootUri = FileManager.CurrentFileManager.RootUri;
            //System.Xml.Linq.XDocument restaurantMenuDoc = System.Xml.Linq.XDocument.Load(rootUri + "usersfolder/" + serverStorageFileName);

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenu", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));

            IRestaurantMenuPublisher restaurantMenu = (from menu in storage.GetObjectCollection<IRestaurantMenuPublisher>()
                                                                   select menu).FirstOrDefault();

           // restaurantMenu.PublishMenu(serverStorageFolder, FileManager.CurrentFileManager, storageRef.Name);
            //var jsonRestaurantMenu = new MenuPresentationModel.JsonMenuPresentation.RestaurantMenu(restaurantMenu);

           

            return true;
        }

    
    }



    /// <MetaDataID>{cf6d8d9e-6df1-4ae3-b40c-515173ec1ddf}</MetaDataID>
    public struct Fraction
    {
        public Fraction(int n, int d)
        {
            N = n;
            D = d;
        }

        public int N { get; private set; }
        public int D { get; private set; }

        public override string ToString()
        {
            if (D>1)
                return $"{this.N.ToString()}/{this.D.ToString()}";
            else
                return N.ToString();
        }
        public static Fraction RealToFraction(double value, double accuracy)
        {
            if (accuracy <= 0.0 || accuracy >= 1.0)
            {
                throw new ArgumentOutOfRangeException("accuracy", "Must be > 0 and < 1.");
            }

            int sign = Math.Sign(value);

            if (sign == -1)
            {
                value = Math.Abs(value);
            }

            // Accuracy is the maximum relative error; convert to absolute maxError
            double maxError = sign == 0 ? accuracy : value * accuracy;

            int n = (int)Math.Floor(value);
            value -= n;

            if (value < maxError)
            {
                return new Fraction(sign * n, 1);
            }

            if (1 - maxError < value)
            {
                return new Fraction(sign * (n + 1), 1);
            }

            // The lower fraction is 0/1
            int lower_n = 0;
            int lower_d = 1;

            // The upper fraction is 1/1
            int upper_n = 1;
            int upper_d = 1;

            while (true)
            {
                // The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                int middle_n = lower_n + upper_n;
                int middle_d = lower_d + upper_d;

                if (middle_d * (value + maxError) < middle_n)
                {
                    // real + error < middle : middle is our new upper
                    upper_n = middle_n;
                    upper_d = middle_d;
                }
                else if (middle_n < (value - maxError) * middle_d)
                {
                    // middle < real - error : middle is our new lower
                    lower_n = middle_n;
                    lower_d = middle_d;
                }
                else
                {
                    // Middle is our best fraction
                    return new Fraction((n * middle_d + middle_n) * sign, middle_d);
                }
            }
        }
    }



}
