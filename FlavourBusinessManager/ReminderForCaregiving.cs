using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager
{

    /// <summary>
    /// When in service context there is a need for someone to act on something.
    /// The care giving reminder takes care to find someone
    /// </summary>
    /// <MetaDataID>{18981145-54b1-4925-8204-7f9173d1a5c6}</MetaDataID>
    [BackwardCompatibilityID("{18981145-54b1-4925-8204-7f9173d1a5c6}")]
    [Persistent()]
    internal class ReminderForCareGiving//<T> where T : IServicesContextWorker
    {
        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{67385023-1991-4a76-87e0-78026b95daa6}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+3")]
        public ClientMessages MessageType;
        /// <MetaDataID>{17ac7329-1146-4427-9a57-b5c848e6d9f2}</MetaDataID>
        public ReminderForCareGiving(ClientMessages messageType,
        List<IServicesContextWorker> candidatesForCareGiving,
        List<IServiceContextSupervisor> supervisorsForCareGiving,
        Message messagePattern,
        DateTime reminderStartTime,
        TimeSpan delayTimeBetweenTriesToFindCareGiver,
        TimeSpan maximumTimeForCareGiverToAct,
        TimeSpan delayTimeBetweenTriesToFindSupervisor,
        TimeSpan maximumTimeBeforeSupervisorTakeCare)
        {
            MessageType = messageType;
            CandidatesForCareGiving = candidatesForCareGiving;
            SupervisorsForCareGiving = supervisorsForCareGiving;
            MessagePattern = messagePattern;

            StartedAt = reminderStartTime;

            DelayTimeBetweenTriesToFindCareGiver = delayTimeBetweenTriesToFindCareGiver;
            MaximumTimeForCareGiverToAct = maximumTimeForCareGiverToAct;
            DelayTimeBetweenTriesToFindSupervisor = delayTimeBetweenTriesToFindSupervisor;
            MaximumTimeBeforeSupervisorTakeCare = maximumTimeBeforeSupervisorTakeCare;
        }


        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{3a9a654a-cf28-44b7-82a5-4a80800639be}</MetaDataID>
        List<IServicesContextWorker> CandidatesForCareGiving;
        /// <MetaDataID>{34105581-800a-4dc4-a82d-de9001158877}</MetaDataID>
        List<IServiceContextSupervisor> SupervisorsForCareGiving;
        /// <MetaDataID>{284430df-e0f0-4514-b257-2a7768833a6a}</MetaDataID>
        TimeSpan DelayTimeBetweenTriesToFindCareGiver;

        /// <MetaDataID>{f5b89e55-d0fc-4d49-b1a2-fbcd7f3b9136}</MetaDataID>
        TimeSpan MaximumTimeForCareGiverToAct = TimeSpan.FromMinutes(4);

        /// <MetaDataID>{b9cd8b1c-7b6b-4e47-8fa7-1ac9fbb26472}</MetaDataID>
        TimeSpan MaximumTimeBeforeSupervisorTakeCare = TimeSpan.FromMinutes(12);


        /// <exclude>Excluded</exclude>
        DateTime _TimeOfLastMessageSendToCareGiver = DateTime.MinValue;

        /// <MetaDataID>{f95180ff-b762-42e2-bf73-6ceb7b787b94}</MetaDataID>
        [PersistentMember(nameof(_TimeOfLastMessageSendToCareGiver))]
        [BackwardCompatibilityID("+1")]
        public DateTime TimeOfLastMessageSendToCareGiver
        {
            get => _TimeOfLastMessageSendToCareGiver; set
            {
                if (_TimeOfLastMessageSendToCareGiver!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TimeOfLastMessageSendToCareGiver=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{7a1628e7-5ee2-46a8-99da-c2e9d1a9b09f}</MetaDataID>
        DateTime TimeOfLastMessageSendToSupervisor { get; set; } = DateTime.MinValue;

        /// <MetaDataID>{1e7ef94e-05b4-4c57-b25a-f6719f3efdc3}</MetaDataID>
        TimeSpan DelayTimeBetweenTriesToFindSupervisor;
        /// <MetaDataID>{e1434619-5735-4724-b958-967fe468910c}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+4")]
        private string UniqueId;


        /// <exclude>Excluded</exclude>
        DateTime                                                                                                                                             _StartedAt;
        /// <MetaDataID>{1250373f-eb5c-4d91-8cb6-855d967b168c}</MetaDataID>
        [PersistentMember(nameof(_StartedAt))]
        [BackwardCompatibilityID("+2")]
        public DateTime StartedAt
        {
            get => _StartedAt; set
            {
                if (_StartedAt!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartedAt=value;
                        stateTransition.Consistent = true;
                    }
                }
            } } 

        /// <MetaDataID>{dadc072f-7824-4c02-92dd-c1a039dbc422}</MetaDataID>
        public int? DurationInMin;


        /// <MetaDataID>{c08ba60a-9f75-4059-845e-70d2bc8cff77}</MetaDataID>
        object CaregiversLock = new object();

        /// <exclude>Excluded</exclude>
        List<Caregiver> _Caregivers = new List<Caregiver>();

        /// <MetaDataID>{2a450ea3-c23a-42dd-9eac-3a8e1fcb327d}</MetaDataID>
        public List<Caregiver> Caregivers
        {
            get
            {
                lock (CaregiversLock)
                {
                    return _Caregivers.ToList();
                }
            }
        }
        FlavourBusinessFacade.EndUsers.FlavourBusinessFacade.EndUsers.Message MessagePattern;

        /// <MetaDataID>{ff311198-e710-46e8-a7a6-741749432a18}</MetaDataID>
        Type WorkerType;

        /// <MetaDataID>{46f8bbc1-315b-4601-aaa2-6ac8bd6cad6e}</MetaDataID>
        bool TerminateThread = false;
        /// <MetaDataID>{95544461-86c7-4628-b977-a4a1bdb6d774}</MetaDataID>
        public void Start()
        {

            var ticks = new DateTime(2022, 1, 1).Ticks;
            UniqueId = GetHashCode().ToString("x") + "_" + (DateTime.Now.Ticks - ticks).ToString("x");



            Task.Run(() =>
            {
                bool terminateThread = false;
                do
                {

                    try
                    {
                        #region Workers
                        var careGivers = Caregivers.Where(x => !(x.Worker is IServiceContextSupervisor)).OrderByDescending(x => x.WillTakeCareTimestamp).ToList();

                        if (careGivers.Any())
                        {
                            //minimum time span between the two remind messages for care giving waiter of long time conversation

                            if (careGivers.First().WillTakeCareTimestamp > TimeOfLastMessageSendToCareGiver)
                            {
                                if ((DateTime.UtcNow - careGivers.First().WillTakeCareTimestamp.Value.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                {

                                    TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                                    SendRemindMessageToCareGiver(careGivers.Select(x => x.Worker).ToList());
                                    ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));
                                }
                            }
                            else
                            {
                                if ((DateTime.UtcNow - TimeOfLastMessageSendToCareGiver.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                {
                                    TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                                    SendRemindMessageToCareGiver(careGivers.Select(x => x.Worker).ToList());
                                    ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));
                                }
                            }


                        }
                        else if ((DateTime.UtcNow - TimeOfLastMessageSendToCareGiver.ToUniversalTime()) > DelayTimeBetweenTriesToFindCareGiver)
                        {
                            TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                            List<IServicesContextWorker> candidatesForCareGiving = null; ;

                            lock (this)
                                candidatesForCareGiving = CandidatesForCareGiving.ToList();

                            SendMessageToFindCareGiver(candidatesForCareGiving);
                            ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));
                        }
                        #endregion


                        #region Supervisors


                        if ((DateTime.UtcNow - this.StartedAt) > MaximumTimeBeforeSupervisorTakeCare)
                        {
                            //Inform supervisor
                            var supervisorCareGivers = Caregivers.Where(x => x.Worker is IServiceContextSupervisor).OrderByDescending(x => x.WillTakeCareTimestamp).ToList();
                            if (supervisorCareGivers.Any())
                            {
                                if (supervisorCareGivers.First().WillTakeCareTimestamp > TimeOfLastMessageSendToSupervisor)
                                {

                                    if ((DateTime.UtcNow - supervisorCareGivers.First().WillTakeCareTimestamp.Value.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                    {

                                        TimeOfLastMessageSendToSupervisor = DateTime.UtcNow;
                                        SendRemindMessageToSupervisor(careGivers.Select(x => x.Worker).ToList());
                                        ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));

                                    }
                                }
                                else
                                {
                                    if ((DateTime.UtcNow - TimeOfLastMessageSendToSupervisor.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                    {
                                        TimeOfLastMessageSendToSupervisor = DateTime.UtcNow;
                                        SendRemindMessageToSupervisor(careGivers.Select(x => x.Worker).ToList());
                                        ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));

                                    }
                                }

                            }
                            else if ((DateTime.UtcNow - TimeOfLastMessageSendToSupervisor.ToUniversalTime()) > DelayTimeBetweenTriesToFindCareGiver)
                            {
                                List<IServiceContextSupervisor> supervisorsForCareGiving = null;
                                lock (this)
                                    supervisorsForCareGiving = SupervisorsForCareGiving.ToList();

                                TimeOfLastMessageSendToSupervisor = DateTime.UtcNow;
                                SendMessageToFindSupervisor(supervisorsForCareGiving);
                                ObjectChangeState?.Invoke(this, nameof(TimeOfLastMessageSendToCareGiver));

                            }
                        }
                        #endregion

                    }
                    catch (Exception error)
                    {


                    }

                    System.Threading.Thread.Sleep(1000);
                    lock (this)
                        terminateThread = TerminateThread;

                } while (!terminateThread);
            });
        }



        /// <MetaDataID>{fd096624-714b-460d-a6c4-a953a9acaeb3}</MetaDataID>
        private void SendRemindMessageToSupervisor(List<IServicesContextWorker> servicesContextWorkers)
        {
            foreach (var worker in servicesContextWorkers)
            {
                var workerActiveShiftWork = worker.ShiftWork;
                if (workerActiveShiftWork != null && workerActiveShiftWork.IsActive())// DateTime.UtcNow > workerActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < workerActiveShiftWork.EndsAt.ToUniversalTime())
                {

                    Message clientMessage = (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId)).FirstOrDefault();


                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        foreach (var entry in this.MessagePattern.Data)
                            clientMessage.Data[entry.Key] = entry.Value;

                        clientMessage.Data["ReminderID"] = UniqueId;
                        clientMessage.Notification = new Notification() { Title = this.MessagePattern.Notification.Title };
                    }
                    (worker as IMessageConsumer).PushMessage(clientMessage);
                    if (!string.IsNullOrWhiteSpace((worker as IMessageConsumer).DeviceToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, (worker as IMessageConsumer).DeviceToken);
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in (worker as IMessageConsumer).Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        ServicesContextRunTime.Current.UpdateWaitersWithUnreadMessages();

                    }
                }
            }
        }

        /// <MetaDataID>{8c8d0ba9-eb08-4679-9f74-9c4369bd8ef3}</MetaDataID>
        private void SendMessageToFindSupervisor(List<IServiceContextSupervisor> candidatesForCareGiving)
        {

            foreach (var worker in candidatesForCareGiving)
            {

                var workerActiveShiftWork = worker.ShiftWork;

                if (workerActiveShiftWork != null && workerActiveShiftWork.IsActive())// DateTime.UtcNow > workerActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < workerActiveShiftWork.EndsAt.ToUniversalTime())
                {
                    Message clientMessage = (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId)).FirstOrDefault();
                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        foreach (var entry in this.MessagePattern.Data)
                            clientMessage.Data[entry.Key] = entry.Value;

                        clientMessage.Data["ReminderID"] = UniqueId;
                        clientMessage.Notification = new Notification() { Title = this.MessagePattern.Notification.Title };
                    }
                    (worker as IMessageConsumer).PushMessage(clientMessage);
                    if (!string.IsNullOrWhiteSpace((worker as IMessageConsumer).DeviceToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, (worker as IMessageConsumer).DeviceToken);
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId) && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        ServicesContextRunTime.Current.UpdateWaitersWithUnreadMessages();

                    }
                }
            }
        }

        /// <MetaDataID>{a9b0ed25-c71a-49b1-baa9-4f132b3e941a}</MetaDataID>
        private void SendRemindMessageToCareGiver(List<IServicesContextWorker> servicesContextWorkers)
        {
            foreach (var worker in servicesContextWorkers)
            {
                var workerActiveShiftWork = worker.ShiftWork;
                if (workerActiveShiftWork != null && workerActiveShiftWork.IsActive())// DateTime.UtcNow > workerActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < workerActiveShiftWork.EndsAt.ToUniversalTime())
                {
                    Message clientMessage = (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId)).FirstOrDefault();
                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        foreach (var entry in this.MessagePattern.Data)
                            clientMessage.Data[entry.Key] = entry.Value;

                        clientMessage.Data["ReminderID"] = UniqueId;
                        clientMessage.Notification = new Notification() { Title = this.MessagePattern.Notification.Title };
                    }
                    (worker as IMessageConsumer).PushMessage(clientMessage);
                    if (!string.IsNullOrWhiteSpace((worker as IMessageConsumer).DeviceToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, (worker as IMessageConsumer).DeviceToken);
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId) && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        ServicesContextRunTime.Current.UpdateWaitersWithUnreadMessages();

                    }
                }
            }
        }

        /// <MetaDataID>{de620283-f575-4133-82da-b80df6857017}</MetaDataID>
        private void SendMessageToFindCareGiver(List<IServicesContextWorker> candidatesForCareGiving)
        {

            foreach (var worker in candidatesForCareGiving)
            {
                var workerActiveShiftWork = worker.ShiftWork;
                if (workerActiveShiftWork != null && workerActiveShiftWork.IsActive())// DateTime.UtcNow > workerActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < workerActiveShiftWork.EndsAt.ToUniversalTime())
                {
                    Message clientMessage = (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId)).FirstOrDefault();
                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        foreach (var entry in this.MessagePattern.Data)
                            clientMessage.Data[entry.Key] = entry.Value;

                        clientMessage.Data["ReminderID"] = UniqueId;
                        clientMessage.Notification = new Notification() { Title = this.MessagePattern.Notification.Title };
                    }
                    (worker as IMessageConsumer).PushMessage(clientMessage);
                    if (!string.IsNullOrWhiteSpace((worker as IMessageConsumer).DeviceToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, (worker as IMessageConsumer).DeviceToken);
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in (worker as IMessageConsumer).Messages.Where(x => x.HasDataValue<string>("ReminderID", UniqueId) && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        ServicesContextRunTime.Current.UpdateWaitersWithUnreadMessages();

                    }
                }
            }
        }

        /// <MetaDataID>{cbe1dfd0-44bd-48e4-af20-d63e91ffc3f7}</MetaDataID>
        public void Stop()
        {
            lock (this)
            {
                TerminateThread = true;
                DurationInMin= (DateTime.UtcNow - StartedAt.ToUniversalTime()).Minutes;
            }
        }

        /// <MetaDataID>{785d469a-2db7-4cbf-8d48-939805d3d0fc}</MetaDataID>
        internal void AddCaregiver(IServicesContextWorker caregiver, Caregiver.CareGivingType careGivingType)
        {
            lock (CaregiversLock)
            {

                _Caregivers.Add(new Caregiver() { Worker = caregiver, CareGiving = careGivingType, WillTakeCareTimestamp = DateTime.UtcNow });


            }
        }
        /// <MetaDataID>{77fbcb66-71a0-46b8-8fb5-9d646c2873c7}</MetaDataID>
        internal void UpdateActiveWorkers(List<IServicesContextWorker> candidatesForCareGiving, List<IServiceContextSupervisor> supervisorsForCareGiving)
        {
            lock (this)
            {
                CandidatesForCareGiving = candidatesForCareGiving;
                SupervisorsForCareGiving = supervisorsForCareGiving;
            }
        }

        /// <MetaDataID>{d3288653-39b3-48f6-b89f-68a9c32f7724}</MetaDataID>
        internal void Init(FlavourBusinessFacade.EndUsers.FlavourBusinessFacade.EndUsers.ClientMessages messageType,
System.Collections.Generic.List<FlavourBusinessFacade.HumanResources.IServicesContextWorker> candidatesForCareGiving,
System.Collections.Generic.List<FlavourBusinessFacade.HumanResources.IServiceContextSupervisor> supervisorsForCareGiving,
FlavourBusinessFacade.EndUsers.FlavourBusinessFacade.EndUsers.Message messagePattern,
System.DateTime reminderStartTime,
System.TimeSpan delayTimeBetweenTriesToFindCareGiver,
System.TimeSpan maximumTimeForCareGiverToAct,
System.TimeSpan delayTimeBetweenTriesToFindSupervisor,
System.TimeSpan maximumTimeBeforeSupervisorTakeCare)
        {
            MessageType = messageType;
            CandidatesForCareGiving = candidatesForCareGiving;
            SupervisorsForCareGiving = supervisorsForCareGiving;
            MessagePattern = messagePattern;

            StartedAt = reminderStartTime;

            DelayTimeBetweenTriesToFindCareGiver = delayTimeBetweenTriesToFindCareGiver;
            MaximumTimeForCareGiverToAct = maximumTimeForCareGiverToAct;
            DelayTimeBetweenTriesToFindSupervisor = delayTimeBetweenTriesToFindSupervisor;
            MaximumTimeBeforeSupervisorTakeCare = maximumTimeBeforeSupervisorTakeCare;
        }
    }
}
