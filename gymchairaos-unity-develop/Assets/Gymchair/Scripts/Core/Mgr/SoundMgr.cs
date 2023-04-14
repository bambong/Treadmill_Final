﻿using System;
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

            //배경음
            _dictAudioClips.Add("bgm_Speed",    Resources.Load<AudioClip>("BGM_Speed_11._Future_Tech_420"));
            _dictAudioClips.Add("bgm_Obstacle", Resources.Load<AudioClip>("BGM_Obstacle_17._Technically_Accurate_353"));

            //효과음
            _dictAudioClips.Add("sfx_CarCrash", Resources.Load<AudioClip>("SFX_CarCrash_Car_Impact_Concrete"));
            _dictAudioClips.Add("sfx_Die",      Resources.Load<AudioClip>("SFX_DieCar_Explosion_7"));
            _dictAudioClips.Add("sfx_Engine",   Resources.Load<AudioClip>("SFX_Engine_SUV_Engine_Loop_8"));
            _dictAudioClips.Add("sfx_Heal",     Resources.Load<AudioClip>("SFX_Heal_21_ui_casual_click15"));
            _dictAudioClips.Add("sfx_Pause",    Resources.Load<AudioClip>("SFX_PauseTrue_28_ui_casual_pause"));
            _dictAudioClips.Add("sfx_Play",     Resources.Load<AudioClip>("SFX_Play_27_ui_casual_play"));
            _dictAudioClips.Add("sfx_BtnClick", Resources.Load<AudioClip>("SFX_UIClick_22_ui_casual_click16"));

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