using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public void Awake()
        {
            // SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        public void FixedUpdate()
        {
            if (Input.GetKey("escape"))
            {
                QuitGame();
            }
        }

        private static void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
