using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("-----Sounds-----")]
    public Sound[] music;
    public Sound[] zDead;
    public Sound[] click;
    public Sound[] purchase;
    public AudioClip[] zombieSFX, playerJump, playerHurt, doors;
    public AudioClip playerWalk, explosion, errorAud, keys, swordSwing, gunClick, reloading;
    [Header("-----Source-----")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource zSFXSource;
    [SerializeField] AudioSource pSFXSource;
    [Header("-----Volume-----")]
    [SerializeField] [Range(0, 1.0f)] float musicVol;
    [SerializeField] [Range(0, 1.0f)] float zomBVol;
    [SerializeField] [Range(0, 1.0f)] float jumpVol;
    [SerializeField] [Range(0, 1.0f)] float hurtVol;
    [SerializeField] [Range(0, 1.0f)] float walkVol;
    [SerializeField] [Range(0, 1.0f)] float exploVol;
    [SerializeField] [Range(0, 1.0f)] float doorsVol;
    [SerializeField] [Range(0, 1.0f)] float swordVol;
    [SerializeField] [Range(0, 1.0f)] float damageVol;
    [SerializeField][Range(0, 1.0f)] float gunVol;

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
    public void clickSound(string name)
    {
        Sound Click = Array.Find(click, x => x.Name == name);
        if(Click == null)
        {
            Debug.Log("Sound Not Found(2)");
        }
        else
        {
            pSFXSource.clip = Click.clip;
            pSFXSource.Play();
        }
    }
    public void purchaseSound(string name)
    {
        Sound Purch = Array.Find(purchase, x => x.Name == name);
        if(Purch == null)
        {
            Debug.Log("Sound Not Found(3)");
        }
        else
        {
            pSFXSource.clip = Purch.clip;
            pSFXSource.Play();
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
        pSFXSource.PlayOneShot(playerWalk, walkVol);
    }
    public void explosionSound()
    {
        pSFXSource.PlayOneShot(explosion, exploVol);
    }
    public void closeDoor()
    {
        pSFXSource.PlayOneShot(doors[0], doorsVol);
    }
    public void openDoor()
    {
        pSFXSource.PlayOneShot(doors[1], doorsVol);
    }
    public void doorLocked()
    {
        pSFXSource.PlayOneShot(doors[2], doorsVol);
    }
    public void keyPickup()
    {
        pSFXSource.PlayOneShot(keys, doorsVol);
    }
    public void error()
    {
        pSFXSource.PlayOneShot(errorAud, doorsVol);
    }
    public void swordElement(AudioClip clip)
    {
        pSFXSource.PlayOneShot(swordSwing, swordVol);
        pSFXSource.PlayOneShot(clip, swordVol);
    }
    public void elementalDamage(AudioClip clip)
    {
        pSFXSource.PlayOneShot(clip, damageVol);
    }
    public IEnumerator gunEmpty(AudioSource source, float rate)
    {
        source.PlayOneShot(gunClick, gunVol);
        yield return new WaitForSeconds(6);
        
 
    }
    public void reloadSound(AudioSource source)
    {
        source.PlayOneShot(reloading, gunVol);
    }
}