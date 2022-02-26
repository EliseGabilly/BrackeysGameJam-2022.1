using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio system support sound
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {

    #region Variable
    private static AudioSource _musicSource;
    private static AudioSource _soundsSource;

    [SerializeField]
    private List<AudioClip> music;
    private int musicIndex = 0;
    [SerializeField]
    private AudioClip hit;
    [SerializeField]
    private AudioClip tp;
    [SerializeField]
    private AudioClip btn;
    [SerializeField]
    private AudioClip next;
    #endregion

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        _musicSource = GetComponentsInChildren<AudioSource>()[0];
        _soundsSource = GetComponentsInChildren<AudioSource>()[1];

        _musicSource.clip = music[musicIndex];
        _musicSource.Play();
        TurnMusicOn(Player.isSoundOn);
    }

    private void Update() {
        if (!_musicSource.isPlaying) {
            PlayNextSong();
        }
    }

    public void TurnMusicOn(bool isOn) {
        _musicSource.volume = isOn ? 0.5f : 0;
    }

    public void TurnSoundOn(bool isOn) {
        _soundsSource.volume = isOn ? 0.5f : 0;
    }

    private void PlayNextSong() {
        musicIndex = (musicIndex + 1) % music.Count;
        _musicSource.clip = music[musicIndex];
        _musicSource.Play();
    }

    public void PlayMusic(AudioClip clip) {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        _soundsSource.PlayOneShot(clip, vol);
    }

    public void PlayHit() {
        PlaySound(hit);
    }

    public void PlayTP() {
        PlaySound(tp);
    }

    public void PlayBtn() {
        PlaySound(btn);
    }

    public void PlayNext() {
        PlaySound(next);
    }
}