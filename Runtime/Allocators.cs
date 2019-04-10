namespace Atuvu.Allocation
{
    public static class Allocators
    {
        const int k_DefaultBufferSizes = 16;

        static readonly BufferAllocator s_BufferAllocator = new BufferAllocator();

        public static void PreCacheBuffer<T>()
        {
            s_BufferAllocator.GetBuffer<T>(k_DefaultBufferSizes);
        }

        public static TempList<T> GetBuffer<T>()
        {
            return s_BufferAllocator.GetBuffer<T>(k_DefaultBufferSizes);
        }
    }
}