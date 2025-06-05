using UnityEngine;
using UnityEngine.SceneManagement;

namespace Keru.Scripts.Visuals.Effects
{
    public class Fading : MonoBehaviour
    {
        public Texture2D fadeOutTexture;
        public float fadeSpeed = 0.5f;
        public int drawDepth = -1000;
        public bool loadOnLevel;
        private float _alpha = 1.0f;
        private float _fadeDir = -1;

        void OnGUI()
        {
            _alpha += _fadeDir * fadeSpeed * Time.unscaledDeltaTime;
            _alpha = Mathf.Clamp01(_alpha);
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        public float BeginFade(int direction)
        {
            _fadeDir = direction;
            return (fadeSpeed);
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (loadOnLevel)
            {
                BeginFade(-1);
            }
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }
}