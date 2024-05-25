using System;
using System.Collections.Generic;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Setting;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static partial class DBootstrap
    {
        #region ================================ FIELDS

        private static List<SetFieldData> _fieldsWithBeforeSceneLoadAttribute = new List<SetFieldData>();
        private static List<SetPropertyData> _propertiesWithBeforeSceneLoadAttribute = new List<SetPropertyData>();
        private static List<SetFieldData> _fieldsWithAfterSceneLoadAttribute = new List<SetFieldData>();
        private static List<SetPropertyData> _propertiesWithAfterSceneLoadAttribute = new List<SetPropertyData>();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static bool IsProjectStarted { get; private set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public static Queue<string> Logs { get; } = new Queue<string>();
        internal static IReadOnlyList<Type> AppTypes { get; private set; }

        #endregion

        #region ================================ METHODS

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            foreach (var fieldData in _fieldsWithAfterSceneLoadAttribute)
                fieldData.FieldInfo.SetValue(null, fieldData.Value);

            foreach (var propertyData in _propertiesWithAfterSceneLoadAttribute)
                propertyData.PropertyInfo.SetValue(null, propertyData.Value);
        }

        private static void InitializeAppTypes()
        {
            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (IsSystemAssembly(assembly) || IsUnityAssembly(assembly) || IsPluginAssembly(assembly)) continue;
                types.AddRange(assembly.GetTypes());
            }

            AppTypes = types;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            foreach (var fieldData in _fieldsWithBeforeSceneLoadAttribute)
                fieldData.FieldInfo.SetValue(null, fieldData.Value);

            foreach (var propertyData in _propertiesWithBeforeSceneLoadAttribute)
                propertyData.PropertyInfo.SetValue(null, propertyData.Value);

            DCore.Create();
        }

        private static bool IsFrameworkAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("IG.HappyCoder.Dramework3");
        }

        private static bool IsPluginAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Sirenix")
                   || assembly.FullName.StartsWith("UniTask")
                   || assembly.FullName.StartsWith("LiteDB")
                   || assembly.FullName.StartsWith("TexturePackerImporter");
        }

        private static bool IsProjectAssembly(Assembly assembly, string projectNamespaceRoot)
        {
            return assembly.FullName.StartsWith(projectNamespaceRoot);
        }

        private static bool IsProjectType(Type type, string projectNamespaceRoot)
        {
            return type != null && string.IsNullOrEmpty(type.FullName) == false && type.FullName.StartsWith(projectNamespaceRoot);
        }

        private static bool IsSystemAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("mscorlib")
                   || assembly.FullName.StartsWith("netstandard")
                   || assembly.FullName.StartsWith("System");
        }

        private static bool IsUnityAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Mono")
                   || assembly.FullName.StartsWith("Bee")
                   || assembly.FullName.StartsWith("Unity")
                   || assembly.FullName.StartsWith("PlayerBuildProgramLibrary")
                   || assembly.FullName.StartsWith("ScriptCompilationBuildProgram")
                   || assembly.FullName.StartsWith("AndroidPlayerBuildProgram")
                   || assembly.FullName.StartsWith("nunit.framework")
                   || assembly.FullName.StartsWith("NiceIO")
                   || assembly.FullName.StartsWith("Anonymously Hosted DynamicMethods Assembly")
                   || assembly.FullName.StartsWith("ExCSS")
                   || assembly.FullName.StartsWith("JetBrains")
                   || assembly.FullName.StartsWith("ExCSS")
                   || assembly.FullName.StartsWith("Newtonsoft");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            Application.logMessageReceived += OnLogMessageReceived;

            ModifyPlayerLoop();
            InitializeAppTypes();

            _fieldsWithBeforeSceneLoadAttribute = new List<SetFieldData>();
            _propertiesWithBeforeSceneLoadAttribute = new List<SetPropertyData>();
            _fieldsWithAfterSceneLoadAttribute = new List<SetFieldData>();
            _propertiesWithAfterSceneLoadAttribute = new List<SetPropertyData>();

            foreach (var type in AppTypes)
            {
                InitializeFieldsSetData(type);
                InitializePropertiesSetData(type);
            }

            IsProjectStarted = true;

            void InitializeFieldsSetData(IReflect type)
            {
                foreach (var fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var setAfterAssembliesLoadedAttribute = fieldInfo.GetCustomAttribute<SetAfterAssembliesLoadedAttribute>();
                    if (setAfterAssembliesLoadedAttribute != null)
                        fieldInfo.SetValue(null, setAfterAssembliesLoadedAttribute.Value);

                    var setBeforeSceneLoadAttribute = fieldInfo.GetCustomAttribute<SetBeforeSceneLoadAttribute>();
                    if (setBeforeSceneLoadAttribute != null)
                        _fieldsWithBeforeSceneLoadAttribute.Add(new SetFieldData(fieldInfo, setBeforeSceneLoadAttribute.Value));

                    var setAfterSceneLoadAttribute = fieldInfo.GetCustomAttribute<SetAfterSceneLoadAttribute>();
                    if (setAfterSceneLoadAttribute != null)
                        _fieldsWithAfterSceneLoadAttribute.Add(new SetFieldData(fieldInfo, setAfterSceneLoadAttribute.Value));
                }
            }

            void InitializePropertiesSetData(IReflect type)
            {
                foreach (var propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var setAfterAssembliesLoadedAttribute = propertyInfo.GetCustomAttribute<SetAfterAssembliesLoadedAttribute>();
                    if (setAfterAssembliesLoadedAttribute != null)
                        propertyInfo.SetValue(null, setAfterAssembliesLoadedAttribute.Value);

                    var setBeforeSceneLoadAttribute = propertyInfo.GetCustomAttribute<SetBeforeSceneLoadAttribute>();
                    if (setBeforeSceneLoadAttribute != null)
                        _propertiesWithBeforeSceneLoadAttribute.Add(new SetPropertyData(propertyInfo, setBeforeSceneLoadAttribute.Value));

                    var setAfterSceneLoadAttribute = propertyInfo.GetCustomAttribute<SetAfterSceneLoadAttribute>();
                    if (setAfterSceneLoadAttribute != null)
                        _propertiesWithAfterSceneLoadAttribute.Add(new SetPropertyData(propertyInfo, setAfterSceneLoadAttribute.Value));
                }
            }
        }

        private static void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            Logs.Enqueue($"{condition}"); //\n{stacktrace}");
        }

        #endregion
    }
}