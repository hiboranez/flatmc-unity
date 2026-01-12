using System;

namespace Util {
    class PerlinNoiseGenerator
    {
        private int[] permutation;

        public PerlinNoiseGenerator(int seed)
        {
            permutation = GeneratePermutation(seed);
        }

        private int[] GeneratePermutation(int seed)
        {
            var random = new Random(seed);
            var permutation = new int[512];
            var p = new int[256];

            for (int i = 0; i < 256; i++)
            {
                p[i] = i;
            }

            for (int i = 0; i < 256; i++)
            {
                int j = random.Next(256 - i) + i;
                int temp = p[i];
                p[i] = p[j];
                p[j] = temp;
            }

            for (int i = 0; i < 512; i++)
            {
                permutation[i] = p[i & 255];
            }

            return permutation;
        }

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Grad(int hash, double x)
        {
            int h = hash & 15;
            double grad = 1 + (h & 7); // Gradient value 1-8
            if ((h & 8) != 0) grad = -grad; // Randomly invert half of the gradients
            return (grad * x);
        }

        public double PerlinNoise(double x)
        {
            // Determine grid cell coordinates
            int X = (int)Math.Floor(x) & 255;
            // Relative x coordinate within the cell
            x -= Math.Floor(x);

            // Compute fade curves for x
            double u = Fade(x);

            // Hash coordinates of the 2 cube corners
            int a = permutation[X];
            int b = permutation[X + 1];

            // And add blended results from 2 corners of the cube
            return Lerp(u, Grad(a, x), Grad(b, x - 1));
        }

        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }
    }
}