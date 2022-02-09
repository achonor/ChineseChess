using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : SingleInstance<AudioManager> {
    public HashSet<Sound> sounds =
       new HashSet<Sound>();

    protected Sound backgoundSound;

    protected float volumeValue = 1.0f;
    protected float volumeValueOffset = 0.5f;

    public Sound PlayBackgroundSound(string soundName) {
        if (null == backgoundSound) {
            backgoundSound = new Sound(soundName, true);
        } else {
            backgoundSound.ChangeClip(soundName);
        }
        backgoundSound.playing = true;
        return backgoundSound;
    }

    /// <summary>
    /// ����һ���µ�������ע����������ָ�������ԣ�����ʼ������
    /// </summary>
    /// <param name="����"></param>
    /// <param name="ѭ��"></param>
    /// <param name="�ж�����"></param>
    /// <param name="��Ϊ"></param>
    /// <returns></returns>
    public Sound PlayNewSound(string soundName, bool loop = false, Action<Sound> callback = null) {
        Sound sound = NewSound(soundName, loop, callback);
        sound.playing = true;
        return sound;
    }

    /// <summary>
    /// ����һ���µ�������ע������������ָ�������� ������
    /// </summary>
    /// <param name="����"></param>
    /// <param name="ѭ��"></param>
    /// <param name="�ж�����"></param>
    /// <param name="��Ϊ"></param>
    /// <returns></returns>
    public Sound NewSound(string soundName, bool loop = false, Action<Sound> callback = null) {
        Sound sound = new Sound(soundName, loop, callback);
        RegisterSound(sound);
        return sound;
    }

    public void SetVolume(float value) {
        volumeValue = value;
        float tmpVolume = volumeValue * volumeValueOffset;
        if (null != backgoundSound) {
            backgoundSound.volume = tmpVolume;
        }
        sounds.ToList().ForEach(sound => {
            sound.volume = tmpVolume;
        });
    }

    //ע��һ����Ƶ�������
    public void RegisterSound(Sound sound) {
        sounds.Add(sound);
    }

    //�Ƴ�һ����Ƶ�������
    public void RemoveSound(Sound sound) {
        sounds.Remove(sound);
    }

    private void Update() {
        if (null != backgoundSound) {
            backgoundSound.Update();
        }
        sounds.ToList().ForEach(sound => {
            sound.Update();
        });
    }

    public AudioSource CreateAudioSource() {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volumeValue * volumeValueOffset;
        return audioSource;
    }
}