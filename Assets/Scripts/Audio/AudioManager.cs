using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sfxPlayer;

    [SerializeField] float fadeDuration = 0.75f;

    float originalMusicVol;

    public static AudioManager i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        originalMusicVol = bgmPlayer.volume;
    }

    public void PlayMusic(AudioClip clip, bool loop=true, bool fade=false)
    {
        if (clip == null) return;
        StartCoroutine(PlayMusicAsync(clip, loop, fade));
    }

    IEnumerator PlayMusicAsync(AudioClip clip, bool loop=true, bool fade=false)
    {
        if (fade)
            yield return bgmPlayer.DOFade(0, fadeDuration).WaitForCompletion();

        bgmPlayer.clip = clip;
        bgmPlayer.loop = loop;
        bgmPlayer.Play();

        if (fade)
            yield return bgmPlayer.DOFade(originalMusicVol, fadeDuration).WaitForCompletion();
    }

}
