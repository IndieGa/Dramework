using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Behaviours
{
    public class DBinder : DBehaviour
    {
        #region ================================ FIELDS

        [Indent] [FoldoutGroup("Components")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Component:")]
        [SerializeField]
        private Component _component;

        [Indent] [FoldoutGroup("Settings")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Instance ID:")]
        [SerializeField]
        private string _instanceID;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Component Component
        {
            get => _component;
            protected set => _component = value;
        }

        public string InstanceID
        {
            get => _instanceID;
            protected set => _instanceID = value;
        }

        #endregion
    }
}