1. 目前只支持IOS

2. Assets/Yodo1SDK_Extension/Editor/Localization/Localization.xlsx 为多语言配置excel，可以根据项目需求自行添加好减少。
 对应语的请到Assets/Yodo1SDK_Extension/Editor/Localization/LanguageUtils.cs里mLanguageDef查看可以支持的语言
 
3. CFBundleDisplayName 为IOS App_Name的key，如果想做多语言appname的支持请自行在Localization.xlsx表格里添加该key和多语言，
 目前配置表里只有ATT弹框的多语言，想添加其他的可以自行在Localization.xlsx表格里添加key和对应多语言

4. Assets/Yodo1SDK_Extension/Editor/Localization/LanguageSettings.asset 为使用的配置文件，修改后选择该文件-> Import Config 导入
配Excel表格
 
常见问题及解决方法：
Q: UnityEditor.iOS_I2Loc.Xcode.dll冲突报错如何解决？
A: 把Assets/Yodo1SDK_Extension/Editor/Localization/Library/UnityEditor.iOS_I2Loc.Xcode.dll删除，使用
   项目自带的即可。
   
Q: Assets/Yodo1SDK_Extension/Editor/Localization/Library/Net20冲突报错如何解决？
A: 把Assets/Yodo1SDK_Extension/Editor/Localization/Library/Net20删除，使用
   项目自带的即可。
   
Q: Assets/Yodo1SDK_Extension/Editor/Localization/Library/Net20导入UnityEditor报错如何解决？
A: 请参考Yodo1SDK 解决方法：https://confluence.yodo1.com/pages/viewpage.action?pageId=24064089。

欢迎各位大佬拍砖！（求大佬轻拍^_^），更欢迎各位大佬贡献和维护代码！