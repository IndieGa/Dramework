using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling
{
    public interface IEarlyStartable
    {
        #region ================================ METHODS

        UniTask OnEarlyStart();

        #endregion
    }
}