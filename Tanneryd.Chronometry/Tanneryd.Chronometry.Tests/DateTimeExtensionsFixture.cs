using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tanneryd.Chronometry.Tests
{
    [TestClass]
    public class DateTimeExtensionsFixture
    {

        [TestMethod]
        public void NextDay()
        {
            var today = new DateTime(2016, 10, 04);
            var nextDay = today.NextMonday();
            Assert.AreEqual(new DateTime(2016, 10, 10), nextDay);

            nextDay = today.NextTuesday(2);
            Assert.AreEqual(new DateTime(2016, 10, 18), nextDay);

            nextDay = today.NextWednesday();
            Assert.AreEqual(new DateTime(2016, 10, 5), nextDay);

            nextDay = today.NextThursday(3);
            Assert.AreEqual(new DateTime(2016, 10, 20), nextDay);

            nextDay = today.NextFriday();
            Assert.AreEqual(new DateTime(2016, 10, 7), nextDay);

            nextDay = today.NextSaturday(2);
            Assert.AreEqual(new DateTime(2016, 10, 15), nextDay);

            nextDay = today.NextSunday();
            Assert.AreEqual(new DateTime(2016, 10, 9), nextDay);
        }

        [TestMethod]
        public void PrevDay()
        {
            var today = new DateTime(2016, 10, 04);
            var prevDay = today.PrevMonday();
            Assert.AreEqual(new DateTime(2016, 10, 3), prevDay);

            prevDay = today.PrevTuesday(2);
            Assert.AreEqual(new DateTime(2016, 9, 20), prevDay);

            prevDay = today.PrevWednesday();
            Assert.AreEqual(new DateTime(2016, 9, 28), prevDay);

            prevDay = today.PrevThursday(3);
            Assert.AreEqual(new DateTime(2016, 9, 15), prevDay);

            prevDay = today.PrevFriday();
            Assert.AreEqual(new DateTime(2016, 9, 30), prevDay);

            prevDay = today.PrevSaturday(2);
            Assert.AreEqual(new DateTime(2016, 9, 24), prevDay);

            prevDay = today.PrevSunday();
            Assert.AreEqual(new DateTime(2016, 10, 2), prevDay);
        }

        [TestMethod]
        public void CalculateQuarter()
        {
            var d = new DateTime(2016, 1, 1);
            Assert.AreEqual(1, d.Quarter());

            d = new DateTime(2016, 2, 1);
            Assert.AreEqual(1, d.Quarter());

            d = new DateTime(2016, 3, 1);
            Assert.AreEqual(1, d.Quarter());

            d = new DateTime(2016, 4, 1);
            Assert.AreEqual(2, d.Quarter());

            d = new DateTime(2016, 5, 1);
            Assert.AreEqual(2, d.Quarter());

            d = new DateTime(2016, 6, 1);
            Assert.AreEqual(2, d.Quarter());

            d = new DateTime(2016, 7, 1);
            Assert.AreEqual(3, d.Quarter());

            d = new DateTime(2016, 8, 1);
            Assert.AreEqual(3, d.Quarter());

            d = new DateTime(2016, 9, 1);
            Assert.AreEqual(3, d.Quarter());

            d = new DateTime(2016, 10, 1);
            Assert.AreEqual(4, d.Quarter());

            d = new DateTime(2016, 11, 1);
            Assert.AreEqual(4, d.Quarter());

            d = new DateTime(2016, 12, 1);
            Assert.AreEqual(4, d.Quarter());
        }
    }
}
