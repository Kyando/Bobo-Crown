using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [Header("Player")] public AudioSource playerAttackSfx;
    public AudioSource playerHitSfx;
    public AudioSource playerTauntSfx;

    [Header("Enemy")] public AudioSource tigerAttackSfx;
    public AudioSource gladiatorAttackSfx;
    public AudioSource kingLaugh1Sfx;
    [Header("Risada plateia")]
    public AudioSource plateiaSfx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void PlaySound(AudioSource soundToPlay)
    {
        soundToPlay.Play();
    }

    public void SetSoundActive(AudioSource soundToPlay, bool isActive)
    {
        soundToPlay.volume = isActive ? 1 : 0;    
    }
}