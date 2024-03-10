using System.IO;
using UnityEngine;

public static class StringExtension
{
    public static bool IsDirectory(this string self)
    {
        var fileInfo = new FileInfo(self);
        return (fileInfo.Attributes & FileAttributes.Directory) != 0;
    }

    public static string ToAssetPath(this string self)
    {
        string assetFullPath = Path.GetFullPath(Application.dataPath);

        return "Assets" + Path.GetFullPath(self).Substring(assetFullPath.Length);
    }
}