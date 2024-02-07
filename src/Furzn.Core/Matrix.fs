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

    type MatrixX<'Scalar when INumberBase<'Scalar>> = Matrix<'Scalar, DX, DX>
    type VectorX<'Scalar when INumberBase<'Scalar>> = Vector<'Scalar, DX>

    type MatrixXi = MatrixX<int>
    type VectorXi = VectorX<int>
    type MatrixXd = MatrixX<double>
    type VectorXd = VectorX<double>

    let vectorX<'Scalar when INumberBase<'Scalar>> (dim: int) = new Vector<'Scalar, DX>(DX dim, D1)

    let matrixX<'Scalar when INumberBase<'Scalar>> (rows: int) (cols: int) =
        new Matrix<'Scalar, DX, DX>(DX rows, DX cols)

    [<Extension>]
    type MatrixExtensions =
        [<Extension>]
        static member inline Resize(mat: inref<MatrixX<_>>, rows: int, cols) =
            mat.Resize(DX rows, DX cols)
