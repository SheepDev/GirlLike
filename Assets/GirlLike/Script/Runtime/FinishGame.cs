using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Playables;

namespace Orb.GirlLike
{
  public class FinishGame : MonoBehaviour
  {
    public PlayableDirector playableDirector;
    private Player Player => GameMode.Current.GetPlayer();

    public void Finish()
    {
      Player.HiddenHUD(true);
      Time.timeScale = 0;
      playableDirector.Play();
    }
  }
}
