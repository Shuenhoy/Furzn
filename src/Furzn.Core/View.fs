namespace Furzn.Core

open System.Numerics
open System
open System.Runtime.CompilerServices

[<AutoOpen>]
module View =
    [<Struct>]
    type MatrixSliceView<'Self, 'Scalar, 'Rows, 'Cols, 'SliceRows, 'SliceCols
        when IDim<'Rows>
        and IDim<'Cols>
        and IDim<'SliceRows>
        and IDim<'SliceCols>
        and INumberBase<'Scalar>
        and IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols>> =

        val mat: 'Self
        val sliceRows: 'SliceRows
        val sliceCols: 'SliceCols
        val rowOffset: int
        val colOffset: int

        new(mat: 'Self, sliceRows: 'SliceRows, sliceCols: 'SliceCols, rowOffset: int, colOffset: int) =
            {
                mat = mat
                sliceRows = sliceRows
                sliceCols = sliceCols
                rowOffset = rowOffset
                colOffset = colOffset
            }

        member self.M = MatExp self

        interface IMatrixTarget<MatrixSliceView<'Self, 'Scalar, 'Rows, 'Cols, 'SliceRows, 'SliceCols>, 'Scalar, 'SliceRows, 'SliceCols> with
            member self.DimRows = self.sliceRows
            member self.DimCols = self.sliceCols

            member self.AtRef(row: int, col: int) =
#if DEBUG
                if row < 0 || row >= self.sliceRows.Dim then
                    raise (new ArgumentOutOfRangeException("row"))

                if col < 0 || col >= self.sliceCols.Dim then
                    raise (new ArgumentOutOfRangeException("col"))
#endif
                &self.mat.AtRef(row + self.rowOffset, col + self.colOffset)

            member self.CoeffRef(row: int, col: int) =
                &self.mat.CoeffRef(row + self.rowOffset, col + self.colOffset)

        interface IMatrixExpression<MatrixSliceView<'Self, 'Scalar, 'Rows, 'Cols, 'SliceRows, 'SliceCols>, 'Scalar, 'SliceRows, 'SliceCols> with
            member self.At(row: int, col: int) =
#if DEBUG
                if row < 0 || row >= self.sliceRows.Dim then
                    raise (new ArgumentOutOfRangeException("row"))

                if col < 0 || col >= self.sliceCols.Dim then
                    raise (new ArgumentOutOfRangeException("col"))
#endif
                self.mat.AtRef(row + self.rowOffset, col + self.colOffset)

            member self.DimRows = self.sliceRows
            member self.DimCols = self.sliceCols
            member self.M = self.M

    [<Extension>]
    type MatrixTargetExtensions =
        [<Extension>]
        static member inline Row<'Self, 'Scalar, 'Rows, 'Cols
            when IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols>>
            (
                self: 'Self,
                row: int
            ) =
            MatrixSliceView(self, D1, self.DimCols, row, 0)
