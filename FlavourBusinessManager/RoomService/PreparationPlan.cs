using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{
    ///// <MetaDataID>{02762bb6-cd92-4b15-ada5-c8c09c003bd6}</MetaDataID>
    //public class PreparationPlan
    //{


    //}



    public class ActionContext
    {
        public Dictionary<PreparationStation, List<ItemsPreparationContext>> ProductionLineActions = new Dictionary<PreparationStation, List<ItemsPreparationContext>>();

        public DateTime? GetPreparationStartsAt(ItemPreparation actionSlot)
        {
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime;

            return null;
        }

        public void SetPreparationStartsAt(ItemPreparation actionSlot, DateTime dateTime)
        {
            if (SlotsPreparationStartsAt.ContainsKey(actionSlot) && SlotsPreparationStartsAt[actionSlot] != dateTime)
            {

            }

            SlotsPreparationStartsAt[actionSlot] = dateTime;

        }

        public DateTime? GetPreparationEndsAt(ItemPreparation actionSlot)
        {
           
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes((actionSlot.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(actionSlot.MenuItem));

            return DateTime.UtcNow + TimeSpanEx.FromMinutes((actionSlot.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(actionSlot.MenuItem));
        }
        Dictionary<ItemPreparation, DateTime> SlotsPreparationStartsAt = new Dictionary<ItemPreparation, DateTime>();

        //        Dictionary<ActionSlot, DateTime> SlotsPreparationEndsAt = new Dictionary<ActionSlot, DateTime>();

        public bool PreparationPlanIsDoubleChecked { get; internal set; }
    }


    static class TimeSpanEx
    {
        public static TimeSpan FromMinutes(double value)
        {
            //return TimeSpan.FromSeconds(value);
            return TimeSpan.FromMinutes(value);
        }
        public static double GetTotalMinutes(this TimeSpan timeSpan)
        {
            //return timeSpan.TotalSeconds;
            return timeSpan.TotalMinutes;
        }
    }

}
