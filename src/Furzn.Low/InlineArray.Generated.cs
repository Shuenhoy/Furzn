namespace Furzn.Low;

using System.Runtime.CompilerServices;


[System.Runtime.CompilerServices.InlineArray(1)]
public struct InlineArray1<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(2)]
public struct InlineArray2<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(3)]
public struct InlineArray3<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(4)]
public struct InlineArray4<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(5)]
public struct InlineArray5<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(6)]
public struct InlineArray6<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(7)]
public struct InlineArray7<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(8)]
public struct InlineArray8<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(9)]
public struct InlineArray9<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(10)]
public struct InlineArray10<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(11)]
public struct InlineArray11<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(12)]
public struct InlineArray12<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(13)]
public struct InlineArray13<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(14)]
public struct InlineArray14<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(15)]
public struct InlineArray15<T>
{
    internal T _element;
}


[System.Runtime.CompilerServices.InlineArray(16)]
public struct InlineArray16<T>
{
    internal T _element;
}





public static class InlineArrayExtensions
{

    public static ref T AtRef<T>(ref this InlineArray1<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray1<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray2<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray2<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray3<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray3<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray4<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray4<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray5<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray5<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray6<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray6<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray7<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray7<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray8<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray8<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray9<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray9<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray10<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray10<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray11<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray11<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray12<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray12<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray13<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray13<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray14<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray14<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray15<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray15<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    public static ref T AtRef<T>(ref this InlineArray16<T> self,int index) {
        #if DEBUG
            return ref self[index];
        #else
            return ref Unsafe.Add(ref self._element, index);
        #endif
    }
    public static ref T CoeffRef<T>(ref this InlineArray16<T> self, int index) {
        return ref Unsafe.Add(ref self._element, index);
    }

    
}