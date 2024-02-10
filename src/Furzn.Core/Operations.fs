namespace Furzn.Core

open System.Numerics
open System.Runtime.InteropServices
open System.Runtime.CompilerServices

[<AutoOpen>]
module Operations =

    type BinaryOp<'Scalar when INumberBase<'Scalar>> =
        static abstract member Apply: 'Scalar -> 'Scalar -> 'Scalar

    type Addition<'Scalar when INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a + b

    type Subtraction<'Scalar when INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a - b

    type Multiplication<'Scalar when INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a * b

    type Division<'Scalar when INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a / b

    [<Struct>]
    type CwiseBinaryOp<'Scalar, 'A, 'B, 'Rows, 'Cols, 'Op
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'A, 'Scalar, 'Rows, 'Cols>
        and IMatrixExpression<'B, 'Scalar, 'Rows, 'Cols>
        and 'Op :> BinaryOp<'Scalar>> =
        val a: 'A
        val b: 'B
        new(MatExp a, MatExp b) = { a = a; b = b }
        static member inline create a b = MatExp <| CwiseBinaryOp(a, b)

        interface IMatrixExpression<CwiseBinaryOp<'Scalar, 'A, 'B, 'Rows, 'Cols, 'Op>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.a.DimRows
            member self.DimCols = self.a.DimCols
            member self.M = MatExp self

            member self.At(row: int, col: int) =
                'Op.Apply (self.a.At(row, col)) (self.b.At(row, col))

    type MatAdd<'Scalar, 'A, 'B, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'A, 'Scalar, 'Rows, 'Cols>
        and IMatrixExpression<'B, 'Scalar, 'Rows, 'Cols>> =
        CwiseBinaryOp<'Scalar, 'A, 'B, 'Rows, 'Cols, Addition<'Scalar>>

    type MatSub<'Scalar, 'A, 'B, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'A, 'Scalar, 'Rows, 'Cols>
        and IMatrixExpression<'B, 'Scalar, 'Rows, 'Cols>> =
        CwiseBinaryOp<'Scalar, 'A, 'B, 'Rows, 'Cols, Subtraction<'Scalar>>

    [<Struct>]
    type ScalarCwiseOp<'Scalar, 'M, 'Rows, 'Cols, 'Op
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'M, 'Scalar, 'Rows, 'Cols>
        and 'Op :> BinaryOp<'Scalar>> =
        val m: 'M
        val s: 'Scalar
        new(MatExp m, s) = { m = m; s = s }

        interface IMatrixExpression<ScalarCwiseOp<'Scalar, 'M, 'Rows, 'Cols, 'Op>, 'Scalar, 'Rows, 'Cols> with
            member self.DimRows = self.m.DimRows
            member self.DimCols = self.m.DimCols
            member self.M = MatExp self

            member self.At(row: int, col: int) = 'Op.Apply (self.m.At(row, col)) self.s

    type MatScalarMul<'Scalar, 'M, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'M, 'Scalar, 'Rows, 'Cols>> =
        ScalarCwiseOp<'Scalar, 'M, 'Rows, 'Cols, Multiplication<'Scalar>>

    type MatScalarDiv<'Scalar, 'M, 'Rows, 'Cols
        when INumberBase<'Scalar>
        and IDim<'Rows>
        and IDim<'Cols>
        and IMatrixExpression<'M, 'Scalar, 'Rows, 'Cols>> =
        ScalarCwiseOp<'Scalar, 'M, 'Rows, 'Cols, Division<'Scalar>>

    [<Struct>]
    type AddHelper =
        | AddHelper

        static member inline (?<-)(AddHelper, a, b) = MatExp <| MatAdd(a, b)
        static member inline (?<-)(AddHelper, a: Matrix<_, _, _>, b) = MatExp <| MatAdd(a.M, b)
        static member inline (?<-)(AddHelper, a, b: Matrix<_, _, _>) = MatExp <| MatAdd(a, b.M)

        static member inline (?<-)(AddHelper, a: Matrix<_, _, _>, b: Matrix<_, _, _>) =
            MatExp <| MatAdd(a.M, b.M)

        static member inline (?<-)(AddHelper, a, b) = a + b

    let inline (+) a b = (?<-) AddHelper a b

    [<Struct>]
    type SubHelper =
        | SubHelper

        static member inline (?<-)(SubHelper, a, b) = MatExp <| MatSub(a, b)
        static member inline (?<-)(SubHelper, a: Matrix<_, _, _>, b) = MatExp <| MatSub(a.M, b)
        static member inline (?<-)(SubHelper, a, b: Matrix<_, _, _>) = MatExp <| MatSub(a, b.M)

        static member inline (?<-)(SubHelper, a: Matrix<_, _, _>, b: Matrix<_, _, _>) =
            MatExp <| MatSub(a.M, b.M)

        static member inline (?<-)(SubHelper, a, b) = a - b

    let inline (-) a b = (?<-) SubHelper a b

    [<Struct>]
    type MulHelper =
        | MulHelper

        static member inline (?<-)(MulHelper, a, b) = MatExp <| MatScalarMul(a, b)
        static member inline (?<-)(MulHelper, a, b) = MatExp <| MatScalarMul(b, a)

        static member inline (?<-)(MulHelper, a: Matrix<_, _, _>, b) =
            MatExp <| MatScalarMul(a.M, b)

        static member inline (?<-)(MulHelper, a, b: Matrix<_, _, _>) =
            MatExp <| MatScalarMul(b.M, a)

        static member inline (?<-)(MulHelper, a, b) = a * b

    let inline (*) a b = (?<-) MulHelper a b

    [<Struct>]
    type DivHelper =
        | DivHelper

        static member inline (?<-)(DivHelper, a, b) = MatExp <| MatScalarDiv(a, b)

        static member inline (?<-)(DivHelper, a: Matrix<_, _, _>, b) =
            MatExp <| MatScalarDiv(a.M, b)


        static member inline (?<-)(DivHelper, a, b) = a / b

    let inline (/) a b = (?<-) DivHelper a b
