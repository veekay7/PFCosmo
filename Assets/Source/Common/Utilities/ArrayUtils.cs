// Copyright 2018 Winterpetal. All Rights Reserved.
// Author: VinTK
using System;

public static class ArrayUtils
{
    // Gets the index from the inputted X and Y coordinates
    public static int GetIndex(int x, int y, int width)
    {
        return x + y * width;
    }

    // Random array shuffle using Fisher-Yates algorithm
    // Retrieved from: https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    // Answerer: Matt Howells
    public static void ShuffleArray<T>(this Random rand, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rand.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }


    public static T GetRandomElement<T>(this Random rand, T[] array)
    {
        int idx = rand.Next(0, array.Length);
        return array[idx];
    }
}
