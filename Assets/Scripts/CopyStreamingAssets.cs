using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Copy contents of StreamingAssets to Application.persistentDataPath
/// </summary>
public class CopyStreamingAssets : MonoBehaviour
{
    private static readonly string[] FileList =
    {
        "index.html",
        "js/bootstrap.min.js",
        "js/sample.js",
        "css/bootstrap.min.css",
        "img/vittascience.svg",
    };

    private static readonly string RegExp = @"((href|src)='[a-z\/.-]+(.?))'".Replace("'", "\"");

    /// <summary>
    /// Log filenames to console on copy
    /// </summary>
    private const bool DebugMode = true;

    /// <summary>
    /// Directory name under StreamingAssets in which your files are stored
    /// </summary>
    private const string BaseDir = "webview-resources";

    public static CopyStreamingAssets Create()
    {
        return new GameObject
        {
            name = "[CopyStreamingAssets]",
            hideFlags = HideFlags.HideAndDontSave,
        }.AddComponent<CopyStreamingAssets>();
    }

    protected IEnumerator Start()
    {
        if (DebugMode)
        {
            Debug.Log("Copy streaming assets");
        }

        var basePath = Path.Combine(Application.streamingAssetsPath, BaseDir);
        foreach (var file in FileList)
        {
            var filePath = Path.Combine(basePath, file);
            var dstPath = filePath.Replace(Application.streamingAssetsPath, Application.persistentDataPath);
            var dstDir = Path.GetDirectoryName(dstPath);
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            if (filePath.Contains("://"))
            {
                var www = new WWW(filePath);
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarningFormat("failed to read file {0}: {1}", file, www.error);
                }
                else
                {
                    if (filePath.EndsWith(".html"))
                    {
                        File.WriteAllText(dstPath, AddUniqueSuffixToResources(www.text));
                    }
                    else
                    {
                        File.WriteAllBytes(dstPath, www.bytes);
                    }

                    if (DebugMode)
                    {
                        Debug.LogFormat("copy {0} to {1}", file, dstPath);
                    }
                }
            }
            else
            {
                try
                {
                    if (filePath.EndsWith(".html"))
                    {
                        var text = File.ReadAllText(filePath);
                        File.WriteAllText(dstPath, AddUniqueSuffixToResources(text));
                    }
                    else
                    {
                        File.Copy(filePath, dstPath, true);
                    }

                    if (DebugMode)
                    {
                        Debug.LogFormat("copy {0} to {1}", file, dstPath);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarningFormat("failed to read file {0}: {1}", file, e.Message);
                }
            }
        }
    }

    private string AddUniqueSuffixToResources(string html)
    {
#if UNITY_EDITOR
        var replacement = "$1?" + Random.value.ToString(CultureInfo.InvariantCulture) + "\"";
        return Regex.Replace(html, RegExp, replacement);
#else
        return html;
#endif
    }
}