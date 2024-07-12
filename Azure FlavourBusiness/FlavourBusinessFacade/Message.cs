using System;
using System.Collections.Generic;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{d01c7c01-2874-4ab8-bafa-6a301bda986e}</MetaDataID>
    [BackwardCompatibilityID("{d01c7c01-2874-4ab8-bafa-6a301bda986e}")]
    [Persistent()]
    public class Message
    {
        /// <MetaDataID>{31260b9e-36d9-4e1f-a3ed-25f57039e54d}</MetaDataID>
        public Message()
        {

        }
        /// <exclude>Excluded</exclude>
        DateTime _NotificationTimestamp;

        /// <MetaDataID>{36af6fcc-a84c-41f4-9616-ca481573079c}</MetaDataID>
        [PersistentMember(nameof(_NotificationTimestamp))]
        [BackwardCompatibilityID("+7")]
        public DateTime NotificationTimestamp
        {
            get => _NotificationTimestamp;
            set
            {
                if (_NotificationTimestamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NotificationTimestamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        int _NotificationsNum;

        /// <MetaDataID>{0021d85b-5c06-4250-adc0-c1e91e8f45f8}</MetaDataID>
        /// <summary>Specifies how many times the message was sent via the FCM channel</summary>
        [PersistentMember(nameof(_NotificationsNum))]
        [BackwardCompatibilityID("+6")]
        public int NotificationsNum
        {
            get => _NotificationsNum;
            set
            {
                if (_NotificationsNum != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NotificationsNum = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
       Member<bool> _MessageHasBeenRead = new Member<bool>();

        /// <MetaDataID>{42704d5d-6c2e-4013-a03c-1338f195c35f}</MetaDataID>
        [PersistentMember(nameof(_MessageHasBeenRead))]
        [BackwardCompatibilityID("+5")]
        public bool MessageHasBeenRead
        {
            get => _MessageHasBeenRead.Value;
            set
            {
                if (_MessageHasBeenRead != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MessageHasBeenRead.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{cc98cb00-d696-423a-84a8-4ff164e62ec7}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+4")]
        private string DataJson;

        /// <exclude>Excluded</exclude>
        System.DateTime _MessageTimestamp;

        /// <MetaDataID>{14278e9e-0355-4d6f-9fcd-cb3780b077d9}</MetaDataID>
        [PersistentMember(nameof(_MessageTimestamp))]
        [BackwardCompatibilityID("+3")]
        public DateTime MessageTimestamp
        {
            get => _MessageTimestamp;
            set
            {

                if (_MessageTimestamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MessageTimestamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{45e8c733-a402-40db-ba46-9a81620ed828}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            var jsonSerializerSettings = new OOAdvantech.Json.JsonSerializerSettings()
            {
                TypeNameHandling = OOAdvantech.Json.TypeNameHandling.All
            };

            DataJson = OOAdvantech.Json.JsonConvert.SerializeObject(Data, jsonSerializerSettings);
        }

        /// <MetaDataID>{ff6517ca-4812-468a-8642-333838e4f0ff}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            var jsonSerializerSettings = new OOAdvantech.Json.JsonSerializerSettings()
            {
                TypeNameHandling = OOAdvantech.Json.TypeNameHandling.All
            };
            if (DataJson != null)
                Data = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(DataJson, jsonSerializerSettings);

            if(Data.ContainsKey("ClientMessageType"))
            {
                object value =Data["ClientMessageType"];
                Data["ClientMessageType"] = (ClientMessages)Convert.ToInt32(value);
            }

        }
        /// <MetaDataID>{dab42425-81c2-4418-8ec9-00b097e0c440}</MetaDataID>
        public bool HasDataValue<T>(string property, T value)
        {
            object dataValue = null;
            Data.TryGetValue(property, out dataValue);
            if (dataValue is T)
                return EqualityComparer<T>.Default.Equals((T)dataValue, value);
            return (dataValue == null && value == null);
        }
        /// <MetaDataID>{b01f8750-cb6f-4279-b2ea-685d8fb4fdde}</MetaDataID>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <exclude>Excluded</exclude>
        Notification _Notification;

        /// <MetaDataID>{eabcd871-1558-4cea-907d-f9083ffa8c9d}</MetaDataID>
        [PersistentMember(nameof(_Notification))]
        [BackwardCompatibilityID("+1")]
        public Notification Notification
        {
            get => _Notification;
            set
            {

                if (_Notification != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Notification = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _MessageID=Guid.NewGuid().ToString("N");

        /// <MetaDataID>{394cb95c-8a77-42f9-a3f2-5b2d9aba2852}</MetaDataID>
        [PersistentMember(nameof(_MessageID))]
        [BackwardCompatibilityID("+2")]
        public string MessageID
        {
            get
            {
                return _MessageID;
            }
            set
            {
                if (_MessageID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MessageID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{ba0e6760-658c-487c-ab18-2b9422a648c6}</MetaDataID>
        public T GetDataValue<T>(string propertyName)
        {
            object value = null;

            if (Data.TryGetValue(propertyName, out value))
            {
                if (typeof(T).IsSubclassOf(typeof(System.Enum)))
                {
                    int numValue = (int)Convert.ChangeType(value, typeof(int));
                    return (T)Enum.ToObject(typeof(T), numValue);
                }
                else
                    return (T)Convert.ChangeType(value, typeof(T)); ;
            }
            else
                return default;

        }

        /// <MetaDataID>{073f76d4-fe3f-460e-9069-8ef38338c7d0}</MetaDataID>
        public object GetDataValue(string propertyName)
        {
            object value = null;

            if (Data.TryGetValue(propertyName, out value))
                return value;
            else
                return null;

        }
    }


  

}