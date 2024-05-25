using System;
using System.Collections.Generic;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    internal interface IPlayerLoopConfig
    {
        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> SubsystemsByTypeFullNames { get; }
        Dictionary<Type, bool> SubsystemsByTypes { get; }

        #endregion
    }
}