using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Atuvu.Allocation
{
    public sealed class OutdatedTempListException : Exception
    {
        public OutdatedTempListException() : base("Trying to use an outdated Temp List. " +
                                                  "A temp chunk is only valid until another part of the " +
                                                  "code request a temp chunk of the same type")
        {
        }
    }

    internal sealed class BufferChunk<T>
    {
        public sealed class Enumerator : IEnumerator<T>
        {
            readonly BufferChunk<T> m_Chunk;
            int m_CurrentIndex;
            int m_Version;

            public Enumerator(BufferChunk<T> chunk)
            {
                m_Chunk = chunk;
                m_CurrentIndex = -1;
            }

            public void Set(int version)
            {
                m_Version = version;
            }

            public bool MoveNext()
            {
                ++m_CurrentIndex;
                return m_CurrentIndex < m_Chunk.list.Count;
            }

            public void Reset()
            {
                m_CurrentIndex = -1;
            }

            public T Current
            {
                get
                {
                    m_Chunk.ThrowExceptionOnOutdatedVersion(m_Version);
                    return m_Chunk.list[m_CurrentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }

        int m_Version;
        readonly List<T> m_List;
        readonly Enumerator m_Enumerator;

        public int version
        {
            get { return m_Version; }
        }

        public List<T> list
        {
            get { return m_List; }
        }

        public Enumerator GetEnumerator()
        {
            m_Enumerator.Set(m_Version);
            return m_Enumerator;
        }

        public BufferChunk(int size)
        {
            m_Version = 0;
            m_List = new List<T>(size);
            m_Enumerator = new Enumerator(this);
        }

        public void PrepareNewVersion()
        {
            m_List.Clear();
            ++m_Version;
        }

        [Conditional("DEBUG")]
        public void ThrowExceptionOnOutdatedVersion(int targetVersion)
        {
            if (m_Version != targetVersion)
                throw new OutdatedTempListException();
        }
    }

    internal sealed class BufferAllocator
    {
        const int k_DefaultCapacity = 32;

        interface IBuffer {}

        class Buffer<T> : IBuffer
        {
            readonly BufferChunk<T> m_Chunk;  //TODO add support for multiple chunk per buffer (offering the possibility of multiple temp list of the same type)

            public TempList<T> CreateTempList()
            {
                m_Chunk.PrepareNewVersion();
                var enumerator = m_Chunk.GetEnumerator();
                return new TempList<T>(m_Chunk, m_Chunk.version, enumerator);
            }

            public Buffer(int defaultCapacity)
            {
                m_Chunk = new BufferChunk<T>(defaultCapacity);
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

            var list = buffer.CreateTempList();
            return list;
        }
    }
}