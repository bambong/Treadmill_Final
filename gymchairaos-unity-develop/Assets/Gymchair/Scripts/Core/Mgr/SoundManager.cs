
using System.Collections.Generic;
using UnityEngine;

    public class SoundManager 
    {
        private Dictionary<string, AudioClip> _dictAudioClips;
        private GameObject[] _audioObject;
        private AudioSource[] _audioSource;

        private GameObject rootGo;
        public void Init()
        {
            initAudioListener();
            loadAudioClips();
        }

        public void initAudioListener()
        {

            rootGo = new GameObject();
            GameObject.DontDestroyOnLoad(rootGo);
            rootGo.name = "Sound_Source";

            _audioObject = new GameObject[2];
            _audioSource = new AudioSource[2];

            _audioObject[0] = new GameObject("AudioBGM");
            _audioObject[1] = new GameObject("AudioEffect");

            _audioObject[0].transform.parent = rootGo.transform;
            _audioObject[1].transform.parent = rootGo.transform;

            _audioSource[0] = _audioObject[0].AddComponent<AudioSource>();
            _audioSource[1] = _audioObject[1].AddComponent<AudioSource>();
        }


        public void loadAudioClips()
        {
            _dictAudioClips = new Dictionary<string, AudioClip>();

            _dictAudioClips.Add("play", Resources.Load<AudioClip>("play"));
            _dictAudioClips.Add("count", Resources.Load<AudioClip>("count"));
            _dictAudioClips.Add("back", Resources.Load<AudioClip>("back"));
            //_dictAudioClips.Add("touch", Resources.Load<AudioClip>("touch"));
            _dictAudioClips.Add("touch", Resources.Load<AudioClip>("Sounds/SFX_Button_Click"));

            //배경음
            _dictAudioClips.Add("bgm_Speed",    Resources.Load<AudioClip>("Sounds/speed-bgm"));
            _dictAudioClips.Add("bgm_Obstacle", Resources.Load<AudioClip>("Sounds/BGM_Obstacle_17._Technically_Accurate_353"));
            _dictAudioClips.Add("bgm_GameSelect", Resources.Load<AudioClip>("Sounds/BGM_GameSelect"));
            _dictAudioClips.Add("bgm_SpeedMenu", Resources.Load<AudioClip>("Sounds/bgm_SpeedMenu"));
            _dictAudioClips.Add("bgm_Balance", Resources.Load<AudioClip>("Sounds/13. Track 13"));

            //효과음
            _dictAudioClips.Add("sfx_CarCrash", Resources.Load<AudioClip>("Sounds/SFX_CarCrash_Car_Impact_Concrete"));
            _dictAudioClips.Add("sfx_Die",      Resources.Load<AudioClip>("Sounds/SFX_DieCar_Explosion_7"));
            _dictAudioClips.Add("sfx_Engine",   Resources.Load<AudioClip>("Sounds/SFX_Engine_SUV_Engine_Loop_8"));
            _dictAudioClips.Add("sfx_Heal",     Resources.Load<AudioClip>("Sounds/SFX_Heal_21_ui_casual_click15"));
            _dictAudioClips.Add("sfx_Pause",    Resources.Load<AudioClip>("Sounds/SFX_PauseTrue_28_ui_casual_pause"));
            _dictAudioClips.Add("sfx_Play",     Resources.Load<AudioClip>("Sounds/SFX_Play_27_ui_casual_play"));
            _dictAudioClips.Add("sfx_BtnClick", Resources.Load<AudioClip>("Sounds/SFX_UIClick_22_ui_casual_click16"));
            _dictAudioClips.Add("sfx_Speed_Clear", Resources.Load<AudioClip>("Sounds/speed-win"));
            _dictAudioClips.Add("sfx_Speed_GameOver", Resources.Load<AudioClip>("Sounds/speed-game-over"));
            _dictAudioClips.Add("sfx_Speed_Ready", Resources.Load<AudioClip>("Sounds/speed-ready-go"));
            _dictAudioClips.Add("sfx_one", Resources.Load<AudioClip>("Sounds/1!"));
            _dictAudioClips.Add("sfx_two", Resources.Load<AudioClip>("Sounds/2!"));
            _dictAudioClips.Add("sfx_three", Resources.Load<AudioClip>("Sounds/3!"));
            _dictAudioClips.Add("sfx_are_you_ready", Resources.Load<AudioClip>("Sounds/Are You Ready"));
            _dictAudioClips.Add("sfx_oops", Resources.Load<AudioClip>("Sounds/oops"));
            _dictAudioClips.Add("sfx_stop", Resources.Load<AudioClip>("Sounds/stop!"));
            _dictAudioClips.Add("sfx_to_the_left", Resources.Load<AudioClip>("Sounds/To the left"));
            _dictAudioClips.Add("sfx_to_the_right", Resources.Load<AudioClip>("Sounds/To the right"));
            _dictAudioClips.Add("sfx_whistle1", Resources.Load<AudioClip>("Sounds/Whistle1"));
            _dictAudioClips.Add("sfx_whistle2", Resources.Load<AudioClip>("Sounds/Whistle2"));
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
        public void PlayTouchEffect() 
        {
            Managers.Sound.PlayEffect("touch");
        }
    }
