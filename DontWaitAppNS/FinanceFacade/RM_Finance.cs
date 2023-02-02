//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinanceFacade.Proxies
{
    using System;
    
    
    public sealed class Pr_IPayment : OOAdvantech.Remoting.MarshalByRefObject, FinanceFacade.IPayment, OOAdvantech.Remoting.RestApi.ITransparentProxy
    {
        
        private OOAdvantech.Remoting.RestApi.Proxy Proxy;
        
        public FinanceFacade.IPayment Org;
        
public event OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle Reconnected
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "add_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "remove_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
            }
public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
            {
                add
                {
                    this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "add_ObjectChangeState",new object[] {value} , new Type[] { typeof(OOAdvantech.ObjectChangeStateHandle)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "remove_ObjectChangeState",new object[] {value} , new Type[] { typeof(OOAdvantech.ObjectChangeStateHandle)});
                }
            }
        
        public Pr_IPayment(OOAdvantech.Remoting.RestApi.Proxy proxy)
        {
            this.Proxy = proxy;
        }
        
        // The Width property for the object.
        public decimal TipsAmount
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_TipsAmount", args, argsTypes);
                return this.Proxy.GetValue<decimal>(retValue);
            }
        }
        
        // The Width property for the object.
        public FinanceFacade.PaymentState State
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_State", args, argsTypes);
                return this.Proxy.GetValue<FinanceFacade.PaymentState>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(FinanceFacade.PaymentState);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_State", args, argsTypes);
            }
        }
        
        // The Width property for the object.
        public string PaymentProviderJson
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_PaymentProviderJson", args, argsTypes);
                return this.Proxy.GetValue<string>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(string);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_PaymentProviderJson", args, argsTypes);
            }
        }
        
        // The Width property for the object.
        public System.Nullable<System.DateTime> TransactionDate
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_TransactionDate", args, argsTypes);
                return this.Proxy.GetValue<System.Nullable<System.DateTime>>(retValue);
            }
        }
        
        // The Width property for the object.
        public System.Collections.Generic.List<FinanceFacade.IItem> Items
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_Items", args, argsTypes);
                return this.Proxy.GetValue<System.Collections.Generic.List<FinanceFacade.IItem>>(retValue);
            }
        }
        
        // The Width property for the object.
        public decimal Amount
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_Amount", args, argsTypes);
                return this.Proxy.GetValue<decimal>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(decimal);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_Amount", args, argsTypes);
            }
        }
        
        // The Width property for the object.
        public FinanceFacade.PaymentType PaymentType
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_PaymentType", args, argsTypes);
                return this.Proxy.GetValue<FinanceFacade.PaymentType>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(FinanceFacade.PaymentType);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_PaymentType", args, argsTypes);
            }
        }
        
        // The Width property for the object.
        public string Currency
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_Currency", args, argsTypes);
                return this.Proxy.GetValue<string>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(string);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_Currency", args, argsTypes);
            }
        }
        
        // The Width property for the object.
        public string Identity
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "get_Identity", args, argsTypes);
                return this.Proxy.GetValue<string>(retValue);
            }
            set
            {
                object[] args = new object[1];
                System.Type[] argsTypes = new System.Type[1];
                args[0] = value;
                argsTypes[0] = typeof(string);
                this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "set_Identity", args, argsTypes);
            }
        }
        
        public OOAdvantech.Remoting.IProxy GetProxy()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "GetProxy", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.IProxy>(retValue);
        }
        
        public void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal tipAmount)
        {
            object[] args = new object[5];
            System.Type[] argsTypes = new System.Type[5];
            args[0] = cardType;
            argsTypes[0] = typeof(string);
            args[1] = accountNumber;
            argsTypes[1] = typeof(string);
            args[2] = isDebit;
            argsTypes[2] = typeof(bool);
            args[3] = transactionID;
            argsTypes[3] = typeof(string);
            args[4] = tipAmount;
            argsTypes[4] = typeof(decimal);
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "CardPaymentCompleted", args, argsTypes);
        }
        
        public void CashPaymentCompleted(decimal tipAmount)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = tipAmount;
            argsTypes[0] = typeof(decimal);
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "CashPaymentCompleted", args, argsTypes);
        }
        
        public void CheckPaymentCompleted(string bankDescription, string bic, string iban, string checkID, string issuer, System.DateTime issueDate, string checkNotes, decimal totalAmount, decimal tipAmount)
        {
            object[] args = new object[9];
            System.Type[] argsTypes = new System.Type[9];
            args[0] = bankDescription;
            argsTypes[0] = typeof(string);
            args[1] = bic;
            argsTypes[1] = typeof(string);
            args[2] = iban;
            argsTypes[2] = typeof(string);
            args[3] = checkID;
            argsTypes[3] = typeof(string);
            args[4] = issuer;
            argsTypes[4] = typeof(string);
            args[5] = issueDate;
            argsTypes[5] = typeof(System.DateTime);
            args[6] = checkNotes;
            argsTypes[6] = typeof(string);
            args[7] = totalAmount;
            argsTypes[7] = typeof(decimal);
            args[8] = tipAmount;
            argsTypes[8] = typeof(decimal);
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "CheckPaymentCompleted", args, argsTypes);
        }
        
        public void Refund()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "Refund", args, argsTypes);
        }
        
        public void PaymentInProgress()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "PaymentInProgress", args, argsTypes);
        }
        
        public void PaymentRequestCanceled()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "PaymentRequestCanceled", args, argsTypes);
        }
        
        public bool CheckForPaymentComplete()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(FinanceFacade.IPayment), "CheckForPaymentComplete", args, argsTypes);
            return this.Proxy.GetValue<bool>(retValue);
        }
    }
    
    public sealed class CNSPr_IPayment_ObjectChangeState : OOAdvantech.Remoting.EventConsumerHandler
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
