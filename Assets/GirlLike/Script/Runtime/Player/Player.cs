using Orb.GirlLike.Controllers;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class Player : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] private OverlapBehaviour overlapInteractive;
#pragma warning restore CS0649

    public PlayerHitPoint HitPoint { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public ItemPickup Pickup { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerCombatSystem Combat { get; private set; }
    public PlayerBag Bag { get; private set; }

    private bool isPickupItem;

    private void Awake()
    {
      Movement = GetComponent<PlayerMovement>();
      Combat = GetComponent<PlayerCombatSystem>();
      Bag = GetComponent<PlayerBag>();
      HitPoint = GetComponent<PlayerHitPoint>();
      Animator = GetComponent<PlayerAnimator>();
      Pickup = GetComponentInChildren<ItemPickup>();
    }

    internal void Interactive(ActionState state)
    {
      if (state != ActionState.Down) return;

      Interactive(overlapInteractive.Overlap());
    }

    private void Interactive(Collider2D[] colliders)
    {
      foreach (var collider in colliders)
      {
        var interactivePlayer = collider.GetComponent<InteractivePlayer>();
        if (interactivePlayer != null)
        {
          interactivePlayer.Interactive(this);
          return;
        }
      }
    }
  }
}
