using System.IO;
using UnityEngine;

public static class DirectoryUtils
{
    /// <summary>
    /// Creates a new directory. If true, the path is successfully created. If false, the path cannot be created and it is possible that the directory already exists.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static void CreateDirectory(string path, out DirectoryInfo directoryInfo)
    {
        try
        {
            if (Directory.Exists(path))
            {
                // The path already exists, just return it.
                directoryInfo = new DirectoryInfo(path);
                return;
            }

            // Path does not exist, create new directory
            directoryInfo = Directory.CreateDirectory(path);
        }
        catch (System.Exception ex)
        {
            if (Application.isPlaying)
                NativeWin32Alert.Error(ex.Message, "Critical Error");
            else
                Debug.LogError(ex.Message);

            directoryInfo = null;
        }
    }


    /// <summary>
    /// Returns the directory info of a path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DirectoryInfo GetDirectoryInfo(string path)
    {
        bool hr = Directory.Exists(path);
        if (hr)
            return new DirectoryInfo(path);
        return null;
    }
}
