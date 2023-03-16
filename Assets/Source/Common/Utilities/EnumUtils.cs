// Copyright 2018 Winterpetal. All Rights Reserved.
// Author: VinTK
using UnityEngine;
using System;

public static class EnumUtils
{
    // Returns the number of elements inside System.Enum
    public static int GetSystemEnumLength<T>()
    {
        int len = Enum.GetNames(typeof(T)).Length;
        return len;
    }


    // Converts an integer value into enum
    public static T IntToEnum<T>(int value)
    {
        T result = (T)Enum.Parse(typeof(T), value.ToString());
        if (result == null)
            throw new UnityException("A value cannot be evaluated into System.Enum type.");
        return result;
    }
}
