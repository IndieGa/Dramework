#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Editor
{
    [CustomEditor(typeof(BuildTool))]
    public class BuildToolInspector : UnityEditor.Editor
    {
        #region ================================ METHODS

        public override void OnInspectorGUI()
        {
            var buildTool = (BuildTool)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Build"))
            {
                if (EditorApplication.isCompiling || BuildPipeline.isBuildingPlayer) return;
                buildTool.Build();
            }
        }

        #endregion
    }
}

#endif