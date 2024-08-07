//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
