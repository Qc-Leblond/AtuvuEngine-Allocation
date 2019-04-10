using System;
using UnityEngine;

namespace Atuvu.Allocation
{
    public static class GetComponentsExtensions
    {
        public static TempList<T> GetComponentsNonAlloc<T>(this GameObject go)
        {
            var buffer = Allocators.GetBuffer<T>();
            go.GetComponents(buffer.list);
            return buffer;
        }

        public static TempList<Component> GetComponentsNonAlloc(this GameObject go, Type type)
        {
            var buffer = Allocators.GetBuffer<Component>();
            go.GetComponents(type, buffer.list);
            return buffer;
        }

        public static TempList<T> GetComponentsInChildrenNonAlloc<T>(this GameObject go)
        {
            return GetComponentsInChildrenNonAlloc<T>(go, false);
        }

        public static TempList<T> GetComponentsInChildrenNonAlloc<T>(this GameObject go, bool includeInactive)
        {
            var buffer = Allocators.GetBuffer<T>();
            go.GetComponentsInChildren(includeInactive, buffer.list);
            return buffer;
        }

        public static TempList<T> GetComponentsInParentNonAlloc<T>(this GameObject go)
        {
            return GetComponentsInParentNonAlloc<T>(go, false);
        }

        public static TempList<T> GetComponentsInParentNonAlloc<T>(this GameObject go, bool includeInactive)
        {
            var buffer = Allocators.GetBuffer<T>();
            go.GetComponentsInParent(includeInactive, buffer.list);
            return buffer;
        }

        public static TempList<T> GetComponentsNonAlloc<T>(this Component component)
        {
            var buffer = Allocators.GetBuffer<T>();
            component.GetComponents(buffer.list);
            return buffer;
        }

        public static TempList<Component> GetComponentsNonAlloc(this Component component, Type type)
        {
            var buffer = Allocators.GetBuffer<Component>();
            component.GetComponents(type, buffer.list);
            return buffer;
        }

        public static TempList<T> GetComponentsInChildrenNonAlloc<T>(this Component component)
        {
            return GetComponentsInChildrenNonAlloc<T>(component, false);
        }

        public static TempList<T> GetComponentsInChildrenNonAlloc<T>(this Component component, bool includeInactive)
        {
            var buffer = Allocators.GetBuffer<T>();
            component.GetComponentsInChildren(includeInactive, buffer.list);
            return buffer;
        }

        public static TempList<T> GetComponentsInParentNonAlloc<T>(this Component component)
        {
            return GetComponentsInParentNonAlloc<T>(component, false);
        }

        public static TempList<T> GetComponentsInParentNonAlloc<T>(this Component component, bool includeInactive)
        {
            var buffer = Allocators.GetBuffer<T>();
            component.GetComponentsInParent(includeInactive, buffer.list);
            return buffer;
        }
    }
}