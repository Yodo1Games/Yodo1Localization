using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PackerEditor : Editor
{
    static string ArchivePath { get { return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Archive"); } }

    [MenuItem("Yodo1/Tools/Release UnityPackage")]
    private static string OnExportUnityPackage()
    {
        //var fileName = K_SDK_PACKAGE_NAME + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_hh") + ".unitypackage";
        var fileName = "Yodo1_Localization.unitypackage";

        // 在这里加入需要打入的目录路径, 日后方便扩展 -- by Eric, 2020-10-09
        string[] assetPathNames = new string[]{
                "Assets/Yodo1SDK_Extension/Editor/Localization" ,
            };

        AssetDatabase.ExportPackage(assetPathNames, fileName, ExportPackageOptions.Recurse);

        var rootDir = Directory.GetParent(Application.dataPath).FullName;
        if (!Directory.Exists(ArchivePath)) Directory.CreateDirectory(ArchivePath);

        string from = Path.Combine(rootDir, fileName);
        string to = Path.Combine(ArchivePath, fileName);

        if (File.Exists(to))
        {
            File.Delete(to);
        }
        File.Move(from, to);

        return to;
    }
}
