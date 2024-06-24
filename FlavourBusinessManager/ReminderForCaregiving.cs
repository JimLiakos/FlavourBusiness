using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.ServicePointRunTime;
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
    internal class ReminderForCareGiving<T> where T : IServicesContextWorker
    {


        public ReminderForCareGiving(List<IServicesContextWorker> candidatesForCareGiving,
            List<IServiceContextSupervisor> supervisorsForCareGiving,
            Message messagePattern,
            DateTime reminderStartTime,
            TimeSpan delayTimeBetweenTriesToFindCareGiver,
            TimeSpan maximumTimeForCareGiverToAct,
            TimeSpan delayTimeBetweenTriesToFindSupervisor,
            TimeSpan maximumTimeBeforeSupervisorTakeCare)
        {
            CandidatesForCareGiving = candidatesForCareGiving;
            SupervisorsForCareGiving = supervisorsForCareGiving;
            MessagePattern = messagePattern;

            StartedAt = reminderStartTime;

            DelayTimeBetweenTriesToFindCareGiver = delayTimeBetweenTriesToFindCareGiver;
            MaximumTimeForCareGiverToAct = maximumTimeForCareGiverToAct;
            DelayTimeBetweenTriesToFindSupervisor = delayTimeBetweenTriesToFindSupervisor;
            MaximumTimeBeforeSupervisorTakeCare = maximumTimeBeforeSupervisorTakeCare;
        }

        List<IServicesContextWorker> CandidatesForCareGiving;
        List<IServiceContextSupervisor> SupervisorsForCareGiving;
        TimeSpan DelayTimeBetweenTriesToFindCareGiver;

        TimeSpan MaximumTimeForCareGiverToAct = TimeSpan.FromMinutes(4);

        TimeSpan MaximumTimeBeforeSupervisorTakeCare = TimeSpan.FromMinutes(12);

        DateTime TimeOfLastMessageSendToCareGiver = DateTime.MinValue;

        DateTime TimeOfLastMessageSendToSupervisor = DateTime.MinValue;

        TimeSpan DelayTimeBetweenTriesToFindSupervisor;
        private string UniqueId;
        DateTime StartedAt = DateTime.UtcNow;


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

        Message MessagePattern;


        bool TerminateThread = false;
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
                        var careGivers = Caregivers.Where(x => x.Worker is T).OrderByDescending(x => x.WillTakeCareTimestamp).ToList();

                        if (careGivers.Any())
                        {
                            //minimum time span between the two remind messages for care giving waiter of long time conversation

                            if (careGivers.First().WillTakeCareTimestamp > TimeOfLastMessageSendToCareGiver)
                            {
                                if ((DateTime.UtcNow - careGivers.First().WillTakeCareTimestamp.Value.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                {

                                    TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                                    SendRemindMessageToCareGiver(careGivers.Select(x => x.Worker).ToList());

                                }
                            }
                            else
                            {
                                if ((DateTime.UtcNow - TimeOfLastMessageSendToCareGiver.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                {
                                    TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                                    SendRemindMessageToCareGiver(careGivers.Select(x => x.Worker).ToList());
                                }
                            }


                        }
                        else if ((DateTime.UtcNow - TimeOfLastMessageSendToCareGiver.ToUniversalTime()) > DelayTimeBetweenTriesToFindCareGiver)
                        {
                            TimeOfLastMessageSendToCareGiver = DateTime.UtcNow;
                            SendMessageToFindCareGiver(CandidatesForCareGiving);
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

                                    }
                                }
                                else
                                {
                                    if ((DateTime.UtcNow - TimeOfLastMessageSendToSupervisor.ToUniversalTime()) > MaximumTimeForCareGiverToAct)
                                    {
                                        TimeOfLastMessageSendToSupervisor = DateTime.UtcNow;
                                        SendRemindMessageToSupervisor(careGivers.Select(x => x.Worker).ToList());
                                    }
                                }

                            }
                            else if ((DateTime.UtcNow - TimeOfLastMessageSendToSupervisor.ToUniversalTime()) > DelayTimeBetweenTriesToFindCareGiver)
                            {
                                TimeOfLastMessageSendToSupervisor = DateTime.UtcNow;
                                SendMessageToFindSupervisor(SupervisorsForCareGiving);
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

        public void Stop()
        {
            lock (this)
                TerminateThread = true;
        }

        internal void AddCaregiver(IServicesContextWorker caregiver, Caregiver.CareGivingType careGivingType)
        {
            lock (CaregiversLock)
            {

                _Caregivers.Add(new Caregiver() { Worker = caregiver, CareGiving = careGivingType, WillTakeCareTimestamp = DateTime.UtcNow });


            }
        }
    }
}
