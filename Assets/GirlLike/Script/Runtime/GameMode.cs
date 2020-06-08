using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Gameplay.Effect;
using System.Collections;

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
    [SerializeField] private CinemachineVirtualCamera freeCam;
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

      characterType = GetSaveCharacterType();
      SpawnPlayer();
    }

    private void Start()
    {
      SetupPlayer();
    }

    public void EnableFreezeCam(bool isEnable)
    {
      cam.enabled = !isEnable;
    }

    public void SetCharacter(PlayerCharacterType character, float delay = 0)
    {
      if (characterType == character) return;
      characterType = character;
      Destroy(currentPlayer.gameObject);
      currentPlayer = null;
      StartCoroutine(SpawnDelay(delay));
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
      player.DisablePauseGame(isDisableHUDAndCombat);
      player.DisableCombat(isDisableHUDAndCombat);
      cam.Follow = player.GetTransform();

      if (freeCam != null) freeCam.Follow = player.GetTransform();
    }

    public PlayerCharacterType GetSaveCharacterType()
    {
      return (PlayerCharacterType)PlayerPrefs.GetInt("CharacterType", 0);
    }

    private void SaveCharacterType()
    {
      PlayerPrefs.SetInt("CharacterType", (int)characterType);
    }

    private IEnumerator SpawnDelay(float delay)
    {
      yield return new WaitForSeconds(delay);
      SpawnPlayer();
      SetupPlayer();
      SaveCharacterType();
      onSetCharacter.Invoke();
    }

    public enum PlayerCharacterType
    {
      Jana, Diana, Thay
    }
  }
}
