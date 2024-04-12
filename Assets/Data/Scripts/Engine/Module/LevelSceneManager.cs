using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Keru.Scripts.Engine.Module
{
    public class LevelSceneManager : MonoBehaviour
    {
        public static LevelSceneManager levelSceneManager;

        public void SetUp()
        {
            levelSceneManager = this;
        }

        public void LoadingScreen()
        {
            LoadScene("LoadingScreen");
        }

        public void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(ChangeScene(scene));
        }

        private IEnumerator ChangeScene(string scene)
        {
            GraphicsManager.graphicsManager.FadeCamera(1);
            LevelBase.levelBase.SetTimeScale(1);

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(scene);
        }
    }
}