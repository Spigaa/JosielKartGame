using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixerAudio, mixerMusic;
    public Slider mixerAudioSlider, mixerMusicSlider;

    public void SetVol (float sliderValue)
    {
        mixerAudio.SetFloat("AudioMaster", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVol(float sliderValue)
    {
        mixerMusic.SetFloat("MasterMusic", Mathf.Log10(sliderValue) * 20);
    }
}