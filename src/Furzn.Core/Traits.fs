namespace Furzn.Core

open System.Numerics

[<AutoOpen>]
module Traits =
    [<Interface>]
    type IVectorExpression<'Self, 'Scalar
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IVectorExpression<'Self, 'Scalar>> =
        abstract member Rows: int
        abstract member Item: int -> 'Scalar with get

    [<Struct>]
    type VectorExpression<'Self, 'Scalar
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IVectorExpression<'Self, 'Scalar>> =
        | VecExp of 'Self

        member inline __.unwrap =
            match __ with
            | VecExp x -> x

        member inline __.Rows = __.unwrap.Rows

        member inline __.Item
            with inline get (index: int) = __.unwrap.Item index


    [<Interface>]
    type IMatrixExpression<'Self, 'Scalar
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IMatrixExpression<'Self, 'Scalar>> =
        abstract member Rows: int
        abstract member Cols: int
        abstract member Item: int * int -> 'Scalar with get

    [<Struct>]
    type MatrixExpression<'Self, 'Scalar
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IMatrixExpression<'Self, 'Scalar>> =
        | MatExp of 'Self

        member inline __.unwrap =
            match __ with
            | MatExp x -> x

        member inline __.Rows = __.unwrap.Rows
        member inline __.Cols = __.unwrap.Cols

        member inline __.Item
            with inline get (row: int, col: int) = __.unwrap.Item(row, col)

    type Scalar<'Scalar when 'Scalar :> INumberBase<'Scalar>> = 'Scalar
    type Vector<'V, 'S when Scalar<'S> and 'V :> IVectorExpression<'V, 'S>> = 'V
    type Matrix<'M, 'S when Scalar<'S> and 'M :> IMatrixExpression<'M, 'S>> = 'M
