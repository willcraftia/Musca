#region Using

using System;

#endregion

namespace Musca
{
    internal static class MathHelper
    {
        public static float Clamp(float value, float min, float max)
        {
            if (max < value) return max;
            if (value < min) return min;
            return value;
        }

        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        public static int Floor(float value)
        {
            return 0 <= value ? (int) value : (int) (value - 1);
        }

        public static float Abs(float value)
        {
            return 0 <= value ? value : -value;
        }

        public static float CalculateGradient(int hash, float x, float y, float z)
        {
            // Convert low 4 bits of hash code into 12 simple
            int h = hash & 15;
            // gradient directions, and compute dot product.
            float u = h < 8 ? x : y;
            // Fix repeats at h = 12 to 15
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}
