using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling
{
    public interface ILastStartable
    {
        #region ================================ METHODS

        UniTask OnLastStart();

        #endregion
    }
}