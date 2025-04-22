using NUnit.Framework;
using System;
using System.Collections.Generic;
using ClockV2.Model;

namespace ClockV2.Tests
{
    [TestFixture]
    public class ClockModelTests
    {
        private ClockModel model;

        [SetUp]
        public void Setup()
        {
            model = new ClockModel();
        }

        [Test]
        public void AddAlarm_ShouldStoreAlarm()
        {
            // Arrange
            var alarm = new Alarm(DateTime.Now.AddMinutes(10), "Test Alarm");

            // Act
            model.AddAlarm(alarm);
            var alarms = model.GetAllAlarms();

            // Assert
            Assert.That(alarms.Count, Is.EqualTo(1));
            Assert.That(alarms[0].Label, Is.EqualTo("Test Alarm"));
        }

        [Test]
        public void GetNextAlarm_ShouldReturnEarliestAlarm()
        {
            // Arrange
            var alarm1 = new Alarm(DateTime.Now.AddMinutes(20), "Later");
            var alarm2 = new Alarm(DateTime.Now.AddMinutes(5), "Soon");

            model.AddAlarm(alarm1);
            model.AddAlarm(alarm2);

            // Act
            var next = model.GetNextAlarm();

            // Assert
            Assert.That(next.Label, Is.EqualTo("Soon"));
        }

        [Test]
        public void RemoveNextAlarm_ShouldRemoveTopAlarm()
        {
            // Arrange
            var alarm = new Alarm(DateTime.Now.AddMinutes(1), "ToRemove");
            model.AddAlarm(alarm);

            // Act
            model.RemoveNextAlarm();

            // Assert
            Assert.IsFalse(model.HasAlarms());
        }

        [Test]
        public void PopIfDue_ShouldPopOnlyIfDue()
        {
            // Arrange
            var dueAlarm = new Alarm(DateTime.Now.AddSeconds(-1), "Due");
            var futureAlarm = new Alarm(DateTime.Now.AddMinutes(1), "Future");

            model.AddAlarm(dueAlarm);
            model.AddAlarm(futureAlarm);

            // Act
            var popped = model.PopIfDue(DateTime.Now);

            // Assert
            Assert.That(popped.Label, Is.EqualTo("Due"));
        }

        [Test]
        public void SetAlarms_ShouldReplaceExistingAlarms()
        {
            // Arrange
            var alarmList = new List<Alarm>
            {
                new Alarm(DateTime.Now.AddMinutes(3), "One"),
                new Alarm(DateTime.Now.AddMinutes(5), "Two")
            };

            model.SetAlarms(alarmList);
            var alarms = model.GetAllAlarms();

            // Assert
            Assert.That(alarms.Count, Is.EqualTo(2));
            Assert.That(alarms[0].Label, Is.EqualTo("One"));
        }
    }
}
