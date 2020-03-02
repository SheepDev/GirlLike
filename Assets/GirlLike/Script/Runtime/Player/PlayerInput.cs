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

      controller.jump.onUpdate += movement.Jump;
      controller.horizontal.onUpdate += movement.SetHorizontal;
    }
  }
}
