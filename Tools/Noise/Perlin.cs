using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Metrics;
using Tools.Noise.Models;
using Tools.Random;

namespace Tools.Noise
{
    /// <summary>
    /// A speed-improved perlin and simplex noise algorithms for 2D.
    /// 
    /// Based on example code by Stefan Gustavson (stegu@itn.liu.se).
    /// Optimisations by Peter Eastman (peastman@drizzle.stanford.edu).
    /// Better rank ordering method by Stefan Gustavson in 2012.
    /// Converted to C# by Nikola Tomasevic.
    /// 
    /// Version 2016-04-20
    /// 
    /// This code was placed in the public domain by its original author,
    /// Stefan Gustavson. You may use it as you see fit, but
    /// attribution is appreciated.
    /// </summary>
    public class PerlinNoise : INoiseGenerator
    {
        #region Old

        ///// Perlin Noise Constructor
        //public PerlinNoise(int width, int height)
        //{
        //    this._maxWidth = width;
        //    this._maxHeight = height;
        //}

        //public int _maxWidth = 256;
        //public int _maxHeight = 256;

        ///// Gets the value for a specific X and Y coordinate
        ///// results in range [-1, 1] * maxHeight
        //public float GetRandomHeight(float X, float Y, float MaxHeight,
        //    float Frequency, float Amplitude, float Persistance,
        //    int Octaves)
        //{
        //    GenerateNoise();
        //    float FinalValue = 0.0f;
        //    for (int i = 0; i < Octaves; ++i)
        //    {
        //        FinalValue += GetSmoothNoise(X * Frequency, Y * Frequency) * Amplitude;
        //        Frequency *= 2.0f;
        //        Amplitude *= Persistance;
        //    }
        //    if (FinalValue < -1.0f)
        //    {
        //        FinalValue = -1.0f;
        //    }
        //    else if (FinalValue > 1.0f)
        //    {
        //        FinalValue = 1.0f;
        //    }
        //    return FinalValue * MaxHeight;
        //}

        ////This function is a simple bilinear filtering function which is good (and easy) enough.        
        //private float GetSmoothNoise(float X, float Y)
        //{
        //    float FractionX = X - (int)X;
        //    float FractionY = Y - (int)Y;
        //    int X1 = ((int)X + _maxWidth) % _maxWidth;
        //    int Y1 = ((int)Y + _maxHeight) % _maxHeight;

        //    //for cool art deco looking images, do +1 for X2 and Y2 instead of -1...

        //    int X2 = ((int)X + _maxWidth - 1) % _maxWidth;
        //    int Y2 = ((int)Y + _maxHeight - 1) % _maxHeight;
        //    float FinalValue = 0.0f;
        //    FinalValue += FractionX * FractionY * Noise[X1, Y1];
        //    FinalValue += FractionX * (1 - FractionY) * Noise[X1, Y2];
        //    FinalValue += (1 - FractionX) * FractionY * Noise[X2, Y1];
        //    FinalValue += (1 - FractionX) * (1 - FractionY) * Noise[X2, Y2];
        //    return FinalValue;
        //}

        //float[,] Noise;
        //bool NoiseInitialized = false;

        ///// create a array of randoms
        //private void GenerateNoise()
        //{
        //    if (NoiseInitialized)                //A boolean variable in the class to make sure we only do this once
        //        return;
        //    Noise = new float[_maxWidth, _maxHeight];    //Create the noise table where MAX_WIDTH and MAX_HEIGHT are set to some value>0            
        //    for (int x = 0; x < _maxWidth; ++x)
        //    {
        //        for (int y = 0; y < _maxHeight; ++y)
        //        {
        //            Noise[x, y] = ((float)(StaticRandom.Instance.Random()) - 0.5f) * 2.0f;  //Generate noise between -1 and 1
        //        }
        //    }
        //    NoiseInitialized = true;
        //}

        //public float[,] Generate(ulong seed)
        //{
        //    GenerateNoise();
        //    return Noise;
        //}

        //public float GetValueAtPoint(ICoordinates coords)
        //{
        //    throw new NotImplementedException();
        //}

        //public float GetValueAtPoint(uint x, uint y)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Fields

        Grad[] grad3 = new Grad[] { new Grad() {X = 1, Y = 1, Z = 0}, new Grad() {X = -1, Y = 1, Z = 0}, new Grad() {X = 1, Y = -1, Z = 0}, new Grad() {X = -1, Y = -1, Z = 0},
                                    new Grad() {X = 1, Y = 0, Z = 1}, new Grad() {X = -1, Y = 0, Z = 1}, new Grad() {X = 1, Y = 0, Z = -1}, new Grad() {X = -1, Y = 0, Z = -1},
                                    new Grad() {X = 0, Y = 1, Z = 1}, new Grad() {X = 0, Y = -1, Z = 1}, new Grad() {X = 0, Y = 1, Z = -1}, new Grad() {X = 0, Y = -1, Z = -1} };

        int[] p = new int[] {151, 160, 137, 91, 90, 15,
                    131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
                    190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
                    88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
                    77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244,
                    102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
                    135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
                    5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
                    223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
                    129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
                    251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
                    49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
                    138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180};

        // To remove the need for index wrapping, double the permutation table length
        int[] perm = new int[512];
        Grad[] gradP = new Grad[512];

        private int _seed = 0;

        #endregion

        #region Methods

        #region Private

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Lerp(double a, double b, double t)
        {
            return (1 - t) * a + t * b;
        }

        #endregion

        #region Public

        // This isn't a very good seeding function, but it works ok. It supports 2^16
        // different seed values. Write something better if you need more seeds.
        public void Seed(double seed)
        {
            //seed = Math.Abs(seed);

            if (seed > 0 && seed < 1)
            {
                // Scale the seed out
                seed *= Math.Pow(2, 16);
            }

            _seed = (int)Math.Floor(seed);

            if (seed < 256)
            {
                _seed |= _seed << 8;
            }

            for (var i = 0; i < 256; i++)
            {
                int v;
                if ((i & 1) == 1)
                {
                    v = p[i] ^ (_seed & 255);
                }
                else
                {
                    v = p[i] ^ ((_seed >> 8) & 255);
                }

                perm[i] = perm[i + 256] = v;
                gradP[i] = gradP[i + 256] = grad3[v % 12];
            }

            //for (var i = 0; i < 256; i++)
            //{
            //    perm[i] = perm[i + 256] = p[i];
            //    gradP[i] = gradP[i + 256] = grad3[perm[i] % 12];
            //}
        }
        
        public double GetValueAtPoint(ICoordinates coords)
        {
            return GetValueAtPoint(coords.X, coords.Y);
        }

        // 2D Perlin Noise
        public double GetValueAtPoint(double x, double y)
        {
            // Find unit grid cell containing point
            int X = (int)Math.Floor(x);
            int Y = (int)Math.Floor(y);
            // Get relative xy coordinates of point within that cell
            x = x - X; y = y - Y;
            // Wrap the integer cells at 255 (smaller integer period can be introduced here)
            X = X & 255; Y = Y & 255;

            // Calculate noise contributions from each of the four corners
            var n00 = gradP[X + perm[Y]].Dot2(x, y);
            var n01 = gradP[X + perm[Y + 1]].Dot2(x, y - 1);
            var n10 = gradP[X + 1 + perm[Y]].Dot2(x - 1, y);
            var n11 = gradP[X + 1 + perm[Y + 1]].Dot2(x - 1, y - 1);

            // Compute the fade curve value for x
            var u = Fade(x);

            // Interpolate the four results
            return Lerp(
                Lerp(n00, n10, u),
                Lerp(n01, n11, u),
               Fade(y));
        }

        // 3D Perlin Noise
        public double GetValueAtPoint(double x, double y, double z)
        {
            // Find unit grid cell containing point
            int X = (int)Math.Floor(x);
            int Y = (int)Math.Floor(y);
            int Z = (int)Math.Floor(z);
            // Get relative xyz coordinates of point within that cell
            x = x - X; y = y - Y; z = z - Z;
            // Wrap the integer cells at 255 (smaller integer period can be introduced here)
            X = X & 255; Y = Y & 255; Z = Z & 255;

            // Calculate noise contributions from each of the eight corners
            var n000 = gradP[X + perm[Y + perm[Z]]].Dot3(x, y, z);
            var n001 = gradP[X + perm[Y + perm[Z + 1]]].Dot3(x, y, z - 1);
            var n010 = gradP[X + perm[Y + 1 + perm[Z]]].Dot3(x, y - 1, z);
            var n011 = gradP[X + perm[Y + 1 + perm[Z + 1]]].Dot3(x, y - 1, z - 1);
            var n100 = gradP[X + 1 + perm[Y + perm[Z]]].Dot3(x - 1, y, z);
            var n101 = gradP[X + 1 + perm[Y + perm[Z + 1]]].Dot3(x - 1, y, z - 1);
            var n110 = gradP[X + 1 + perm[Y + 1 + perm[Z]]].Dot3(x - 1, y - 1, z);
            var n111 = gradP[X + 1 + perm[Y + 1 + perm[Z + 1]]].Dot3(x - 1, y - 1, z - 1);

            // Compute the fade curve value for x, y, z
            var u = Fade(x);
            var v = Fade(y);
            var w = Fade(z);

            // Interpolate
            return Lerp(
                Lerp(
                  Lerp(n000, n100, u),
                  Lerp(n001, n101, u), w),
                Lerp(
                  Lerp(n010, n110, u),
                  Lerp(n011, n111, u), w),
               v);
        }

        #endregion

        #endregion
    }
}
