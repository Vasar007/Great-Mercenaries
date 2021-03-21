using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GreatMercenaries.Assets.Scripts.UI
{
    public class LoadingManager : MonoBehaviour
    {
        public enum AppState
        {
            Loading = 0,
            Running = 1
        }

        public static AppState currentAppState;
        public float timeDelay = 3.0f;

        [SerializeField]
        private Image loadingImage;


        private void Start()
        {
            loadingImage.enabled = true;
            StartCoroutine(LoadingScreen());
            // Plenty of code to create and place object.
        }

        private IEnumerator LoadingScreen()
        {
            currentAppState = AppState.Loading;
            float time = 0.0f;
            while (time < timeDelay)
            {
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(loadingImage);
            currentAppState = AppState.Running;
        }

        private void Update()
        {

        }
    }
}
