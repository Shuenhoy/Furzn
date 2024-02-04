namespace Furzn.Core

open System
open System.Numerics
open System.Runtime.CompilerServices

[<AutoOpen>]
module Matrix =
    type Matrix<'Scalar, 'Rows, 'Cols when IDim<'Rows> and IDim<'Cols> and INumberBase<'Scalar>> =
        MatrixBase<'Scalar, 'Rows, 'Cols, HeapStorage<'Scalar>>

    type Vector<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        Matrix<'Scalar, 'Rows, D1>


    [<Extension>]
    type VectorExtensions<'Scalar, 'Rows when INumberBase<'Scalar> and IDim<'Rows>> =
        [<Extension>]
        static member Item(vec: Vector<'Scalar, 'Rows>, index: int) = &vec.AtRef(index, 0)


    let vectorX<'Scalar when INumberBase<'Scalar>> (dim: int) =
        new Vector<'Scalar, DX>(DX dim, D1())

    let matrixX<'Scalar when INumberBase<'Scalar>> (rows: int) (cols: int) =
        new Matrix<'Scalar, DX, DX>(DX rows, DX cols)
