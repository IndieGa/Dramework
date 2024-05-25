using System.Collections.Generic;


namespace IG.HappyCoder.AI.Domino.Runtime
{
    public class Plan
    {
        #region ================================ FIELDS

        private readonly Planner _planner;
        private Action _current;
        private bool _checkInitialState;
        private bool _executing;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal Plan(Planner planner, IReadOnlyList<Action> actions)
        {
            _planner = planner;
            if (actions.Count > 1)
            {
                for (var i = 0; i < actions.Count - 1; i++)
                {
                    actions[i + 1].Next = actions[i];
                    actions[i + 1].onNext += OnNext;
                }
            }

            actions[0].Next = null;
            actions[0].onNext += OnNext;
            _current = actions[^1];
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string Current => _current.ID;
        public State State => _planner.GetState(_current.InitialStateID);

        /// <summary>
        /// 0 - Idle
        /// 1 - Executing
        /// 2 - Stop
        /// 3 - Success
        /// 4 - Failed
        /// </summary>
        public byte Status { get; private set; }
        private bool IsInitialStateValid => _current.IgnoreCheckingInitialState || (_checkInitialState && _planner.GetState(_current.InitialStateID).IsValid);

        #endregion

        #region ================================ METHODS

        public void Start(bool checkInitialState = false)
        {
            _checkInitialState = checkInitialState;

            if (_executing || _current == null || IsInitialStateValid == false) return;

            _executing = true;
            Status = 1;
            _current.Execute();
        }

        public void Stop()
        {
            if (_executing == false) return;
            _executing = false;
            _checkInitialState = false;
            Status = 2;
            _current.Stop();
        }

        private void OnNext(Action action)
        {
            _current = action;
            if (_current == null)
            {
                _executing = false;
                _checkInitialState = false;
                Status = 3;
            }
            else
            {
                if (IsInitialStateValid == false)
                {
                    Status = 4;
                    return;
                }
                _current.Execute();
            }
        }

        #endregion
    }
}