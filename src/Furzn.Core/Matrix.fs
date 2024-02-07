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

    type MatrixX2<'Scalar when INumberBase<'Scalar>> = Matrix<'Scalar, DX, D2>
    type MatrixX3<'Scalar when INumberBase<'Scalar>> = Matrix<'Scalar, DX, D3>
    type MatrixX4<'Scalar when INumberBase<'Scalar>> = Matrix<'Scalar, DX, D4>

    type MatrixXi = MatrixX<int>
    type VectorXi = VectorX<int>
    type MatrixX2i = MatrixX2<int>
    type MatrixX3i = MatrixX3<int>
    type MatrixX4i = MatrixX4<int>

    type MatrixXd = MatrixX<double>
    type VectorXd = VectorX<double>
    type MatrixX2d = MatrixX2<double>
    type MatrixX3d = MatrixX3<double>
    type MatrixX4d = MatrixX4<double>


    let vectorX<'Scalar when INumberBase<'Scalar>> (dim: int) = new Vector<'Scalar, DX>(DX dim, D1)

    let matrixX<'Scalar when INumberBase<'Scalar>> (rows: int) (cols: int) =
        new Matrix<'Scalar, DX, DX>(DX rows, DX cols)

    let matrixX2<'Scalar when INumberBase<'Scalar>> (rows: int) =
        new Matrix<'Scalar, DX, D2>(DX rows, D2)

    let matrixX3<'Scalar when INumberBase<'Scalar>> (rows: int) =
        new Matrix<'Scalar, DX, D3>(DX rows, D3)

    let matrixX4<'Scalar when INumberBase<'Scalar>> (rows: int) =
        new Matrix<'Scalar, DX, D4>(DX rows, D4)

    [<Extension>]
    type MatrixExtensions =

        [<Extension>]
        static member AppendRow(mat: byref<Matrix<_, DX, _>>) =
            if mat.Rows * mat.Cols + mat.Cols > mat.Storage.Length then
                let newLength = max 1.0 (sqrt (double mat.Storage.Length)) |> ceil |> int
                mat.Reserve(newLength * newLength)

            mat.Resize(DX(mat.Rows + 1), mat.DimCols)


        [<Extension>]
        static member inline Resize(mat: byref<MatrixX<_>>, rows: int, cols) =
            mat.Resize(DX rows, DX cols)

        [<Extension>]
        static member inline Resize(mat: byref<MatrixX2<_>>, rows: int) = mat.Resize(DX rows, D2)

        [<Extension>]
        static member inline Resize(mat: byref<MatrixX3<_>>, rows: int) = mat.Resize(DX rows, D3)

        [<Extension>]
        static member inline Resize(mat: byref<MatrixX4<_>>, rows: int) = mat.Resize(DX rows, D4)
