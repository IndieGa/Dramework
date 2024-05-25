using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Factories.Interfaces
{
    public interface IGlobalFactory
    {
        #region ================================ METHODS

        T Create<T>() where T : class, new();
        T CreateAndBind<T>() where T : class, new();
        T CreateAndBind<T>(string instanceID) where T : class, new();
        T CreateAndBind<T>(string containerID, string instanceID) where T : class, new();
        T Instantiate<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : Component;
        T InstantiateAndBind<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : Component;
        T InstantiateAndBind<T>(GameObject prefab, Transform parent, string instanceID, bool worldPositionStays = true) where T : Component;
        T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID = "", string instanceID = "", bool worldPositionStays = true) where T : Component;

        #endregion
    }
}