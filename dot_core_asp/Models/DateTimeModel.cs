using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class DateTimeModel
    {
        public string DateTime {get; set;}

        public int DateTimeDifference(int year1, int month1, int day1, int year2, int month2, int day2)
        {
            int daysOfNonSkudYear = 365;
            int startOfYearDaysDiff = ZeroPointDiff(year1, month1, day1);
            int endOfYearDaysDiff = ZeroPointDiff(year2, month2, day2, false);
            
            // Determine skudaar in between and add integer multiple of days on a year.
            List<int> years = new List<int>();
            List<int> daysInYears = new List<int>();
            if (year1 - year2 != 0)
            {
                int yearInBetween = year1 + 1;
                while (yearInBetween < year2)
                {
                    years.Add(yearInBetween);
                    if (IsSkudYear(yearInBetween))
                    {
                        daysInYears.Add(daysOfNonSkudYear + 1);
                    }
                    else
                    {
                        daysInYears.Add(daysOfNonSkudYear);
                    }
                    yearInBetween += 1;
                }

                int totalDaysInYearsBetween = daysInYears.AsQueryable().Sum();
                int daysOfYear1 = IsSkudYear(year1) ? daysOfNonSkudYear + 1 - startOfYearDaysDiff : daysOfNonSkudYear - startOfYearDaysDiff;
                int daysOfYear2 = IsSkudYear(year2) ? daysOfNonSkudYear + 1 - endOfYearDaysDiff : daysOfNonSkudYear - endOfYearDaysDiff;
                return daysOfYear1 + daysOfYear2 + totalDaysInYearsBetween;
            }
            else
            {
                int daysInYear = IsSkudYear(year1) ? 366 : 365;
                return daysInYear - (startOfYearDaysDiff + endOfYearDaysDiff);
            }
        }

        public int ZeroPointDiff(int year, int month, int day, bool isRelativeToStartOfYear = true)
        {
            int daysDiff = 0;
            bool isSkudYear = IsSkudYear(year);
            int daysInAYear = isSkudYear ? 366 : 365;

            int daysJan = 31;
            int daysFeb = isSkudYear ? 29 : 28;
            int daysMarch = 31;
            int daysApril = 30;
            int daysMay = 31;
            int daysJune = 30;
            int daysJuly = 31;
            int daysAug = 31;
            int daysSep = 30;
            int daysOkt = 31;
            int daysNov = 30;
            int daysDec = 31;

            switch (month)
            {
                case 1:
                    daysDiff = day;
                    break;
                case 2:
                    daysDiff = daysJan + day;
                    break;
                case 3:
                    daysDiff = daysJan + daysFeb + day;
                    break;
                case 4:
                    daysDiff = daysJan + daysFeb + daysMarch + day;
                    break;
                case 5:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + day;
                    break;
                case 6:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + day;
                    break;
                case 7:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + day;
                    break;
                case 8:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + daysJuly + day;
                    break;
                case 9:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + daysJuly + daysAug + day;
                    break;
                case 10:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + daysJuly + daysAug + daysSep + day;
                    break;
                case 11:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + daysJuly + daysAug + daysSep + daysOkt + day;
                    daysDiff = daysInAYear - (daysNov - day) - daysDec;
                    break;
                case 12:
                    daysDiff = daysJan + daysFeb + daysMarch + daysApril + daysMay + daysJune + daysJuly + daysAug + daysSep + daysOkt + daysNov + day;
                    daysDiff = daysInAYear - (daysDec - day);
                    break;
            }

            if (!isRelativeToStartOfYear)
            {
                daysDiff = daysInAYear - daysDiff;
            }
            return daysDiff;
        }


        public bool IsSkudYear(int year)
        {
            // Skudaar is used since a year in reality is 365,25 which gets corrected by adding a day to the month of February every fourth year only for the current year.
            // This leave February with 29 days instead of 28 during others years.
            // A year modulus 4 with no rest is a Skudaar, which means, 2004, 2008, 2012, 2016, 2020, 2024 etc.
            return 0 == (year % 4);
        }
    
        public int TestDateTimeDiff(int year1=2000, int month1=7, int day1=16)
        {
            int year2 = 2019;
            int month2 = 2;
            int day2 = 15;

            int diffInDays = this.DateTimeDifference(year1, month1, day1, year2, month2, day2);
            Console.WriteLine("================= Diff in days using custom method =========================");
            Console.WriteLine(diffInDays);
            Console.WriteLine("================= Diff in days using DateTime =========================");
            DateTime date1 = new DateTime(year1, month1, day1);
            DateTime date2 = new DateTime(year2, month2, day2);
            Console.WriteLine(date2.Subtract(date1).Days);
            Console.WriteLine("================= Are the results equal =========================");
            Console.WriteLine(diffInDays == date2.Subtract(date1).Days);
            return diffInDays == date2.Subtract(date1).Days ? 0 : 1;

        }

        public void TestMultipleDateTimeDiff()
        {
            int sum = 0;
            int year = 2000;
            int month = 1;
            int day = 1;
            while (year <= 2019)
            {                
                while (month <= 12)
                {   
                    while (day <= 28)
                    {
                        if (year == 2019 & month == 2 & day == 15)
                        {
                            goto End;
                        }
                        sum += this.TestDateTimeDiff(year, month, day);
                        day += 1;
                    }
                    day = 1;
                    month += 1;
                }
                month = 1;
                year += 1;
            }
            End: Console.WriteLine("================= Sum of bool results =========================");
            Console.WriteLine(sum);

        }
    }
}