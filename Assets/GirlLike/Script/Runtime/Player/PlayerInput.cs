using System;
using Orb.GirlLike.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerInput : MonoBehaviour
  {
    public Controller controller;
    public bool blockPauseGame;

    [Header("Events")]
    public UnityEvent onShow;
    public UnityEvent onHidden;
    public UnityEvent onPauseGame;
    public UnityEvent onResumeGame;
    private Player player;
    private bool isPause;

    private void Start()
    {
      player = GetComponent<Player>();
      var movement = player.Movement;
      var combat = player.Combat;
      var bag = player.Bag;

      controller.pauseGame.onUpdate += PauseGame;
      controller.jump.onUpdate += movement.Jump;
      controller.horizontal.onUpdate += movement.SetMoveAxis;
      controller.attack.onUpdate += combat.Attack;
      controller.dash.onUpdate += combat.Dash;
      controller.interactive.onUpdate += player.Interactive;
      controller.useItem.onUpdate += bag.UseItem;
      controller.scroll.onUpdate += SwitchItem;
      controller.removeItem.onUpdate += RemoveItem;
    }

    private void PauseGame(ActionState state)
    {
      if (state != ActionState.Down || isPause || blockPauseGame) return;

      Disable(true);
      isPause = true;
      Time.timeScale = 0;
      onPauseGame.Invoke();
    }

    public void ResumeGame()
    {
      if (!isPause) return;

      Disable(false);
      isPause = false;
      Time.timeScale = 1;
      onResumeGame.Invoke();
    }

    private void RemoveItem(ActionState state)
    {
      if (state == ActionState.Down)
      {
        player.Bag.StartDestroySelectedItem();
      }
      else if (state == ActionState.Up)
      {
        player.Bag.CancelDestroySelectedItem();
      }
    }

    private void SwitchItem(float value)
    {
      if (value > 0)
      {
        player.Bag.NextItem();
      }
      else if (value < 0)
      {
        player.Bag.PreviousItem();
      }
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
