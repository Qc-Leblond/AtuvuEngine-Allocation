using System.Collections.Generic;

namespace Atuvu.Allocation
{
    public sealed class TempList<T> : List<T>
    {
        internal TempList(int capacity) : base(capacity)
        {
        }
    }
}