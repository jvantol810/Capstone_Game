using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip meleeAttackSound;
    public AudioClip explosionSound;
    public AudioClip shootWebSound;
    //[System.Serializable]
    //public struct SoundEffect
    //{
    //    public UnityEvent triggeringEvent;
    //    public AudioClip soundClip;
    //    public AudioSource source;
    //    public SoundEffect(UnityEvent triggeringEvent, AudioClip soundClip, AudioSource source)
    //    {
    //        this.triggeringEvent = triggeringEvent;
    //        this.soundClip = soundClip;
    //        this.source = source;

    //        triggeringEvent.AddListener(Play);
    //    }

    //    public void Play()
    //    {
    //        source.PlayOneShot(soundClip);
    //    }
    //}
    //[SerializeField]
    //public List<SoundEffect> SoundEffects;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnMeleeAttack.AddListener(PlayMeleeSound);
        GameEvents.OnBombExplode.AddListener(PlayExplosionSound);
        GameEvents.OnShootWeb.AddListener(PlayWebShootSound);
    }

    public void PlayMeleeSound(AudioSource source)
    {
        source.PlayOneShot(meleeAttackSound);
    }

    public void PlayExplosionSound(AudioSource source)
    {
        source.PlayOneShot(explosionSound);
    }

    public void PlayWebShootSound(AudioSource source)
    {
        Debug.Log("Play shoot web sound!");
        source.PlayOneShot(shootWebSound);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
