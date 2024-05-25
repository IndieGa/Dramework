using Cysharp.Threading.Tasks;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.ResourceManagement
{
    public interface IResource
    {
        #region ================================ METHODS

        T GetAsset<T>() where T : Object;
        UniTask<T> InstantiateAsync<T>(bool autoRelease = false) where T : Object;
        UniTask<T> LoadAsync<T>() where T : Object;
        void Release();

        #endregion
    }
}