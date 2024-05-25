using IG.HappyCoder.Dramework3.Runtime.Behaviours;


namespace IG.HappyCoder.Dramework3.Runtime.Factories.Core
{
    public abstract class DPoolFactoryItemComponent : DBehaviour
    {
        #region ================================ PROPERTIES AND INDEXERS

        internal bool Active { get; private set; }

        #endregion

        #region ================================ METHODS

        public void Release()
        {
            Active = false;
            OnRelease();
        }

        internal void Activate()
        {
            Active = true;
            OnActivate();
        }

        private void OnActivate()
        {
        }

        private void OnRelease()
        {
        }

        #endregion
    }
}