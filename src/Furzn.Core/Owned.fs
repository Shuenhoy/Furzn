namespace Furzn.Core

open CommunityToolkit.HighPerformance.Buffers
open System.Numerics
open System
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<AutoOpen>]
module Owned =
    let inline private uncheckedIndexRef (mem: Span<'T>) (index: int) =
        let memRef = &MemoryMarshal.GetReference<'T>(mem)
        &Unsafe.Add<'T>(&memRef, index)


    type Matrix<'Scalar, 'Rows, 'Cols when IDim<'Rows> and IDim<'Cols> and INumberBase<'Scalar>>
        (rows: 'Rows, cols: 'Cols) =
        let buffer = MemoryOwner<'Scalar>.Allocate(rows.Dim * cols.Dim)

        member self.AtRef(row: int, col: int) =
#if DEBUG
            &buffer.Span[(row * cols.Dim) + col]
#else
            self.CoeffRef(row, col)
#endif

        member __.CoeffRef(row: int, col: int) =
            &uncheckedIndexRef buffer.Span (row * cols.Dim + col)

        member __.Rows = rows.Dim
        member __.Cols = cols.Dim
        member this.M = MatExp this

        interface IDisposable with
            member __.Dispose() = buffer.Dispose()

        interface IMatrixExpression<Matrix<'Scalar, 'Rows, 'Cols>, 'Scalar, 'Rows, 'Cols> with
            member self.Rows = self.Rows
            member self.Cols = self.Cols

            member this.Item
                with get (row: int, col: int) = this.AtRef(row, col)

    type Vector<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        Matrix<'Scalar, 'Rows, D1>

    [<Extension>]
    type MatrixExtensions<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        [<Extension>]
        static member Item(vec: Matrix<'Scalar, 'Rows, D2>, r: int, c: int) = &vec.AtRef(r, c)

        [<Extension>]
        static member Item(vec: Matrix<'Scalar, 'Rows, D3>, r: int, c: int) = &vec.AtRef(r, c)

        [<Extension>]
        static member Item(vec: Matrix<'Scalar, 'Rows, D4>, r: int, c: int) = &vec.AtRef(r, c)

        [<Extension>]
        static member Item(vec: Matrix<'Scalar, 'Rows, DX>, r: int, c: int) = &vec.AtRef(r, c)


    [<Extension>]
    type VectorExtensions<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        [<Extension>]
        static member Item(vec: Vector<'Scalar, 'Rows>, index: int) = &vec.AtRef(index, 0)


    let vectorX<'Scalar when INumberBase<'Scalar>> (dim: int) =
        new Vector<'Scalar, DX>(DX dim, D1())

    let matrixX<'Scalar when INumberBase<'Scalar>> (rows: int) (cols: int) =
        new Matrix<'Scalar, DX, DX>(DX rows, DX cols)
