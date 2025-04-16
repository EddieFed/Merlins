using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts
{
    
    // This script will randomly play selected songs from it's parents contained Audio Source elements. If none exist, no music will play!...
    public class BgMusicManager : MonoBehaviour
    {

        private List<AudioSource> _audioSources;
        private int currentTrack;
        private void Start()
        {
            // Load all adjacent audio sources
            _audioSources = new List<AudioSource>(FindObjectsByType<AudioSource>(FindObjectsSortMode.InstanceID));
            currentTrack = Random.Range(0, _audioSources.Count);
            _audioSources[currentTrack].Play();
        }

        private void Update()
        {
            if (_audioSources[currentTrack].isPlaying)
            {
                return;
            }
            
            // Random song order!
            currentTrack = Random.Range(0, _audioSources.Count);
            _audioSources[currentTrack].Play();
        }
    }
}