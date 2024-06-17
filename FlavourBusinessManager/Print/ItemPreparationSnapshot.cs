using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using OOAdvantech;
using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.Printing
{
    /// <MetaDataID>{6e353cd0-e2f5-4a52-826b-6a0c7789dcce}</MetaDataID>
    public class ItemPreparationSnapshot
    {
        public ItemPreparationSnapshot()
        {
        }
        public ItemPreparationSnapshot(ItemPreparation itemPreparation)
        {
            SessionID = itemPreparation.SessionID;
            MultilingualDescription = itemPreparation.MultilingualDescription;
            Quantity = itemPreparation.Quantity;
            ModifiedItemPrice = itemPreparation.ModifiedItemPrice;
            uid = itemPreparation.uid;
            StateTimestamp = itemPreparation.StateTimestamp;


            OptionsChanges = itemPreparation.OptionsChanges.OfType<OptionChange>().Select(x => new OptionChangeSnapshot(x)).ToList();

        }

        [JsonIgnore]
        public string Description
        {
            get
            {
                return MultilingualDescription.GetValue<string>();
            }
        }


        public string SessionID { get; set; }
        public Multilingual MultilingualDescription { get; set; }
        public double Quantity { get; set; }
        public double ModifiedItemPrice { get; set; }

        [JsonIgnore]
        public double Amount
        {
            get
            {
                return Quantity * ModifiedItemPrice;
            }
        }

        public string uid { get; set; }

        public DateTime StateTimestamp { get; set; }
        public List<OptionChangeSnapshot> OptionsChanges { get; set; }
    }

    /// <MetaDataID>{efbb7662-3337-4116-85f1-34557986890b}</MetaDataID>
    public class OptionChangeSnapshot
    {
        public OptionChangeSnapshot(OptionChange optionChange)
        {
            OptionUri = optionChange.OptionUri;
            MultilingualDescription = optionChange.MultilingualDescription;
            NewLevelUri = optionChange.NewLevelUri;
            OptionChangeType = optionChange.OptionChangeType;
            Without = optionChange.Without;


        }
        public OptionChangeSnapshot()
        {

        }

        public string OptionUri { get; set; }
        public Multilingual MultilingualDescription { get; set; }


        [JsonIgnore]
        public string Description
        {
            get
            {
                return MultilingualDescription.GetValue<string>();
            }
        }
        public string NewLevelUri { get; set; }
        public OptionChangeType OptionChangeType { get; private set; }
        public bool Without { get; set; }
    }
}
