using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _isMute;


    private void Awake ()
    {
        _audioSource = GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey("Mute"))
        {
            int value = PlayerPrefs.GetInt("Mute");
            if (value == 0)
            {
                _isMute = false;
            }

            if (value == 1)
            {
                _isMute = true;
            }
        }
        else
        {
            _isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
        }

        _audioSource.mute = _isMute;
    }

    private void Update ()
    {
        int value = PlayerPrefs.GetInt("Mute");
        if (value == 0)
        {
            _isMute = false;
        }
        if (value == 1)
        {
            _isMute = true;
        }

        _audioSource.mute = _isMute;
    }
}
