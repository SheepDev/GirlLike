using Orb.GirlLike.Controllers;
using Orb.GirlLike.Itens;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerInput : MonoBehaviour
  {
    public Controller controller;

    [Header("Events")]
    public UnityEvent onShow;
    public UnityEvent onHidden;
    private PlayerAnimator animator;

    public ItemPickup pickup { get; private set; }

    private void Awake()
    {
      animator = GetComponent<PlayerAnimator>();
      var movement = GetComponent<PlayerMovement>();
      var combat = GetComponent<PlayerCombatSystem>();
      pickup = GetComponentInChildren<ItemPickup>();

      controller.jump.onUpdate += movement.Jump;
      controller.horizontal.onUpdate += movement.SetMoveAxis;
      controller.attack.onUpdate += combat.Attack;
      controller.dash.onUpdate += combat.Dash;
      controller.interactive.onUpdate += Interactive;
    }

    private void Interactive(ActionState state)
    {
      if (state != ActionState.Down) return;

      pickup.Pickup();
    }

    public void Hidden(bool isHidden)
    {
      controller.enabled = !isHidden;
      if (isHidden)
        onHidden.Invoke();
      else
        onShow.Invoke();
    }
  }
}
