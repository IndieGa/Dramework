using System.Diagnostics.CodeAnalysis;

using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public interface IFlag
    {
        #region ================================ PROPERTIES AND INDEXERS

        bool IsNotRaised { get; }
        bool IsRaised { get; }

        #endregion
    }

    public interface IFlag<out T> : IFlag
    {
        #region ================================ PROPERTIES AND INDEXERS

        T Data { get; }

        #endregion
    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DFlag : IFlag
    {
        #region ================================ PROPERTIES AND INDEXERS

        public bool IsNotRaised => !IsRaised;
        public bool IsRaised { get; private set; }

        #endregion

        #region ================================ METHODS

        public void Lower()
        {
            IsRaised = false;
        }

        public void Raise(bool autoLower = true, int delayFrame = 1)
        {
            IsRaised = true;
            if (autoLower)
                AutoLower(delayFrame).Forget();
        }

        private async UniTaskVoid AutoLower(int delayFrame)
        {
            await UniTask.DelayFrame(delayFrame);
            Lower();
        }

        #endregion
    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DFlag<T> : IFlag<T>
    {
        #region ================================ PROPERTIES AND INDEXERS

        public T Data { get; private set; }
        public bool IsNotRaised => !IsRaised;
        public bool IsRaised { get; private set; }

        #endregion

        #region ================================ METHODS

        public void Lower()
        {
            IsRaised = true;
        }

        public void Raise(T data, bool autoLower = true, int delayFrame = 1)
        {
            Data = data;
            if (autoLower)
                AutoLower(delayFrame).Forget();
        }

        private async UniTaskVoid AutoLower(int delayFrame)
        {
            await UniTask.DelayFrame(delayFrame);
            Lower();
        }

        #endregion
    }
}