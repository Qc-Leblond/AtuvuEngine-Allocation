using System;
using System.Collections.Generic;

namespace Atuvu.Allocation
{
    internal sealed class BufferAllocator
    {
        const int k_DefaultCapacity = 32;

        interface IBuffer {}

        class Buffer<T> : IBuffer
        {
            public TempList<T> list { get; private set; } 

            public Buffer(int defaultCapacity)
            {
                list = new TempList<T>(defaultCapacity);
            }
        }

        readonly Dictionary<Type, IBuffer> m_Buffers = new Dictionary<Type, IBuffer>(64);

        public TempList<T> GetBuffer<T>(int defaultCapacity = k_DefaultCapacity)
        {
            Type type = typeof(T);
            IBuffer rawBuffer;
            Buffer<T> buffer;
            if (!m_Buffers.TryGetValue(type, out rawBuffer))
            {
                buffer = new Buffer<T>(defaultCapacity);
                m_Buffers.Add(type, buffer);
            }
            else
            {
                buffer = (Buffer<T>)rawBuffer;
            }

            var list = buffer.list;
            list.Clear();
            return list;
        }
    }
}