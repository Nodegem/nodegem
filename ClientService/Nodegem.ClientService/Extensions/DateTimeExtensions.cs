using System;

namespace Nodegem.ClientService.Extensions
{
    public static class DateTimeExtensions
    {
        public static int YearDifference(this DateTime lValue, DateTime rValue)
        {
            var zeroTime = new DateTime(1, 1, 1);

            var a = new DateTime(2007, 1, 1);
            var b = new DateTime(2008, 1, 1);

            var span = b - a;
            return (zeroTime + span).Year - 1;
        }
        
        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
    }
}