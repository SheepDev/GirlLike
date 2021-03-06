using System;
using System.Collections.Generic;
using Orb.GirlLike.Controllers;
using Orb.GirlLike.Ememies;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Players.UI;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class Player : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] private GameObject m_light;
    [SerializeField] private OverlapBehaviour overlapInteractive;
#pragma warning restore CS0649
    private Transform cacheTransform;
    public float defaultGravity;

    public PlayerHitPoint HitPoint { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public ItemPickup Pickup { get; private set; }
    public PlayerHUD HUD { get; private set; }
    public PlayerStatus Status { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public Target Target { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerCombatSystem Combat { get; private set; }
    public PlayerBag Bag { get; private set; }

    private void Awake()
    {
      Movement = GetComponent<PlayerMovement>();
      Combat = GetComponent<PlayerCombatSystem>();
      Bag = GetComponent<PlayerBag>();
      HitPoint = GetComponent<PlayerHitPoint>();
      Animator = GetComponent<PlayerAnimator>();
      Pickup = GetComponentInChildren<ItemPickup>();
      HUD = GetComponent<PlayerHUD>();
      Status = GetComponent<PlayerStatus>();
      Rigidbody = GetComponent<Rigidbody2D>();
      Input = GetComponent<PlayerInput>();
      Target = GetComponent<Target>();
      defaultGravity = Rigidbody.gravityScale;
    }

    private void LateUpdate()
    {
      var position = GetTransform().position;
      position.z = 0;
      GetTransform().position = position;
    }

    public void HiddenHUD(bool isHidden)
    {
      HUD.Canvas.enabled = !isHidden;
      m_light.SetActive(!isHidden);
    }

    public void DisableCombat(bool isDisable)
    {
      Combat.isEnable = !isDisable;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    internal void Interactive(ActionState state)
    {
      if (state != ActionState.Down) return;

      var colliders = overlapInteractive.GetOverlapColliders();
      if (SearchCloseItem(colliders, out var item))
      {
        Bag.Add(item);
      }
      else
      {
        Interactive(colliders);
      }
    }

    private void Interactive<T>(ICollection<T> components) where T : Component
    {
      foreach (var collider in components)
      {
        var interactivePlayer = collider.GetComponent<InteractivePlayer>();
        if (interactivePlayer != null)
        {
          interactivePlayer.Interactive(this);
          return;
        }
      }
    }

    internal void DisablePauseGame(bool isDisableHUDAndCombat)
    {
      Input.blockPauseGame = isDisableHUDAndCombat;
    }

    private bool SearchCloseItem<T>(ICollection<T> components, out Item foundItem) where T : Component
    {
      foreach (var component in components)
      {
        var item = component.GetComponent<Item>();

        if (item != null)
        {
          foundItem = item;
          return true;
        }
      }

      foundItem = default;
      return false;
    }
  }
}
