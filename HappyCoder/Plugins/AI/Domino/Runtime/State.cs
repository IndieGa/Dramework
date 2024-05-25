namespace IG.HappyCoder.AI.Domino.Runtime
{
    public abstract class State
    {
        #region ================================ PROPERTIES AND INDEXERS

        public abstract int ID { get; }

        /// <summary>
        /// Checks whether the state matches the current state of the model
        /// </summary>
        public abstract bool IsValid { get; }

        #endregion
    }
}