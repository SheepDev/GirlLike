using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.AI;
using Orb.GirlLike.Players;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Swallower : Enemy
  {
    public Transform mouthPivo;
    public int totalDamage;
    public float forceJump;
    public float forceSpitting;
    public OverlapBehaviour overlap;

    private Player player;

    protected override void Awake()
    {
      base.Awake();
      isNotPlayWalkAnimation = true;
      overlap.onOverlap.AddListener(Eat);
    }

    protected override void OnEnable()
    {
      canMove = canAttack = true;
    }

    protected override IEnumerator Attack()
    {
      OnIdle();
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);
      Animator.Play("OutFloor");
      canMove = canAttack = false;
      isAttack = true;
      yield return new WaitForSeconds(.4f);
    }

    protected void JumpToEat_AnimTrigger()
    {
      Animator.Play("Eat");
      StartCoroutine(Jump());
    }

    protected IEnumerator Jump()
    {
      yield return new WaitForSeconds(.2f);
      var isLeft = TargetIsLeft();
      var force = isLeft ? Vector3.left : Vector3.right;
      force *= forceJump;
      RB2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void Eat(List<Collider2D> colliders)
    {
      if (player != null) return;

      foreach (var collider in colliders)
      {
        var player = collider.GetComponentInParent<Player>();
        if (player != null)
        {
          this.player = player;
          player.GetComponent<PlayerInput>().Hidden(true);
          Animator.SetBool("isChewing", true);
          return;
        }
      }
    }

    private void StartChewing_AnimTrigger()
    {
      if (player != null)
        StartCoroutine(Chewing());
      else
        StartCoroutine(Vunerable());
    }

    private IEnumerator Chewing()
    {
      for (int i = 0; i < totalDamage; i++)
      {
        yield return new WaitForSeconds(1.2f);
        player.HitPoint.ApplyDamage(1);
      }

      Animator.SetBool("isChewing", false);
      isAttack = false;
      yield return AttackCountdown(attackCountdown);
    }

    private IEnumerator Vunerable()
    {
      yield return new WaitForSeconds(3);
      Animator.Play("EnterFloor");
      yield return new WaitForSeconds(.3f);
      isAttack = false;
      canMove = true;
      yield return AttackCountdown(attackCountdown);
    }

    private void Spitting_AnimTrigger()
    {
      var isLeft = !sprite.sprite.flipX;
      var direction = isLeft ? Vector3.left : Vector3.right;
      direction *= forceSpitting;

      player.Input.Hidden(false);
      player.GetTransform().position = mouthPivo.position;
      var playerMoviment = player.Movement;
      var hitPoint = player.HitPoint;

      playerMoviment._rigidbody.velocity = Vector3.zero;
      playerMoviment._rigidbody.AddForce(direction, ForceMode2D.Impulse);
      playerMoviment.DisableForSeconds(hitPoint.disableInputInSeconds);
      hitPoint.IgnoreDamage(hitPoint.ignoreDamageDelay, true);
      Animator.Play("EnterFloor");
      this.player = null;

      canMove = true;
    }

    protected override void OnIdle()
    {
      MoveTo.enabled = false;
    }
  }
}
