// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using UnityEngine;

namespace UnityEngine.Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 Left(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global left direction vector.");
                return Vector3.right * -1.0f;
            }

            return transform.right * -1.0f;
        }


        public static Vector3 Right(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global right direction vector.");
                return Vector3.right;
            }

            return transform.right;
        }


        public static Vector3 Up(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global up direction vector.");
                return Vector3.up;
            }

            return transform.up;
        }


        public static Vector3 Down(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global down direction vector.");
                return Vector2.up * -1.0f;
            }

            return transform.up * -1.0f;
        }


        public static Vector2 Left2D(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global left direction vector.");
                return Vector3.right * -1.0f;
            }

            Vector2 result = new Vector2(transform.right.x, transform.right.y) * -1.0f;
            return result;
        }


        public static Vector2 Right2D(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global right direction vector.");
                return Vector2.right;
            }

            Vector2 result = new Vector2(transform.right.x, transform.right.y);
            return result;
        }


        public static Vector2 Up2D(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global up direction vector.");
                return Vector2.up;
            }

            Vector2 result = new Vector2(transform.up.x, transform.up.y);
            return result;
        }


        public static Vector2 Down2D(this Transform transform)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null. Returning global down direction vector.");
                return Vector2.up * -1.0f;
            }

            Vector2 result = new Vector2(transform.up.x, transform.up.y) * -1.0f;
            return result;
        }
    }
}
