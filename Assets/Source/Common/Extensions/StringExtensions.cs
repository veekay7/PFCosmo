// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
namespace UnityEngine.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrWhiteSpaceOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
    }
}
