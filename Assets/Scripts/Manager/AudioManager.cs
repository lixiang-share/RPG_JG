using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class AudioManager {
        private AudioSource audio;
        private Dictionary<string, AudioClip> sounds = new Dictionary<string,AudioClip>();
        public static AudioManager instance = null;

        //各种音频的Key值
        public const string AUDIO_BTN = "audio_btn";

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                   instance = new AudioManager();
                }
               return instance;
            }
        }

        private AudioManager()
        {
            Debug.Log(this);
            LoadAllAudio();
        }

        public void LoadAllAudio()
        {
            //加载按钮音乐
            Add(AUDIO_BTN, LoadAudioClip("Sounds/audio_btn"));
        }

        public AudioClip LoadAudioClip(string path)
        {
            AudioClip ac = Get(path);
            if (ac == null)
            {
                ac = (AudioClip)Resources.Load(path, typeof(AudioClip));
            }
            return ac;
        }

        /// <summary>
        /// 添加一个声音
        /// </summary>
        void Add(string key, AudioClip value) {
            if (sounds.ContainsKey(key) ||!key.isValid()|| value == null) return;
            sounds.Add(key, value);
        }

        /// <summary>
        /// 获取一个声音
        /// </summary>
        AudioClip Get(string key) {
            if (!sounds.ContainsKey(key)) return null;
            return sounds[key] as AudioClip;
        }

 /*       /// <summary>
        /// 是否播放背景音乐，默认是1：播放
        /// </summary>
        /// <returns></returns>
        public bool CanPlayBackSound() {
            string key = AppConst.AppPrefix + "BackSound";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="canPlay"></param>
        public void PlayBacksound(string name, bool canPlay) {
            if (audio.clip != null) {
                if (name.IndexOf(audio.clip.name) > -1) {
                    if (!canPlay) {
                        audio.Stop();
                        audio.clip = null;
                        Util.ClearMemory();
                    }
                    return;
                }
            }
            if (canPlay) {
                audio.loop = true;
                audio.clip = LoadAudioClip(name);
                audio.Play();
            } else {
                audio.Stop();
                audio.clip = null;
              //  Util.ClearMemory();
            }
        }

        /// <summary>
        /// 是否播放音效,默认是1：播放
        /// </summary>
        /// <returns></returns>
        public bool CanPlaySoundEffect() {
            string key = AppConst.AppPrefix + "SoundEffect";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }

        /// <summary>
        /// 播放音频剪辑
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        public void Play(AudioClip clip, Vector3 position) {
            if (!CanPlaySoundEffect()) return;
            AudioSource.PlayClipAtPoint(clip, position); 
        }
        */
        public void PlayBtnSounds()
        {
            if (sounds.ContainsKey(AUDIO_BTN))
            {
                NGUITools.PlaySound(sounds[AUDIO_BTN], PlayerSettingMgr.Instance.BtnVolume);
            }
        }
    }
