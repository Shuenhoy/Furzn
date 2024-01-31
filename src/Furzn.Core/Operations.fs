namespace Furzn.Core

open System.Numerics
open System.Runtime.InteropServices

[<AutoOpen>]
module Operations =
    [<Struct>]
    type VecAdd<'Scalar, 'A, 'B
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IVectorExpression<'A, 'Scalar>
        and 'B :> IVectorExpression<'B, 'Scalar>>
        (a: VectorExpression<'A, 'Scalar>, b: VectorExpression<'B, 'Scalar>) =
        interface IVectorExpression<VecAdd<'Scalar, 'A, 'B>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) = a[index] + b[index]

    [<Struct>]
    type ScalarVecMul<'Scalar, 'A
        when 'Scalar :> INumberBase<'Scalar> and 'A :> IVectorExpression<'A, 'Scalar>>
        (b: 'Scalar, a: VectorExpression<'A, 'Scalar>) =
        interface IVectorExpression<ScalarVecMul<'Scalar, 'A>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) = a[index] * b

    [<Struct>]
    type MatVecMul<'Scalar, 'A, 'B
        when 'Scalar :> INumberBase<'Scalar>
        and 'A :> IMatrixExpression<'A, 'Scalar>
        and 'B :> IVectorExpression<'B, 'Scalar>>(a: 'A, b: VectorExpression<'B, 'Scalar>) =
        interface IVectorExpression<MatVecMul<'Scalar, 'A, 'B>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) =
                    let mutable sum: 'Scalar = LanguagePrimitives.GenericZero

                    for i in 0 .. a.Cols - 1 do
                        sum <- sum + a[index, i] * b[i]

                    sum


    [<Struct>]
    type AddHelper =
        | AddHelper

        static member inline vecadd a b = { unwrap = VecAdd(a, b) }

        static member inline (?<-)(AddHelper, a, b) = AddHelper.vecadd a b
        static member inline (?<-)(AddHelper, a: VectorX<_>, b) = AddHelper.vecadd a.V b
        static member inline (?<-)(AddHelper, a, b: VectorX<_>) = AddHelper.vecadd a b.V

        static member inline (?<-)(AddHelper, a: VectorX<_>, b: VectorX<_>) =
            AddHelper.vecadd a.V b.V



        static member inline (?<-)(AddHelper, a, b) = a + b

    let inline (+) a b = (?<-) AddHelper a b

    [<Struct>]
    type LeftMulTag = | LeftMulTag

    [<Struct>]
    type RightMulTag = | RightMulTag


    [<Struct>]
    type MulHelper =
        | MulHelper

        static member inline (?<-)(MulHelper, a, b) = { unwrap = MatVecMul(a, b) }

        static member inline (?<-)(MulHelper, a, b) = { unwrap = ScalarVecMul(a, b) }
        static member inline (?<-)(MulHelper, a, b) = { unwrap = ScalarVecMul(b, a) }

        static member inline (?<-)(MulHelper, a: VectorX<_>, b) = { unwrap = ScalarVecMul(b, a.V) }

        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = { unwrap = ScalarVecMul(a, b.V) }

        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = { unwrap = MatVecMul(a, b.V) }

        static member inline (?<-)(MulHelper, a, b) = a * b



    let inline (*) a b = (?<-) MulHelper a b
