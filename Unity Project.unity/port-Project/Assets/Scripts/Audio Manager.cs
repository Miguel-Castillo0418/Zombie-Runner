using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("-----Sounds-----")]
    public Sound[] music, zDead;
    public AudioClip[] zombieSFX, playerJump, playerHurt, playerWalk, explosion;
    [Header("-----Source-----")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource zSFXSource;
    [SerializeField] AudioSource pSFXSource;
    [Header("-----Volume-----")]
    [SerializeField] float musicVol;
    [SerializeField] float zomBVol;
    [SerializeField] float jumpVol;
    [SerializeField] float hurtVol;
    [SerializeField] float walkVol;
    [SerializeField] float exploVol;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void playMusic(string name)
    {
        Sound song = Array.Find(music, x => x.Name == name);
        if (song == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            MusicSource.clip = song.clip;
            MusicSource.volume = musicVol;
            MusicSource.Play();
        }
    }

    public void playZombie()
    {
        if (!EnemyAI.isSound)
            StartCoroutine(ZombieSound());
    }
    public void stopSound()
    {
        zSFXSource.Stop();
    }
    public void zombDeath(string name)
    {
        Sound dead = Array.Find(zDead, x => x.Name == name);
        if (dead == null)
        {
            Debug.Log("Sound Not Found(1)");
        }
        else
        {
            zSFXSource.clip = dead.clip;
            zSFXSource.Play();
        }
    }
    IEnumerator ZombieSound()
    {
        EnemyAI.isSound = true;
        zSFXSource.PlayOneShot(zombieSFX[UnityEngine.Random.Range(0, zombieSFX.Length)], zomBVol);
        yield return new WaitForSeconds(5.5f);
        EnemyAI.isSound = false;
    }
    public void jumpSound()
    {
        pSFXSource.PlayOneShot(playerJump[UnityEngine.Random.Range(0, playerJump.Length)], jumpVol);
    }
    public void hurtSound()
    {
        pSFXSource.PlayOneShot(playerHurt[UnityEngine.Random.Range(0, playerHurt.Length)], hurtVol);
    }
    public void walkSound()
    {
        pSFXSource.PlayOneShot(playerWalk[UnityEngine.Random.Range(0, playerWalk.Length)], walkVol);
    }
    public void explosionSound()
    {
        pSFXSource.PlayOneShot(explosion[UnityEngine.Random.Range(0, explosion.Length)], exploVol);
    }
}