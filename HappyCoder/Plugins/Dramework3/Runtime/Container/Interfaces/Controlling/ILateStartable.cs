using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling
{
    public interface ILateStartable
    {
        #region ================================ METHODS

        UniTask OnLateStart();

        #endregion
    }
}