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
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IVectorExpression<'Self, 'Scalar>> = {
        unwrap: 'Self
    } with

        member inline __.Rows = __.unwrap.Rows

        member inline __.Item
            with inline get (index: int) = __.unwrap.Item index


    [<Interface>]
    type IMatrixExpression<'Self, 'Scalar
        when 'Scalar :> INumberBase<'Scalar> and 'Self :> IMatrixExpression<'Self, 'Scalar>> =
        abstract member Rows: int
        abstract member Cols: int
        abstract member Item: int * int -> 'Scalar with get
