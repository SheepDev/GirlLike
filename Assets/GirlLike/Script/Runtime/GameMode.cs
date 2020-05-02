using Orb.GirlLike.Players;
using UnityEngine;
using Cinemachine;
using Gameplay.Effect;

namespace Orb.GirlLike
{
  public class GameMode : MonoBehaviour
  {
    private static GameMode Current;

#pragma warning disable CS0649
    [SerializeField] private Transform playerStart;
    [SerializeField] private Parallax parallax;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Player[] playerCharactes;
#pragma warning restore CS0649
    private Player currentPlayer;

    private void Awake()
    {
      if (Current == null)
        Current = this;
      else
        Destroy(this);

      var player = GetPlayer();
      player.Movement.parallax = parallax;
      cam.Follow = player.GetTransform();
    }

    public Player GetPlayer()
    {
      if (currentPlayer == null)
      {
        currentPlayer = Instantiate(GetPlayerPrefab());
      }

      return currentPlayer;
    }

    private Player GetPlayerPrefab()
    {
      var playerCharacter = (PlayerCharacter)PlayerPrefs.GetInt("PlayerCharacter", 0);

      switch (playerCharacter)
      {
        case PlayerCharacter.Jana:
          return playerCharactes[0];
        case PlayerCharacter.Diana:
          return playerCharactes[1];
        case PlayerCharacter.Thay:
          return playerCharactes[2];
        default:
          throw new System.Exception("Player Prefab not found");
      }
    }

    public enum PlayerCharacter
    {
      Jana, Diana, Thay
    }
  }
}
