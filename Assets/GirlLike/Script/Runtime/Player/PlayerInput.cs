using Orb.GirlLike.Controllers;
using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerInput : MonoBehaviour
  {
    public Controller controller;

    public ItemPickup pickup { get; private set; }

    private void Awake()
    {
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
  }
}
