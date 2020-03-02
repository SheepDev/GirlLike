
using System;
using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  [Serializable]
  public class Button
  {
    public Action onUpdate;

    internal void CheckState(KeyCode keyCode)
    {
      if (Input.GetKeyDown(keyCode))
      {
        onUpdate.Invoke(ActionState.Down);
      }
      else if (Input.GetKey(keyCode))
      {
        onUpdate.Invoke(ActionState.Stay);
      }
      if (Input.GetKeyUp(keyCode))
      {
        onUpdate.Invoke(ActionState.Up);
      }
    }
  }

  public delegate void Action(ActionState state);

  [Serializable]
  public enum ActionState
  {
    Down, Stay, Up
  }
}