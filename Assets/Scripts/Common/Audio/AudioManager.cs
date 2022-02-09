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
    /// 播放一个新的声音，注册它，给它指定的属性，并开始播放它
    /// </summary>
    /// <param name="名字"></param>
    /// <param name="循环"></param>
    /// <param name="中断其他"></param>
    /// <param name="行为"></param>
    /// <returns></returns>
    public Sound PlayNewSound(string soundName, bool loop = false, Action<Sound> callback = null) {
        Sound sound = NewSound(soundName, loop, callback);
        sound.playing = true;
        return sound;
    }

    /// <summary>
    /// 创建一个新的声音，注册它，并给它指定的属性 不播放
    /// </summary>
    /// <param name="名字"></param>
    /// <param name="循环"></param>
    /// <param name="中断其他"></param>
    /// <param name="行为"></param>
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

    //注册一个音频播放组件
    public void RegisterSound(Sound sound) {
        sounds.Add(sound);
    }

    //移除一个音频播放组件
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