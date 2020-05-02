using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Orb.GirlLike.Audio
{
  public class AudioManager : MonoBehaviour
  {
    [SerializeField] private AudioMixer mixer;

    [Header("Events")]
    public UnityEvent onMute;
    public UnityEvent onUnmute;

    private bool isMute;

    public void ToggleMute()
    {
      if (isMute)
        Unmute();
      else
        Mute();
    }

    public void Mute()
    {
      mixer.SetFloat("Master", 0);
    }

    public void Unmute()
    {
      mixer.SetFloat("Master", 1);
    }
  }
}
