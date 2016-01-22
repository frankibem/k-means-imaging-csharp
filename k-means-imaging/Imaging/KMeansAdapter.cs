using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k_means_imaging.Imaging
{
    public class KMeansAdapter
    {
        /// <summary>
        /// Maps the bitmap to a Matrix which can be used by the K-Means algorithm
        /// </summary>
        /// <param name="bitmap">The bitmap to be converted</param>
        public static double[,] MapToKMeans(Bitmap bitmap)
        {
            int pixels = bitmap.Height * bitmap.Width;
            var result = new double[pixels, 3];

            for(int i = 0; i < pixels; i++)
            {
                var x = i % bitmap.Width;
                var y = (i - x) / bitmap.Width;
                result[i, 0] = bitmap.GetPixel(x, y).R;
                result[i, 1] = bitmap.GetPixel(x, y).G;
                result[i, 2] = bitmap.GetPixel(x, y).B;
            }
            return result;
        }

        /// <summary>
        /// Converts the result from K-Means back a bitmap
        /// </summary>
        /// <param name="centroids">Clustered determined from K-Means</param>
        /// <param name="indices">For each mapped index, the indices[i] is the appropriate cluster</param>
        /// <param name="height">Vertical height of the image in pixels</param>
        /// <param name="width">Horizontal width of the image in pixels</param>
        public static Bitmap RecoverFromKMeans(double[,] centroids, int[] indices, int height, int width)
        {
            var result = new Bitmap(width, height);

            for(int i = 0; i < indices.Count(); i++)
            {
                var x = i % width;
                var y = (i - x) / width;
                result.SetPixel(x, y,
                    Color.FromArgb((int)centroids[indices[i], 0],
                    (int)centroids[indices[i], 1],
                    (int)centroids[indices[i], 2]));
            }

            return result;
        }

    }
}