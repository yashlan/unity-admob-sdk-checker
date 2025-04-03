#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class CustomMenuAdmob
{
    private const string gitHubReleaseUrl = "https://api.github.com/repos/googleads/googleads-mobile-unity/releases/latest";
    private const string manifestDirectory = "Assets/GoogleMobileAds"; // Location of the manifest folder
    private const string manifestFilePrefix = "GoogleMobileAds_version-"; // Prefix of the manifest file
    private const string manifestFileSuffix = "_manifest"; // Suffix of the manifest file

    [MenuItem("Yashlan/Check Update Admob SDK", validate = false)] //rename it as you want "My Menu/Check Update Admob SDK" if needed
    private static void CheckUpdateAdmobSDK()
    {
        // Start the asynchronous request to get the latest version from GitHub
        EditorCoroutineUtility.StartCoroutineOwnerless(FetchLatestVersionFromGitHub());
    }

    private static string GetLocalAdMobVersion() //check the version we used right now
    {
        string version = "NOT FOUND VERSION";

        try
        {
            if (Directory.Exists(manifestDirectory))
            {
                string[] files = Directory.GetFiles(manifestDirectory, "*_manifest.txt");
                // Debug.Log($"Files found in directory: {string.Join(", ", files)}");

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        // Debug.Log($"Checking file: {fileName}");

                        if (fileName.StartsWith(manifestFilePrefix) && fileName.EndsWith(manifestFileSuffix))
                        {
                            version = "v" + fileName.Replace(manifestFilePrefix, "").Replace(manifestFileSuffix, "");
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("No AdMob manifest file found in the directory.");
                }
            }
            else
            {
                Debug.LogError($"Manifest directory not found: {manifestDirectory}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading manifest file: {ex.Message}");
        }

        return version;
    }

    private static IEnumerator FetchLatestVersionFromGitHub()
    {
        string usedVersion = GetLocalAdMobVersion();
        UnityWebRequest request = UnityWebRequest.Get(gitHubReleaseUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            string latestVersion = ParseVersionFromJson(jsonResponse);

            if (!string.IsNullOrEmpty(latestVersion))
            {
                // Debug.Log($"Latest AdMob SDK Version from GitHub: {latestVersion}");
                CompareVersions(usedVersion, latestVersion);
            }
            else
            {
                Debug.LogError("Failed to parse the latest version from GitHub.");
            }
        }
        else
        {
            Debug.LogError($"Failed to fetch data from GitHub: {request.error}");
        }
    }

    private static string ParseVersionFromJson(string jsonResponse)
    {
        try
        {
            // Parse the JSON response to extract the version number
            var json = JsonUtility.FromJson<GitHubReleaseResponse>(jsonResponse);
            return json.tag_name; // This will return the version tag
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing JSON response: {ex.Message}");
            return null;
        }
    }

    private static void CompareVersions(string currentVersion, string latestVersion)
    {
        if (currentVersion == "NOT FOUND VERSION")
        {
            Debug.LogError("Unable to find the current version of AdMob SDK.");
            return;
        }

        Version currentVer = new Version(currentVersion.Substring(1)); //use substring to remove 'v' char
        Version latestVer = new Version(latestVersion.Substring(1)); //use substring to remove 'v' char

        int comparisonResult = currentVer.CompareTo(latestVer);
        
        if (comparisonResult < 0)
        {
            string url = $"https://github.com/googleads/googleads-mobile-unity/releases/tag/{latestVersion}";
            Debug.LogWarning($"Your AdMob SDK version <b>{currentVersion}</b> is outdated. Please update to the latest version <b>{latestVersion}</b>.\nLink to update: <color=cyan><u>{url}</u></color>");

            bool download = EditorUtility.DisplayDialog(
                "Update Available",
                $"Your AdMob SDK version {currentVersion} is outdated. Do you want to download the latest version {latestVersion}?",
                "Yes", "Not Now"
            );

            if (download)
            {
                Application.OpenURL(url);
            }
        }
        else if (comparisonResult > 0)
        {
            Debug.Log($"Your AdMob SDK version <b>{currentVersion}</b> is ahead of the latest release. Consider checking if this is a custom or experimental version.");
        }
        else
        {
            Debug.Log($"Your AdMob SDK version <b>{currentVersion}</b> is up to date.");
        }
    }

    [Serializable]
    public class GitHubReleaseResponse
    {
        public string tag_name; // The version tag name (e.g., v9.5.0)
    }
}
#endif
