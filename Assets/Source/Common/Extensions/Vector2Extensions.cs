// Copyright 2018 Winterpetal. All Rights Reserved.
// Author: VinTK
using UnityEngine;

namespace UnityEngine.Extensions
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Zeros the vector if its magnitude is smaller than smallMagnitude.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <param name="smallMagnitude">Small value.</param>
        public static void ZeroIfMagnitudeSmallerThan(this Vector2 vector, float smallMagnitude)
        {
            if (vector.magnitude < smallMagnitude)
            {
                vector = Vector2.zero;
            }
        }


        /// <summary>
        /// Zeros the vector if its square magnitude is smaller than smallSqrMagnitude.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <param name="smallMagnitude">Small value.</param>
        public static void ZeroIfSqrMagnitudeSmallerThan(this Vector2 vector, float smallSqrMagnitude)
        {
            if (vector.sqrMagnitude < smallSqrMagnitude)
            {
                vector = Vector2.zero;
            }
        }


        /// <summary>
        /// Checks if two Vector2s are similar.
        /// </summary>
        public static bool IsSimilar(this Vector2 a, Vector2 b)
        {
            Vector2 tmp = a - b;
            float dot = Vector2.Dot(tmp, tmp);
            float max = Mathf.Max(dot, 0.0f);
            float sqrt = Mathf.Sqrt(max);
            return sqrt < 1e-12f;
        }
    }
}
