using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationUI.ViewModel
{
    /// <MetaDataID>{918e6ea3-332c-433c-8a1c-e9c6f3caae62}</MetaDataID>
    public class PrintReceiptsItemStateViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <MetaDataID>{9804a21b-b1da-4f71-99de-f2b4565b0419}</MetaDataID>
        public readonly ServicePointType ServicePointType;
        /// <MetaDataID>{34bef80e-ca9f-4852-8619-41492ca2918d}</MetaDataID>
        private PrintReceiptCondition _PrintReceiptCondition;

        /// <MetaDataID>{1b657ce2-595a-4846-80b8-7f40f558c4a6}</MetaDataID>
        static Dictionary<ServicePointType, string> ServicePointTypeDescriptions = new Dictionary<ServicePointType, string>();

        /// <MetaDataID>{e1313c38-1be6-489f-81fc-18f72ebfef75}</MetaDataID>
        static PrintReceiptsItemStateViewModel()
        {
            ServicePointTypeDescriptions[ServicePointType.Delivery] = Properties.Resources.DeliveryServicePointDescription;
            ServicePointTypeDescriptions[ServicePointType.HallServicePoint] = Properties.Resources.HallServicePointDescription;
            ServicePointTypeDescriptions[ServicePointType.TakeAway] = Properties.Resources.TakeawayServicePointDescription;
        }

        /// <MetaDataID>{468ebfae-1cc8-4381-8bfb-ee917867288d}</MetaDataID>
        public PrintReceiptsItemStateViewModel(PrintReceiptCondition printReceiptCondition)
        {

            ServicePointType = printReceiptCondition.ServicePointType;
            this._PrintReceiptCondition = printReceiptCondition;
            SelectedItemState = PrintReceiptsStates.Where(x => x.State == printReceiptCondition.ItemState).FirstOrDefault();
            if (SelectedItemState == null)
                SelectedItemState = PrintReceiptsStates.Where(x => x.State == ItemPreparationState.OnRoad).FirstOrDefault();

            if (printReceiptCondition.IsPaid != null && printReceiptCondition.IsPaid.Value)
                IsPaid = true;
        }


        /// <MetaDataID>{a092816a-5164-49c2-b9da-4e3b4aa43f1e}</MetaDataID>
        public PrintReceiptCondition PrintReceiptCondition
        {
            get
            {
                if (IsPaid)
                    _PrintReceiptCondition.IsPaid = true;
                _PrintReceiptCondition.ItemState = SelectedItemState?.State;
                return _PrintReceiptCondition;
            }
        }
        /// <MetaDataID>{7747ad16-0c68-4d90-8f33-608281a9488c}</MetaDataID>
        public string Description
        {

            get => ServicePointTypeDescriptions[ServicePointType];
        }

        /// <MetaDataID>{7b6969d5-9d1c-48d3-8ae6-f4e8507babad}</MetaDataID>
        public ItemState SelectedItemState { get; set; }

        /// <MetaDataID>{bfef59fc-4a0f-468e-b2a2-ec1820ad5168}</MetaDataID>
        public List<ItemState> PrintReceiptsStates { get; } = new List<ItemState>() { new ItemState(), new ItemState() { State = ItemPreparationState.PendingPreparation }, new ItemState() { State = ItemPreparationState.OnRoad }, new ItemState() { State = ItemPreparationState.IsBilled } };
        /// <MetaDataID>{a040d2ea-a586-41c3-baf9-04ef10fb85c5}</MetaDataID>
        public bool IsPaid { get; set; }
    }

    /// <MetaDataID>{d4f6ba04-bdbf-471c-a6b2-f468a4defa6c}</MetaDataID>
    public class ItemState
    {
        /// <MetaDataID>{4ca1d12e-9e12-41d0-8e52-0cd00a04ba27}</MetaDataID>
        static Dictionary<ItemPreparationState, string> ItemPreparationStateDescriptions = new Dictionary<ItemPreparationState, string>();

        /// <MetaDataID>{9840203c-59f0-47b4-981e-6e5f9fa07a03}</MetaDataID>
        static ItemState()
        {
            ItemPreparationStateDescriptions[ItemPreparationState.PendingPreparation] = Properties.Resources.PendingPreparationItemStateDescription;
            ItemPreparationStateDescriptions[ItemPreparationState.OnRoad] = Properties.Resources.OnRoadtemStateDescription;
            ItemPreparationStateDescriptions[ItemPreparationState.IsBilled] = Properties.Resources.IsBilledtemStateDescription;
        }

        /// <MetaDataID>{75da376c-d111-4dad-beec-8dab6c47a1c6}</MetaDataID>
        public ItemPreparationState? State { get; set; }
        /// <MetaDataID>{8a5ebc85-a2b3-4363-b333-232eea2cc1b0}</MetaDataID>
        public string Name
        {
            get
            {
                if (State != null)
                    return ItemPreparationStateDescriptions[State.Value];
                else
                    return Properties.Resources.NoneItemStateDescription;

            }
        }
    }
}
