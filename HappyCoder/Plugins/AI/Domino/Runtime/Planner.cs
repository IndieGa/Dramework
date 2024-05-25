using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;


namespace IG.HappyCoder.AI.Domino.Runtime
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Planner
    {
        #region ================================ FIELDS

        private readonly Dictionary<int, State> _states = new Dictionary<int, State>(32);
        private readonly Dictionary<int, List<Action>> _actions = new Dictionary<int, List<Action>>(32);
        private readonly List<int> _initialStateIDs = new List<int>(32);
        private List<List<Action>> _plans;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public Planner()
        {
        }

#if UNITY_EDITOR
        public Planner(Func<int, string> onStateNameRequest)
        {
            this.onStateNameRequest += onStateNameRequest;
        }
#endif

        #endregion

        #region ================================ EVENTS AND DELEGATES

#if UNITY_EDITOR
        private event Func<int, string> onStateNameRequest;
#endif

        #endregion

        #region ================================ METHODS

        public void AddAction(Action action)
        {
            if (_actions.TryGetValue(action.GoalStateID, out var list))
            {
                if (IsActionAlreadyInList()) return;
                list.Add(action);
            }
            else
            {
                _actions.Add(action.GoalStateID, new List<Action>(32) { action });
            }

            if (IsInitialStateIdAlreadyInList()) return;
            _initialStateIDs.Add(action.InitialStateID);
            return;

            bool IsActionAlreadyInList()
            {
                foreach (var item in list)
                {
                    if (item.GetType() == action.GetType())
                        return true;
                }
                return false;
            }

            bool IsInitialStateIdAlreadyInList()
            {
                foreach (var initialStateID in _initialStateIDs)
                {
                    if (initialStateID == action.InitialStateID)
                        return true;
                }
                return false;
            }
        }

        public void AddState(State state)
        {
            _states.TryAdd(state.ID, state);
        }

        public bool TryGetPlan(int goalStateID, out Plan plan)
        {
            plan = null;
            _plans = new List<List<Action>>();

            if
            (
                _states.ContainsKey(goalStateID) == false // if there are no goal state
                || _actions.ContainsKey(goalStateID) == false // if there are no action leading to the goal state
                || _states[_actions[goalStateID][0].GoalStateID].IsValid // or if the current state of the world is equal to the goal state
                || AllInitialStatesIsNotEqualCurrentState() // or if there is no action with a initial state equal to the current state of the world
            )
            {
                LogResult();
                return false;
            }

            // create all possible plans
            CreatePlans(goalStateID, _actions, Array.Empty<Action>());

            LogResult();

            if (_plans.Count == 0) return false;

            // sort the plans by length and leave only those equal to the minimum length
            OrderPlansByLength();
            var minLength = GetMinPlanLength();
            var plans = SelectPlansWithMinLength();

            // if there are more than one plan, select the plan with the highest priority
            if (plans.Length > 1)
            {
                var maxPriority = 0;
                var maxChain = Array.Empty<Action>();
                foreach (var chain in plans)
                {
                    var sum = chain.Sum(t => t.Priority);
                    if (sum < maxPriority) continue;
                    maxPriority = sum;
                    maxChain = chain.ToArray();
                }
                plan = new Plan(this, maxChain);
            }
            else
            {
                plan = new Plan(this, plans[0]);
            }

            return true;

            bool AllInitialStatesIsNotEqualCurrentState()
            {
                foreach (var stateID in _initialStateIDs)
                {
                    if (_states[stateID].IsValid)
                        return false;
                }

                return true;
            }

            void OrderPlansByLength()
            {
                var array = _plans.ToArray();
                var d = array.Length / 2;
                while (d >= 1)
                {
                    for (var i = 0; i < array.Length; i++)
                    {
                        var j = i;
                        while (j >= d && array[j - d].Count > array[j].Count)
                        {
                            (array[j], array[j - d]) = (array[j - d], array[j]);
                            j -= d;
                        }
                    }
                    d /= 2;
                }
            }

            int GetMinPlanLength()
            {
                var min = int.MaxValue;
                foreach (var actions in _plans)
                {
                    if (actions.Count < min)
                        min = actions.Count;
                }
                return min;
            }

            List<Action>[] SelectPlansWithMinLength()
            {
                var result = new List<List<Action>>();
                foreach (var actions in _plans)
                {
                    if (actions.Count == minLength)
                        result.Add(actions);
                }
                return result.ToArray();
            }
        }

        internal State GetState(int id)
        {
            return _states[id];
        }

        private void CreatePlans(int goalStateID, IReadOnlyDictionary<int, List<Action>> baseActions, IReadOnlyCollection<Action> basePlan)
        {
            // create a copy of the base set of actions
            var actions = new Dictionary<int, List<Action>>();
            var keys = baseActions.Keys.ToArray();
            for (var i = 0; i < baseActions.Count; i++)
            {
                var key = keys[i];
                actions.Add(key, new List<Action>(baseActions[key]));
            }

            while (actions[goalStateID].Count > 0)
            {
                // create a copy of the base plan
                var plan = new List<Action>(basePlan);
                // get the last action from the end of the copy of the set of actions and delete it from there
                var action = actions[goalStateID][^1];
                actions[goalStateID].Remove(action);

                //  if the action has an initial state equal to the current one, add it to the plan and complete it.
                if (_states[action.InitialStateID].IsValid)
                {
                    plan.Add(action);
                    _plans.Add(plan);
#if UNITY_EDITOR
                    if (onStateNameRequest != null)
                        LogStep(plan);
#endif
                }
                // else if the world contains actions that lead to the initial state of the current action, add the current action to the plan and recursively complete the plan
                else if (actions.ContainsKey(action.InitialStateID) && actions[action.InitialStateID].Count > 0)
                {
                    plan.Add(action);
                    CreatePlans(action.InitialStateID, actions, plan);
                }
#if UNITY_EDITOR
                else if (onStateNameRequest != null)
                {
                    plan.Add(action);
                    LogStep(plan);
                }
#endif
            }
        }

        partial void LogResult();
        partial void LogStep(IReadOnlyList<Action> actions);

        #endregion
    }
}