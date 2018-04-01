using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Tooltip("SceneToLoadOnPlay is the name of the scene that will be loaded when users click play")]
    public string SceneToLoadOnPlay = "GameScene";

    [Tooltip("WebpageURL defines the URL that will be opened when users click on your branding icon")]
    public string WebpageURL = "https://github.com/Vasar007";

    [Tooltip("SoundButtons define the SoundOn[0] and SoundOff[1] Button objects.")]
    public List<Button> SoundButtons = new List<Button>();

    [Tooltip("AudioClip defines the audio to be played on button click.")]
    public AudioClip AudioClip;

    [Tooltip("AudioSource defines the Audio Source component in this scene.")]
    public AudioSource AudioSource;

    public float GameExitDelay = 0.5f;

    // The private variable '_scene' defined below is used for example/development purposes.
    // It is used in correlation with the Escape_Menu script to return to last scene on key press.
    private Scene _scene;


    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetInt("Mute", 0);
        }
        
        _scene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("LastScene", _scene.name); 
        Debug.Log(_scene.name);
    }
    
    public void OpenWebpage()
    {
        AudioSource.PlayOneShot(AudioClip);
        Application.OpenURL(WebpageURL);
    }
    
    public void PlayGame()
    {
        AudioSource.PlayOneShot(AudioClip);
        PlayerPrefs.SetString("LastScene", _scene.name);
        SceneManager.LoadScene(SceneToLoadOnPlay);
    }
    
    public void Mute()
    {
        AudioSource.PlayOneShot(AudioClip);
        SoundButtons[0].interactable = true;
        SoundButtons[1].interactable = false;
        PlayerPrefs.SetInt("Mute", 1);
    }
    
    public void Unmute()
    {
        AudioSource.PlayOneShot(AudioClip);
        SoundButtons[0].interactable = false;
        SoundButtons[1].interactable = true;
        PlayerPrefs.SetInt("Mute", 0);
    }
    
    public void QuitGame()
    {
        AudioSource.PlayOneShot(AudioClip);

        //StartCoroutine(Run());

        #if !UNITY_EDITOR
            Application.Quit();
        #endif
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator Run()
    {
        yield return new WaitForSeconds(GameExitDelay);

        #if !UNITY_EDITOR
            Application.Quit();
        #endif

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
