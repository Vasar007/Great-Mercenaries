using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private void Update()
    {
        if (LoadingManager.currentAppState == LoadingManager.AppState.Loading) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetString("LastScene"));
        }
    }
}
