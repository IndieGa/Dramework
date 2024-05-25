using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization
{
    public interface IEarlyInitializable
    {
        #region ================================ METHODS

        UniTask OnEarlyInitialize();

        #endregion
    }
}