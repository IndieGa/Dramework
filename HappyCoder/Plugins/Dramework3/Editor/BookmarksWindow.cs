using System.Reflection;

using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Editor
{
    public class BookmarksWindow : EditorWindow
    {
        #region ================================ FIELDS

        private const string ADD_TIP = "To add asset object to bookmarks right click on asset and choose 'Add to Bookmarks' in context menu";

        private Vector2 _scrollPos;
        private EditorStorage _storage;
        private bool _editMode;
        private bool _loaded;

        #endregion

        #region ================================ METHODS

        [MenuItem("Assets/Add to Bookmarks", false, 19)]
        private static void AddToBookmarks(MenuCommand menuCommand)
        {
            var activeObject = Selection.activeObject;
            if (activeObject == null) return;
            EditorStorage.AddBookmark(activeObject);
        }

        private static void ScrollAssetsViewToTheBottom()
        {
            var assembly = typeof(EditorWindow).Assembly;
            var windowType = assembly.GetType("UnityEditor.ProjectBrowser");
            var controllerType = assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController");

            var projectBrowser = GetWindow(windowType);
            var treeViewController = windowType.GetField("m_AssetTree", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(projectBrowser);
            var state = controllerType.GetProperty("state", BindingFlags.Public | BindingFlags.Instance)!.GetValue(treeViewController) as TreeViewState;

            var contentRect = (Rect)controllerType.GetField("m_ContentRect", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(treeViewController);
            var visibleRect = (Rect)controllerType.GetField("m_VisibleRect", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(treeViewController);

            state!.scrollPos = new Vector2(state.scrollPos.x, contentRect.height - visibleRect.height);
        }

        [MenuItem("Window/Dramework 3/Bookmarks")]
        private static void ShowWindow()
        {
            var window = GetWindow<BookmarksWindow>();
            window.titleContent = new GUIContent("Bookmarks");
            window._storage = EditorStorage.GetStorage();
            window.Show();
        }

        private void OnGUI()
        {
            if (_storage == null) return;

            if (_storage.Bookmarks.Count == 0 || _editMode)
            {
                var textStyle = EditorStyles.label;
                textStyle.wordWrap = true;
                EditorGUILayout.LabelField(ADD_TIP, textStyle);
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var labelWidth = GUILayout.Width(20);
            var buttonWidth = GUILayout.Width(30);
            var bookmarks = _storage.Bookmarks;

            Iterate();

            if (bookmarks.Count > 0)
            {
                if (_editMode)
                {
                    if (GUILayout.Button("Exit Edit Mode"))
                    {
                        _editMode = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Edit"))
                    {
                        _editMode = true;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            return;

            void Iterate()
            {
                for (var i = 0; i < bookmarks.Count; i++)
                {
                    var bookmark = bookmarks[i];

                    EditorGUILayout.BeginHorizontal();

                    if (_editMode)
                    {
                        if (i == 0)
                        {
                            GUI.enabled = false;
                        }

                        if (GUILayout.Button("▲", buttonWidth))
                        {
                            bookmarks.RemoveAt(i);
                            bookmarks.Insert(i - 1, bookmark);
                            SaveStorage();
                        }

                        if (i == 0)
                        {
                            GUI.enabled = true;
                        }

                        if (i == bookmarks.Count - 1)
                        {
                            GUI.enabled = false;
                        }

                        if (GUILayout.Button("▼", buttonWidth))
                        {
                            bookmarks.RemoveAt(i);
                            bookmarks.Insert(i + 1, bookmark);
                            SaveStorage();
                        }

                        if (i == bookmarks.Count - 1)
                        {
                            GUI.enabled = true;
                        }

                        if (GUILayout.Button("X", buttonWidth))
                        {
                            bookmarks.RemoveAt(i);
                            SaveStorage();
                        }
                    }
                    else
                    {
                        GUILayout.Label(i.ToString(), labelWidth);
                    }


                    EditorGUILayout.ObjectField(bookmark, typeof(Object), true);


                    if (_editMode == false)
                    {
                        if (GUILayout.Button("▼", buttonWidth))
                        {
                            ScrollAssetsViewToTheBottom();
                            EditorGUIUtility.PingObject(bookmark);
                        }

                        if (Selection.activeObject == bookmark)
                        {
                            GUI.enabled = false;
                        }

                        if (GUILayout.Button("►", buttonWidth))
                        {
                            ScrollAssetsViewToTheBottom();
                            Selection.activeObject = bookmark;
                        }

                        if (Selection.activeObject == bookmark)
                        {
                            GUI.enabled = true;
                        }
                    }


                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void SaveStorage()
        {
            EditorUtility.SetDirty(_storage);
            AssetDatabase.SaveAssetIfDirty(_storage);
        }

        #endregion
    }
}