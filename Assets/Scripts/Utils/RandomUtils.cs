using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Castle.CustomUtil {
    public static class RandomUtils
    {
        private static Random rng = new Random();

        /// <summary>
        /// Get random value in range
        /// exclude max
        /// Exp: Range(0, 10) will return value from 0-9
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            return rng.Next(min, max);
        }
        
        public static float Range(float min, float max)
        {
            return (float) (rng.NextDouble() * (max - min) + min);
        }
    }
}

