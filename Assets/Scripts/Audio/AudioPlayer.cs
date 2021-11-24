using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioClip _throw;
    [SerializeField] private AudioClip _fail;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _hit;
    
    private AudioSource _audioSource;
    private bool _isEndedBackgroundMusic;
    private float _startVolumeBackground;
    private float _volume;
    private Tweener _tween;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _startVolumeBackground = _backgroundSource.volume;
        _isEndedBackgroundMusic = false;
        _volume = 0.8f;
    }

    public void PlayThrowSound()
    {
        _audioSource.clip = _throw;
        _audioSource.volume = _volume;
        _audioSource.Play();
    }
    
    public void PlayFailSound()
    {
        _audioSource.clip = _fail;
        _audioSource.volume = 0.8f;
        _audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        _tween = _backgroundSource.DOFade(0.05f, 2.5f);
    }

    public void PlayBackgroundMusic()
    {
        _tween.Kill();
        _backgroundSource.volume = _startVolumeBackground;
    }

    public void PlayWinSound()
    {
        _audioSource.volume = 1;
        _audioSource.clip = _win;
        _audioSource.Play();
    }
    
    public void StopWinSound()
    {
        _audioSource.Stop();
    }

    public void PlayHit()
    {
        _audioSource.volume = 0.04f;
        _audioSource.clip = _hit;
        _audioSource.Play();
    }
}
