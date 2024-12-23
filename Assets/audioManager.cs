using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    private int bgmIdx;

    private void Awake() => audioManager = this;

    private void Update() {
        if (!bgm[bgmIdx].isPlaying) {
            PlayRandomBGM();
        }
    }

    public void PlaySFX(int idx) {
        if (idx < sfx.Length) {
            sfx[idx].pitch = Random.Range(.85f, 1.15f);
            sfx[idx].Play();
        }
    }

    public void StopSFX(int idx) {
        sfx[idx].Stop();
    }

    public void PlayRandomBGM() {
        bgmIdx = Random.Range(0, bgm.Length);
        PlayBGM(bgmIdx);
    }

    public void PlayBGM(int idx) {
        StopBGM();
        bgm[idx].Play();
    }

    public void StopBGM() {
        for (int i = 0; i < bgm.Length; i++) {
            bgm[i].Stop();
        }
    }
}
