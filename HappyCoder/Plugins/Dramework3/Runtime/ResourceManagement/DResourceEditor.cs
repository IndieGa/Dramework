#if UNITY_EDITOR


using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;


namespace IG.HappyCoder.Dramework3.Runtime.ResourceManagement
{
    internal partial class DResource
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal DResource(string sceneID, string groupID, string id, bool loadAtStart, Object asset)
        {
            _sceneID = sceneID;
            _groupID = groupID;
            _id = id;
            _loadAtStart = loadAtStart;

            switch (asset)
            {
                case ScriptableObject:
                    _assetType = ScriptableObjectType;
                    break;
                case Texture2D:
                {
                    var path = AssetDatabase.GetAssetPath(asset);
                    var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (importer != null)
                    {
                        _assetType = importer.textureType == TextureImporterType.Sprite ? SpriteType : TextureType;
                    }
                    break;
                }
                default:
                    _assetType = asset.GetType().Name;
                    break;
            }


            switch (_assetType)
            {
                case GameObjectType:
                    _assetReference = new AssetReferenceGameObject(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case ScriptableObjectType:
                    _assetReference = new AssetReferenceT<ScriptableObject>(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case MaterialType:
                    _assetReference = new AssetReferenceT<Material>(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case TextureType:
                    _assetReference = new AssetReferenceTexture2D(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case SpriteType:
                    _assetReference = new AssetReferenceSprite(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case SpriteAtlasType:
                    _assetReference = new AssetReferenceAtlasedSprite(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case AudioClipType:
                    _assetReference = new AssetReferenceT<AudioClip>(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
                case AudioMixerControllerType:
                    _assetReference = new AssetReferenceT<AudioMixer>(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());
                    break;
            }
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal Object EditorAsset => _assetReference.editorAsset;

        #endregion

        #region ================================ METHODS

        [FoldoutGroup("@Title")] [HorizontalGroup("@Title/Assets Reference", 80)]
        [Button("Select", ButtonHeight = 21)]
        private void SelectAsset()
        {
            Selection.activeObject = _assetReference.editorAsset;
        }

        #endregion
    }
}

#endif