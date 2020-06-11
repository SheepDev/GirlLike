using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Playables;

namespace Orb.GirlLike
{
  public class FinishGame : MonoBehaviour
  {
    public PlayableDirector playableDirector;
    private Player player;

    private void Start()
    {
      player = GameMode.Current.GetPlayer();
    }

    public void Finish()
    {
      player.HiddenHUD(true);
      Time.timeScale = 0;
      playableDirector.Play();
    }
  }
}
