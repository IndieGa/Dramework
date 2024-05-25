using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InjectRemoteFileAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string Filename;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InjectRemoteFileAttribute(string filename)
        {
            Filename = filename;
        }

        #endregion
    }
}