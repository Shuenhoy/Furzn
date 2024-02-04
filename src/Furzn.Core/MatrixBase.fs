namespace Furzn.Core

open System.Numerics
open System

[<AutoOpen>]
module Owned =


    type MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IStorage<'Storage, 'Scalar>
        and 'Storage :> IDisposable>(rows: 'Rows, cols: 'Cols) =
        let storage = 'Storage.Create(rows.Dim * cols.Dim)

        member __.AtRef(row: int, col: int) = &storage.AtRef(row * cols.Dim + col)
        member __.CoeffRef(row: int, col: int) = &storage.CoeffRef(row * cols.Dim + col)

        member __.Rows = rows.Dim
        member __.Cols = cols.Dim
        member this.M = MatExp this

        interface IMatrixTarget<MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = rows
            member __.DimCols = cols

            member self.AtRef(row: int, col: int) = &self.AtRef(row, col)

            member self.CoeffRef(row: int, col: int) = &self.CoeffRef(row, col)

        interface IDisposable with
            member __.Dispose() = storage.Dispose()

        interface IMatrixExpression<MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = rows
            member __.DimCols = cols

            member this.At(row: int, col: int) = this.AtRef(row, col)

    [<Struct>]
    type ValueMatrixImpl<'Scalar, 'Rows, 'Cols, 'Storage
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IStorage<'Storage, 'Scalar>
        and 'Storage: struct> =
        val storage: 'Storage
        val rows: 'Rows
        val cols: 'Cols

        new(rows: 'Rows, cols: 'Cols) =
            { storage = 'Storage.Create(rows.Dim * cols.Dim); rows = rows; cols = cols }

        member self.AtRef(row: int, col: int) =
            &self.storage.AtRef(row * self.cols.Dim + col)

        member self.CoeffRef(row: int, col: int) =
            &self.storage.CoeffRef(row * self.cols.Dim + col)

        member self.Rows = self.rows.Dim
        member self.Cols = self.cols.Dim

        interface IMatrixTarget<ValueMatrixImpl<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.rows
            member self.DimCols = self.cols

            member self.AtRef(row: int, col: int) = &self.AtRef(row, col)

            member self.CoeffRef(row: int, col: int) = &self.CoeffRef(row, col)

    [<Struct>]
    type ValueMatrixBase<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixTarget<'Self, 'Scalar, 'Rows, 'Cols>
        and 'Self: struct>(mat: 'Self) =
        member this.M = MatExp this

        member self.AtRef(row: int, col: int) = &mat.AtRef(row, col)
        member self.CoeffRef(row: int, col: int) = &mat.CoeffRef(row, col)
        member self.DimRows = mat.DimRows
        member self.DimCols = mat.DimCols

        interface IMatrixTarget<ValueMatrixBase<'Self, 'Scalar, 'Rows, 'Cols>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = mat.DimRows
            member __.DimCols = mat.DimCols

            member __.AtRef(row: int, col: int) = &mat.AtRef(row, col)

            member __.CoeffRef(row: int, col: int) = &mat.CoeffRef(row, col)

        interface IMatrixExpression<ValueMatrixBase<'Self, 'Scalar, 'Rows, 'Cols>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = mat.DimRows
            member __.DimCols = mat.DimCols

            member __.At(row: int, col: int) = mat.AtRef(row, col)
