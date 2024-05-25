#if UNITY_EDITOR

using System.Collections.Generic;

using UnityEngine;


namespace IG.HappyCoder.AI.Domino.Runtime
{
    public partial class Planner
    {
        #region ================================ METHODS

        partial void LogResult()
        {
            if (onStateNameRequest == null) return;

            if (_plans == null || _plans.Count == 0)
            {
                Debug.Log("Plan is not created");
                return;
            }

            Debug.Log($"{_plans.Count} plan{(_plans.Count > 1 ? "s are" : " is")} created");

            var actionsLog = string.Empty;
            for (var i = 0; i < _plans.Count; i++)
            {
                var actions = _plans[i];
                for (var j = actions.Count - 1; j >= 0; j--)
                {
                    actionsLog = $"{actionsLog}" +
                                 $"{actions[j].ID} " +
                                 $"({onStateNameRequest.Invoke(actions[j].InitialStateID)} -> " +
                                 $"{onStateNameRequest.Invoke(actions[j].GoalStateID)}){(j > 0 ? " + " : string.Empty)}";
                }

                actionsLog = $"{actionsLog}{(i <= _plans.Count ? "\n" : string.Empty)}";
            }

            if (string.IsNullOrEmpty(actionsLog)) return;

            Debug.Log(actionsLog);
        }

        partial void LogStep(IReadOnlyList<Action> actions)
        {
            var actionsLog = "Step: ";
            for (var i = actions.Count - 1; i >= 0; i--)
            {
                actionsLog = $"{actionsLog}" +
                             $"{actions[i].ID} " +
                             $"({onStateNameRequest.Invoke(actions[i].InitialStateID)} -> " +
                             $"{onStateNameRequest.Invoke(actions[i].GoalStateID)}){(i > 0 ? " + " : string.Empty)}";
            }

            Debug.Log(actionsLog);
        }

        #endregion
    }
}

#endif