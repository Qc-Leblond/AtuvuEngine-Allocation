using System.Collections;
using System.Collections.Generic;

namespace Atuvu.Allocation
{
    public struct TempList<T> : IReadOnlyList<T>
    {
        readonly int m_Version;
        readonly BufferChunk<T> m_Buffer;
        readonly BufferChunk<T>.Enumerator m_Enumerator;
        readonly List<T> m_List;

        public bool IsValid
        {
            get { return m_Version == m_Buffer.version; }
        }

        public int Count
        {
            get
            {
                m_Buffer.ThrowExceptionOnOutdatedVersion(m_Version);
                return m_Buffer.list.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                m_Buffer.ThrowExceptionOnOutdatedVersion(m_Version);
                return m_Buffer.list[index];
            }
        }

        internal TempList(BufferChunk<T> buffer, int version, BufferChunk<T>.Enumerator enumerator)
        {
            m_List = buffer.list;
            m_Buffer = buffer;
            m_Enumerator = enumerator;
            m_Version = version;
        }

        internal List<T> list { get { return m_List; } }

        public IEnumerator<T> GetEnumerator()
        {
            m_Buffer.ThrowExceptionOnOutdatedVersion(m_Version);
            return m_Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}