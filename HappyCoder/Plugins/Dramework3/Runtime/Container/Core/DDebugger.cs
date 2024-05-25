using IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    public class DDebugger : ScriptableObject
    {
        #region ================================ METHODS

        protected bool TryGetObject<T>(string sceneName, out T obj)
        {
            obj = default;
            foreach (var dispatcher in FindObjectsOfType<DDispatcher>())
            {
                if (dispatcher.SceneName != sceneName) continue;
                obj = (T)dispatcher.GetContainerObject(typeof(T), null);
                return true;
            }

            return false;
        }

        #endregion
    }
}