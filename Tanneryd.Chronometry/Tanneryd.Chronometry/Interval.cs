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

namespace Tanneryd.Chronometry
{
    public class Interval
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public TimeSpan TimeSpan => new TimeSpan(Stop.Ticks-Start.Ticks);

        public Interval()
        {
        }

        public Interval(DateTime start, DateTime stop)
        {
            Start = start;
            Stop = stop;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///     true if this interval is completely contained by the other interval, false otherwise
        /// </returns>
        public bool IsEnclosedBy(Interval other)
        {
            bool b = (Start <= other.Stop &&
                      Stop >= other.Start);

            return b;
        }
        
        public Interval Intersect(Interval other)
        {
            var days = Start.DaysUntil(Stop);
            var otherDays = other.Start.DaysUntil(other.Stop);

            var intersectingDays = days.Intersect(otherDays).OrderBy(d=>d).ToArray();
            if (intersectingDays.Any())
            {
                return new Interval
                {
                    Start = intersectingDays.First(),
                    Stop = intersectingDays.Last()
                };
            }

            return null;
        }

        public IEnumerable<Interval> Except(Interval other)
        {
            var days = Start.DaysUntil(Stop);
            var otherDays = other.Start.DaysUntil(other.Stop);

            var result = days.Except(otherDays).OrderBy(d => d).ToArray();
            if (result.Any())
            {
                var interval = new Interval {Start = result[0], Stop = result[0]};
                for (int i = 1; i < result.Length; i++)
                {
                    if (result[i-1].AddDays(1) == result[i]) continue;
                    interval.Stop = result[i - 1];
                    yield return interval;
                    interval = new Interval { Start = result[i], Stop = result[i] };
                }
            }
        }

        /// <summary>
        /// Create new intervals of the the supplied calendar unit type.
        /// The first and last unit type returned might not be complete.
        /// </summary>
        /// <param name="calendarUnit"></param>
        /// <returns></returns>
        public IEnumerable<Interval> Slice(CalendarUnit calendarUnit)
        {
            var days = Start.DaysUntil(Stop).ToArray();

            var results = new List<Interval>();
            if (calendarUnit == CalendarUnit.Day)
            {
                results.AddRange(days.Select(d => new Interval {Start = d, Stop = d}));
            }
            else if (calendarUnit == CalendarUnit.Week)
            {
                var numberOfDaysInFirstWeek = 0;
                int diff = days.First().DayOfWeek - DayOfWeek.Monday;
                if (diff < 0)
                    numberOfDaysInFirstWeek = 1;
                else
                    numberOfDaysInFirstWeek = 7 - diff;

                numberOfDaysInFirstWeek = Math.Min(numberOfDaysInFirstWeek, days.Length);
                results.Add(new Interval { Start = days.First(), Stop = days.Skip(numberOfDaysInFirstWeek - 1).First() });

                var groupedDays = days.Skip(numberOfDaysInFirstWeek)
                    .Select((day, index) => new { day, index })
                    .GroupBy(g => g.index / 7, i => i.day);

                results.AddRange(groupedDays.Select(g => new Interval { Start = g.First(), Stop = g.Last() }));
            }
            else if (calendarUnit == CalendarUnit.Month)
            {
                results.AddRange(days
                    .GroupBy(d => new { d.Year, d.Month })
                    .Select(g =>
                    {
                        var sortedDays = g.OrderBy(d => d).ToArray();
                        return new Interval { Start = sortedDays.First(), Stop = sortedDays.Last() };
                    }));
            }
            else if (calendarUnit == CalendarUnit.Quarter)
            {
                results.AddRange(days
                    .GroupBy(d => new { Year = d.Year, Quarter = d.Quarter() })
                    .Select(g =>
                    {
                        var sortedDays = g.OrderBy(d => d).ToArray();
                        return new Interval { Start = sortedDays.First(), Stop = sortedDays.Last() };
                    }));
            }
            else if (calendarUnit == CalendarUnit.Year)
            {
                results.AddRange(days
                    .GroupBy(d => d.Year )
                    .Select(g =>
                    {
                        var sortedDays = g.OrderBy(d => d).ToArray();
                        return new Interval { Start = sortedDays.First(), Stop = sortedDays.Last() };
                    }));
            }
            else if (calendarUnit == CalendarUnit.Century)
            {
                results.AddRange(days
                    .GroupBy(d => d.Year / 100)
                    .Select(g =>
                    {
                        var sortedDays = g.OrderBy(d => d).ToArray();
                        return new Interval { Start = sortedDays.First(), Stop = sortedDays.Last() };
                    }));
            }
            else if (calendarUnit == CalendarUnit.Millenium)
            {
                results.AddRange(days
                    .GroupBy(d => d.Year / 1000)
                    .Select(g =>
                    {
                        var sortedDays = g.OrderBy(d => d).ToArray();
                        return new Interval { Start = sortedDays.First(), Stop = sortedDays.Last() };
                    }));
            }
            else if (calendarUnit == CalendarUnit.Aeon)
            {
                results.Add(new Interval { Start = days.First(), Stop = days.Last() });
            }

            return results;
        }

        public DateTime[] Mondays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Monday);
        }

        public DateTime[] Tuesdays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Tuesday);
        }

        public DateTime[] Wednesdays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Wednesday);
        }

        public DateTime[] Thursdays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Thursday);
        }

        public DateTime[] Fridays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Friday);
        }

        public DateTime[] Saturdays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Saturday);
        }

        public DateTime[] Sundays()
        {
            return FilterOnDayOfWeek(DayOfWeek.Sunday);
        }

        public DateTime[] FilterOnDayOfWeek(DayOfWeek dayOfWeek)
        {
            return Start.DaysUntil(Stop).Where(d => d.DayOfWeek == dayOfWeek).OrderBy(d=>d).ToArray();
        }
    }
}