using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Sound {
    public string name;
    public AudioSource source;
    public Action<Sound> callback;

    //音量
    public float volume {
        get {
            return source.volume;
        }
        set {
            source.volume = value;
        }
    }

    //声音播放进度
    public float progress {
        get {
            if (null == source.clip)
                return 0f;
            return (float)source.timeSamples / (float)source.clip.samples;
        }
    }
    ///如果声音完成播放，则返回true
    ///循环声音 返回 false
    public bool finished {
        get {
            return !source.loop && 1f <= progress;
        }
    }
    // 播放
    public bool playing {
        get {
            return source.isPlaying;
        }
        set {
            PlayOrPause(value);
        }
    }
    /// <summary>
    /// 注册音频
    /// </summary>
    /// <param name="newName">音频名字 必须有这个音频</param>   
    public Sound(string newName, bool loop = false, Action<Sound> callback = null) {
        name = newName;
        source = AudioManager.Instance.CreateAudioSource();
        source.clip = (AudioClip)Resources.Load("Audio/" + name, typeof(AudioClip));
        if (null == source.clip) {
            Debug.Log(name + " is not found!");
            return;
        }
        source.loop = loop;
        this.callback = callback;
    }
    public void ChangeClip(string newName) {
        if (newName == name) {
            return;
        }
        name = newName;
        source.clip = (AudioClip)Resources.Load("Audio/" + name, typeof(AudioClip));
        if (null == source.clip) {
            Debug.Log(name + " is not found!");
        }
        Reset();
    }
    public void Update() {
        if (finished) {
            Finish();
        }
    }
    /// <summary>
    /// 播放或者暂停 
    /// </summary>
    /// <param name="play">播放或者暂停</param>
    public void PlayOrPause(bool play) {
        if (play) {
            if (!source.isPlaying) {
                source.Play();
            }
        } else {
            source.Pause();
        }
    }
    ///在声音完成时执行必要的操作
    public void Finish() {
        PlayOrPause(false);
        if (null != callback) {
            callback(this);
        }
        this.Destroy();
    }
    //重新开始声音
    public void Reset() {
        source.time = 0f;
    }
    //释放
    public void Destroy() {
        if (null != source) {
            MonoBehaviour.Destroy(source);
        }
        source = null;
        AudioManager.Instance.RemoveSound(this);
    }
}