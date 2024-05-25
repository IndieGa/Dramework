using System;


namespace IG.HappyCoder.AI.Domino.Runtime
{
    public abstract class Action
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        protected Action(string id, int initialStateID, int goalStateID, int priority, bool ignoreCheckingInitialState = false)
        {
            ID = id;
            InitialStateID = initialStateID;
            GoalStateID = goalStateID;
            Priority = priority;
            IgnoreCheckingInitialState = ignoreCheckingInitialState;
        }

        #endregion

        #region ================================ EVENTS AND DELEGATES

        internal event Action<Action> onNext;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string ID { get; }
        internal int GoalStateID { get; }
        internal bool IgnoreCheckingInitialState { get; }
        internal int InitialStateID { get; }
        internal Action Next { get; set; }
        internal int Priority { get; }

        #endregion

        #region ================================ METHODS

        public abstract void Execute();
        public abstract void Stop();
        protected void ExecuteNext()
        {
            onNext?.Invoke(Next);
        }

        #endregion
    }
}