using FlavourBusinessML.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PreparationDataMining
{



    /// <MetaDataID>{31b1fa9b-3472-4967-9e38-6676c05fa103}</MetaDataID>
    public class ProductionResource : INotifyPropertyChanged
    {
        static List<string> output = new List<string>();

        public ProductionResource()
        {

            foreach (var ItemPreparationData in LoadItemPreparationData())
            {
                ModelInput sampleData = new ModelInput()
                {
                    ItemName = ItemPreparationData.ItemName,
                    OnPreparationItemsResourceUnits = ItemPreparationData.OnPreparationItemsResourceUnits,
                    Amount = (float)ItemPreparationData.Amount,
                    AvgTimeToComplete = (float)ItemPreparationData.AvgTimeToComplete,
                    PendingItemsResourceUnits = ItemPreparationData.PendingItemsResourceUnits,
                    ItemResourceUnit = ItemPreparationData.ItemResourceUnit,
                    ItemPreparationTime = ItemPreparationData.ItemPreparationTime,
                    DefaultItemPreparationTime = (float)ItemPreparationData.DefaultItemPreparationTime
                };
                var predictionResult = ConsumeModel.Predict(sampleData);

                ItemPreparationStatics itemPreparation = new ItemPreparationStatics()
                {
                    Amount = sampleData.Amount,
                    AvgRemainingTimeToPrepare = sampleData.AvgTimeToComplete,
                    DefultPreparationTime = TimeSpan.FromSeconds(sampleData.DefaultItemPreparationTime),
                    ItemName = sampleData.ItemName,
                    itemResourceUnits = (int)sampleData.ItemResourceUnit,
                    OnPreparationItemsResourceUnits = sampleData.OnPreparationItemsResourceUnits,
                    PendingItemsResourceUnits = sampleData.PendingItemsResourceUnits,
                    PreparationTime = sampleData.ItemPreparationTime,
                    PredictedTime = predictionResult.Score,
                    InputTime = ItemPreparationData.InputTime,
                    TimeDif = Math.Abs(sampleData.ItemPreparationTime - predictionResult.Score)
                };
                _PreparationStatics.Add(itemPreparation);
            }



            int tt = 0;



            Task.Run(() =>
                {
                    while (true)
                    {
                        int OnProductionItems = 0;
                        bool refreshUI = false;
                        lock (ResourceAllocationAsynch)
                        {
                            int count = 0;
                            foreach (var pendingProduction in PendingProductions.ToList())
                            {

                                var order = PendingProductions.Where(x => x.Order == pendingProduction.Order);
                                var resourceUnits = order.Sum(x => x.Amount * x.ResourceConsuming.ResourceUnits);
                               
                                if (AvailableResourceUnits > resourceUnits)
                                {
                                    foreach (var production in order.ToList())
                                    {
                                        production.ProductionStart = DateTime.Now;
                                        production.ProductionEnd = production.ProductionStart + production.ResourceConsuming.ResourceConsumingTime;

                                        production.ProductionEnd += TimeSpan.FromSeconds(3 * ((100 - AvailableResourceUnits) / 100));


                                        production.AvailableResourceUnits = AvailableResourceUnits;
                                        Productions.Add(production);
                                        refreshUI = true;
                                        PendingProductions.Remove(production);

                                        output.Add(production.ResourceConsuming.Name + "Remove pending:" + DateTime.Now.ToLongTimeString());

                                        ItemPreparationData itemPreparationData = new ItemPreparationData();

                                        SaveItemPreparationData(production);
                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if ((pendingProduction.InputTime - PendingProductions[0].InputTime).TotalSeconds > 2)
                                            break;

                                    }
                                    catch (Exception error)
                                    {


                                    }
                                }

                                //else
                                //    break;
                            }
                            if (PendingProductions.Count == 0)
                            {

                            }
                            else
                            {

                            }
                            if (ProductsOnPreparation.Count != OnProductionItems)
                            {
                                OnProductionItems = ProductsOnPreparation.Count;
                                refreshUI = true;
                            }

                        }
                        if (refreshUI)
                            RefreshUi();

                        try
                        {

                            var sss = (from production in Productions
                                       select new ItemPreparationStatics()
                                       {
                                           ItemName = production.ResourceConsuming.Name,
                                           Amount = (float)production.Amount,
                                           PreparationTime = (production.ProductionEnd.Value - production.InputTime).TotalSeconds,
                                           PredictedTime = production.ItemPreparationTimePrediction,
                                           TimeDif = Math.Abs(production.ItemPreparationTimePrediction - (production.ProductionEnd.Value - production.InputTime).TotalSeconds),
                                           OnPreparationItemsResourceUnits = production.OnPreparationItemsResourceUnits,
                                           AvgRemainingTimeToPrepare = production.AveragePendingTime,
                                           PendingItemsResourceUnits = production.PendingItemsResourceUnits,
                                           itemResourceUnits = production.ResourceConsuming.ResourceUnits,
                                           DefultPreparationTime = production.ResourceConsuming.ResourceConsumingTime,
                                           //Production = production
                                       }).OrderByDescending(x => x.InputTime).ToList();

                        }
                        catch (Exception error)
                        {

                        }


                        if (PendingProductions.Count == 0 && Productions.Count > 0 && (System.DateTime.Now - Productions.OrderBy(x => x.ProductionEnd).Last().ProductionEnd.Value).TotalSeconds > 30)
                        {
                           
                            if (runsCount-- > 0)
                            {
                                Productions.Clear();
                                Task.Run(() =>
                                {
                                    PreparationTimeEstimator();
                                });
                            }
                        }

                        if (ProductsOnPreparation.Count != OnProductionItems)
                            System.Threading.Thread.Sleep(500);
                    }
                });
        }

        int runsCount = 0;
        public void PreparationTimeEstimator()
        {
            Random random = new Random();


            List<ResourceConsumingInfo> resourceInfos = new List<ResourceConsumingInfo>() {
                new ResourceConsumingInfo() { Name = "Σουβλακι", ResourceConsumingTime = TimeSpan.FromSeconds(5), ResourceUnits = 3 ,Resource=this},
                 new ResourceConsumingInfo() { Name = "μπριζόλες", ResourceConsumingTime = TimeSpan.FromSeconds(15), ResourceUnits = 12 ,Resource=this},
                 new ResourceConsumingInfo() { Name = "μπιφτέκια", ResourceConsumingTime = TimeSpan.FromSeconds(9), ResourceUnits = 6 ,Resource=this},
                 new ResourceConsumingInfo() { Name = "Κοτόπουλο", ResourceConsumingTime = TimeSpan.FromSeconds(12), ResourceUnits = 15 ,Resource=this}
            };

            DateTime timeBaseLine = DateTime.Now;
            string OrderName = "Order1";
            List<Production> order1 = new List<Production>();
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });

            Allocate(order1);

            System.Threading.Thread.Sleep(10000);
            timeBaseLine = DateTime.Now;
            OrderName = "Order 1";
            List<Production> order2 = new List<Production>();
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[3], InputTime = timeBaseLine });

            Allocate(order2);

            System.Threading.Thread.Sleep(6000);
            timeBaseLine = DateTime.Now;

            List<Production> order3 = new List<Production>();
            OrderName = "Order 2";
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });
            Allocate(order3);
            System.Threading.Thread.Sleep(3000);
            timeBaseLine = DateTime.Now;
            OrderName = "Order 3";
            order1 = new List<Production>();
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[3], InputTime = timeBaseLine });

            Allocate(order1);
            System.Threading.Thread.Sleep(1000);
            timeBaseLine = DateTime.Now;
            order2 = new List<Production>();
            OrderName = "Order 4";
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });

            Allocate(order2);

            System.Threading.Thread.Sleep(2000);
            timeBaseLine = DateTime.Now;
            order1 = new List<Production>();
            OrderName = "Order 5";
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[3], InputTime = timeBaseLine });

            Allocate(order1);
            System.Threading.Thread.Sleep(1000);
            timeBaseLine = DateTime.Now;
            order3 = new List<Production>();
            OrderName = "Order 7";
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });
            Allocate(order3);

            System.Threading.Thread.Sleep(4000);
            timeBaseLine = DateTime.Now;

            order1 = new List<Production>();
            OrderName = "Order 8";
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order1.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });

            Allocate(order1);

            System.Threading.Thread.Sleep(11000);
            timeBaseLine = DateTime.Now;
            order2 = new List<Production>();
            OrderName = "Order 9";
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order2.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[3], InputTime = timeBaseLine });

            Allocate(order2);

            System.Threading.Thread.Sleep(15000);
            timeBaseLine = DateTime.Now;

            order3 = new List<Production>();
            OrderName = "Order 10";
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 7), ResourceConsuming = resourceInfos[0], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 3), ResourceConsuming = resourceInfos[1], InputTime = timeBaseLine });
            order3.Add(new Production() { Order = OrderName, Amount = random.Next(1, 5), ResourceConsuming = resourceInfos[2], InputTime = timeBaseLine });
            Allocate(order3);

        }

        List<ItemPreparationStatics> _PreparationStatics = new List<ItemPreparationStatics>();

        public List<ItemPreparationStatics> PreparationStatics
        {
            get
            {
                if (StoredData)
                    return _PreparationStatics;

                try
                {

                    var sss = (from production in Productions
                               select new ItemPreparationStatics()
                               {
                                   ItemName = production.ResourceConsuming.Name,
                                   Order = production.Order,
                                   PreparationTime = (production.ProductionEnd.Value - production.InputTime).TotalSeconds,
                                   PredictedTime = production.ItemPreparationTimePrediction,
                                   TimeDif = Math.Abs(production.ItemPreparationTimePrediction - (production.ProductionEnd.Value - production.InputTime).TotalSeconds),
                                   OnPreparationItemsResourceUnits = production.OnPreparationItemsResourceUnits,
                                   AvgRemainingTimeToPrepare = production.AveragePendingTime,
                                   PendingItemsResourceUnits = production.PendingItemsResourceUnits,
                                   itemResourceUnits = production.ResourceConsuming.ResourceUnits,
                                   DefultPreparationTime = production.ResourceConsuming.ResourceConsumingTime,
                                   InputTime = production.InputTime,
                                   ProductionStart = production.ProductionStart.Value,
                                   Amount = (float)production.Amount
                                   //Production = production
                               }).OrderByDescending(x => x.InputTime).ToList();

                    return sss;

                }
                catch (Exception error)
                {

                }
                return new List<ItemPreparationStatics>();
            }
        }
        public string Name { get; internal set; }

        object ResourceAllocationAsynch = new object();

        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(@"Data Source=MROCKET\SQLEXPRESS01;Initial Catalog=PreparationDataMining;Integrated Security=True");
        internal void Allocate(List<Production> order)
        {
            var resourceUnits = order.Sum(x => x.Amount * x.ResourceConsuming.ResourceUnits);
            if (AvailableResourceUnits > resourceUnits)
            {
                lock (ResourceAllocationAsynch)
                {
                    foreach (var production in order)
                    {

                        System.Threading.Thread.Sleep(200);
                        {
                            production.ProductionStart = DateTime.Now;
                            production.ProductionEnd = production.ProductionStart + production.ResourceConsuming.ResourceConsumingTime;
                            production.ProductionEnd += TimeSpan.FromSeconds(3 * ((100 - AvailableResourceUnits) / 100));

                            production.ProductionForcastEnd = production.ProductionEnd;
                            Productions.Add(production);
                            production.OnProductionItems = Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).Count();
                            production.OnPreparationItemsResourceUnits = (int)Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).Sum(x => { return x.ResourceConsuming.ResourceUnits * x.Amount; });
                            SaveItemPreparationData(production);

                        }
                    }
                }
            }
            else
            {
                lock (ResourceAllocationAsynch)
                {
                    foreach (var production in order)
                    {
                        production.OnPreparationItemsResourceUnits = (int)Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).Sum(x => { return x.ResourceConsuming.ResourceUnits * x.Amount; });
                        production.OnProductionItems = Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).Count();
                        production.PendingItems = PendingProductions.Count;
                        production.PendingItemsResourceUnits = (int)PendingProductions.Sum(x => { return x.ResourceConsuming.ResourceUnits * x.Amount; });

                        production.PendingTime = DateTime.Now;
                        PendingProductions.Add(production);
                        production.ProductionForcastEnd = DateTime.Now + production.ResourceConsuming.ResourceConsumingTime;
                        var onProductionItems = Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd);
                        production.AveragePendingTime = onProductionItems.Sum(x => (x.ProductionEnd.Value - DateTime.Now).TotalSeconds) / Productions.Count;
                    } 
                }

            }


            RefreshUi();
        }



        public List<ItemPreparationData> LoadItemPreparationData()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            System.Data.SqlClient.SqlCommand command = connection.CreateCommand();

            string query = @"SELECT [ID]
                          ,[OnPreparationItemsResourceUnits]
                          ,[AvgTimeToComplete]
                          ,[PendingItemsResourceUnits]
                          ,[ItemResourceUnit]
                          ,[DefaultItemPreparationTime]
                          ,[ItemName]
                          ,[ItemPreparationTime]
                          ,[Amount]
                          ,[InputTime]
                           FROM [ItemPreparationData]";

            command.CommandText = query;
            System.Data.DataTable table = new System.Data.DataTable("ItemPreparationData");
            table.Load(command.ExecuteReader());

            // table.Columns.OfType<System.Data.DataColumn>().Select(x=>x.ColumnName).ToArray()
            return (from row in table.Rows.OfType<System.Data.DataRow>()
                    select new ItemPreparationData()
                    {
                        ItemName = row["ItemName"] as string,
                        OnPreparationItemsResourceUnits = (int)row["OnPreparationItemsResourceUnits"],
                        AvgTimeToComplete = (int)row["AvgTimeToComplete"],
                        PendingItemsResourceUnits = (int)row["PendingItemsResourceUnits"],
                        ItemResourceUnit = (int)row["ItemResourceUnit"],
                        DefaultItemPreparationTime = (int)row["DefaultItemPreparationTime"],
                        ItemPreparationTime = (int)row["ItemPreparationTime"],
                        Amount = (float)(double)row["Amount"],
                        InputTime = (DateTime)row["InputTime"],
                    }).ToList();





        }
        private void SaveItemPreparationData(Production production)
        {
            ModelInput sampleData = new ModelInput()
            {
                OnPreparationItemsResourceUnits = production.OnPreparationItemsResourceUnits,
                Amount = (float)production.Amount,
                AvgTimeToComplete = (float)production.AveragePendingTime,
                PendingItemsResourceUnits = production.PendingItemsResourceUnits,
                ItemResourceUnit = (float)production.Amount * production.ResourceConsuming.ResourceUnits,
                DefaultItemPreparationTime = (float)production.ResourceConsuming.ResourceConsumingTime.TotalSeconds,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);
            production.ItemPreparationTimePrediction = predictionResult.Score;

            ItemPreparationData itemPreparationData = new ItemPreparationData();
            itemPreparationData.ItemName = production.ResourceConsuming.Name;
            itemPreparationData.Amount = (float)production.Amount;
            itemPreparationData.ItemResourceUnit = (float)production.Amount * production.ResourceConsuming.ResourceUnits;
            itemPreparationData.DefaultItemPreparationTime = (int)production.ResourceConsuming.ResourceConsumingTime.TotalSeconds;
            itemPreparationData.ItemPreparationTime = (int)(production.ProductionEnd.Value - production.InputTime).TotalSeconds;// - production.ProductionForcastEnd.Value).TotalSeconds;
            itemPreparationData.OnPreparationItemsResourceUnits = production.OnPreparationItemsResourceUnits;
            itemPreparationData.PendingItemsResourceUnits = production.PendingItemsResourceUnits;
            itemPreparationData.AvgTimeToComplete = (int)production.AveragePendingTime;
            itemPreparationData.InputTime = production.InputTime;
            string insert = @"INSERT INTO [dbo].[ItemPreparationData]
            ([Amount]
            ,[OnPreparationItemsResourceUnits]
            ,[AvgTimeToComplete]
            ,[PendingItemsResourceUnits]
            ,[ItemResourceUnit]
            ,[DefaultItemPreparationTime]
            ,[ItemName]
            ,[ItemPreparationTime]
            ,[InputTime])
              VALUES
            (@Amount
            ,@OnPreparationItemsResourceUnits
            , @AvgTimeToComplete
            , @PendingItemsResourceUnits
            , @ItemResourceUnit
            , @DefaultItemPreparationTime
            , @ItemName
            , @ItemPreparationTime
            ,@InputTime)";

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                System.Data.SqlClient.SqlCommand command = connection.CreateCommand();

                command.Parameters.Add("@Amount", System.Data.SqlDbType.Float).Value = itemPreparationData.Amount;
                command.Parameters.Add("@OnPreparationItemsResourceUnits", System.Data.SqlDbType.Int).Value = itemPreparationData.OnPreparationItemsResourceUnits;
                command.Parameters.Add("@AvgTimeToComplete", System.Data.SqlDbType.Int).Value = itemPreparationData.AvgTimeToComplete;

                command.Parameters.Add("@PendingItemsResourceUnits", System.Data.SqlDbType.Int).Value = itemPreparationData.PendingItemsResourceUnits;
                command.Parameters.Add("@ItemResourceUnit", System.Data.SqlDbType.Int).Value = itemPreparationData.ItemResourceUnit * itemPreparationData.Amount;
                command.Parameters.Add("@DefaultItemPreparationTime", System.Data.SqlDbType.Int).Value = itemPreparationData.DefaultItemPreparationTime;
                command.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = itemPreparationData.ItemName;
                command.Parameters.Add("@ItemPreparationTime", System.Data.SqlDbType.Int).Value = itemPreparationData.ItemPreparationTime;
                command.Parameters.Add("@InputTime", System.Data.SqlDbType.DateTime).Value = itemPreparationData.InputTime;
                command.CommandText = insert;
                command.ExecuteNonQuery();
            }
            catch (Exception error)
            {

                throw;
            }

        }



        public double AvailableResourceUnits => 100 - Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).Sum(x => { return x.Amount * x.ResourceConsuming.ResourceUnits; });

        public readonly List<Production> Productions = new List<Production>();
        public readonly List<Production> PendingProductions = new List<Production>();

        public event PropertyChangedEventHandler PropertyChanged;

        public int PendingsCount { get => PendingProductions.Count; }


        bool _StoredData = true;
        public bool StoredData
        {
            get => _StoredData;
            set
            {
                _StoredData = value;
                RefreshUi();
            }
        }

        public void RefreshUi()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProductsOnPreparation)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PendingsCount)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailableResourceUnits)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreparationStatics)));


                /* Your code here */
            }));
        }
        public List<Production> ProductsOnPreparation
        {
            get
            {
                lock (ResourceAllocationAsynch)
                {
                    return Productions.Where(x => x.ProductionStart != null && DateTime.Now > x.ProductionStart && DateTime.Now < x.ProductionEnd).ToList();
                }
            }
        }
    }


    /// <MetaDataID>{c0424467-3b7b-420b-8aa5-0afb81dd0c9b}</MetaDataID>
    public class ItemPreparationStatics
    {
        internal int itemResourceUnits;
        public string Order { get; internal set; }
        public string ItemName { get; internal set; }
        public float Amount { get; internal set; }
        public double PreparationTime { get; internal set; }
        public float PredictedTime { get; internal set; }
        public double TimeDif { get; internal set; }
        // public int OnProductionItems { get; internal set; }
        public double AvgRemainingTimeToPrepare { get; internal set; }
        //public int PendingItems { get; internal set; }
        public TimeSpan DefultPreparationTime { get; internal set; }
        //public Production Production { get; internal set; }
        public float OnPreparationItemsResourceUnits { get; internal set; }
        public float PendingItemsResourceUnits { get; internal set; }
        public DateTime InputTime { get; internal set; }
        public DateTime ProductionStart { get; internal set; }

    }

    /// <MetaDataID>{53003075-24a4-4349-ad43-bd919de63fd9}</MetaDataID>
    public class ItemPreparationData
    {
        public float Amount;
        public int OnPreparationItemsResourceUnits;
        public int PendingItemsResourceUnits;
        public float ItemResourceUnit;
        public int DefaultItemPreparationTime;
        public string ItemName;
        public int ItemPreparationTime;

        public int AvgTimeToComplete { get; internal set; }
        public DateTime InputTime { get; internal set; }
    }
}