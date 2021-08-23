using FlavourBusinessML.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PreparationDataMining
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            ModelInput sampleData = new ModelInput()
            {
                OnPreparationItemsResourceUnits = 99,
                Amount = 4,
                AvgTimeToComplete = 1,
                PendingItemsResourceUnits = 90,
                ItemResourceUnit = 6,
                DefaultItemPreparationTime = 9,
            };
            var score = ConsumeModel.Predict(sampleData).Score;

            base.OnStartup(e);
        }
    }
}
