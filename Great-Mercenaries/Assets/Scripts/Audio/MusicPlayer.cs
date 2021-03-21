using System.Collections.Generic;
using UnityEngine;

namespace GreatMercenaries.Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [Tooltip("_audioSource defines the Audio Source component in this scene.")]
        private AudioSource _audioSource;

        [Tooltip("AudioTracks defines the audio clips to be played continuously through out the scene.")]
        public List<AudioClip> AudioTracks = new List<AudioClip>();

        [Space(20)]
        [Header("Music Player Options")]
        [Tooltip("PlayTracks acts as the Play/Stop function of the Music Player")]
        public bool PlayTracks;

        [Tooltip("Skips to the next available AudioTracks clip.")]
        public bool NextTrack;

        [Tooltip("Skips to the previous AudioTracks clip")]
        public bool PrevTrack;

        [Tooltip("Loops the current AudioTracks clip.")]
        public bool LoopTrack;

        [Space(20)]
        [Header("Debugging/ReadOnly")]
        [Tooltip("PlayingTrack is a ReadOnly variable that displays the current AudioTracks clip that is playing")]
        public int PlayingTrack;

        [Tooltip("IsMute returns the status of muting of AudioSource.")]
        public bool IsMute;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = AudioTracks[0];
            PlayingTrack = 0;

            // Below is the imported Awake methods for global muting.
            if (PlayerPrefs.HasKey("Mute"))
            {
                int value = PlayerPrefs.GetInt("Mute");
                switch (value)
                {
                    case 0:
                        IsMute = false;
                        break;
                    case 1:
                        IsMute = true;
                        break;
                }
            }
            else
            {
                IsMute = false;
                PlayerPrefs.SetInt("Mute", 0);
            }

            if (IsMute)
            {
                _audioSource.mute = true;
            }
            else
            {
                _audioSource.mute = false;
                _audioSource.Play();
            }
        }

        private void Update()
        {
            if (!PlayTracks)
            {
                _audioSource.Stop();
            }
            if (PlayTracks && !_audioSource.isPlaying)
            {
                StartPlayer();
            }

            _audioSource.loop = LoopTrack;

            if (NextTrack)
            {
                PlayNextTrack();
            }
            if (PrevTrack)
            {
                PlayPreviousTrack();
            }

            // Below is the imported Update methods for global muting.
            int value = PlayerPrefs.GetInt("Mute");
            switch (value)
            {
                case 0:
                    IsMute = false;
                    break;
                case 1:
                    IsMute = true;
                    break;
            }

            _audioSource.mute = IsMute;
        }

        public void StartPlayer()
        {
            if (!LoopTrack)
            { // If the Audio Source is not set to loop the clip we will play the next clip.
                PlayNextTrack();
            }
            else
            { // If Audio Source is set to loop we will play the same track again.
                _audioSource.Play();
            }
        }

        public void PlayNextTrack()
        {
            NextTrack = false;
            _audioSource.Stop();
            int newCount = PlayingTrack + 1; // Find the next track.

            if (newCount > AudioTracks.Count - 1)
            { // Loop to beginning of _audioTracks. Prevents Array Index out of range errors.
                _audioSource.clip = AudioTracks[0]; PlayingTrack = 0;
            }
            else
            {
                _audioSource.clip = AudioTracks[newCount]; PlayingTrack = newCount;
            }
            _audioSource.Play();
            Debug.Log("Called NextTrack: next=" + newCount + " : playing=" + PlayingTrack +
                      " : name= " + AudioTracks[PlayingTrack].name);
        }

        public void PlayPreviousTrack()
        {
            PrevTrack = false;
            _audioSource.Stop();
            int newCount = PlayingTrack - 1; // Find the previous track

            if (newCount < 0)
            { // Loops to end of _audioTracks. Prevents Array Index out of range errors.
                _audioSource.clip = AudioTracks[AudioTracks.Count - 1];
                PlayingTrack = AudioTracks.Count - 1;
            }
            else
            {
                _audioSource.clip = AudioTracks[newCount];
                PlayingTrack = newCount;
            }
            _audioSource.Play();
            Debug.Log("Called PreviousTrack: next=" + newCount + " : playing=" + PlayingTrack +
                      " : name= " + AudioTracks[PlayingTrack].name);
        }

    }
}
