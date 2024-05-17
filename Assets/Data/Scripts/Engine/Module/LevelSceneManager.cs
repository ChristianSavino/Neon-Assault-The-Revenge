using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(ChangeScene(levelCode));
        }

        private IEnumerator ChangeScene(LevelCode levelCode)
        {
            JukeBox.jukebox.StopMusic(true);
            
            var scene = GetNonPlayableLevelName(levelCode);
            if(string.IsNullOrEmpty(scene))
            {
                var levelData = LevelBase.CurrentSave.AllLevelData.Where(x => x.Code == levelCode).FirstOrDefault();
                scene = levelData.LevelType == LevelType.Game ? "LoadingScreen" : levelData.SceneName;
            }
      
            GraphicsManager.graphicsManager.FadeCamera(1);
            LevelBase.levelBase.SetTimeScale(1);

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(scene);
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
    }
}