using UnityEngine;
using static Orb.GirlLike.GameMode;

namespace Orb.GirlLike.Players
{
  public class CharacterSelect : InteractivePlayer
  {
#pragma warning disable CS0649
    [SerializeField] private PlayerCharacterType characterType;
#pragma warning restore CS0649
    private Animator animator;
    private bool isAvaliable;

    private void Start()
    {
      animator = GetComponentInChildren<Animator>();
      CheckAvaliable();

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
      GameMode.Current.SetCharacter(characterType);
    }

    public void Avaliable(bool isAvaliable)
    {
      this.isAvaliable = isAvaliable;
      animator.Play(isAvaliable ? "Idle" : "Empty");
    }
  }
}
