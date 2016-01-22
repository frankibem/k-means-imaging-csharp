using Algorithms;
using k_means_imaging.Imaging;
using System.Drawing;

namespace k_means_imaging.Examples
{
    public class Examples
    {
        public void Example1()
        {
            // Read in the image
            string imageLink = @"Input Image Link Here";
            Bitmap bitmap = new Bitmap(imageLink);

            // Map, run K-Means and recover image
            var mappedValues = KMeansAdapter.MapToKMeans(bitmap);
            var kMeansResult = KMeans.Run(mappedValues, 10, true, 10);
            var result = KMeansAdapter.RecoverFromKMeans(kMeansResult.Item1,
                kMeansResult.Item2, bitmap.Height, bitmap.Width);

            // Save the result
            result.Save(@"Output Image Link Here");
        }
    }
}