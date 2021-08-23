using FlavourBusinessML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationDataMining
{
    /// <MetaDataID>{e9caf131-c2e5-420d-80dd-b0c563013d61}</MetaDataID>
    public class ResourceConsumingInfo
    {
        public string Name;
        public int ResourceUnits;

        public TimeSpan ResourceConsumingTime;

        public ProductionResource Resource;
    }

    /// <MetaDataID>{7e80913c-5993-4e96-9fe3-0d89604560b6}</MetaDataID>
    public class Production
    {

        public string Order;

        public string Name
        {
            get
            {
                return this.ResourceConsuming.Name;
            }
        }
        public ResourceConsumingInfo ResourceConsuming;

        public DateTime InputTime;

        double _Amount;
        public double Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;

            }
        }
        public double ResourceUnits { get => Amount * this.ResourceConsuming.ResourceUnits; }
        public DateTime? ProductionStart;
        public DateTime? ProductionEnd;
        public DateTime? ProductionForcastEnd;

        public int OnProductionItems { get; internal set; }
        public int PendingItems { get; internal set; }
        public double AveragePendingTime { get; internal set; }
        public DateTime PendingTime { get; internal set; }
        public double AvailableResourceUnits { get; internal set; }
        public int OnPreparationItemsResourceUnits { get; internal set; }
        public int PendingItemsResourceUnits { get; internal set; }
        public float ItemPreparationTimePrediction { get; internal set; }
    }
}
