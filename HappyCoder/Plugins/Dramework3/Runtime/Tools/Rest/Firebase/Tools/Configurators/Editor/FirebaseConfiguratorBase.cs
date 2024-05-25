using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Editor
{
    public class FirebaseConfiguratorBase : ScriptableObject
    {
        #region ================================ FIELDS

        protected bool LogEnabled;

        #endregion

        #region ================================ METHODS

        [ContextMenu("Configurator/Log")]
        private void SwitchLog()
        {
            LogEnabled = !LogEnabled;
        }

        #endregion
    }
}