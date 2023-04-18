using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Benchmarker.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Benchmarker.MVVM.Model;
using Benchmarker.MVVM.View;
using Benchmarker;

namespace UnitTests
{
    [TestClass]
    public class GraphableServiceTests
    {
        [TestMethod]
        public void drawsALine()
        {
            var gService = new TestGraphableService(5);
            gService.CalculateNext();
            gService.CalculateNext();
            gService.CalculateNext();

            string result = gService.GetGraphString(2, 5);

            Assert.AreEqual("0,2 1,2 2,2 3,1 4,0 ", result);
        }

        [TestMethod]
        public void scalesLineHeight()
        {
            var gService = new TestGraphableService(5);
            gService.CalculateNext();
            gService.CalculateNext();
            gService.CalculateNext();

            string result = gService.GetGraphString(1, 5);

            Assert.AreEqual("0,1 1,1 2,1 3,0.5 4,0 ", result);
        }

        [TestMethod]
        public void scalesLineWidth()
        {
            var gService = new TestGraphableService(5);
            gService.CalculateNext();
            gService.CalculateNext();
            gService.CalculateNext();

            string result = gService.GetGraphString(2, 10);

            Assert.AreEqual("0,2 2,2 4,2 6,1 8,0 ", result);
        }
    }

    public class TestGraphableService : GraphableService
    {
        public TestGraphableService(int size) :base(size) { }

        private int prevValue = 0;
        protected override double GetRawNext()
        {
            return prevValue++;
        }
    }
}
