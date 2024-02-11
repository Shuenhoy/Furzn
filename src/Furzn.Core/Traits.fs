namespace Furzn.Core

open System.Numerics
open System.Runtime.CompilerServices


[<AutoOpen>]
module Traits =


    [<Interface>]
    type IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols
        when IDim<'Rows>
        and IDim<'Cols>
        and INumberBase<'Scalar>
        and IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>> =
        abstract member DimRows: 'Rows
        abstract member DimCols: 'Cols
        abstract member At: int * int -> 'Scalar
        abstract member M: MatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>

    and [<Struct>] MatrixExpression<'Self, 'Scalar, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'Self, 'Scalar, 'Rows, 'Cols>> =
        | MatExp of 'Self

        member inline __.unwrap =
            match __ with
            | MatExp x -> x

        member self.DimRows = self.unwrap.DimRows
        member self.DimCols = self.unwrap.DimCols


    type IVectorExpression<'Self, 'Scalar, 'Rows
        when IDim<'Rows> and INumberBase<'Scalar> and IVectorExpression<'Self, 'Scalar, 'Rows>> =
        IMatrixExpression<'Self, 'Scalar, 'Rows, D1>
