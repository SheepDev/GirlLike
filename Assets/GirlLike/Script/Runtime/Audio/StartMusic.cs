using UnityEngine;

namespace Orb.GirlLike.Audio
{
  public class StartMusic : MonoBehaviour
  {
    public AudioClip music;

    private void Start()
    {
      AudioManager.Current.PlayMusic(music);
    }
  }
}
