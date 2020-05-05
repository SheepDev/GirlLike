using UnityEngine;
using static Orb.GirlLike.GameMode;

namespace Orb.GirlLike.Players
{
  public class CharacterSelect : InteractivePlayer
  {
#pragma warning disable CS0649
    [SerializeField] private float delaySpawn;
    [SerializeField] private PlayerCharacterType characterType;
#pragma warning restore CS0649
    private Animator animator;
    private bool isAvaliable;

    private void Start()
    {
      animator = GetComponentInChildren<Animator>();
      CheckAvaliable();
      if (characterType == GameMode.Current.GetSaveCharacterType())
      {
        animator.Play("Empty");
      }

      var gameMode = GameMode.Current;
      gameMode.onSetCharacter
        .AddListener(() => CheckAvaliable());
    }

    private void CheckAvaliable()
    {
      Avaliable(GameMode.Current.CurrentCharacterType != characterType);
    }

    public override void Interactive(Player player)
    {
      if (!isAvaliable) return;
      Avaliable(false);
      GameMode.Current.SetCharacter(characterType, delaySpawn);
    }

    public void Avaliable(bool isAvaliable)
    {
      if (this.isAvaliable == isAvaliable) return;

      this.isAvaliable = isAvaliable;
      animator.Play(isAvaliable ? "Idle" : "Selected");
    }
  }
}
