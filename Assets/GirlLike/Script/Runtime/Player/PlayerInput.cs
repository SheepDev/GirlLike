using Orb.GirlLike.Controllers;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerInput : MonoBehaviour
  {
    public Controller controller;

    private void Awake()
    {
      var movement = GetComponent<PlayerMovement>();
      var combat = GetComponent<PlayerCombatSystem>();

      controller.jump.onUpdate += movement.Jump;
      controller.horizontal.onUpdate += movement.SetMoveAxis;
      controller.attack.onUpdate += combat.Attack;
      controller.dash.onUpdate += combat.Dash;
    }
  }
}
