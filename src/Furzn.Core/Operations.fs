namespace Furzn.Core

open System.Numerics
open System.Runtime.InteropServices
open System.Runtime.CompilerServices

[<AutoOpen>]
module Operations =

    type BinaryOp<'Scalar when 'Scalar :> INumberBase<'Scalar>> =
        static abstract member Apply: 'Scalar -> 'Scalar -> 'Scalar

    type Addition<'Scalar when 'Scalar :> INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a + b

    type Subtraction<'Scalar when 'Scalar :> INumberBase<'Scalar>> =
        interface BinaryOp<'Scalar> with
            [<MethodImpl(MethodImplOptions.AggressiveInlining)>]
            static member Apply a b = a - b


    [<Struct>]
    type VecVecCwiseBinaryOp<'Scalar, 'A, 'B, 'Op
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IVectorExpression<'A, 'Scalar>
        and 'B :> IVectorExpression<'B, 'Scalar>
        and 'Op :> BinaryOp<'Scalar>>
        (a: VectorExpression<'A, 'Scalar>, b: VectorExpression<'B, 'Scalar>) =
        static member inline create a b =
            VecExp <| VecVecCwiseBinaryOp(a, b): VectorExpression<_, _>

        interface IVectorExpression<VecVecCwiseBinaryOp<'Scalar, 'A, 'B, 'Op>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) = 'Op.Apply a[index] b[index]

    [<Struct>]
    type VecScalarCwiseBinaryOp<'Scalar, 'A, 'Op
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IVectorExpression<'A, 'Scalar>
        and 'Op :> BinaryOp<'Scalar>>(a: VectorExpression<'A, 'Scalar>, b: 'Scalar) =
        static member inline create a b =
            VecExp <| VecScalarCwiseBinaryOp(a, b): VectorExpression<_, _>

        interface IVectorExpression<VecScalarCwiseBinaryOp<'Scalar, 'A, 'Op>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) = 'Op.Apply a[index] b

    [<Struct>]
    type MatMatCwiseBinaryOp<'Scalar, 'A, 'B, 'Op
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IMatrixExpression<'A, 'Scalar>
        and 'B :> IMatrixExpression<'B, 'Scalar>
        and 'Op :> BinaryOp<'Scalar>>
        (a: MatrixExpression<'A, 'Scalar>, b: MatrixExpression<'B, 'Scalar>) =
        static member inline create a b =
            MatExp <| MatMatCwiseBinaryOp(a, b): MatrixExpression<_, _>

        interface IMatrixExpression<MatMatCwiseBinaryOp<'Scalar, 'A, 'B, 'Op>, 'Scalar> with
            member __.Rows = a.Rows
            member __.Cols = a.Cols

            member __.Item
                with get (row: int, col: int) = 'Op.Apply a[row, col] b[row, col]


    type VecAdd<'Scalar, 'A, 'B when Scalar<'Scalar> and Vector<'A, 'Scalar> and Vector<'B, 'Scalar>>
        = VecVecCwiseBinaryOp<'Scalar, 'A, 'B, Addition<'Scalar>>

    type VecSub<'Scalar, 'A, 'B when Scalar<'Scalar> and Vector<'A, 'Scalar> and Vector<'B, 'Scalar>>
        = VecVecCwiseBinaryOp<'Scalar, 'A, 'B, Subtraction<'Scalar>>

    type MatAdd<'Scalar, 'A, 'B when Scalar<'Scalar> and Matrix<'A, 'Scalar> and Matrix<'B, 'Scalar>>
        = MatMatCwiseBinaryOp<'Scalar, 'A, 'B, Addition<'Scalar>>

    type MatSub<'Scalar, 'A, 'B when Scalar<'Scalar> and Matrix<'A, 'Scalar> and Matrix<'B, 'Scalar>>
        = MatMatCwiseBinaryOp<'Scalar, 'A, 'B, Subtraction<'Scalar>>

    type VecScalarMul<'Scalar, 'A when Scalar<'Scalar> and Vector<'A, 'Scalar>> =
        VecScalarCwiseBinaryOp<'Scalar, 'A, Addition<'Scalar>>

    [<Struct>]
    type MatVecMul<'Scalar, 'A, 'B
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IMatrixExpression<'A, 'Scalar>
        and 'B :> IVectorExpression<'B, 'Scalar>>
        (a: MatrixExpression<'A, 'Scalar>, b: VectorExpression<'B, 'Scalar>) =
        interface IVectorExpression<MatVecMul<'Scalar, 'A, 'B>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) =
                    let mutable sum: 'Scalar = LanguagePrimitives.GenericZero

                    for i in 0 .. a.Cols - 1 do
                        sum <- sum + a[index, i] * b[i]

                    sum

    let inline matvecmul a b = VecExp <| MatVecMul(a, b)


    [<Struct>]
    type AddHelper =
        | AddHelper


        static member inline (?<-)(AddHelper, a, b) = VecExp <| VecAdd(a, b)
        static member inline (?<-)(AddHelper, a: VectorX<_>, b) = VecExp <| VecAdd(a.V, b)
        static member inline (?<-)(AddHelper, a, b: VectorX<_>) = VecExp <| VecAdd(a, b.V)

        static member inline (?<-)(AddHelper, a: VectorX<_>, b: VectorX<_>) =
            VecExp <| VecAdd(a.V, b.V)

        static member inline (?<-)(AddHelper, a, b) = a + b

    let inline (+) a b = (?<-) AddHelper a b



    [<Struct>]
    type MulHelper =
        | MulHelper

        static member inline (?<-)(MulHelper, a, b) = matvecmul a b
        static member inline (?<-)(MulHelper, a, b) = VecExp <| VecScalarMul(a, b)
        static member inline (?<-)(MulHelper, a, b) = VecExp <| VecScalarMul(b, a)
        static member inline (?<-)(MulHelper, a: VectorX<_>, b) = VecExp <| VecScalarMul(a.V, b)
        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = VecExp <| VecScalarMul(b.V, a)
        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = matvecmul a b.V
        static member inline (?<-)(MulHelper, a, b) = a * b



    let inline (*) a b = (?<-) MulHelper a b
