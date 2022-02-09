using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Sound {
    public string name;
    public AudioSource source;
    public Action<Sound> callback;

    //����
    public float volume {
        get {
            return source.volume;
        }
        set {
            source.volume = value;
        }
    }

    //�������Ž���
    public float progress {
        get {
            if (null == source.clip)
                return 0f;
            return (float)source.timeSamples / (float)source.clip.samples;
        }
    }
    ///���������ɲ��ţ��򷵻�true
    ///ѭ������ ���� false
    public bool finished {
        get {
            return !source.loop && 1f <= progress;
        }
    }
    // ����
    public bool playing {
        get {
            return source.isPlaying;
        }
        set {
            PlayOrPause(value);
        }
    }
    /// <summary>
    /// ע����Ƶ
    /// </summary>
    /// <param name="newName">��Ƶ���� �����������Ƶ</param>   
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
    /// ���Ż�����ͣ 
    /// </summary>
    /// <param name="play">���Ż�����ͣ</param>
    public void PlayOrPause(bool play) {
        if (play) {
            if (!source.isPlaying) {
                source.Play();
            }
        } else {
            source.Pause();
        }
    }
    ///���������ʱִ�б�Ҫ�Ĳ���
    public void Finish() {
        PlayOrPause(false);
        if (null != callback) {
            callback(this);
        }
        this.Destroy();
    }
    //���¿�ʼ����
    public void Reset() {
        source.time = 0f;
    }
    //�ͷ�
    public void Destroy() {
        if (null != source) {
            MonoBehaviour.Destroy(source);
        }
        source = null;
        AudioManager.Instance.RemoveSound(this);
    }
}