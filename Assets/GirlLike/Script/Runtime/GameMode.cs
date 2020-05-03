using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Gameplay.Effect;

namespace Orb.GirlLike
{
  public class GameMode : MonoBehaviour
  {
    public static GameMode Current;
    public bool isDisableHUDAndCombat;

    public UnityEvent onSetCharacter;
#pragma warning disable CS0649
    [SerializeField] private Transform playerStart;
    [SerializeField] private Parallax parallax;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Player[] playerCharactes;
#pragma warning restore CS0649
    private Player currentPlayer;
    private PlayerCharacterType characterType;
    public PlayerCharacterType CurrentCharacterType => characterType;

    private void Awake()
    {
      if (Current == null)
        Current = this;
      else
        Destroy(this);

      characterType = (PlayerCharacterType)PlayerPrefs.GetInt("CharacterType", 0);
      SpawnPlayer();
    }

    private void Start()
    {
      SetupPlayer();
    }

    public void SetCharacter(PlayerCharacterType character)
    {
      if (characterType == character) return;
      characterType = character;
      Destroy(currentPlayer.gameObject);
      currentPlayer = null;
      SpawnPlayer();
      SetupPlayer();
      SaveCharacterType();
      onSetCharacter.Invoke();
    }

    public Player GetPlayer()
    {
      if (currentPlayer == null)
      {
        currentPlayer = Instantiate(GetPlayerPrefab(characterType));
      }

      return currentPlayer;
    }

    public void CloseGame()
    {
      Application.Quit(0);
    }

    private void SpawnPlayer()
    {
      var player = GetPlayer();
      var playerTransform = player.GetTransform();
      playerTransform.position = playerStart.position;
    }

    private Player GetPlayerPrefab(PlayerCharacterType characterType)
    {
      switch (characterType)
      {
        case PlayerCharacterType.Jana:
          return playerCharactes[0];
        case PlayerCharacterType.Diana:
          return playerCharactes[1];
        case PlayerCharacterType.Thay:
          return playerCharactes[2];
        default:
          throw new System.Exception("Player Prefab not found");
      }
    }

    private void SetupPlayer()
    {
      var player = GetPlayer();
      player.Movement.parallax = parallax;
      player.HiddenHUD(isDisableHUDAndCombat);
      player.DisableCombat(isDisableHUDAndCombat);
      cam.Follow = player.GetTransform();
    }

    private void SaveCharacterType()
    {
      PlayerPrefs.SetInt("CharacterType", (int)characterType);
    }

    public enum PlayerCharacterType
    {
      Jana, Diana, Thay
    }
  }
}
