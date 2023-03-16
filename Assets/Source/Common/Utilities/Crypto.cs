// Copyright 2018 Winterpetal. All Rights Reserved.
// Author: VinTK
using System.Security.Cryptography;
using System.Text;

public static class Crypto
{
    // Calculates a MD5 hash from a string
    public static string ToMD5(string input, bool bIsLowerCase)
    {
        // Return the MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // Convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        string capitalization = "X2";
        if (bIsLowerCase)
            capitalization = "x2";

        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString(capitalization));
        }

        return sb.ToString();
    }
}
