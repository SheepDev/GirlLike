using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class CharacterSelect : InteractivePlayer
  {
    private Animator animator;
    [SerializeField] private bool isAvaliable;

    private void Awake()
    {
      animator = GetComponentInChildren<Animator>();
      Avaliable(isAvaliable);
    }

    public override void Interactive(Player player)
    {
      if (!isAvaliable) return;
      Avaliable(false);
    }

    public void Avaliable(bool isAvaliable)
    {
      this.isAvaliable = isAvaliable;
      animator.Play(isAvaliable ? "Loop" : "Empty");
    }
  }
}
