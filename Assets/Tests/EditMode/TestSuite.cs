using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using PacMan.GameSystem;
using PacMan.AI;
using PacMan.Player;
using PacMan.Utils;

namespace PacMan.Tests
{
    /// <summary>
    /// Test suite that groups all test categories
    /// </summary>
    [TestFixture]
    public class TestSuite
    {
        [Test]
        public void TestSuite_RunAllTests()
        {
            // This is a meta-test to verify our test framework is working
            Assert.Pass("Test framework is properly configured");
        }
    }
}