using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    NORMAL,
    SFX_HIT,
    END
}

public class SoundManager 
    : Singleton<SoundManager>
{
    

    [System.Serializable]
    public struct SoundInfo
    {
        public string      name;
        public AudioClip   audio;
    }

    [SerializeField]
    private SoundInfo[] bgmSound;
    [SerializeField]
    private SoundInfo[] sfxSound;

    private Dictionary<string, AudioClip> bgmAudioClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxAudioClips = new Dictionary<string, AudioClip>();

    private AudioSource                        bgmPlayer;
    private Dictionary<SFXType, AudioSource[]> sfxPlayer = new Dictionary<SFXType, AudioSource[]>();

    protected override void Initialized()
    {
        base.Initialized();

        Transform bgm     = transform.Find("BGM");
        Transform sfx     = transform.Find("SFX");
        Transform sfx_hit = transform.Find("SFX_Hit");

        if (bgm)
        {
            AudioSource audioSource = bgm.GetComponent<AudioSource>();
            bgmPlayer = new AudioSource();
            bgmPlayer = bgm.GetComponent<AudioSource>();
        }

        if (sfx)
        {
            AudioSource[] audioSources = sfx.GetComponents<AudioSource>();
            AudioSource[] audio = new AudioSource[audioSources.Length];
            audio = sfx.GetComponents<AudioSource>();
            sfxPlayer.Add(SFXType.NORMAL, audio);
        }

        if (sfx_hit)
        {
            AudioSource[] audioSources = sfx_hit.GetComponents<AudioSource>();
            AudioSource[] audio = new AudioSource[audioSources.Length];
            audio = sfx_hit.GetComponents<AudioSource>();
            sfxPlayer.Add(SFXType.SFX_HIT, audio);
        }

        foreach (SoundInfo elem in bgmSound)
            bgmAudioClips.Add(elem.name, elem.audio);

        foreach (SoundInfo elem in sfxSound)
            sfxAudioClips.Add(elem.name, elem.audio);
    }

    public void PlayBGM(string _SoundName)
    {
        if (!bgmAudioClips.ContainsKey(_SoundName))
            return;

        bgmPlayer.clip = bgmAudioClips[_SoundName];
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string _SoundName, SFXType _Type = SFXType.NORMAL)
    {
        if (!sfxAudioClips.ContainsKey(_SoundName))
            return;

        if (!sfxPlayer.ContainsKey(_Type))
            return;

        for (int i = 0; i < sfxPlayer[_Type].Length; ++i)
        {
            if (sfxPlayer[_Type][i].isPlaying)
                continue;

            sfxPlayer[_Type][i].clip = sfxAudioClips[_SoundName];
            sfxPlayer[_Type][i].Play();

            return;
        }

        //Debug.Log("All SFX Use");
    }

    private void Update()
    {
        if (!bgmPlayer.isPlaying)
            PlayBGM("MainBGM");
    }
}
