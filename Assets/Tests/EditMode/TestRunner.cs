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
    /// Test runner that executes all unit tests in sequence
    /// </summary>
    public class TestRunner
    {
        [Test]
        public void RunAllTests()
        {
            // This is a placeholder test to show how to organize all tests
            // In practice, Unity's test runner will automatically discover and run all tests
            Assert.Pass("Test runner initialized successfully");
        }
    }
}