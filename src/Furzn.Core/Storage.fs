namespace Furzn.Core

open CommunityToolkit.HighPerformance.Buffers
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System


[<AutoOpen>]
module Storage =
    let inline internal uncheckedIndexRef (mem: Span<'T>) (index: int) =
        let memRef = &MemoryMarshal.GetReference<'T>(mem)
        &Unsafe.Add<'T>(&memRef, index)

    [<Interface>]
    type IStorage<'Self, 'Scalar> =
        abstract member AtRef: int -> byref<'Scalar>
        abstract member CoeffRef: int -> byref<'Scalar>
        abstract member Length: int
        static abstract member Create: int -> 'Self

    [<Struct>]
    type HeapStorage<'Scalar> =
        val buffer: MemoryOwner<'Scalar>
        val length: int
        new(length: int) = { buffer = MemoryOwner<'Scalar>.Allocate(length); length = length }

        member self.AtRef i =
#if DEBUG
            &self.buffer.Span[i]
#else
            self.CoeffRef i
#endif
        member self.CoeffRef i = &uncheckedIndexRef self.buffer.Span i

        interface IDisposable with
            member self.Dispose() = self.buffer.Dispose()

        interface IStorage<HeapStorage<'Scalar>, 'Scalar> with

            member self.AtRef i = &self.AtRef i
            member self.CoeffRef i = &self.CoeffRef i
            member self.Length = self.length
            static member Create l = new HeapStorage<_>(l)
