namespace Furzn.Core

open System.Numerics
open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices

open Furzn.Low

[<AutoOpen>]
module Owned =


    type MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IStorage<'Storage, 'Scalar>
        and 'Storage :> IDisposable>(rows: 'Rows, cols: 'Cols) =
        let mutable storage = 'Storage.Create(rows.Dim * cols.Dim)
        let mutable _rows = rows
        let mutable _cols = cols

        member __.AtRef(row: int, col: int) = &storage.AtRef(row * _cols.Dim + col)

        member __.CoeffRef(row: int, col: int) =
            &storage.CoeffRef(row * _cols.Dim + col)

        member __.Reserve(size: int) =
            if size > storage.Length then
                let newStorage = 'Storage.Create(size)

                storage.Span.CopyTo(newStorage.Span)

                storage.Dispose()
                storage <- newStorage

        member self.Resize(rows: 'Rows, cols: 'Cols) =
            _rows <- rows
            _cols <- cols
            self.Reserve(rows.Dim * cols.Dim)

        member internal __.Storage = &storage
        member __.Rows = _rows.Dim
        member __.Cols = _cols.Dim
        member __.DimRows = _rows
        member __.DimCols = _cols
        member this.M = MatExp this

        member __.Shape = struct (_rows.Dim, _cols.Dim)
        override self.ToString() = targetToString &self

        interface IMatrixTarget<MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = _rows
            member __.DimCols = _cols

            member self.AtRef(row: int, col: int) = &self.AtRef(row, col)

            member self.CoeffRef(row: int, col: int) = &self.CoeffRef(row, col)

        interface IDisposable with
            member __.Dispose() = storage.Dispose()

        interface IMatrixExpression<MatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member __.DimRows = _rows
            member __.DimCols = _cols

            member this.At(row: int, col: int) = this.AtRef(row, col)

    [<Struct>]
    type ValueMatrixUnsafeRef<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>> =
        val pointer: voidptr

        new(input: inref<'Self>) =
            { pointer = Unsafe.AsPointer(&UnsafeHelper.AsRef<'Self>(&input)) }

        member self.Deref = &Unsafe.AsRef<'Self>(self.pointer)

        interface IMatrixExpression<ValueMatrixUnsafeRef<'Self, 'Scalar, 'Rows, 'Cols>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.Deref.DimRows
            member self.DimCols = self.Deref.DimCols

            member self.At(row: int, col: int) = self.Deref.At(row, col)



    [<Struct>]
    type ValueMatrixBase<'Scalar, 'Rows, 'Cols, 'Storage
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

        member this.Shape = struct (this.rows.Dim, this.cols.Dim)

        member self.AtRef(row: int, col: int) =
            &UnsafeHelper.AsRef<'Storage>(&self.storage).AtRef(row * self.cols.Dim + col)

        member self.CoeffRef(row: int, col: int) =
            &UnsafeHelper.AsRef<'Storage>(&self.storage).CoeffRef(row * self.cols.Dim + col)

        member self.Rows = self.rows.Dim
        member self.Cols = self.cols.Dim
        member self.M = MatExp <| ValueMatrixUnsafeRef &self
        override self.ToString() = targetToString &self

        interface IMatrixExpression<ValueMatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.rows
            member self.DimCols = self.cols

            member self.At(row: int, col: int) = self.AtRef(row, col)

        interface IMatrixTarget<ValueMatrixBase<'Scalar, 'Rows, 'Cols, 'Storage>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.rows
            member self.DimCols = self.cols

            member self.AtRef(row: int, col: int) = &self.AtRef(row, col)

            member self.CoeffRef(row: int, col: int) = &self.CoeffRef(row, col)
