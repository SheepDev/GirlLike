using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Orb.GirlLike.Audio
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Current;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float speed;

    [Header("Events")]
    public UnityEvent onMute;
    public UnityEvent onUnmute;
    private float cacheVolume;
    private bool isMute;

    public bool IsMute => isMute;
    public AudioSource Source { get; private set; }

    private void Awake()
    {
      if (Current == null)
      {
        Current = this;
        Source = GetComponent<AudioSource>();
        mixer.GetFloat("MusicVolume", out var value);
        cacheVolume = value;
        DontDestroyOnLoad(this.gameObject);
      }
      else
      {
        Destroy(this.gameObject);
      }
    }

    public void PlayMusic(AudioClip audio)
    {
      if (Source.clip == audio) return;

      if (isMute)
      {
        Source.clip = audio;
      }
      else
      {
        StopAllCoroutines();
        StartCoroutine(SwitchMusic(audio));
      }
    }

    public void ToggleMute()
    {
      if (isMute)
        Unmute();
      else
        Mute();
    }

    public void Mute()
    {
      StopAllCoroutines();
      StartCoroutine(MuteAnimation());
      isMute = true;
      onMute.Invoke();
    }

    public void Unmute()
    {
      StopAllCoroutines();
      StartCoroutine(UnmuteAnimation());
      isMute = false;
      onUnmute.Invoke();
    }

    public void SetVolume(float volume)
    {
      mixer.SetFloat("MusicVolume", volume);
    }

    private IEnumerator SwitchMusic(AudioClip clip)
    {
      yield return MuteAnimation();
      Source.Stop();
      Source.clip = clip;
      Source.Play();
      yield return UnmuteAnimation();
    }

    private IEnumerator MuteAnimation()
    {
      mixer.GetFloat("MusicVolume", out var value);
      var currentVolume = value;

      while (currentVolume > -80f)
      {
        currentVolume -= speed * Time.deltaTime;
        currentVolume = Mathf.Max(currentVolume, -80f);
        SetVolume(currentVolume);
        yield return null;
      }
    }

    private IEnumerator UnmuteAnimation()
    {
      mixer.GetFloat("MusicVolume", out var value);
      var currentVolume = value;

      while (currentVolume < cacheVolume)
      {
        currentVolume += speed * Time.deltaTime;
        currentVolume = Mathf.Min(currentVolume, cacheVolume);
        SetVolume(currentVolume);
        yield return null;
      }
    }
  }
}
