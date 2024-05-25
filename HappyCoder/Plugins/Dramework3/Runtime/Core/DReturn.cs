using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public static class DReturn
    {
        #region ================================ FIELDS

        private static readonly UniTask _task = new UniTask();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static UniTask CompletedTask => _task;

        #endregion
    }
}