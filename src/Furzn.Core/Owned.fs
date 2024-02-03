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

        member this.Assign(m: MatrixExpression<_, 'Scalar, 'Rows, 'Cols>) =
            for r in 0 .. rows.Dim - 1 do
                for c in 0 .. cols.Dim - 1 do
                    this.AtRef(r, c) <- m.At(r, c)

        member this.AssignSlice
            (
                startR: int,
                endR: int,
                startC: int,
                endC: int,
                m: MatrixExpression<_, 'Scalar, 'Rows, 'Cols>
            ) =
            for r in startR .. endR - 1 do
                for c in startC .. endC - 1 do
                    this.AtRef(r, c) <- m.At(r - startR, c - startC)

        member inline this.AssignSlice
            (
                startR: Option<int>,
                endR: Option<int>,
                startC: Option<int>,
                endC: Option<int>,
                m: MatrixExpression<_, 'Scalar, 'Rows, 'Cols>
            ) =
            let inline dft v d =
                match v with
                | Some x -> x
                | None -> d

            let startR = dft startR 0
            let endR = dft endR <| this.Rows - 1
            let startC = dft startC 0
            let endC = dft endC <| this.Cols - 1
            this.AssignSlice(startR, endR, startC, endC, m)



        interface IDisposable with
            member __.Dispose() = buffer.Dispose()

        interface IMatrixExpression<Matrix<'Scalar, 'Rows, 'Cols>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = rows
            member self.DimCols = cols

            member this.At(row: int, col: int) = this.AtRef(row, col)

    let matrixLike (m: MatrixExpression<_, 'Scalar, _, _>) =
        new Matrix<'Scalar, _, _>(m.DimRows, m.DimCols)

    [<Extension>]
    type MatrixExpressionExtensions<'Scalar, 'A, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'A, 'Scalar, 'Rows, 'Cols>> =
        [<Extension>]
        static member inline Eval(a: MatrixExpression<'A, 'Scalar, 'Rows, 'Cols>) =
            let output = matrixLike a
            output.Assign a
            output

        [<Extension>]
        static member inline EvalTo
            (
                a: MatrixExpression<'A, 'Scalar, 'Rows, 'Cols>,
                output: outref<Matrix<'Scalar, 'Rows, 'Cols>>
            ) =
            output.Assign a

    let inline (!<) (a: MatrixExpression<_, _, _, _>) = a.Eval()
    let inline (>->) (a: MatrixExpression<_, _, _, _>) (b: outref<Matrix<_, _, _>>) = a.EvalTo &b


    type Vector<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        Matrix<'Scalar, 'Rows, D1>


    [<Extension>]
    type VectorExtensions<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        [<Extension>]
        static member Item(vec: Vector<'Scalar, 'Rows>, index: int) = &vec.AtRef(index, 0)


    let vectorX<'Scalar when INumberBase<'Scalar>> (dim: int) =
        new Vector<'Scalar, DX>(DX dim, D1())

    let matrixX<'Scalar when INumberBase<'Scalar>> (rows: int) (cols: int) =
        new Matrix<'Scalar, DX, DX>(DX rows, DX cols)
