// Copyright 2018 VinTK. All Rights Reserved.
// Author: VinTK
using System;
using System.Text.RegularExpressions;

public static class Validators
{
    /// <summary>
    /// Retrieved from: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsInvalidEmail(string email)
    {
        if (String.IsNullOrEmpty(email))
            return false;

        bool hr = Regex.IsMatch(
                email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase);
        if (!hr)
            return true;
        return false;
    }


    /// <summary>
    /// Checks if string is empty
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsStringEmpty(string str)
    {
        if (String.IsNullOrEmpty(str))
            return true;
        return false;
    }


    /// <summary>
    /// Is the string above the minimum character count?
    /// </summary>
    /// <param name="str"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static bool NotBelowMinLength(string str, int min)
    {
        int charcterCount = str.Length;
        if (charcterCount < min)
            return true;
        return false;
    }


    /// <summary>
    /// Has the string exceeded the maximum allowed character count?
    /// </summary>
    /// <param name="str"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool HasExceedMaxLength(string str, int max)
    {
        int characterCount = str.Length;
        if (characterCount > max)
            return false;
        return true;
    }


    /// <summary>
    /// Are both strings equal?
    /// </summary>
    /// <param name="pass0"></param>
    /// <param name="pass1"></param>
    /// <returns></returns>
    public static bool ArePasswordsNotEqual(string pass0, string pass1)
    {
        if (String.Equals(pass0, pass1))
            return false;
        return true;
    }
}
