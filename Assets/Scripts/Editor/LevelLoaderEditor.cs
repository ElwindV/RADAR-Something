using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelLoader))]
    public class LevelLoaderEditor : UnityEditor.Editor 
    {
        private int _tabNumber;

        public override void OnInspectorGUI()
        {
            _tabNumber = GUILayout.Toolbar(_tabNumber, new[]{"Standard", "Generate"});

            switch(_tabNumber)
            {
                case 1: // "Generate"
                    #region Generate
                    var myScript = (LevelLoader)target;

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Generate Map")){
                        myScript.LoadLevel();
                    }

                    break;
                #endregion

                case 0: // "Standard"
                    #region Standard Settings
                    DrawDefaultInspector();
                    break;
                #endregion
            }
        }

    }
}
