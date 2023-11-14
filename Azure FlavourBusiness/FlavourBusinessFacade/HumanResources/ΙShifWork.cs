using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{313cecf4-7009-411b-8a13-75ec51498f82}</MetaDataID>
    [BackwardCompatibilityID("{313cecf4-7009-411b-8a13-75ec51498f82}")]
    [GenerateFacadeProxy]
    [HttpVisible]

    public interface IShiftWork : IActivity
    {
        /// <MetaDataID>{359d4451-92ac-44d8-a4a6-5345dcd8084e}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        System.DateTime EndsAt { get; }



        /// <MetaDataID>{c27feaa5-d323-4133-82d5-a16879b44560}</MetaDataID>
        [Association("WorkerShifWork", Roles.RoleB, "484dfd7b-d501-43b7-871c-f247f3e648b1", "4b06fadb-9a5d-4494-a8da-c060d22d8dbe")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        IServicesContextWorker Worker { get; }

        /// <MetaDataID>{c9ce9f96-f8c9-45f9-bc85-58e42a2e9e3e}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        System.DateTime StartsAt { get; }

        /// <MetaDataID>{6a244954-71e1-4d1f-a22a-bd1781a99a01}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double PeriodInHours { get; }
    }

    /// <MetaDataID>{6975b5f1-e6a6-4722-8254-a0d69770bca5}</MetaDataID>
    public static class ShiftWorkClientside
    {
        public static bool IsActive(this IShiftWork shiftWork)
        {

            var d = (DateTime.UtcNow +TimeSpan.FromMinutes(15)).ShiftworkRound();

            if (shiftWork != null)
            {
                var startedAt = shiftWork.StartsAt;
                var workingHours = shiftWork.PeriodInHours;

                //var billingPayments = (shiftWork as IDebtCollection)?.BillingPayments;
                double overtimeRation = (3.00 / 8);
                var overtime = workingHours * overtimeRation;
                var utcNow = DateTime.UtcNow.ShiftworkRound();//.Date + TimeSpan.FromHours(hour);
                if (utcNow.ToUniversalTime() >= startedAt.ToUniversalTime() && startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours + overtime) > utcNow.ToUniversalTime()) //  utcNow >= startedAt.ToUniversalTime() && utcNow <= startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        public static DateTime ShiftworkRound(this DateTime date)
        {


            double hour = date.Hour +((double)date.Minute/ 60);

            hour = Math.Round(hour * 2) / 2;

            //date.Millisecond.setMilliseconds(0);
            //this.StartAt.setSeconds(0);
            double minutes = (hour - Math.Truncate(hour)) * 60;
            hour=Math.Truncate(hour);


            date=  new DateTime(
                            date.Year,
                            date.Month,
                            date.Day,
                            (int)hour,
                            (int)minutes,
                            0,
                            0,
                            date.Kind);
            return date;
        }

    }
}