// Copyright 2019 Winterpetal. All Rights Reserved.
// Referenced from: https://gist.github.com/roydejong/130a91e1835154a3acaeda78c9dfbbd7
using System;
using System.Runtime.InteropServices;

/// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-messagebox</see>
public static class NativeWin32Alert
{
    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    public static System.IntPtr GetWindowHandle()
    {
        return GetActiveWindow();
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern int MessageBox(IntPtr hwnd, String lpText, String lpCaption, uint uType);


    /// <summary>
    /// Shows Error alert box with OK button.
    /// </summary>
    /// <param name="text">Main alert text / content.</param>
    /// <param name="caption">Message box title.</param>
    public static void Error(string text, string caption)
    {
        try
        {
            MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000000L | 0x00000010L));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
