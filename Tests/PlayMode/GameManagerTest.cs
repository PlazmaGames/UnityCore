using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PlazmaGames.Core;
using PlazmaGames.Core.MonoSystem;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlazmaGames.Tests
{
    public interface ITestMonoSystem : IMonoSystem { }
    public class TestMonoSystem : MonoBehaviour, ITestMonoSystem { }

    public class GameManagerTest
    {
        [UnityTest, Order(1)]
        public IEnumerator HasGameManagerTest()
        {
            Assert.AreNotEqual(GameManager.Instance, null);
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator AddMonoSystemTest()
        {
            TestMonoSystem _testMS = new TestMonoSystem();
            GameManager.AddMonoSystem<TestMonoSystem, ITestMonoSystem>(_testMS);
            Assert.IsTrue(GameManager.HasMonoSystem<ITestMonoSystem>());
            yield return null;
        }

        [UnityTest, Order(3)]
        public IEnumerator RemoveMonoSystemTest()
        {
            GameManager.RemoveMonoSystem<ITestMonoSystem>();
            Assert.IsFalse(GameManager.HasMonoSystem<ITestMonoSystem>());
            yield return null;
        }
    }
}
