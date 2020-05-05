using Orb.GirlLike.Audio;
using UnityEngine;

namespace Orb.GirlLike
{
  public class Radio : MonoBehaviour
  {
    private bool isMute;

    public Animator Animator { get; private set; }

    private void Awake()
    {
      Animator = GetComponent<Animator>();
    }

    private void Update()
    {
      var isMute = AudioManager.Current.IsMute;
      if (this.isMute != isMute)
      {
        this.isMute = isMute;
        PlayAnimation();
      }
    }

    private void PlayAnimation()
    {
      Animator.Play(isMute ? "Mute" : "Unmute");
    }
  }
}
