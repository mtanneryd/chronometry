/*
 * Copyright ©  2017-2019 Tånneryd IT AB
 * 
 * This file is part of the nuget package Tanneryd.Chronometry.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tanneryd.Chronometry
{
    public static class DateTimeExtensions
    {
        public static int[] NumberOfDaysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public static int Quarter(this DateTime self)
        {
            var quarter = ((self.Month - 1) / 3) + 1;
            return quarter;
        }

        public static IEnumerable<DateTime> DaysUntil(this DateTime from, DateTime to)
        {
            while (from <= to)
            {
                yield return new DateTime(from.Year, from.Month, from.Day);
                from = from.AddDays(1);
            }
        }

        public static IEnumerable<DateTime> MonthsUntil(this DateTime from, DateTime to)
        {
            var fromMonth = new DateTime(from.Year, from.Month, 1);
            var toMonth = new DateTime(to.Year, to.Month, 1);

            while (fromMonth.Date <= toMonth.Date)
            {
                yield return fromMonth;
                fromMonth = fromMonth.AddMonths(1);
            }
        }

        public static IEnumerable<DateTime> YearsUntil(this DateTime from, DateTime to)
        {
            while (from.Year <= to.Year)
            {
                yield return new DateTime(from.Year, 1, 1);
                from = from.AddYears(1);
            }
        }

        public static bool IsSameMonth(this DateTime self, DateTime other)
        {
            return self.Year == other.Year && self.Month == other.Month;
        }

        public static bool IsLeapYear(this DateTime self)
        {
            var year = self.Year;
            bool isLeapYear = ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0));
            return isLeapYear;
        }

        public static DateTime FirstDayOfMonth(this DateTime self)
        {
            return new DateTime(self.Year, self.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime self)
        {
            var day = NumberOfDaysInMonth[self.Month];
            if (self.Month == 2 && IsLeapYear(self)) day++;
            return new DateTime(self.Year, self.Month, day);
        }

        public static DateTime NextMonday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Monday, count);
        }
        public static DateTime NextTuesday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Tuesday, count);
        }
        public static DateTime NextWednesday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Wednesday, count);
        }
        public static DateTime NextThursday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Thursday, count);
        }
        public static DateTime NextFriday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Friday, count);
        }
        public static DateTime NextSaturday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Saturday, count);
        }
        public static DateTime NextSunday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Sunday, count);
        }
        private static DateTime NextDay(DateTime self, DayOfWeek dayOfWeek, int count)
        {
            if (count < 1) throw new ArgumentException("count must be > 0");

            var interval = new Interval { Start = self.AddDays(1), Stop = self.AddDays(7 * count) };
            var next = interval.FilterOnDayOfWeek(dayOfWeek).Skip(count - 1).First();

            return next;
        }

        public static DateTime PrevMonday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Monday, count);
        }
        public static DateTime PrevTuesday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Tuesday, count);
        }
        public static DateTime PrevWednesday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Wednesday, count);
        }
        public static DateTime PrevThursday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Thursday, count);
        }
        public static DateTime PrevFriday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Friday, count);
        }
        public static DateTime PrevSaturday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Saturday, count);
        }
        public static DateTime PrevSunday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Sunday, count);
        }
        private static DateTime PrevDay(DateTime self, DayOfWeek dayOfWeek, int count)
        {
            if (count < 1) throw new ArgumentException("count must be > 0");

            var interval = new Interval { Start = self.AddDays(-7 * count), Stop = self.AddDays(-1) };
            var next = interval.FilterOnDayOfWeek(dayOfWeek)
                .Reverse()
                .Skip(count - 1)
                .First();

            return next;
        }
    }
}
