using System;
using System.Collections.Generic;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.RemoteFiles
{
    [Serializable]
    public class RemoteFileSceneData
    {
        #region ================================ FIELDS

        [LabelWidth(ConstantValues.Int_80)] [LabelText("Scene ID:")]
        [SerializeField] [HideLabel]
        private string _sceneID;

        [SerializeField]
        private List<string> _urls;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string SceneID
        {
            get => _sceneID;
            set => _sceneID = value;
        }

        public List<string> Urls
        {
            get => _urls;
            set => _urls = value;
        }

        #endregion
    }
}