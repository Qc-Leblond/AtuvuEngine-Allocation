using System;
using NUnit.Framework;
using UnityEngine;
using Is = NUnit.Framework.Is;
using UnityEngine.TestTools.Constraints;

namespace Atuvu.Allocation.Tests
{
    public sealed class TempList
    {
        DummyComponent m_Root;

        [OneTimeSetUp]
        public void Startup()
        {
            Allocators.PreCacheBuffer<DummyComponent>();
            m_Root = new GameObject("Root").AddComponent<DummyComponent>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(m_Root.gameObject);
        }
        
        [Test]
        public void TempList_ForEach_NoAlloc()
        {
            var list = m_Root.GetComponentsNonAlloc<DummyComponent>();
            Assert.That(() =>
            {
                DummyComponent comp = null;
                foreach (var component in list)
                {
                    comp = component;
                }
            },
            Is.Not.AllocatingGCMemory());
        }

        [Test]
        public void AccessingElementAtIndex_Outdated_ThrowsException()
        {
            Assert.Catch<OutdatedTempListException>(() =>
            {
                var tempList = m_Root.GetComponentsNonAlloc<DummyComponent>();
                m_Root.GetComponentsNonAlloc<DummyComponent>();
                var element = tempList[0];
            });
        }

        [Test]
        public void AccessingCount_Outdated_ThrowsException()
        {
            Assert.Catch<OutdatedTempListException>(() =>
            {
                var tempList = m_Root.GetComponentsNonAlloc<DummyComponent>();
                m_Root.GetComponentsNonAlloc<DummyComponent>();
                var count = tempList.Count;
            });
        }

        [Test]
        public void AccessingEnumerator_Outdated_ThrowsException()
        {
            Assert.Catch<OutdatedTempListException>(() =>
            {
                var tempList = m_Root.GetComponentsNonAlloc<DummyComponent>();
                m_Root.GetComponentsNonAlloc<DummyComponent>();
                var enumerator = tempList.GetEnumerator();
            });
        }

        [Test]
        public void AccessingEnumeratorCurrent_Outdated_ThrowsException()
        {
            Assert.Catch<OutdatedTempListException>(() =>
            {
                var tempList = m_Root.GetComponentsNonAlloc<DummyComponent>();
                var enumerator = tempList.GetEnumerator();
                m_Root.GetComponentsNonAlloc<DummyComponent>();
                var current = enumerator.Current;
            });
        }
    }
}