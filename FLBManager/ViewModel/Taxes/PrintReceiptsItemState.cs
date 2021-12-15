using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel.Taxes
{
    /// <MetaDataID>{918e6ea3-332c-433c-8a1c-e9c6f3caae62}</MetaDataID>
    public class PrintReceiptsItemState : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ServicePointType ServicePointType;
        public PrintReceiptsItemState(ServicePointType servicePointType, ItemPreparationState state)
        {
            ServicePointType = servicePointType;
            SelectedItemState = PrintReceiptsStates.Where(x => x.State == state).FirstOrDefault();
            if (SelectedItemState == null)
                SelectedItemState = PrintReceiptsStates.Where(x => x.State == ItemPreparationState.OnRoad).FirstOrDefault();
        }
        public string Description
        {
            get => ServicePointType.ToString();
        }

        public ItemState SelectedItemState { get; set; }

        public List<ItemState> PrintReceiptsStates { get; } = new List<ItemState>() { new ItemState() { State = ItemPreparationState.PendingPreparation }, new ItemState() { State = ItemPreparationState.OnRoad }, new ItemState() { State = ItemPreparationState.IsBilled } };
    }

    /// <MetaDataID>{d4f6ba04-bdbf-471c-a6b2-f468a4defa6c}</MetaDataID>
    public class ItemState
    {
        public ItemPreparationState State { get; set; }
        public string Name
        {
            get
            {
                return State.ToString();
            }
        }
    }
}
