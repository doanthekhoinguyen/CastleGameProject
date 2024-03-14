using System.Collections;
using UnityEngine;

namespace Castle.CustomUtil
{
    public enum SFX
    {
        Coin,
        GameWin,
        GameLose
    }
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource[] sfxSources;
        [SerializeField] private AudioClip[] audioLib;
        
        [SerializeField] private AudioClip[] audioPenalty;

        private bool canPlayPenaltySfx = true;

        public void PlayBgm(bool isPlay = true)
        {
            if (isPlay)
            {
                bgmSource.Play();
            } 
            else
            {
                bgmSource.Stop();
            }
        }
        
        public void PlaySfx(SFX clipName)
        {
            var source = GetSfxSource();
            source.clip = audioLib[(int) clipName];
            source.Play();
        }
        
        public void PlaySfx(AudioClip clip)
        {
            var source = GetSfxSource();
            source.clip = clip;
            source.Play();
        }

        public void PlayCoinSfx()
        {
            PlaySfx(audioLib[(int)SFX.Coin]);
        }

        public void PlayPenaltySfx()
        {
            int randomIndex = RandomUtils.Range(0, audioPenalty.Length);
            PlaySfx(audioPenalty[randomIndex]);
            canPlayPenaltySfx = false;
            StartCoroutine(StartSfxTimer());
        }

        private IEnumerator StartSfxTimer()
        {
            yield return new WaitForSeconds(3);
            canPlayPenaltySfx = true;
        }

        private AudioSource GetSfxSource()
        {
            for (int i = 0; i < sfxSources.Length; i++)
            {
                if (sfxSources[i].isPlaying) continue;

                return sfxSources[i];
            }
            return sfxSources[0];
        }

        // call by ServiceLocator.Instance.AudioManager.SetBGMVolume(0->1);

        public void SetBGMVolume(float value)
        {
            bgmSource.volume = Mathf.Clamp01(value);
        }

        public void SetSFXVolume(float value)
        {
            for (int i = 0; i < sfxSources.Length; i++)
            {
                sfxSources[i].volume = Mathf.Clamp01(value);
            }
        }
    }
}