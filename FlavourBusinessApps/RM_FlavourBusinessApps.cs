//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaiterApp.ViewModel.Proxies
{
    using System;
    
    
    public sealed class CNSPr_IWaiterPresentation_LayTheTableRequest : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(WaiterApp.ViewModel.IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = waiterPresentation;
            argsTypes[0] = typeof(WaiterApp.ViewModel.IWaiterPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            args[2] = servicePointIdentity;
            argsTypes[2] = typeof(string);
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.LaytheTableRequestHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.LaytheTableRequestHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.LaytheTableRequestHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IWaiterPresentation_ServicePointChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(WaiterApp.ViewModel.IWaiterPresentation waiterPresentation, string servicePointIdentity, FlavourBusinessFacade.ServicesContextResources.ServicePointState newState)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = waiterPresentation;
            argsTypes[0] = typeof(WaiterApp.ViewModel.IWaiterPresentation);
            args[1] = servicePointIdentity;
            argsTypes[1] = typeof(string);
            args[2] = newState;
            argsTypes[2] = typeof(FlavourBusinessFacade.ServicesContextResources.ServicePointState);
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.ServicePointChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.ServicePointChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.ServicePointChangeStateHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IWaiterPresentation_ItemsReadyToServeRequest : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(WaiterApp.ViewModel.IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = waiterPresentation;
            argsTypes[0] = typeof(WaiterApp.ViewModel.IWaiterPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            args[2] = servicePointIdentity;
            argsTypes[2] = typeof(string);
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.ItemsReadyToServeRequesttHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.ItemsReadyToServeRequesttHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.ItemsReadyToServeRequesttHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IWaiterPresentation_MealConversationTimeout : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(WaiterApp.ViewModel.IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity, string sessionIdentity)
        {
            object[] args = new object[4];
            System.Type[] argsTypes = new System.Type[4];
            args[0] = waiterPresentation;
            argsTypes[0] = typeof(WaiterApp.ViewModel.IWaiterPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            args[2] = servicePointIdentity;
            argsTypes[2] = typeof(string);
            args[3] = sessionIdentity;
            argsTypes[3] = typeof(string);
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.MealConversationTimeoutHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.MealConversationTimeoutHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.MealConversationTimeoutHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IWaiterPresentation_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
}
namespace TakeAwayApp.ViewModel.Proxies
{
    using System;
    
    
    public sealed class CNSPr_IFlavoursServiceOrderTakingStation_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
}
namespace ServiceContextManagerApp.Proxies
{
    using System;
    
    
    public sealed class CNSPr_IServicesContextPresentation_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IServicesContextPresentation_ItemsStateChanged : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(System.Collections.Generic.Dictionary<string, FlavourBusinessFacade.RoomService.ItemPreparationState> newItemsState)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = newItemsState;
            argsTypes[0] = typeof(System.Collections.Generic.Dictionary<string, FlavourBusinessFacade.RoomService.ItemPreparationState>);
            object retValue = this.Invoke(typeof(FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IServicesContextPresentation_MealCoursesUpdated : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(System.Collections.Generic.IList<FlavourBusinessManager.RoomService.ViewModel.MealCourse> mealCourses)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = mealCourses;
            argsTypes[0] = typeof(System.Collections.Generic.IList<FlavourBusinessManager.RoomService.ViewModel.MealCourse>);
            object retValue = this.Invoke(typeof(FlavourBusinessManager.RoomService.ViewModel.MealCoursesUpdatedHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new FlavourBusinessManager.RoomService.ViewModel.MealCoursesUpdatedHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new FlavourBusinessManager.RoomService.ViewModel.MealCoursesUpdatedHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IServicesContextPresentation_ServicePointChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(ServiceContextManagerApp.IServicesContextPresentation servicesContextPresentation, string servicePointIdentity, FlavourBusinessFacade.ServicesContextResources.ServicePointState newState)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = servicesContextPresentation;
            argsTypes[0] = typeof(ServiceContextManagerApp.IServicesContextPresentation);
            args[1] = servicePointIdentity;
            argsTypes[1] = typeof(string);
            args[2] = newState;
            argsTypes[2] = typeof(FlavourBusinessFacade.ServicesContextResources.ServicePointState);
            object retValue = this.Invoke(typeof(ServiceContextManagerApp.ServicePointChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new ServiceContextManagerApp.ServicePointChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new ServiceContextManagerApp.ServicePointChangeStateHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IServicesContextPresentation_DelayedMealAtTheCounter : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(ServiceContextManagerApp.ISupervisorPresentation supervisorPresentation, string messageID)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = supervisorPresentation;
            argsTypes[0] = typeof(ServiceContextManagerApp.ISupervisorPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(ServiceContextManagerApp.DelayedMealAtTheCounterHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new ServiceContextManagerApp.DelayedMealAtTheCounterHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new ServiceContextManagerApp.DelayedMealAtTheCounterHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_ISupervisorPresentation_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
}
namespace PreparationStationDevice.Proxies
{
    using System;
    
    
    public sealed class CNSPr_IFlavoursPreparationStation_PreparationItemsLoaded : OOAdvantech.Remoting.EventConsumerHandler
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
    
    public sealed class CNSPr_IFlavoursPreparationStation_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
}
namespace DontWaitApp.Proxies
{
    using System;
    
    
    public sealed class CNSPr_IFoodServicesClientSessionViewModel_SharedItemChanged : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(DontWaitApp.IFoodServicesClientSessionViewModel flavoursOrderServer, DontWaitApp.Messmate messmate, string sharedItemUid, string messageID)
        {
            object[] args = new object[4];
            System.Type[] argsTypes = new System.Type[4];
            args[0] = flavoursOrderServer;
            argsTypes[0] = typeof(DontWaitApp.IFoodServicesClientSessionViewModel);
            args[1] = messmate;
            argsTypes[1] = typeof(DontWaitApp.Messmate);
            args[2] = sharedItemUid;
            argsTypes[2] = typeof(string);
            args[3] = messageID;
            argsTypes[3] = typeof(string);
            object retValue = this.Invoke(typeof(DontWaitApp.SharedItemChangedHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new DontWaitApp.SharedItemChangedHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new DontWaitApp.SharedItemChangedHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IFoodServicesClientSessionViewModel_MenuItemProposal : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(DontWaitApp.IFoodServicesClientSessionViewModel flavoursOrderServer, DontWaitApp.Messmate messmate, string menuItemUri, string messageID)
        {
            object[] args = new object[4];
            System.Type[] argsTypes = new System.Type[4];
            args[0] = flavoursOrderServer;
            argsTypes[0] = typeof(DontWaitApp.IFoodServicesClientSessionViewModel);
            args[1] = messmate;
            argsTypes[1] = typeof(DontWaitApp.Messmate);
            args[2] = menuItemUri;
            argsTypes[2] = typeof(string);
            args[3] = messageID;
            argsTypes[3] = typeof(string);
            object retValue = this.Invoke(typeof(DontWaitApp.MenuItemProposalHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new DontWaitApp.MenuItemProposalHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new DontWaitApp.MenuItemProposalHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IFoodServicesClientSessionViewModel_MessmatesWaitForYouToDecide : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(DontWaitApp.IFoodServicesClientSessionViewModel flavoursOrderServer, System.Collections.Generic.List<DontWaitApp.Messmate> messmates, string messageID)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = flavoursOrderServer;
            argsTypes[0] = typeof(DontWaitApp.IFoodServicesClientSessionViewModel);
            args[1] = messmates;
            argsTypes[1] = typeof(System.Collections.Generic.List<DontWaitApp.Messmate>);
            args[2] = messageID;
            argsTypes[2] = typeof(string);
            object retValue = this.Invoke(typeof(DontWaitApp.MessmatesWaitForYouToDecideHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new DontWaitApp.MessmatesWaitForYouToDecideHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new DontWaitApp.MessmatesWaitForYouToDecideHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IFoodServicesClientSessionViewModel_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IFlavoursOrderServer_PartOfMealRequest : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(DontWaitApp.IFlavoursOrderServer flavoursOrderServer, DontWaitApp.Messmate messmate, string messageID)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = flavoursOrderServer;
            argsTypes[0] = typeof(DontWaitApp.IFlavoursOrderServer);
            args[1] = messmate;
            argsTypes[1] = typeof(DontWaitApp.Messmate);
            args[2] = messageID;
            argsTypes[2] = typeof(string);
            object retValue = this.Invoke(typeof(DontWaitApp.PartOfMealRequestHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new DontWaitApp.PartOfMealRequestHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new DontWaitApp.PartOfMealRequestHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IFlavoursOrderServer_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object _object, string member)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = _object;
            argsTypes[0] = typeof(object);
            args[1] = member;
            argsTypes[1] = typeof(string);
            object retValue = this.Invoke(typeof(OOAdvantech.ObjectChangeStateHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.ObjectChangeStateHandle(this.Invoke));
        }
    }
}
namespace CourierApp.ViewModel.Proxies
{
    using System;
    
    
    public sealed class CNSPr_ICourierActivityPresentation_ItemsReadyToServeRequest : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(CourierApp.ViewModel.ICourierActivityPresentation courierActivityPresentation, string messageID, string servicePointIdentity)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = courierActivityPresentation;
            argsTypes[0] = typeof(CourierApp.ViewModel.ICourierActivityPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            args[2] = servicePointIdentity;
            argsTypes[2] = typeof(string);
            object retValue = this.Invoke(typeof(CourierApp.ViewModel.ItemsReadyToServeRequesttHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new CourierApp.ViewModel.ItemsReadyToServeRequesttHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new CourierApp.ViewModel.ItemsReadyToServeRequesttHandle(this.Invoke));
        }
    }
}
