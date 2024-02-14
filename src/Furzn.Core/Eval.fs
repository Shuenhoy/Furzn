namespace Furzn.Core

[<AutoOpen>]
module Eval =
    type EvalHelper =
        | EvalHelper

        static member inline ($)(EvalHelper, a: MatrixExpression<_, 'Scalar, DX, DX>) =
            let ret = new Matrix<'Scalar, _, _>(a.DimRows, a.DimCols)
            ret.AssignSlice(None, None, None, None, a)
            ret

        static member inline ($)(EvalHelper, a: MatrixExpression<_, 'Scalar, DX, _>) =
            let ret = new Matrix<'Scalar, _, _>(a.DimRows, a.DimCols)
            ret.AssignSlice(None, None, None, None, a)
            ret

        static member inline ($)(EvalHelper, a: MatrixExpression<_, 'Scalar, _, DX>) =
            let ret = new Matrix<'Scalar, _, _>(a.DimRows, a.DimCols)
            ret.AssignSlice(None, None, None, None, a)
            ret

        static member inline ($)(EvalHelper, a: MatrixExpression<_, 'Scalar, D2, D1>) =
            let ret =
                new ValueMatrixBase<'Scalar, _, _, InlineStorage2<'Scalar>>(a.DimRows, a.DimCols)

            ret.AssignSlice(None, None, None, None, a)
            ret


    let inline eval x = EvalHelper $ x

    let inline (|Eval|) (a) = eval a
    let inline (|Vector2d|) (a: MatrixExpression<_, _, D2, D1>) = eval a
