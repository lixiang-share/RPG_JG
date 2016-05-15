using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class AudioManager : MonoBehaviour{
        public static AudioManager instance = null;
        //各种音频的Key值
        public const string AUDIO_BTN = "audio_btn";
        public const string Man_Comm_Attack01 = "man_attack_01";
        public const string Man_Comm_Attack02 = "man_attack_02";
        public const string Man_Comm_Attack03 = "man_attack_03";
        public const string Hurt = "hurt";
        public const string SKill_One = "skill_one";
        public const string Skill_Two = "ice_attack";
        private AudioSource audio;
        private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();


        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                    instance.Init();
                }
                return instance;
            }
        }
        public void Init()
        {
            LoadAllAudio();
        }

        public void LoadAllAudio()
        {
            Add(AUDIO_BTN);
            Add(Man_Comm_Attack01);
            Add(Man_Comm_Attack02);
            Add(Man_Comm_Attack03);
            Add(Hurt);
            Add(SKill_One);
            Add(Skill_Two);
        }

        private void Add(string name)
        {
            Add(name, LoadAudioClip(name));
        }
        /// <summary>
        /// 添加一个声音
        /// </summary>
        void Add(string key, AudioClip value)
        {
            if (sounds.ContainsKey(key) || !UITools.isValidString(key) || value == null) return;
            sounds.Add(key, value);
        }
        private AudioClip LoadAudioClip(string name)
        {
            string path = "Sounds/" + name;
            AudioClip ac = Get(path);
            if (ac == null)
            {
                ac = (AudioClip)Resources.Load(path, typeof(AudioClip));
            }
            return ac;
        }

        /// <summary>
        /// 获取一个声音
        /// </summary>
        AudioClip Get(string key)
        {
            if (!sounds.ContainsKey(key)) return null;
            return sounds[key] as AudioClip;
        }


        public void PlayAudio(string audioName)
        {
            if (sounds.ContainsKey(audioName))
            {
                NGUITools.PlaySound(sounds[audioName], PlayerSettingMgr.Instance.BtnVolume);
            }
        }
     
    }
