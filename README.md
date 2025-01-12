# **Check AdMob SDK Update for Unity**

A Unity Editor script to check for the latest version of the AdMob SDK using GitHub's API. This tool simplifies the process of ensuring your AdMob SDK is up-to-date by comparing the locally installed version with the latest release on GitHub.

<div align="center">
  <img src="https://github.com/yashlan/unity-admob-sdk-checker/blob/main/ss/ss_confirm_window.png" width="450" />
</div>

---

## **Features**
- Automatically detects the local AdMob SDK version based on manifest files.
- Fetches the latest version of the AdMob SDK from the GitHub repository.
- Compares the local version with the latest release:
  - Displays a warning if the version is outdated.
  - Provides an option to open the download page for the latest version.
  - Notifies if the local version is ahead of the release or already up to date.
- Integrates seamlessly into the Unity Editor with a customizable menu item.

---

## **Setup Instructions**
1. Download file `CustomMenuAdmob.cs`.
2. Add the `CustomMenuAdmob.cs` script to your Unity project under the `Editor` folder (`Assets/Editor`).
3. Ensure your AdMob SDK is installed.

---

## **Usage**
1. Open Unity Editor.
2. Go to the menu bar and select **My Menu > Check Update Admob SDK** (or the menu name you've chosen in the script).
3. The script will:
   - Fetch the latest AdMob SDK version from GitHub.
   - Compare it with the currently installed version.
   - Display the result in the Unity console.
   - If outdated, it shows a confirmation dialog with a link to update.

<img src="https://github.com/yashlan/unity-admob-sdk-checker/blob/main/ss/ss_console.png" width="700" />

---

## **Customizing the Menu**
<img src="https://github.com/yashlan/unity-admob-sdk-checker/blob/main/ss/ss_menu_item.png" width="350" />

The menu name can be changed by modifying the `MenuItem` attribute in this section's script.  
Here's the code:  
```csharp
[MenuItem("My Menu/Check Update Admob SDK", validate = false, priority = 1)] //line 16 in CustomMenuAdmob.cs script

