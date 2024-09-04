using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core;
using System.Linq;

namespace PlazmaGames.Audio
{
    [System.Serializable]
    internal class AudioFile
    {
        public int id;
        public string name;
        public AudioClip audio;
    }

    public sealed class AudioMonoSystem : MonoBehaviour, IAudioMonoSystem
    {
        [Header("Settings")]
        [Range(0f, 1f)] [SerializeField] private float _overallSound = 1.0f;
        [Range(0f, 1f)] [SerializeField] private float _ambientSound = 1.0f;
        [Range(0f, 1f)] [SerializeField] private float _sfxSound = 1.0f;
        [Range(0f, 1f)][SerializeField] private float _musicSound = 1.0f;

        [Header("Audio Clips/Sources")]
        [SerializeField] private List<AudioFile> _musicSounds;
        [SerializeField] private List<AudioFile> _ambientSounds;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _ambientSource;

        [SerializeField] private List<AudioSource> _audioSources;

        /// <summary>   
        /// Private member that plays music given an audioclip and sound level between 0 and 1. 
        /// Music is play to compleation and only one audio clip can be played at once.
        /// </summary>
        private void PlayMusic(AudioClip audio, float sound)
        {
            _musicSource.volume = sound;
            _musicSource.clip = audio;
            _musicSource.Play();
        }

        /// <summary>   
        /// Private member that plays SfX sound given an audioclip and sound level between 0 and 1.
        /// Multiple audio files can be played at once. 
        /// </summary>
        private void PlayAmbient(AudioClip audio, float sound)
        {
            _ambientSource.volume = sound;
            _ambientSource.PlayOneShot(audio);
        }

        private void PlayAmbientLoop(AudioClip audio, float sound)
        {
            _ambientSource.volume = sound;
            _ambientSource.clip = audio;
            _ambientSource.loop = true;
            _ambientSource.Play();
        }

        /// <summary>   
        /// Finds and plays music with a given name if such an Audio file exist.
        /// </summary>
        private void PlayMusic(string name)
        {
            AudioFile music = _musicSounds.Find(e => name.CompareTo(e.name) == 1);
            if (music == null) return;
            PlayMusic(music.audio, _overallSound * _musicSound);
        }

        /// <summary>   
        /// Finds and plays music with a given an id if such an Audio file exist.
        /// </summary>
        private void PlayMusic(int id)
        {
            AudioFile music = _musicSounds.Find(e => id == e.id);
            if (music == null) return;
            PlayMusic(music.audio, _overallSound * _musicSound);
        }

        /// <summary>   
        /// Finds and plays a SfX sound with a given name if such an Audio file exist.
        /// </summary>
        private void PlayAmbient(string name, bool allowOverlay = true)
        {
            AudioFile sfx = _ambientSounds.Find(e => name.CompareTo(e.name) == 0);
            if (sfx == null) return;
            if (!(!allowOverlay && _ambientSource.isPlaying)) PlayAmbient(sfx.audio, _overallSound * _sfxSound);
        }

        /// <summary>   
        /// Finds and plays a SfX sound with a given an id if such an Audio file exist.
        /// </summary>
        private void PlayAmbient(int id, bool loop = true)
        {
            AudioFile sfx = _ambientSounds.Find(e => id == e.id);
            if (sfx == null) return;
            if (!loop) PlayAmbient(sfx.audio, _overallSound * _sfxSound);
            else PlayAmbientLoop(sfx.audio, _overallSound * _ambientSound);
        }

        /// <summary>
        /// Forces the SFX audio player to stop playing the current sound
        /// </summary>
        private void StopAmbient()
        {
            _ambientSource.Stop();
        }

        private void StopMusic()
        {
            _musicSource.Stop();
        }

        public void PlayAudio(string name, AudioType audioType, bool loop = true)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    PlayMusic(name);
                    break;
                case AudioType.Ambient:
                    PlayAmbient(name, loop);
                    break;
                default:
                    Debug.LogWarning($"{audioType} is not a vaild Audio Type.");
                    break;
            }
        }

        public void PlayAudio(int id, AudioType audioType, bool loop = true)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    PlayMusic(id);
                    break;
                case AudioType.Ambient:
                    PlayAmbient(id, loop);
                    break;
                default:
                    Debug.LogWarning($"{audioType} is not a vaild Audio Type.");
                    break;
            }
        }

        public void StopAudio(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    StopMusic();
                    break;
                case AudioType.Ambient:
                    StopAmbient();
                    break;
                default:
                    Debug.LogWarning($"{audioType} is not a vaild Audio Type.");
                    break;
            }
        }

        public void SetOverallVolume(float volume)
        {
            _overallSound = volume;
            foreach (AudioSource src in _audioSources) src.volume = _overallSound * _sfxSound;
            _ambientSource.volume = _overallSound * _ambientSound;
            _musicSource.volume = _overallSound * _musicSound;
        }

        public void SetSfXVolume(float volume)
        {
            _sfxSound = volume;
            foreach (AudioSource src in _audioSources) src.volume = _overallSound * _sfxSound;
            _ambientSource.volume = _overallSound * _ambientSound;
            _musicSource.volume = _overallSound * _musicSound;
        }

        public void SetAmbientVolume(float volume)
        {
            _ambientSound = volume;
            _ambientSource.volume = _overallSound * _ambientSound;
        }

        public void SetMusicVolume(float volume)
        {
            _musicSound = volume;
            _musicSource.volume = _overallSound * _musicSound;
        }

        public float GetOverallVolume()
        {
            return _overallSound;
        }

        public float GetSfXVolume()
        {
            return _sfxSound;
        }

        public float GetMusicVolume()
        {
            return _musicSound;
        }

        public float GetAmbientVolume()
        {
            return _ambientSound;
        }

        private void Start()
        {
            _audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList();
            SetOverallVolume(_overallSound);
            SetSfXVolume(_sfxSound);
            SetAmbientVolume(_ambientSound);
            SetMusicVolume(_musicSound);
        }
    }
}