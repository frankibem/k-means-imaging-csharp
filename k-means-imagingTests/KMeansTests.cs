using k_means_imagingTests;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Supervised.Tests
{
    [TestClass]
    public class KMeansTests : KMeans
    {
        Matrix<double> input = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0 },
                { 2, 2 },
                { 5, 5 },
                { 7, 7 },
            });

        [TestMethod]
        public void InitializeClustersTest()
        {
            int clusters = 2;
            var result = InitializeClusters(input, clusters);

            int row1 = (int)result[0, 0];
            int row2 = (int)result[1, 0];
            Assert.AreEqual(2, result.RowCount);
            Assert.AreEqual(row1, (int)result[0, 1]);
            Assert.AreEqual(row2, (int)result[1, 1]);
        }

        [TestMethod]
        public void FindClosestCentroidTest()
        {
            Matrix<double> centroids = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0 },
                { 7, 7 }
            });

            var expected = new List<int>(new int[] { 0, 0, 1, 1 });
            var actual = FindClosestCentroid(input, centroids);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RunTest()
        {
            var expectedCentroids = new double[,] { { 1, 1 }, { 6, 6 } };
            var expectedIndices = new List<int>(new int[] { 0, 0, 1, 1 });

            var result = KMeans.Run(input.ToArray(), 2);

            var actualCentroids = result.Item1;
            var actualIndices = result.Item2;

            Assert.AreEqual(expectedCentroids.Length, actualCentroids.Length);
            Assert.AreEqual(actualIndices.Length, actualIndices.Length);
            CollectionAssert.IsSubsetOf(actualCentroids, expectedCentroids);
            CollectionAssert.IsSubsetOf(expectedIndices, actualIndices);
        }

        [TestMethod]
        public void ScaleAndNormalizeTest()
        {
            var input = new double[,]
            {
                { 6, 9, 12 },
                { 1, 3, 7 },
                { 5, 8, 2 },
                { 11, 10, 4 }
            };

            var expected = new double[,]
            {
                { 0.060783 ,  0.482451 ,  1.322043 },
                { -1.154878,  -1.447352,   0.172440},
                { -0.182349,   0.160817,  -0.977162},
                {  1.276444 ,  0.804084 , -0.517321 },
            };

            var expectedMeansAndStds = new List<Tuple<double, double>>();
            expectedMeansAndStds.Add(Tuple.Create(5.7500, 4.1130));
            expectedMeansAndStds.Add(Tuple.Create(7.5, 3.1091));
            expectedMeansAndStds.Add(Tuple.Create(6.25, 4.3493));

            var result = ScaleAndNormalize(input);
            var actualMatrix = result.Item1.ToArray();
            var actualMeansAndStds = result.Item2;

            Assert.AreEqual(4, actualMatrix.GetLength(0));
            Assert.AreEqual(3, actualMatrix.GetLength(1));
            TestHelpers.AssertEquals(expected, actualMatrix, 0.000001);

            for(int i = 0; i < actualMeansAndStds.Count; i++)
            {
                Assert.AreEqual(actualMeansAndStds[i].Item1, expectedMeansAndStds[i].Item1, 0.0001);
                Assert.AreEqual(actualMeansAndStds[i].Item2, expectedMeansAndStds[i].Item2, 0.0001);
            }
        }

        [TestMethod]
        public void DenormalizeTest()
        {
            DenseMatrix input = DenseMatrix.OfArray(new double[,]
            {
                { 1.80987, -0.84646, -0.22455 },
                { -1.36534, 0.89181, -0.25964 },
                { 0.69854, 0.92959, 2.72272 }
            });

            var meansAndStds = new List<Tuple<double, double>>();
            meansAndStds.Add(Tuple.Create(26.2, 12.598));
            meansAndStds.Add(Tuple.Create(46.4, 26.463));
            meansAndStds.Add(Tuple.Create(19.4, 28.501));

            double[,] expected =
            {
                { 49, 24, 13 },
                { 9, 70, 12 },
                { 35, 71, 97 }
            };

            Denormalize(ref input, meansAndStds);
            for(int i = 0; i < input.RowCount; i++)
                for(int j = 0; j < input.ColumnCount; j++)
                    Assert.AreEqual(expected[i, j], input[i, j], 0.001);
        }
    }
}