
#if UNITY_IOS || UNITY_IPHONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS_I2Loc.Xcode;
using System.IO;
namespace Yodo1.Localization
{
	public class LanguagePostProcessBuild_IOS : Editor
	{
		[PostProcessBuild(20001)]
		public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
		{
			if (buildTarget != BuildTarget.iOS)
				return;

			List<LanguageSettingsData> languageSettingsDatas = LanguageSettings.Instance.GetLanguageSettingsData();
			if (languageSettingsDatas.Count < 1)
			{
				return;
			}
			try
			{
				List<string> langCodes = LanguageSettings.Instance.GetLanguageCodes();
				//----[ Export localized languages to the info.plist ]---------

				string plistPath = pathToBuiltProject + "/Info.plist";
				PlistDocument plist = new PlistDocument();
				plist.ReadFromString(File.ReadAllText(plistPath));

				PlistElementDict rootDict = plist.root;

				// Get Language root
				var langArray = rootDict.CreateArray("CFBundleLocalizations");

				// Set the Language Codes
				foreach (var code in langCodes)
				{
					if (code == null || code.Length < 2)
						continue;
					langArray.AddString(code);
				}

				rootDict.SetString("CFBundleDevelopmentRegion", langCodes[0]);

				// Write to file
				File.WriteAllText(plistPath, plist.WriteToString());

				//--[ Localize App Name ]----------

				string LocalizationRoot = pathToBuiltProject + "/I2Localization";
				if (!Directory.Exists(LocalizationRoot))
					Directory.CreateDirectory(LocalizationRoot);

				var project = new PBXProject();
				string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
				//if (!projPath.EndsWith("xcodeproj"))
				//projPath = projPath.Substring(0, projPath.LastIndexOfAny("/\\".ToCharArray()));

				project.ReadFromFile(projPath);
				//var targetName = PBXProject.GetUnityTargetName();
				//string projBuild = project.TargetGuidByName( targetName );
				project.RemoveLocalizationVariantGroup("I2 Localization");

				foreach (LanguageSettingsData languageSettingsData in languageSettingsDatas)
				{
					if (languageSettingsData == null)
					{
						continue;
					}
					AddLocalizableStrings(LocalizationRoot, project, languageSettingsData);
				}
				project.WriteToFile(projPath);

			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
			}
		}

		private static void AddLocalizableStrings(string LocalizationRoot, PBXProject project, LanguageSettingsData languageSettingsData)
		{
			string key = languageSettingsData.key;
			List<LanguageSettingsItemData> languageSettingsItemDatas = languageSettingsData.languages;

			if (languageSettingsItemDatas == null)
			{
				return;
			}

			foreach (LanguageSettingsItemData languageSettingsItem in languageSettingsItemDatas)
			{
				List<string> langCodes = LanguageUtils.GetLanguageCodes(languageSettingsItem.language);
				if (langCodes.Count < 1)
				{
					continue;
				}
				string content = languageSettingsItem.languageContent;
				foreach (var code in langCodes)
				{
					if (code == null || code.Length < 2)
						continue;

					var LanguageDirRoot = LocalizationRoot + "/" + code + ".lproj";
					if (!Directory.Exists(LanguageDirRoot))
						Directory.CreateDirectory(LanguageDirRoot);

					var infoPlistPath = LanguageDirRoot + "/InfoPlist.strings";
					List<string> dataList = new List<string>();
					if (File.Exists(infoPlistPath))
					{
						dataList.AddRange(File.ReadAllLines(infoPlistPath));
						if (dataList != null && dataList.Count > 0)
						{
							for (int index = 0; index < dataList.Count; ++index)
							{
								if (dataList[index].Contains(key))
								{
									dataList[index] = string.Format("{0} = \"{1}\";", key, content);
									continue;
								}
								dataList.Add(string.Format("{0} = \"{1}\";", key, content));
							}
						}
					}
					else
					{
						dataList.Add(string.Format("{0} = \"{1}\";", key, content));
					}
					File.WriteAllLines(infoPlistPath, dataList.ToArray());

					var langProjectRoot = "I2Localization/" + code + ".lproj";

					//var stringPaths = LanguageDirRoot + "/Localizable.strings";
					//File.WriteAllText(stringPaths, string.Empty);

					//project.AddLocalization(langProjectRoot + "/Localizable.strings", langProjectRoot + "/Localizable.strings", "I2 Localization");
					project.AddLocalization(langProjectRoot + "/InfoPlist.strings", langProjectRoot + "/InfoPlist.strings", "I2 Localization");
				}

			}
		}
	}
}
#endif
