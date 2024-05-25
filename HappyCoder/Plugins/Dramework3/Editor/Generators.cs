using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation;
using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection;
using IG.HappyCoder.Dramework3.Runtime.Container.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Newtonsoft.Json;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

using BindAttribute = IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection.BindAttribute;


namespace IG.HappyCoder.Dramework3.Editor
{
    public static class Generators
    {
        #region ================================ FIELDS

        private const string PATH_COMMON = "Assets/Plugins/Dramework 3/Runtime/Generated";
        private const string PATH_ANIMATORS = "Assets/Plugins/Dramework 3/Runtime/Generated/Animators";
        private const string NAMESPACE_ANIMATORS = "namespace IG.HappyCoder.Dramework3.Runtime.Generated.Animators";
        private const string PATH_INSTANCES = "Assets/Plugins/Dramework 3/Runtime/Generated/Instances";
        private const string NAMESPACE_INSTANCES = "namespace IG.HappyCoder.Dramework3.Runtime.Generated.Instances";
        private const string PATH_SCENES = "Assets/Plugins/Dramework 3/Runtime/Generated/Scenes";
        private const string NAMESPACE_SCENES = "namespace IG.HappyCoder.Dramework3.Runtime.Generated.Scenes";
        private const string PATH_ATLASES = "Assets/Plugins/Dramework 3/Runtime/Generated/Atlases";
        private const string NAMESPACE_ATLASES = "namespace IG.HappyCoder.Dramework3.Runtime.Generated.Atlases";

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string LastFoundBinderInstancesFilesKey => $"{Application.productName}_editor_last_found_binder_instances_files";
        private static string LastFoundInstancesFilesKey => $"{Application.productName}_editor_last_found_instances_files";
        private static string LastFoundScenesKey => $"{Application.productName}_editor_last_found_scenes";

        #endregion

        #region ================================ METHODS

        [MenuItem("Tools/Happy Coder/Dramework 3/Generation/Delete Assembly Installers _F8", false, 22)]
        private static void ClearAssemblyInstallers()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            DProjectConfigEditorProxy.ClearInstallers();
            var guids = AssetDatabase.FindAssets("AssemblyInstaller", new[] { "Assets" });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            foreach (var path in paths)
                AssetDatabase.DeleteAsset(path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        private static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path) == false) return;
            AssetDatabase.DeleteAsset(Helpers.IOTools.GetRelativePath(path));
        }

        private static void GenerateAnimatorsInfoFile()
        {
            var guids = AssetDatabase.FindAssets("t:AnimatorController", new[] { DProjectConfigEditorProxy.ProjectRootFolder });
            var animatorControllers = guids.Select(g => AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(g))).ToArray();

            if (animatorControllers.Length == 0)
            {
                DeleteDirectory(PATH_ANIMATORS);
                return;
            }

            var paths = new List<string>();
            foreach (var animatorController in animatorControllers)
            {
                var index = 0;
                var className = $"{Helpers.StringTools.ClearText(animatorController.name)}Animator";
                var layers = new HashSet<AnimatorControllerLayer>(animatorController.layers);
                var clips = new HashSet<AnimationClip>(animatorController.animationClips.OrderBy(c => c.name));
                var file = GetAnimatorInfoFile(NAMESPACE_ANIMATORS, className, clips, animatorController.parameters, layers.ToDictionary(layer => layer.name, _ => index++));
                var path = Path.Combine(PATH_ANIMATORS, $"{className}.cs");
                paths.Add(path);
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_ANIMATORS);
                File.WriteAllText(path, file);
            }

            var key = $"{Application.productName}_Dramework3_Animators_Info";
            var json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json) == false)
            {
                var lastPaths = JsonConvert.DeserializeObject<List<string>>(json);
                foreach (var path in lastPaths.Where(path => paths.Contains(path) == false && File.Exists(path)))
                    File.Delete(path);
            }

            json = JsonConvert.SerializeObject(paths);
            PlayerPrefs.SetString(key, json);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Generation/Generate Assembly Installers _F7", false, 21)]
        private static void GenerateAssemblyInstallers()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var scenes = Helpers.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { DProjectConfigEditorProxy.ProjectRootFolder })
                .OrderBy(s => s.name)
                .Select(s => s.name)
                .ToArray();

            foreach (var path in Helpers.EditorTools.GetAsmDefPaths(DProjectConfigEditorProxy.ProjectRootFolder))
            {
                var assemblyName = Path.GetFileNameWithoutExtension(path);
                var split = assemblyName.Split('-');
                if (split.Length < 3 || scenes.All(s => split[0].Split('_').All(sn => sn != s))) continue;

                var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
                if (assembly == null) continue;

                var sceneID = split[0];
                var installerNamespace = Path.GetDirectoryName(path)?
                    .Replace("Assets\\", "")
                    .Replace("Code\\", "")
                    .Replace("\\Code", "")
                    .Replace("Scripts\\", "")
                    .Replace("\\Scripts", "")
                    .Replace(" ", "")
                    .Replace("\\", ".");
                var usings = string.Empty;
                var modelsData = new List<(int order, string data)>();
                var factoriesData = new List<(int order, string data)>();
                var systemsData = new List<(int order, string data)>();

                foreach (var type in assembly.GetTypes())
                    InitializeUsingsAndData(type);

                if (modelsData.Count == 0 && factoriesData.Count == 0 && systemsData.Count == 0) continue;

                int.TryParse(split[1], out var order);
                var file = ClassTemplates.AssemblyInstaller
                    .Replace("#USINGS#", usings)
                    .Replace("#NAMESPACE#", installerNamespace)
                    .Replace("#SCENEID#", sceneID)
                    .Replace("#ORDER#", order.ToString())
                    .Replace("#MODULENAME#", split[2])
                    .Replace("#MODELSDATA#", GetData(modelsData))
                    .Replace("#FACTORIESDATA#", GetData(factoriesData))
                    .Replace("#SYSTEMSDATA#", GetData(systemsData));

                if (File.Exists(path) && File.ReadAllText(path) != file)
                    WriteAssemblyInstallerFile(Path.GetDirectoryName(path), file);

                continue;

                void InitializeUsingsAndData(Type type)
                {
                    var createAttribute = type.GetCustomAttribute<CreateAttribute>();
                    if (createAttribute == null) return;

                    var bindAttribute = type.GetCustomAttribute<BindAttribute>();

                    var constructorsInfo = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var constructor = constructorsInfo.FirstOrDefault();
                    if (constructor == null) return;
                    var constructorParams = constructor.GetParameters();
                    var parameters = string.Empty;
                    InitializeParameters();

                    if (usings.Contains("using UnityEngine;\n") == false)
                        usings = $"{usings}// ReSharper disable once RedundantUsingDirective\nusing UnityEngine;\n";

                    if (usings.Contains($"using {type.Namespace};\n") == false)
                        usings = $"{usings}using {type.Namespace};\n";

                    foreach (var param in constructorParams)
                    {
                        if (usings.Contains($"using {param.ParameterType.Namespace};\n") == false)
                            usings = $"{usings}using {param.ParameterType.Namespace};\n";
                    }

                    switch (createAttribute)
                    {
                        case CreateModelAttribute:
                            modelsData.Add((createAttribute.Order, $"\t\t\t\tnew DInstallData(new {type.Name}({parameters}), {(bindAttribute != null).ToString().ToLower()}, \"{bindAttribute?.InstanceID}\"),\n"));
                            break;
                        case CreateFactoryAttribute:
                            factoriesData.Add((createAttribute.Order, $"\t\t\t\tnew DInstallData(new {type.Name}({parameters}), {(bindAttribute != null).ToString().ToLower()}, \"{bindAttribute?.InstanceID}\"),\n"));
                            break;
                        case CreateSystemAttribute:
                            systemsData.Add((createAttribute.Order, $"\t\t\t\tnew DInstallData(new {type.Name}({parameters}), {(bindAttribute != null).ToString().ToLower()}, \"{bindAttribute?.InstanceID}\"),\n"));
                            break;
                    }
                    return;

                    void InitializeParameters()
                    {
                        for (var i = 0; i < constructorParams.Length; i++)
                        {
                            var asInstanceAttribute = constructorParams[i].GetCustomAttribute<InjectInstanceAttribute>();
                            var asResourceAttribute = constructorParams[i].GetCustomAttribute<InjectResourceAttribute>();
                            var asResourceGroupAttribute = constructorParams[i].GetCustomAttribute<InjectResourceGroupAttribute>();
                            var asAssetAttribute = constructorParams[i].GetCustomAttribute<InjectAssetAttribute>();
                            var asRemoteFileAttribute = constructorParams[i].GetCustomAttribute<InjectRemoteFileAttribute>();
                            var instantiateAssetAttribute = constructorParams[i].GetCustomAttribute<InstantiateAssetAttribute>();

                            if (asInstanceAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetObject<{constructorParams[i].ParameterType.Name}>(" +
                                             $"\"{asInstanceAttribute.InstanceID}\"){(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else if (asResourceAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetResource(" +
                                             $"typeof({type.Name}), " +
                                             $"\"{sceneID}\", " +
                                             $"\"{asResourceAttribute.GroupID}\", " +
                                             $"\"{asResourceAttribute.ResourceID}\")" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else if (asResourceGroupAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetResourceGroup(" +
                                             $"typeof({type.Name}), " +
                                             $"\"{sceneID}\", " +
                                             $"\"{asResourceGroupAttribute.GroupID}\")" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else if (asAssetAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetAsset<{constructorParams[i].ParameterType.Name}>(" +
                                             $"typeof({type.Name}), " +
                                             $"\"{sceneID}\", " +
                                             $"\"{asAssetAttribute.GroupID}\", " +
                                             $"\"{asAssetAttribute.ResourceID}\")" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else if (asRemoteFileAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetRemoteFile(" +
                                             $"\"{asRemoteFileAttribute.Filename}\")" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else if (instantiateAssetAttribute != null)
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tInstantiateAsset<{constructorParams[i].ParameterType.Name}>(" +
                                             $"typeof({type.Name}), " +
                                             $"\"{sceneID}\", " +
                                             $"\"{instantiateAssetAttribute.GroupID}\", " +
                                             $"\"{instantiateAssetAttribute.ResourceID}\", " +
                                             $"{instantiateAssetAttribute.Active.ToString().ToLower()})" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                            else
                            {
                                parameters = $"{parameters}\n\t\t\t\t\t\t\t\tGetObject<{constructorParams[i].ParameterType.Name}>()" +
                                             $"{(i < constructorParams.Length - 1 ? ", " : "")}";
                            }
                        }
                    }
                }

                string GetData(IReadOnlyList<(int order, string data)> list)
                {
                    var result = string.Empty;
                    list = list.OrderBy(i => i.order).ToArray();
                    return list.Aggregate(result, (current, item) => $"{current}{item.data}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateBindersFile()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var instances = scene.GetRootGameObjects()
                    .SelectMany(o => o.GetComponentsInChildren<DBinder>(true))
                    .Where(b => string.IsNullOrEmpty(b.InstanceID) == false)
                    .Select(b => b.InstanceID)
                    .OrderBy(o => o)
                    .ToArray();

                var className = $"{scene.name}Binders";
                var path = Path.Combine(PATH_INSTANCES, $"{className}.cs");

                if (instances.Length == 0)
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    continue;
                }

                var file = GetStringFile(NAMESPACE_INSTANCES, className, instances);
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_INSTANCES);
                File.WriteAllText(path, file);
            }
        }

        private static void GenerateClassInstancesFile()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
            var instances = new List<string>();

            foreach (var type in types)
            {
                var bindAttribute = type.GetCustomAttribute<BindAttribute>();
                if (bindAttribute == null || string.IsNullOrEmpty(bindAttribute.InstanceID)) continue;

                if (instances.Any(i => i == bindAttribute.InstanceID)) continue;
                instances.Add(bindAttribute.InstanceID);
            }

            const string className = "ClassInstanceID";
            var path = Path.Combine(PATH_INSTANCES, $"{className}.cs");
            if (instances.Count == 0)
            {
                if (File.Exists(path))
                    File.Delete(path);
                return;
            }

            instances = instances.OrderBy(i => i).ToList();
            var file = GetStringFile(NAMESPACE_INSTANCES, className, instances);
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            CreateDirectory(PATH_INSTANCES);
            File.WriteAllText(path, file);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Generation/Generate Identifiers _F6", false, 20)]
        private static void GenerateIdentifiers()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            GenerateSceneNamesFile();
            GenerateInstancesFiles();
            GenerateSpriteAtlasKeysFile();
            GenerateAnimatorsInfoFile();
            DProjectConfigEditorProxy.InitializeProjectSettings();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateInstancesFiles()
        {
            GenerateClassInstancesFile();
            GenerateBindersFile();
            if (Directory.Exists(PATH_INSTANCES) == false) return;
            var files = Directory.GetFiles(PATH_INSTANCES);
            if (files.Length > 0) return;
            DeleteDirectory(PATH_INSTANCES);
        }

        private static void GenerateSceneNamesFile()
        {
            var scenes = Helpers.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { DProjectConfigEditorProxy.ProjectRootFolder })
                .OrderBy(s => s.name)
                .ToArray();

            if (scenes.Length == 0) return;

            const string className = "SceneName";
            var sceneNames = new List<string>();
            foreach (var sceneName in scenes.Select(s => s.name))
            {
                if (sceneNames.Contains(sceneName)) continue;
                sceneNames.Add(sceneName);
            }
            var file = GetStringFile(NAMESPACE_SCENES, className, sceneNames);
            var path = Path.Combine(PATH_SCENES, $"{className}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == file) return;
            File.WriteAllText(path, file);
        }

        private static void GenerateSpriteAtlasKeysFile()
        {
            var spriteAtlases = Helpers.EditorTools.LoadAssets<SpriteAtlas>(string.Empty, new[] { DProjectConfigEditorProxy.ProjectRootFolder }).ToArray();

            if (spriteAtlases.Length == 0)
            {
                DeleteDirectory(PATH_ATLASES);
                return;
            }

            var paths = new List<string>();
            foreach (var spriteAtlas in spriteAtlases)
            {
                var sprites = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(sprites);
                sprites = sprites.OrderBy(s => s.name).ToArray();
                var className = $"{Helpers.StringTools.ClearText(spriteAtlas.name)}";
                var file = GetStringFile(NAMESPACE_ATLASES, className, sprites.Select(s => s.name.Replace("(Clone)", "")));
                var path = Path.Combine(PATH_ATLASES, $"{className}.cs");
                paths.Add(path);
                if (File.Exists(path) && File.ReadAllText(path) == file) continue;
                CreateDirectory(PATH_ATLASES);
                File.WriteAllText(path, file);
            }

            var key = $"{Application.productName}_Dramework3_Sprite_Atlas_Keys";
            var json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json) == false)
            {
                var lastPaths = JsonConvert.DeserializeObject<List<string>>(json);
                foreach (var path in lastPaths.Where(path => paths.Contains(path) == false && File.Exists(path)))
                    File.Delete(path);
            }

            json = JsonConvert.SerializeObject(paths);
            PlayerPrefs.SetString(key, json);
        }

        private static string GetAnimatorInfoFile(string nameSpace, string className, IReadOnlyCollection<AnimationClip> clips,
            IEnumerable<AnimatorControllerParameter> parameters, Dictionary<string, int> layers)
        {
            var hashes = clips.ToDictionary(clip => Helpers.StringTools.ClearText(clip.name), clip => Animator.StringToHash(clip.name));
            var clipsInfo = clips.ToDictionary(clip => clip.name, clip => clip.length);

            var file = "using System.Collections.Generic;\n\n";
            file += nameSpace;
            file += "\n{";

            file += $"\n\tpublic static class {className}";
            file += "\n\t{";

            file += "\n\t\tpublic static class Clips";
            file += "\n\t\t{";
            file = hashes.Aggregate(file, (current, pair) => current + $"\n\t\t\tpublic const int {Helpers.StringTools.ClearText(pair.Key)} = {pair.Value};");
            file += "\n\t\t}";

            file += "\n\n\t\tpublic static class Parameters";
            file += "\n\t\t{";
            file = parameters.Aggregate(file, (current, parameter) => current + $"\n\t\t\tpublic const int {Helpers.StringTools.ClearText(parameter.name)} = {parameter.nameHash};");
            file += "\n\t\t}";

            file += "\n\n\t\tpublic static class Layers";
            file += "\n\t\t{";
            file = layers.Aggregate(file, (current, pair) => current + $"\n\t\t\tpublic const int {Helpers.StringTools.ClearText(pair.Key)} = {pair.Value};");
            file += "\n\t\t}";

            file += "\n\n\t\tpublic enum ClipNameHash";
            file += "\n\t\t{";
            file = clips.Aggregate(file, (current, clip) => current + $"\n\t\t\t{Helpers.StringTools.ClearText(clip.name)} = {Animator.StringToHash(clip.name)},");
            file += "\n\t\t}";

            file += "\n\n\t\t public static float GetClipLength(string clipName)";
            file += "\n\t\t{";
            file += "\n\t\t\treturn ClipLengthByName[clipName];";
            file += "\n\t\t}";

            file += "\n\n\t\t public static float GetClipLength(int clipNameHash)";
            file += "\n\t\t{";
            file += "\n\t\t\treturn ClipLengthByNameHash[clipNameHash];";
            file += "\n\t\t}";

            file += "\n\n\t\t public static float GetClipLength(ClipNameHash clipNameHash)";
            file += "\n\t\t{";
            file += "\n\t\t\treturn ClipLengthByNameHash[(int) clipNameHash];";
            file += "\n\t\t}";

            file += "\n\n\t\t private static readonly Dictionary<string, float> ClipLengthByName = new Dictionary<string, float>";
            file += "\n\t\t{";
            file = clipsInfo.Aggregate(file, (current, clipInfo) => current + $"\n\t\t\t{{\"{clipInfo.Key}\", {clipInfo.Value.ToString(CultureInfo.InvariantCulture)}f}},");
            file += "\n\t\t};";

            file += "\n\n\t\t private static readonly Dictionary<int, float> ClipLengthByNameHash = new Dictionary<int, float>";
            file += "\n\t\t{";
            file = clipsInfo.Aggregate(file, (current, clipInfo) => current + $"\n\t\t\t{{{Animator.StringToHash(clipInfo.Key)}, {clipInfo.Value.ToString(CultureInfo.InvariantCulture)}f}},");
            file += "\n\t\t};";

            file += "\n\t}";

            file += "\n}";
            return file;
        }

        private static string GetStringFile(string nameSpace, string className, IEnumerable<string> fields)
        {
            var file = nameSpace;
            file += "\n{";
            file += $"\n\tpublic static class {className}";
            file += "\n\t{";
            file = fields.Aggregate(file, (current, field) => current + $"\n\t\tpublic const string {Helpers.StringTools.ClearText(field)} = \"{field}\";");
            file += "\n\t}";
            file += "\n}";
            return file;
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            CreateDirectory(PATH_COMMON);
            CreateDirectory(PATH_SCENES);
        }

        private static void WriteAssemblyInstallerFile(string path, string file)
        {
            var fullPath = Path.Combine(path, "AssemblyInstaller.cs");
            File.WriteAllText(fullPath, file);
        }

        #endregion
    }
}