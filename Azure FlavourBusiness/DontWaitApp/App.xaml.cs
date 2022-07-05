using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.HashFunction.CRC;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;
using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace DontWaitApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{f9e00b1e-8725-4050-a603-1d5b2c472f33}</MetaDataID>
    public partial class App : Application,IAppLifeTime
    {

        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();

        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;


        protected override void OnStartup(StartupEventArgs e)
        {
            SerializeTaskScheduler.RunAsync();

            //CultureInfo ci = new CultureInfo(1033);
            //Thread.CurrentThread.CurrentCulture = ci;
            //Thread.CurrentThread.CurrentUICulture = ci;
            string sessionoken = Guid.NewGuid().ToString("N");

            string url = String.Format("https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&language={1}&key={2}&sessiontoken={3}", "Ρηγα", System.Globalization.CultureInfo.CurrentCulture.Name, "AIzaSyAuon626ZLzKmYgmCCpAF3dvILvSizjaTI", sessionoken);



            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);
            //SerializationBinder.NamesTypesDictionary["MenuModel.MealType"] = typeof(MenuModel.MealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.FixedMealType"] = typeof(MenuModel.FixedMealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.MealCourseType"] = typeof(MenuModel.MealCourseType);



            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";


            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealType)] = "MenuModel.MealType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.FixedMealType)] = "MenuModel.FixedMealType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealCourseType)] = "MenuModel.MealCourseType";


            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";

            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";


            //XDocument doc = XDocument.Load(@"C:\Users\Jim\Google Drive\Κτηματολόγιο\GGRS87\03-02-2019.kml");

            //foreach(var element in doc.Root.Element("{http://www.opengis.net/kml/2.2}Document").Elements("{http://www.opengis.net/kml/2.2}Placemark"))
            //{
            //    string line = element.Element("{http://www.opengis.net/kml/2.2}Point").Element("{http://www.opengis.net/kml/2.2}coordinates").Value;
            //    string longstr  = line.Substring(0, line.IndexOf(",")).ToString().Trim();
            //    line = line.Substring(line.IndexOf(",") + 1).ToString().Trim();
            //    string latstr = line.Substring(0, line.IndexOf(",")).ToString().Trim();
            //    double lat = double.Parse(latstr, new System.Globalization.CultureInfo(1033));
            //    double _long = double.Parse(longstr, new System.Globalization.CultureInfo(1033));

            //    var coord = SpatialReference.GreekGrid.WorldToGreek(new System.Device.Location.GeoCoordinate(lat, _long));
            //    var res = string.Format(new System.Globalization.CultureInfo(1033), "{0},{1}", coord[0], coord[1]);
            //    System.Diagnostics.Debug.WriteLine(res);


            //}
            //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Jim\Google Drive\Κτηματολόγιο\GGRS87\Μπαρτσεϊκα.txt");
            //string line = null;
            //while ((line = file.ReadLine()) != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(line))
            //    {
            //        string latstr = line.Substring(0, line.IndexOf(",")).ToString().Trim();
            //        string longstr = line.Substring(line.IndexOf(",") + 1).ToString().Trim();
            //        double lat = double.Parse(latstr, new System.Globalization.CultureInfo(1033));
            //        double _long = double.Parse(longstr, new System.Globalization.CultureInfo(1033));
            //        var coord = SpatialReference.GreekGrid.WorldToGreek(new System.Device.Location.GeoCoordinate(lat, _long));
            //        var res = string.Format(new System.Globalization.CultureInfo(1033), "{0},{1}", coord[0], coord[1]);
            //        System.Diagnostics.Debug.WriteLine(res);
            //    }

            //}

            //file.Close();
            Dictionary<string, List<string>> partitionIds = new Dictionary<string, List<string>>();
            int count = 5000;
            while (count > 0)
            {
                count--;
                string id = Guid.NewGuid().ToString("N");
                string partitionKey = CRCFactory.Instance.Create(CRCConfig.CRC4_ITU).ComputeHash(System.Text.Encoding.UTF8.GetBytes(id)).AsHexString();
                List<string> ids = null;
                if (!partitionIds.TryGetValue(partitionKey, out ids))
                {
                    ids = new List<string>();
                    partitionIds[partitionKey] = ids;
                }
                ids.Add(id);

            }

            //new Inu<FlavourBusinessFacade.IOrganization>().Contains(.Contains()

            foreach (var col in partitionIds.Values)
            {
                System.Diagnostics.Debug.WriteLine(col.Count);
            }





            if (e.Args.Length > 0)
                foreach (var arg in e.Args)
                {
                    string simDescription = arg.Substring(0, arg.IndexOf(";"));
                    string simIdentity = arg.Substring(arg.IndexOf(";") + 1);
                    OOAdvantech.Net.DeviceOOAdvantechCore.LinesPhoneNumbers.Add(new OOAdvantech.SIMCardData() { SIMCardIdentity = simIdentity, SIMCardDescription = simDescription });
                }

            base.OnStartup(e);
        }



        void ApplicationStart(object sender, StartupEventArgs e)
        {

            DeviceSelectorWindow selector = new DeviceSelectorWindow();
            selector.Show();
          

        }
        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }
    }
}
