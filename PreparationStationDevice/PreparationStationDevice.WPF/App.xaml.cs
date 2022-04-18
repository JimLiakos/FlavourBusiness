using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace PreparationStationDevice.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{dc5867f9-72db-4125-8c62-4f0391370c3e}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();

        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;



        protected override void OnStartup(StartupEventArgs e)
        {
            SerializeTaskScheduler.RunAsync();

            DeviceSelectorWindow mainWindow = new DeviceSelectorWindow();
            mainWindow.Show();

            var json = System.IO.File.ReadAllText(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup\StationVelocity.json");

            var itemsPreparationHistory = OOAdvantech.Json.JsonConvert.DeserializeObject<List<ItemPreparationTimeSpan>>(json);
            var calculator = new SimpleMovingAverage(k: 7);
            int i = 1;
            List<int> averagePercs = new List<int>();
            List<int> smoothAveragePercs = new List<int>();

            List<int> timespans = new List<int>();
            while (itemsPreparationHistory.Count >= i)
            {
                var itemsPreparationHistorypart = itemsPreparationHistory.Take(i).ToList();

                if (itemsPreparationHistorypart.Count > 5)
                    itemsPreparationHistorypart = itemsPreparationHistorypart.Skip(itemsPreparationHistorypart.Count - 5).ToList();
                var averageDif = itemsPreparationHistorypart.Sum(x => x.DurationDif) / itemsPreparationHistorypart.Count;
                var averagePreparationTimeSpanInMin = itemsPreparationHistorypart.Sum(x => x.PreparationTimeSpanInMin) / itemsPreparationHistorypart.Count;
                if (itemsPreparationHistorypart.Count == 1)
                    timespans.Add(0);
                else
                {
                    timespans.Add((int)Math.Ceiling((itemsPreparationHistorypart[itemsPreparationHistorypart.Count - 1].PreparationEndsAt - itemsPreparationHistorypart[itemsPreparationHistorypart.Count - 2].PreparationEndsAt).TotalMinutes));
                }

                
                var avargePerc = (int)Math.Ceiling((averageDif / averagePreparationTimeSpanInMin) * 100);
                averagePercs.Add(avargePerc);

               // smoothAveragePercs.Add((int)Math.Ceiling(calculator.Update((int)Math.Ceiling((itemsPreparationHistory[i-1].DurationDif / itemsPreparationHistory[i-1].PreparationTimeSpanInMin) * 100))));
                
            
                i++;

            }
            

       

            var input = averagePercs;
            foreach (var value in input)
            {
                var sma = calculator.Update(value);
                smoothAveragePercs.Add((int)Math.Ceiling(sma));
            }


            base.OnStartup(e);
        }



    }

    public class SimpleMovingAverage
    {
        private readonly int _k;
        private readonly int[] _values;

        private int _index = 0;
        private int _sum = 0;

        public SimpleMovingAverage(int k)
        {
            if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k), "Must be greater than 0");

            _k = k;
            _values = new int[k];
        }

        public double Update(int nextInput)
        {
            // calculate the new sum
            _sum = _sum - _values[_index] + nextInput;

            // overwrite the old value with the new one
            _values[_index] = nextInput;

            // increment the index (wrapping back to 0)
            _index = (_index + 1) % _k;

            // calculate the average
            return ((double)_sum) / _k;
        }
    }
}
