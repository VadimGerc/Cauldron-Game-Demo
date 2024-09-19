using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagement
{
    /// <summary>
    /// Loads a scene after the fading transition
    /// </summary>
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance;

        public Image fadeImage;

        private void Awake()
        {
            // Since this object DontDestroyOnLoad, it must be alone in the scene
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        /// <summary>
        /// Starts a coroutine that will load a scene by index after the screen is filled with black
        /// </summary>
        /// <param name="index"></param>
        public void LoadScene(int index)
        {
            StartCoroutine(StartFadeTransitionAnimation(delegate { LoadLevelProcess(index); }));
        }

        /// <summary>
        /// Runs a coroutine that will smoothly remove black from the screen (after the scene is loaded)
        /// </summary>
        private void EndLoading()
        {
            StartCoroutine(EndFadeTransitionAnimation());
        }

        private IEnumerator StartFadeTransitionAnimation(UnityAction action)
        {
            fadeImage.gameObject.SetActive(true);

            var fadeTransitionColor = fadeImage.color;
            fadeTransitionColor.a = 0;
            fadeImage.color = fadeTransitionColor;

            while (true)
            {
                if (fadeImage)
                {
                    fadeTransitionColor = fadeImage.color;
                    fadeTransitionColor.a += Time.deltaTime * 3;
                    fadeImage.color = fadeTransitionColor;

                    if (fadeTransitionColor.a >= 1)
                    {
                        action?.Invoke();
                        break;
                    }
                }

                yield return 0;
            }
        }

        private IEnumerator EndFadeTransitionAnimation()
        {
            while (true)
            {
                if (fadeImage)
                {
                    var fadeTransitionColor = fadeImage.color;
                    fadeTransitionColor.a -= Time.deltaTime * 3;
                    fadeImage.color = fadeTransitionColor;

                    if (fadeTransitionColor.a <= 0)
                    {
                        fadeImage.gameObject.SetActive(false);
                        break;
                    }
                }

                yield return 0;
            }
        }

        /// <summary>
        /// Load a level by index
        /// </summary>
        /// <param name="index"></param>
        private void LoadLevelProcess(int index)
        {
            SceneManager.LoadSceneAsync(index).completed += LoadHandler;
        }

        private void LoadHandler(AsyncOperation obj)
        {
            EndLoading();
        }
    }
}
