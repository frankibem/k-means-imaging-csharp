using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;

namespace Extensions
{
    public static class MatrixExtensions
    {
        public static DenseMatrix UpdateColumn(this DenseMatrix matrix, int column, Vector<double> updates)
        {
            if(matrix.RowCount != updates.Count())
            {
                throw new ArgumentException("Column lengths must match");
            }

            if((column < 0) || (column >= matrix.ColumnCount))
            {
                throw new ArgumentException("column must be between 0 and less then the column count");
            }

            for(int i = 0; i < matrix.RowCount; i++)
            {
                matrix[i, column] = updates[i];
            }

            return matrix;
        }
    }
}