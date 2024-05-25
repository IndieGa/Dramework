namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public class DAsmDef
    {
        #region ================================ PROPERTIES AND INDEXERS

        public bool allowUnsafeCode { get; set; }
        public bool autoReferenced { get; set; }
        public string[] defineConstraints { get; set; }
        public string[] excludePlatforms { get; set; }
        public string[] includePlatforms { get; set; }
        public string name { get; set; }
        public bool noEngineReferences { get; set; }
        public bool overrideReferences { get; set; }
        public string[] precompiledReferences { get; set; }
        public string[] references { get; set; }
        public string rootNamespace { get; set; }
        public string[] versionDefines { get; set; }

        #endregion
    }
}