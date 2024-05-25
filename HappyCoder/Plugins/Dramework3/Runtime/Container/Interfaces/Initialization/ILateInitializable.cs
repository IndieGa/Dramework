using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization
{
    public interface ILateInitializable
    {
        #region ================================ METHODS

        UniTask OnLateInitialize();

        #endregion
    }
}