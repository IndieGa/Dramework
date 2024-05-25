namespace IG.HappyCoder.Dramework3.Runtime.Factories.Core
{
    public abstract class DPoolFactoryItem
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

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnRelease()
        {
        }

        internal void Activate()
        {
            Active = true;
            OnActivate();
        }

        #endregion
    }
}