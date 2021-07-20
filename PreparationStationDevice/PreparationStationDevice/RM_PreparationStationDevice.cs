using System;
using System.Collections.Generic;
using System.Text;

namespace PreparationStationDevice.Proxies
{
    /// <MetaDataID>{3ad934c6-bc5b-43bc-aa38-1efeed844562}</MetaDataID>
    public sealed class CNSPr_FlavoursPreparationStation_PreparationItemsLoaded : OOAdvantech.Remoting.EventConsumerHandler
    {

        public void Invoke(PreparationStationDevice.FlavoursPreparationStation sender)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = sender;
            argsTypes[0] = typeof(PreparationStationDevice.FlavoursPreparationStation);
            object retValue = this.Invoke(typeof(PreparationStationDevice.PreparationItemsLoadedHandle), "Invoke", args, argsTypes);
        }

        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new PreparationStationDevice.PreparationItemsLoadedHandle(this.Invoke));
        }

        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new PreparationStationDevice.PreparationItemsLoadedHandle(this.Invoke));
        }
    }
}
