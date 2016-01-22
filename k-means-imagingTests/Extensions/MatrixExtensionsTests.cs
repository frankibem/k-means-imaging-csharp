using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Extensions.Tests
{
    [TestClass]
    public class MatrixExtensionsTests
    {
        [TestMethod]
        public void UpdateColumnTest()
        {
            DenseMatrix input = DenseMatrix.OfArray(new double[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            });
            var expected = new double[,]
            {
                { 12, 2, 3 },
                { 13, 5, 6 },
                { 14, 8, 9 }
            };

            input.UpdateColumn(0, DenseVector.OfArray(new double[] { 12, 13, 14 }));

            CollectionAssert.AreEqual(expected, input.ToArray());
        }
    }
}