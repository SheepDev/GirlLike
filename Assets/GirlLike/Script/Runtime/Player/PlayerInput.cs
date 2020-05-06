using Orb.GirlLike.Controllers;
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
    private Player player;

    private void Start()
    {
      player = GetComponent<Player>();
      var movement = player.Movement;
      var combat = player.Combat;
      var bag = player.Bag;

      controller.jump.onUpdate += movement.Jump;
      controller.horizontal.onUpdate += movement.SetMoveAxis;
      controller.attack.onUpdate += combat.Attack;
      controller.dash.onUpdate += combat.Dash;
      controller.interactive.onUpdate += player.Interactive;
      controller.nextItem.onUpdate += bag.NextItem;
      controller.previousItem.onUpdate += bag.PreviousItem;
      controller.useItem.onUpdate += bag.UseItem;
    }

    public void Disable(bool isDisable)
    {
      controller.enabled = !isDisable;
    }

    public void Hidden(bool isHidden)
    {
      controller.enabled = !isHidden;
      player.Rigidbody.gravityScale = isHidden ? 0 : player.defaultGravity;
      GameMode.Current.EnableFreezeCam(isHidden);

      if (isHidden)
        onHidden.Invoke();
      else
        onShow.Invoke();
    }
  }
}
