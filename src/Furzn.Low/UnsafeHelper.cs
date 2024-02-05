namespace Furzn.Low;

using System.Runtime.CompilerServices;

public static class UnsafeHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AsRef<T>(in T ptr) => ref Unsafe.AsRef<T>(in ptr);
}

