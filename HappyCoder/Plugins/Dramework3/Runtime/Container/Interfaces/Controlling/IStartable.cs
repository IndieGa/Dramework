using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling
{
    public interface IStartable
    {
        #region ================================ METHODS

        UniTask OnStart();

        #endregion
    }
}