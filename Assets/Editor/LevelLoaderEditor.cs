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
            _tabNumber = GUILayout.Toolbar(_tabNumber, new string[]{"Generate", "Standard"});

            switch(_tabNumber)
            {
                case 0: // "Generate"
                    #region Generate
                    var myScript = (LevelLoader)target;

                    /*
                EditorGUILayout.BeginHorizontal();
                    myScript.level = EditorGUILayout.ObjectField("Level Image", myScript.level, typeof(Texture2D)) as Texture2D;
                    myScript.sourceRect = EditorGUILayout.RectField("Source Rect", myScript.sourceRect);
                EditorGUILayout.EndHorizontal();

                PixelToObject[] Objects = myScript.Objects;

                for(int i = 0; i < Objects.Length; i++) {
                    PixelToObject pto = Objects[i];

                    EditorGUILayout.BeginHorizontal();
                        pto.inputColor = EditorGUILayout.ColorField("Input", pto.inputColor);
                        pto.outputObject = EditorGUILayout.ObjectField("Output", pto.outputObject, typeof(GameObject)) as GameObject;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                        pto.extraHeight = EditorGUILayout.FloatField("Height", pto.extraHeight);
                        pto.spawnFloor = EditorGUILayout.Toggle("Extra floor", pto.spawnFloor);
                    EditorGUILayout.EndHorizontal();

                    Objects[i] = pto;
                }
                */
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Generate Map")){
                        myScript.LoadLevel();
                    }

                    break;
                #endregion

                case 1: // "Standard"
                    #region Standard Settings
                    DrawDefaultInspector();
                    break;
                #endregion
            }


        }

    }
}
