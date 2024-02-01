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

    let inline vecadd a b : VectorExpression<_, _> = { unwrap = VecAdd(a, b) }

    [<Struct>]
    type ScalarVecMul<'Scalar, 'A
        when 'Scalar :> INumberBase<'Scalar> and 'A :> IVectorExpression<'A, 'Scalar>>
        (b: 'Scalar, a: VectorExpression<'A, 'Scalar>) =
        interface IVectorExpression<ScalarVecMul<'Scalar, 'A>, 'Scalar> with
            member __.Rows = a.Rows

            member __.Item
                with get (index: int) = a[index] * b

    let inline scalvecmul a b : VectorExpression<_, _> = { unwrap = ScalarVecMul(a, b) }

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

    let inline matvecmul a b : VectorExpression<_, _> = { unwrap = MatVecMul(a, b) }

    [<Struct>]
    type AddHelper =
        | AddHelper


        static member inline (?<-)(AddHelper, a, b) = vecadd a b
        static member inline (?<-)(AddHelper, a: VectorX<_>, b) = vecadd a.V b
        static member inline (?<-)(AddHelper, a, b: VectorX<_>) = vecadd a b.V

        static member inline (?<-)(AddHelper, a: VectorX<_>, b: VectorX<_>) = vecadd a.V b.V



        static member inline (?<-)(AddHelper, a, b) = a + b

    let inline (+) a b = (?<-) AddHelper a b



    [<Struct>]
    type MulHelper =
        | MulHelper

        static member inline (?<-)(MulHelper, a, b) = matvecmul a b

        static member inline (?<-)(MulHelper, a, b) = scalvecmul a b
        static member inline (?<-)(MulHelper, a, b) = scalvecmul b a

        static member inline (?<-)(MulHelper, a: VectorX<_>, b) = scalvecmul b a.V

        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = scalvecmul a b.V

        static member inline (?<-)(MulHelper, a, b: VectorX<_>) = matvecmul a b.V

        static member inline (?<-)(MulHelper, a, b) = a * b



    let inline (*) a b = (?<-) MulHelper a b
