namespace Furzn.Core

open System.Numerics
open System.Runtime.CompilerServices


[<AutoOpen>]
module Traits =

    [<Interface>]
    type IDim<'Self when IDim<'Self>> =
        abstract member Dim: int

    [<Struct>]
    type D1 =
        interface IDim<D1> with
            member _.Dim = 1

    [<Struct>]
    type D2 =
        interface IDim<D2> with
            member _.Dim = 2

    [<Struct>]
    type D3 =
        interface IDim<D3> with
            member _.Dim = 3

    [<Struct>]
    type D4 =
        interface IDim<D4> with
            member _.Dim = 4

    [<Struct>]
    type DX(dim: int) =
        interface IDim<DX> with
            member _.Dim = dim

    [<Interface>]
    type IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>> =
        abstract member Rows: int
        abstract member Cols: int
        abstract member Item: int * int -> 'Scalar with get

    [<Struct>]
    type MatrixExpression<'Self, 'Scalar, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>> =
        | MatExp of 'Self

        member inline __.unwrap =
            match __ with
            | MatExp x -> x

        member inline __.Rows = __.unwrap.Rows
        member inline __.Cols = __.unwrap.Cols

        member __.Item
            with get (row: int, col: int) = __.unwrap[row, col]

    type IVectorExpression<'Self, 'Scalar, 'Rows
        when IDim<'Rows> and INumberBase<'Scalar> and IVectorExpression<'Self, 'Scalar, 'Rows>> =
        IMatrixExpression<'Self, 'Scalar, 'Rows, D1>
