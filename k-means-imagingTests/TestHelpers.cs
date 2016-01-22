using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace k_means_imagingTests
{
    public class TestHelpers
    {
        public static void AssertEquals(double[,] expected, double[,] actual, double tolerance)
        {
            for(int i = 0; i < expected.GetLength(0); i++)
                for(int j = 0; j < expected.GetLength(1); j++)
                    Assert.AreEqual(expected[i, j], actual[i, j], tolerance);
        }
    }
}