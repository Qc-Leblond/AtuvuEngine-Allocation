using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Constraints;
using Is = NUnit.Framework.Is;
using Object = UnityEngine.Object;

namespace Atuvu.Allocation.Tests
{
    public sealed class GetComponents
    {
        static readonly Func<Component, TempList<DummyComponent>>[] s_ComponentMethods =
        {
            (comp) => comp.GetComponentsNonAlloc<DummyComponent>(),
            (comp) => comp.GetComponentsInChildrenNonAlloc<DummyComponent>(),
            (comp) => comp.GetComponentsInParentNonAlloc<DummyComponent>(),
        };

        static readonly Func<GameObject, TempList<DummyComponent>>[] s_GameObjectMethods =
        {
            (go) => go.GetComponentsNonAlloc<DummyComponent>(),
            (go) => go.GetComponentsInChildrenNonAlloc<DummyComponent>(),
            (go) => go.GetComponentsInParentNonAlloc<DummyComponent>(),
        };

        DummyComponent m_Root;
        Transform m_Child;
        DummyComponent m_Leaf;

        [OneTimeSetUp]
        public void Setup()
        {
            Allocators.PreCacheBuffer<DummyComponent>();

            m_Root = new GameObject("Root").AddComponent<DummyComponent>();
            m_Root.gameObject.AddComponent<DummyComponent>();

            m_Child = new GameObject("Child").transform;
            m_Child.SetParent(m_Root.transform);

            m_Leaf = new GameObject("GrandChild 0").AddComponent<DummyComponent>();
            m_Leaf.transform.SetParent(m_Child);

            for (int i = 0; i < 2; ++i)
            {
                var grandchild = new GameObject("GrandChild " + (i+1)).AddComponent<DummyComponent>();
                grandchild.transform.SetParent(m_Child);
            }

            for (int i = 0; i < 2; ++i)
            {
                var grandchild = new GameObject("GrandChild " + (i + 3)).AddComponent<DummyComponent>();
                grandchild.transform.SetParent(m_Child);
                grandchild.gameObject.SetActive(false);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_Root.gameObject);
        }

        [Test]
        public void GameObject_GetComponents_ReturnRightComponents()
        {
            var list = m_Root.gameObject.GetComponentsNonAlloc<DummyComponent>();
            Assert.AreEqual(list.Count, 2);
        }

        [Test]
        [TestCase(false, ExpectedResult = 5, TestName = "Without Inactive")]
        [TestCase(true, ExpectedResult = 7, TestName = "With Inactive")]
        public int GameObject_GetComponentsInChildren_ReturnRightComponents(bool includeInactive)
        {
            var list = m_Root.gameObject.GetComponentsInChildrenNonAlloc<DummyComponent>(includeInactive);
            return list.Count;
        }

        [Test]
        public void GameObject_GetComponentsInParent_ReturnRightComponents()
        {
            var list = m_Leaf.gameObject.GetComponentsInParentNonAlloc<DummyComponent>();
            Assert.AreEqual(list.Count, 3);
        }

        [Test]
        public void Component_GetComponents_ReturnRightComponents()
        {
            var list = m_Root.GetComponentsNonAlloc<DummyComponent>();
            Assert.AreEqual(list.Count, 2);
        }

        [Test]
        [TestCase(false, ExpectedResult = 5, TestName = "Without Inactive")]
        [TestCase(true, ExpectedResult = 7, TestName = "With Inactive")]
        public int Component_GetComponentsInChildren_ReturnRightComponents(bool includeInactive)
        {
            var list = m_Root.GetComponentsInChildrenNonAlloc<DummyComponent>(includeInactive);
            return list.Count;
        }

        [Test]
        public void Component_GetComponentsInParent_ReturnRightComponents()
        {
            var list = m_Leaf.GetComponentsInParentNonAlloc<DummyComponent>();
            Assert.AreEqual(list.Count, 3);
        }

        [Test]
        [TestCaseSource(nameof(s_ComponentMethods))]
        public void GameObject_NoAllocation(Func<Component, TempList<DummyComponent>> func)
        {
            Assert.That(() => { func.Invoke(m_Root); }, Is.Not.AllocatingGCMemory());
        }

        [Test]
        [TestCaseSource(nameof(s_GameObjectMethods))]
        public void GameObject_NoAllocation(Func<GameObject, TempList<DummyComponent>> func)
        {
            Assert.That(() => { func.Invoke(m_Root.gameObject); }, Is.Not.AllocatingGCMemory());
        }
    }
}