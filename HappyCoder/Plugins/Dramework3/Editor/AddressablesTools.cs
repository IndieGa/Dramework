using System.Collections.Generic;
using System.IO;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Callbacks;


namespace IG.HappyCoder.Dramework3.Editor
{
    public static class AddressablesTools
    {
        #region ================================ FIELDS

        private const string DIRECTORY = "Assets/Plugins/Dramework 3/Editor/Code";
        private const string ADDRESSABLES_DATA_FILENAME = "AddressablesData.cs";
        private const string ASSEMBLY_DEFINE_FILENAME = "IG.HappyCoder.Dramework3.Generated.asmdef";

        #endregion

        #region ================================ METHODS

        private static void GenerateAddressablesDataClass()
        {
            var classText = "";
            classText += "namespace IG.Happy_Coder.Dramework_2.Editor\n";
            classText += "{\n";
            classText += "\tpublic static class AddressablesData\n";
            classText += "\t{\n";

            foreach (var assetGroup in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                classText += $"\t\tpublic static class {Helpers.StringTools.ClearText(assetGroup.Name)}\n";
                classText += "\t\t{\n";
                foreach (var entry in assetGroup.entries.Where(e => e.IsFolder == false))
                {
                    classText += $"\t\t\tpublic const string {Helpers.StringTools.ClearText(entry.ToString())} = \"{entry}\";\n";
                }
                classText += "\t\t}\n";
            }

            classText += "\t}\n";
            classText += "}\n\n";

            if (Directory.Exists(DIRECTORY) == false)
                Directory.CreateDirectory(DIRECTORY);

            var fullPath = Path.Combine(DIRECTORY, ADDRESSABLES_DATA_FILENAME);
            File.WriteAllText(fullPath, classText);
        }

        private static void GenerateAssemblyDefineFile()
        {
            var fileText = "{\n\"name\": \"IG.HappyCoder.Dramework3.Generated\"\n}";
            var fullPath = Path.Combine(DIRECTORY, ASSEMBLY_DEFINE_FILENAME);
            if (File.Exists(fullPath)) return;
            File.WriteAllText(fullPath, fileText);
        }

        [DidReloadScripts]
        private static void Init()
        {
            // if (AddressableAssetSettingsDefaultObject.Settings == null) return;
            // AddressableAssetSettingsDefaultObject.Settings.OnModification -= OnAddressablesModification;
            // AddressableAssetSettingsDefaultObject.Settings.OnModification += OnAddressablesModification;
        }

        private static void OnAddressablesModification(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent evn, object obj)
        {
            if (evn != AddressableAssetSettings.ModificationEvent.GroupAdded
                && evn != AddressableAssetSettings.ModificationEvent.GroupMoved
                && evn != AddressableAssetSettings.ModificationEvent.GroupRenamed
                && evn != AddressableAssetSettings.ModificationEvent.GroupRemoved
                && evn != AddressableAssetSettings.ModificationEvent.EntryAdded
                && evn != AddressableAssetSettings.ModificationEvent.EntryCreated
                && evn != AddressableAssetSettings.ModificationEvent.EntryModified
                && evn != AddressableAssetSettings.ModificationEvent.EntryMoved
                && evn != AddressableAssetSettings.ModificationEvent.EntryRemoved) return;


            GenerateAddressablesDataClass();
            GenerateAssemblyDefineFile();
            AssetDatabase.Refresh();
        }

        private static void OnValidate()
        {
            GenerateAddressablesDataClass();
            GenerateAssemblyDefineFile();
        }

        #endregion
    }

    public static class AddressablesInfo
    {
        #region ================================ PROPERTIES AND INDEXERS

        public static IEnumerable<string> Groups => AddressableAssetSettingsDefaultObject.Settings.groups.Select(g => Helpers.StringTools.ClearText(g.Name));

        #endregion

        #region ================================ METHODS

        public static IEnumerable<string> GetKeys(string groupName)
        {
            var keys = new List<string> { "None" };

            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                ConsoleLogger.LogError($"Addressables group {groupName} is not found");
                return keys;
            }

            keys = group.entries.Select(e => e.ToString()).ToList();
            return keys;
        }

        #endregion
    }
}