using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mfarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public string starSceneName = string.Empty;

        private CanvasGroup fadeCanvasGroup;

        private bool isFade;

        private void OnEnable()
        {
            EventHandler.TransitonEvent += OnTrasitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitonEvent -= OnTrasitionEvent;
        }

        private void OnTrasitionEvent(string sceneName, Vector3 targetPos)
        {
            if(!isFade)
            StartCoroutine(Transition(sceneName, targetPos));
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(LoadSceneSetActive(starSceneName));
            fadeCanvasGroup = GameObject.FindObjectOfType<CanvasGroup>();
            EventHandler.CallAfterSceneLoadedEvent();
        }

        /// <summary>
        /// 加载场景并设置激活
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }

        private IEnumerator Transition(string sceneName, Vector3 tartgetPositon)
        {
            EventHandler.CallBeforSceneUnLoadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

           
            yield return LoadSceneSetActive(sceneName);

            EventHandler.CallMoveToPositon(tartgetPositon);
            
            EventHandler.CallAfterSceneLoadedEvent();
            yield return Fade(0);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;

            while(!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Settings.fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;

            isFade = false;
        }
    }

}
