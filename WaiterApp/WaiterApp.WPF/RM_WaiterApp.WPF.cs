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
    
    
    public sealed class CNSPr_ISecureUser_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
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