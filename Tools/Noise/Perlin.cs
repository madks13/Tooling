using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Metrics;
using Tools.Random;

namespace Tools.Noise
{
    /// Perlin Noise
    public class PerlinNoise : INoiseGenerator
    {
        /// Perlin Noise Constructot
        public PerlinNoise(int width, int height)
        {
            this._maxWidth = width;
            this._maxHeight = height;
        }

        public int _maxWidth = 256;
        public int _maxHeight = 256;

        /// Gets the value for a specific X and Y coordinate
        /// results in range [-1, 1] * maxHeight
        public float GetRandomHeight(float X, float Y, float MaxHeight,
            float Frequency, float Amplitude, float Persistance,
            int Octaves)
        {
            GenerateNoise();
            float FinalValue = 0.0f;
            for (int i = 0; i < Octaves; ++i)
            {
                FinalValue += GetSmoothNoise(X * Frequency, Y * Frequency) * Amplitude;
                Frequency *= 2.0f;
                Amplitude *= Persistance;
            }
            if (FinalValue < -1.0f)
            {
                FinalValue = -1.0f;
            }
            else if (FinalValue > 1.0f)
            {
                FinalValue = 1.0f;
            }
            return FinalValue * MaxHeight;
        }

        //This function is a simple bilinear filtering function which is good (and easy) enough.        
        private float GetSmoothNoise(float X, float Y)
        {
            float FractionX = X - (int)X;
            float FractionY = Y - (int)Y;
            int X1 = ((int)X + _maxWidth) % _maxWidth;
            int Y1 = ((int)Y + _maxHeight) % _maxHeight;

            //for cool art deco looking images, do +1 for X2 and Y2 instead of -1...

            int X2 = ((int)X + _maxWidth - 1) % _maxWidth;
            int Y2 = ((int)Y + _maxHeight - 1) % _maxHeight;
            float FinalValue = 0.0f;
            FinalValue += FractionX * FractionY * Noise[X1, Y1];
            FinalValue += FractionX * (1 - FractionY) * Noise[X1, Y2];
            FinalValue += (1 - FractionX) * FractionY * Noise[X2, Y1];
            FinalValue += (1 - FractionX) * (1 - FractionY) * Noise[X2, Y2];
            return FinalValue;
        }

        float[,] Noise;
        bool NoiseInitialized = false;

        /// create a array of randoms
        private void GenerateNoise()
        {
            if (NoiseInitialized)                //A boolean variable in the class to make sure we only do this once
                return;
            Noise = new float[_maxWidth, _maxHeight];    //Create the noise table where MAX_WIDTH and MAX_HEIGHT are set to some value>0            
            for (int x = 0; x < _maxWidth; ++x)
            {
                for (int y = 0; y < _maxHeight; ++y)
                {
                    Noise[x, y] = ((float)(StaticRandom.Instance.Random()) - 0.5f) * 2.0f;  //Generate noise between -1 and 1
                }
            }
            NoiseInitialized = true;
        }

        public float[,] Generate(ulong seed)
        {
            GenerateNoise();
            return Noise;
        }

        public float GetValueAtPoint(ICoordinates coords)
        {
            throw new NotImplementedException();
        }

        public float GetValueAtPoint(uint x, uint y)
        {
            throw new NotImplementedException();
        }
    }
}
