namespace IG.HappyCoder.Dramework3.Runtime.Tools.Constants
{
    public static class ClassTemplates
    {
        #region ================================ FIELDS

        public const string EmptyClass = "#USINGS#" +
                                         "#NAMESPACE#" +
                                         "{\n" +
                                         "\tpublic static class #CLASSNAME#\n" +
                                         "\t{" +
                                         "#MEMBERS#\n" +
                                         "\t}\n" +
                                         "}";

        public const string EmptySubClass = "\n#TAB#public static class #CLASSNAME#\n" +
                                            "#TAB#{" +
                                            "#MEMBERS#\n" +
                                            "#TAB#}";

        public const string AssemblyInstaller = "using System.Diagnostics.CodeAnalysis;\n\n" +
                                                "using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation;\n" +
                                                "using IG.HappyCoder.Dramework3.Runtime.Container.Core;\n\n" +
                                                "#USINGS#\n" +
                                                "namespace #NAMESPACE#\n" +
                                                "{\n" +
                                                "\t[Installer(\"#SCENEID#\", #ORDER#, \"#MODULENAME#\")]\n" +
                                                "\t[SuppressMessage(\"ReSharper\", \"ClassNeverInstantiated.Global\")]\n" +
                                                "\t[SuppressMessage(\"ReSharper\", \"UnusedType.Global\")]\n" +
                                                "\t[SuppressMessage(\"ReSharper\", \"RedundantExplicitArrayCreation\")]\n" +
                                                "\tinternal class AssemblyInstaller : DInstaller\n" +
                                                "\t{\n" +
                                                "\t\tpublic override DInstallData[] CreateFactories()\n" +
                                                "\t\t{\n" +
                                                "\t\t\treturn new DInstallData[]\n" +
                                                "\t\t\t{\n" +
                                                "#FACTORIESDATA#" +
                                                "\t\t\t};\n" +
                                                "\t\t}\n\n" +
                                                "\t\tpublic override DInstallData[] CreateModels()\n" +
                                                "\t\t{\n" +
                                                "\t\t\treturn new DInstallData[]\n" +
                                                "\t\t\t{\n" +
                                                "#MODELSDATA#" +
                                                "\t\t\t};\n" +
                                                "\t\t}\n\n" +
                                                "\t\tpublic override DInstallData[] CreateSystems()\n" +
                                                "\t\t{\n" +
                                                "\t\t\treturn new DInstallData[]\n" +
                                                "\t\t\t{\n" +
                                                "#SYSTEMSDATA#" +
                                                "\t\t\t};\n" +
                                                "\t\t}\n" +
                                                "\t}\n" +
                                                "}";

        #endregion
    }
}