namespace Furzn.Core

open CommunityToolkit.HighPerformance.Buffers
open System.Numerics
open System
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<AutoOpen>]
module Owned =
    let inline private uncheckedIndex (mem: Span<'T>) (index: int) =
        let memRef = &MemoryMarshal.GetReference<'T>(mem)
        Unsafe.Add<'T>(&memRef, index)

    let inline private uncheckedIndexSet (mem: Span<'T>) (index: int) (value: 'T) =
        let memRef = &MemoryMarshal.GetReference<'T>(mem)
        Unsafe.Add<'T>(&memRef, index) <- value

    type VectorX<'Scalar when 'Scalar :> INumberBase<'Scalar> and 'Scalar: unmanaged>(rows: int) =
        let buffer = MemoryOwner<'Scalar>.Allocate(rows)

        member __.Item
            with set (index: int) (value: 'Scalar) = uncheckedIndexSet buffer.Span index value
            and get (index: int) = uncheckedIndex buffer.Span index

        static member (!!)(this: VectorX<'Scalar>) = this.V
        member this.V = VecExp this

        member __.at
            with set (index: int) (value: 'Scalar) = buffer.Span[index] <- value
            and get (index: int) = buffer.Span[index]

        interface IDisposable with
            member __.Dispose() = buffer.Dispose()

        interface IVectorExpression<VectorX<'Scalar>, 'Scalar> with
            member __.Rows = rows

            member this.Item
                with get (index: int) = this[index]

    type MatrixX<'Scalar when 'Scalar :> INumberBase<'Scalar> and 'Scalar: unmanaged>
        (rows: int, cols: int) =
        let buffer = MemoryOwner<'Scalar>.Allocate(rows * cols)

        member __.Item
            with set (row: int, col: int) (value: 'Scalar) =
                buffer.Span[(row * cols + col)] <- value
            and get (row: int, col: int) = buffer.Span[row * cols + col]

        member this.M = MatExp this

        interface IDisposable with
            member __.Dispose() = buffer.Dispose()

        interface IMatrixExpression<MatrixX<'Scalar>, 'Scalar> with
            member __.Rows = rows
            member __.Cols = cols

            member this.Item
                with get (row: int, col: int) = this[row, col]
