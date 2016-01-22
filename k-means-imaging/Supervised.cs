using Extensions;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Supervised
{
    public class KMeans
    {
        /// <summary>
        /// Randomly chooses "clusters" examples from the input to serve as the initial centroids
        /// </summary>
        /// <param name="input">Input matrix of examples</param>
        /// <param name="clusters">The number of clusters to return</param>
        protected static DenseMatrix InitializeClusters(Matrix<double> input, int clusters)
        {
            var indices = Combinatorics.GenerateVariation(input.RowCount, clusters);
            var result = new DenseMatrix(clusters, input.ColumnCount);

            for(int i = 0; i < indices.Count(); i++)
            {
                for(int j = 0; j < input.ColumnCount; j++)
                {
                    result[i, j] = input[indices[i], j];
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of the index of the closest centroid for each example in the input
        /// </summary>
        /// <param name="input">The input data</param>
        /// <param name="centroids">The centroids</param>
        protected static List<int> FindClosestCentroid(Matrix<double> input, Matrix<double> centroids)
        {
            var result = new List<int>(input.RowCount);
            foreach(var example in input.EnumerateRows())
            {
                // Calculate the distance from the centroids
                List<double> distances = new List<double>(centroids.RowCount);
                foreach(var centroid in centroids.EnumerateRows())
                    distances.Add((example - centroid).L2Norm());

                // Pick the centroid with the smallest distance
                result.Add(distances.MinIndex());
            }

            return result;
        }

        /// <summary>
        /// Runs the K-Means algorithm on the given input and returns the determined clusters
        /// </summary>
        /// <param name="input">The input data. Each row should represent an example</param>
        /// <param name="clusterCount">The hnumber of clusters to determine</param>
        /// <param name="maxIters">The maximum number of iterations to run the algorithm for</param>
        public static Tuple<double[,], int[]> Run(double[,] input, int clusterCount,
            bool scaleAndNormalize = false, int maxIters = 10)
        {
            var inputMatrix = DenseMatrix.OfArray(input);
            List<Tuple<double, double>> scale = null;
            if(scaleAndNormalize)
            {
                var scaleResult = ScaleAndNormalize(input);
                inputMatrix = scaleResult.Item1;
                scale = scaleResult.Item2;
            }

            var centroids = InitializeClusters(inputMatrix, clusterCount);
            List<int> indices = null;

            for(int i = 0; i < maxIters; i++)
            {
                // Step 1: Cluster Assignment
                indices = FindClosestCentroid(inputMatrix, centroids);

                // Step 2: Move Centroid
                List<List<Vector<double>>> clusters = new List<List<Vector<double>>>(clusterCount);
                for(int j = 0; j < clusterCount; j++)
                    clusters.Add(new List<Vector<double>>());

                // Aggregate based on assigned cluster
                for(int j = 0; j < indices.Count(); j++)
                    clusters[indices[j]].Add(inputMatrix.Row(j));

                // Compute mean fo reach cluster assignment
                for(int j = 0; j < clusterCount; j++)
                {
                    if(clusters[j].Count() > 0)
                    {
                        Vector<double> sum = new DenseVector(inputMatrix.ColumnCount);
                        clusters[j].ForEach(example => sum += example);
                        var mean = sum / clusters[j].Count();

                        for(int z = 0; z < inputMatrix.ColumnCount; z++)
                            centroids[j, z] = mean[z];
                    }
                }
            }

            // Denormalize the output
            if(scaleAndNormalize)
            {
                Denormalize(ref centroids, scale);
            }

            return Tuple.Create(centroids.ToArray(), indices.ToArray());
        }

        /// <summary>
        /// For each example x, normalizes and scales using the mean and standard deviation
        /// i.e. x -> (x - mean)/ std
        /// </summary>
        /// <param name="input">Input data to be normalized and scaled</param>
        /// <returns></returns>
        protected static Tuple<DenseMatrix, List<Tuple<double, double>>> ScaleAndNormalize(double[,] input)
        {
            var result = DenseMatrix.OfArray(input);
            var meansAndStds = new List<Tuple<double, double>>(input.GetLength(1));

            // Calculate mean and std for each column
            foreach(var column in result.EnumerateColumns())
                meansAndStds.Add(Statistics.MeanStandardDeviation(column));

            // Convert each column
            for(int i = 0; i < meansAndStds.Count(); i++)
            {
                var example = result.Column(i);
                var meanVector = DenseVector.Create(example.Count(), meansAndStds[i].Item1);

                var converted = (example - meanVector) / meansAndStds[i].Item2;
                for(int j = 0; j < converted.Count; j++)
                    result[j, i] = converted[j];
            }

            return Tuple.Create(result, meansAndStds);
        }

        /// <summary>
        /// Uses the computed means and standard deviations to rescale the matrix
        /// </summary>
        /// <param name="matrix">Input matrix to be denormalized</param>
        /// <param name="meansAndStds">List of means and standard deviations for each column</param>
        protected static void Denormalize(ref DenseMatrix matrix, List<Tuple<double, double>> meansAndStds)
        {
            for(int i = 0; i < matrix.ColumnCount; i++)
            {
                var update = (matrix.Column(i) * meansAndStds[i].Item2) + (DenseVector.Create(matrix.RowCount, meansAndStds[i].Item1));
                matrix.UpdateColumn(i, update);
            }
        }
    }
}