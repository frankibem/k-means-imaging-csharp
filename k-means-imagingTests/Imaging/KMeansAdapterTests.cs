using Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace k_means_imaging.Imaging.Tests
{
    [TestClass]
    public class KMeansAdapterTests
    {
        [TestMethod]
        public void MapToKMeansTest()
        {
            Bitmap input = new Bitmap(3, 2);
            // Initialize pixels
            input.SetPixel(0, 0, Color.FromArgb(60, 23, 250));
            input.SetPixel(1, 0, Color.FromArgb(93, 61, 247));
            input.SetPixel(2, 0, Color.FromArgb(0, 0, 0));
            input.SetPixel(0, 1, Color.FromArgb(10, 11, 30));
            input.SetPixel(1, 1, Color.FromArgb(200, 14, 9));
            input.SetPixel(2, 1, Color.FromArgb(16, 250, 255));

            var expected = new double[,]
            {
                { 60, 23, 250 },
                { 93, 61, 247 },
                { 0, 0, 0 },
                { 10, 11, 30 },
                { 200, 14, 9 },
                { 16, 250, 255 }
            };

            var actual = KMeansAdapter.MapToKMeans(input);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecoverFromKMeansTest()
        {
            var input = new double[,]
            {
                { 0, 0, 0 },
                { 2, 2, 2 },
                { 5, 5, 5 },
                { 7, 7, 7 },
            };

            var result = KMeans.Run(input, 2, false, 5);
            var expected = new double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 6, 6, 6 }, { 6, 6, 6 } };

            var actualBitmap = KMeansAdapter.RecoverFromKMeans(result.Item1, result.Item2, 2, 2);
            Assert.AreEqual(2, actualBitmap.Height);
            Assert.AreEqual(2, actualBitmap.Width);

            var actualArray = new double[4, 3];
            for(int i = 0; i < actualBitmap.Width * actualBitmap.Height; i++)
            {
                var x = i % actualBitmap.Width;
                var y = (i - x) / actualBitmap.Width;

                actualArray[i, 0] = actualBitmap.GetPixel(x, y).R;
                actualArray[i, 1] = actualBitmap.GetPixel(x, y).G;
                actualArray[i, 2] = actualBitmap.GetPixel(x, y).B;
            }

            CollectionAssert.AreEqual(expected, actualArray);
        }
    }
}