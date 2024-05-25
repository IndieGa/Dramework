using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization
{
    public interface ILastInitializable
    {
        #region ================================ METHODS

        UniTask OnLastInitialize();

        #endregion
    }
}