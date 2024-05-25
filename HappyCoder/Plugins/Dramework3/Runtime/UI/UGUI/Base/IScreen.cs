using Cysharp.Threading.Tasks;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base
{
    public interface IScreen
    {
        #region ================================ PROPERTIES AND INDEXERS

        bool IsShown { get; }
        int SortingLayerID { get; set; }
        int SortingOrder { get; set; }

        #endregion

        #region ================================ METHODS

        void Hide();
        UniTask Hide(float duration);
        void Show();
        UniTask Show(float duration);

        #endregion
    }
}