namespace Furzn.Core

open System.Runtime.CompilerServices
open System.Numerics


[<AutoOpen>]
module AssignTarget =
    [<Interface>]
    type IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols>> =
        abstract member DimRows: 'Rows
        abstract member DimCols: 'Cols
        abstract member AtRef: int * int -> byref<'Scalar>
        abstract member CoeffRef: int * int -> byref<'Scalar>

    [<Extension>]
    type MatrixTargetExtensions<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols>> =
        static member AssignSlice
            (
                self: byref<'Self>,
                startR: int,
                endR: int,
                startC: int,
                endC: int,
                m: MatrixExpression<_, 'Scalar, 'Rows, 'Cols>
            ) =
            for r in startR..endR do
                for c in startC..endC do
                    self.AtRef(r, c) <- m.At(r - startR, c - startC)

            &self

        [<Extension>]
        static member inline AssignSlice
            (
                self: byref<'Self>,
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
            let endR = dft endR <| self.DimRows.Dim - 1
            let startC = dft startC 0
            let endC = dft endC <| self.DimCols.Dim - 1
            MatrixTargetExtensions.AssignSlice(&self, startR, endR, startC, endC, m)
