﻿using Drikka.Geo.Data.ExecutionPlan;
using Drikka.Geo.Data.Tests.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace Drikka.Geo.Data.Tests.ExecutionPlain
{
    [TestClass]
    public class QueryPlanTest
    {
        [TestMethod]
        public void GetText_Returns_SelectText()
        {
            var mapping = new PersonMap();
            mapping.ExecuteMapping();

            var select = new QueryPlan(mapping, null, null);

            select.CreatePlanParameter().SqlText.ToUpper().Should().Be("SELECT ID, NAME, AGE FROM PERSON");
        }

        [TestMethod]
        public void GetText_Returns_SelectFormatedText()
        {
            var mapping = new FormatedPersonMap();
            mapping.ExecuteMapping();

            var select = new QueryPlan(mapping, null, null);

            select.CreatePlanParameter().SqlText.ToUpper().Should().Be("SELECT ID, ASBINARY(NAME) AS NAME, AGE FROM PERSON");
        }
    }
}
