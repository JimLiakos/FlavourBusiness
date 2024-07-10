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
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.LayTheTableRequestHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.LayTheTableRequestHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.LayTheTableRequestHandle(this.Invoke));
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
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.ItemsReadyToServeRequestHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.ItemsReadyToServeRequestHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.ItemsReadyToServeRequestHandle(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IWaiterPresentation_MealConversationTimeExceeded : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(WaiterApp.ViewModel.IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity, string sessionIdentity, FlavourBusinessFacade.ServicesContextResources.CaregivingMessageType caregivingMessageType, System.Collections.Generic.List<string> namesOfDelayedCustomers)
        {
            object[] args = new object[6];
            System.Type[] argsTypes = new System.Type[6];
            args[0] = waiterPresentation;
            argsTypes[0] = typeof(WaiterApp.ViewModel.IWaiterPresentation);
            args[1] = messageID;
            argsTypes[1] = typeof(string);
            args[2] = servicePointIdentity;
            argsTypes[2] = typeof(string);
            args[3] = sessionIdentity;
            argsTypes[3] = typeof(string);
            args[4] = caregivingMessageType;
            argsTypes[4] = typeof(FlavourBusinessFacade.ServicesContextResources.CaregivingMessageType);
            args[5] = namesOfDelayedCustomers;
            argsTypes[5] = typeof(System.Collections.Generic.List<string>);
            object retValue = this.Invoke(typeof(WaiterApp.ViewModel.MealConversationTimeExceededHandle), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new WaiterApp.ViewModel.MealConversationTimeExceededHandle(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new WaiterApp.ViewModel.MealConversationTimeExceededHandle(this.Invoke));
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
