using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Core.Mgr
{
    public class SoundMgr : Behaviour.ManagerBehaviour<SoundMgr>
    {
        Dictionary<string, AudioClip> _dictAudioClips;

        AudioClip[] _audioClips;

        GameObject[] _audioObject;
        AudioSource[] _audioSource;

        private void Awake()
        {
            initAudioListener();
            loadAudioClips();

            PlayBGM("back");
        }

        public void initAudioListener()
        {
            _audioObject = new GameObject[2];
            _audioSource = new AudioSource[2];

            _audioObject[0] = new GameObject("AudioBGM");
            _audioObject[1] = new GameObject("AudioEffect");

            _audioObject[0].transform.parent = gameObject.transform;
            _audioObject[1].transform.parent = gameObject.transform;

            _audioSource[0] = _audioObject[0].AddComponent<AudioSource>();
            _audioSource[1] = _audioObject[1].AddComponent<AudioSource>();
        }


        public void loadAudioClips()
        {
            _dictAudioClips = new Dictionary<string, AudioClip>();

            _dictAudioClips.Add("play", Resources.Load<AudioClip>("play"));
            _dictAudioClips.Add("count", Resources.Load<AudioClip>("count"));
            _dictAudioClips.Add("back", Resources.Load<AudioClip>("back"));
            _dictAudioClips.Add("touch", Resources.Load<AudioClip>("touch"));
        }

        public void PlayBGM(string name)
        {
            if (!_dictAudioClips.ContainsKey(name))
                return;

            _audioSource[0].Stop();
            _audioSource[0].clip = _dictAudioClips[name];
            _audioSource[0].loop = true;
            _audioSource[0].Play();
        }

        public void StopBGM()
        {
            _audioSource[0].Stop();
        }

        public void PlayEffect(string name)
        {
            if (!_dictAudioClips.ContainsKey(name))
                return;

            _audioSource[1].Stop();
            _audioSource[1].clip = _dictAudioClips[name];
            _audioSource[1].loop = false;
            _audioSource[1].Play();
        }

        public override void OnCoreMessage(object msg)
        {
        }
    }
}