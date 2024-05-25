#if UNITY_EDITOR

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class AddressableExtensions
    {
        #region ================================ METHODS

        public static void AddAddressableAssetLabel(this Object source, string label)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return;
            var entry = source.GetAddressableAssetEntry();
            if (entry == null || entry.labels.Contains(label)) return;
            entry.labels.Add(label);
            AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.LabelAdded, entry, true);
        }

        public static AddressableAssetEntry GetAddressableAssetEntry(this Object source)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return null;
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            return addressableSettings.FindAssetEntry(sourceGuid);
        }

        public static HashSet<string> GetAddressableAssetLabels(this Object source)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return null;
            var entry = source.GetAddressableAssetEntry();
            return entry?.labels;
        }

        public static string GetAddressableAssetPath(this Object source)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return string.Empty;
            var entry = source.GetAddressableAssetEntry();
            return entry != null ? entry.address : string.Empty;
        }

        public static AddressableAssetGroup GetCurrentAddressableAssetGroup(this Object source)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return null;
            var entry = source.GetAddressableAssetEntry();
            return entry?.parentGroup;
        }

        public static bool IsInAddressableAssetGroup(this Object source, string groupName)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return false;
            var group = source.GetCurrentAddressableAssetGroup();
            return group != null && group.Name == groupName;
        }

        public static void RemoveAddressableAssetLabel(this Object source, string label)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return;
            var entry = source.GetAddressableAssetEntry();
            if (entry == null || entry.labels.Contains(label) == false) return;
            entry.labels.Remove(label);
            AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.LabelRemoved, entry, true);
        }

        public static void SetAddressableAssetAddress(this Object source, string address)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return;
            var entry = source.GetAddressableAssetEntry();
            if (entry == null) return;
            entry.address = address;
        }

        public static void SetAddressableAssetGroup(this Object source, string groupName)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return;
            var group = Helpers.Helpers.AddressablesTools.GroupExists(groupName) == false
                ? Helpers.Helpers.AddressablesTools.CreateGroup(groupName)
                : Helpers.Helpers.AddressablesTools.GetGroup(groupName);
            source.SetAddressableAssetGroup(group);
        }

        public static void SetAddressableAssetGroup(this Object source, AddressableAssetGroup group)
        {
            if (source == null || AssetDatabase.Contains(source) == false) return;
            var entry = source.GetAddressableAssetEntry();
            if (entry == null || source.IsInAddressableAssetGroup(group.Name)) return;
            entry.parentGroup = group;
        }

        #endregion
    }
}

#endif