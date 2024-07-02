using Keru.Scripts.Engine.Master;
using System;
using System.Collections;
using System.Linq;
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
            LoadScene(LevelCode.LoadingScreen);
        }

        public void ReloadScene()
        {
            LoadScene(LevelCode.Reset);
        }

        public void LoadScene(LevelCode levelCode)
        {
            JukeBox.jukebox.StopMusic(true);

            var scene = GetNonPlayableLevelName(levelCode);
            if (string.IsNullOrEmpty(scene))
            {
                var levelData = MasterLevelData.AllLevels.FirstOrDefault(x => x.Code == LevelBase.CurrentSave.CurrentLevelCode);
                scene = levelData.LevelType == LevelType.Game ? "LoadingScreen" : levelData.SceneName;
            }

            LevelBase.levelBase.SetTimeScale(1);

            StartCoroutine(LoadSceneAsync(scene));
        }

        private string GetNonPlayableLevelName(LevelCode levelCode)
        {
            switch (levelCode)
            {
                case LevelCode.Reset:
                    return SceneManager.GetActiveScene().name;
                case LevelCode.Intro:
                    return "Intro";
                case LevelCode.MainMenu:
                    return "Main Menu";
                case LevelCode.GameMainMenu:
                    return "Game Main Menu";
            }

            return "";
        }

        IEnumerator LoadSceneAsync(string scene)
        {
            GraphicsManager.graphicsManager.FadeCamera(1);

            yield return new WaitForSecondsRealtime(2);

            var loadScene = SceneManager.LoadSceneAsync(scene);
            loadScene.allowSceneActivation = false;

            var startTime = DateTime.Now;          
            while (loadScene.progress < 0.9f)
            {
                yield return new WaitForEndOfFrame();
            }

            loadScene.allowSceneActivation = true;
        }
    }
}