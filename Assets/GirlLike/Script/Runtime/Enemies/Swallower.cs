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
    private bool isAttack;
    private bool isAllowToAttack;
    private bool isAttackCountdown;
    private PlayerHitPoint player;

    public bool IsAllowToAttack => isAllowToAttack && !isAttackCountdown;
    public MoveTo MoveTo { get; private set; }
    public Animator Animator { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      MoveTo = GetComponent<MoveTo>();
      Animator = GetComponent<Animator>();

      overlap.onOverlap.AddListener(Eat);
    }

    private void OnEnable()
    {
      isAllowToAttack = true;
    }

    private void Update()
    {
      if (isAttack) return;

      var targetPoint = target.GetCenter();
      if (IsAllowToAttack && BoundsConstains(attackBounds, targetPoint))
      {
        Stop();
        Animator.Play("OutFloor");
        isAttack = true;
        return;
      }

      var isFollow = BoundsConstains(followBounds, targetPoint);
      if (isFollow)
      {
        GoToTarget();
      }
    }

    private void StartAttack()
    {
      StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);
      isAttack = true;
      Animator.Play("Eat");
      yield return new WaitForSeconds(.2f);
      yield return new WaitForSeconds(.2f);
      var force = isLeft ? Vector3.left : Vector3.right;
      force *= forceJump;
      _rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void Eat(List<Collider2D> colliders)
    {
      if (player != null) return;

      foreach (var collider in colliders)
      {
        var player = collider.GetComponentInParent<PlayerHitPoint>();
        if (player != null)
        {
          this.player = player;
          player.GetComponent<PlayerInput>().Hidden(true);
          Animator.SetBool("isChewing", true);
          return;
        }
      }
    }

    private void StartChewing()
    {
      if (player != null)
        StartCoroutine(Chewing());
      else
        StartCoroutine(Vunerable());
    }

    protected override void OnDie()
    {
      StopAllCoroutines();
      Animator.Play("Die");
    }

    private void GoToTarget()
    {
      var isLeft = TargetIsLeft();
      MoveTo.enabled = true;
      MoveTo.Direction(isLeft ? Vector3.left : Vector3.right);
    }

    private IEnumerator Chewing()
    {
      for (int i = 0; i < totalDamage; i++)
      {
        yield return new WaitForSeconds(1.2f);
        player.ApplyDamage(1);
      }

      Animator.SetBool("isChewing", false);
      isAttack = false;
      yield return AttackCooldown();
    }

    private IEnumerator Vunerable()
    {
      yield return new WaitForSeconds(3);
      Animator.Play("EnterFloor");
      yield return new WaitForSeconds(.3f);
      isAttack = false;
      yield return AttackCooldown();
    }

    private IEnumerator AttackCooldown()
    {
      isAttackCountdown = true;
      yield return new WaitForSeconds(2);
      isAttackCountdown = false;
    }

    private void Spitting()
    {
      var playerMoviment = player.GetComponent<PlayerMovement>();
      var isLeft = !sprite.sprite.flipX;
      var direction = isLeft ? Vector3.left : Vector3.right;
      direction *= forceSpitting;

      player.GetComponent<PlayerInput>().Hidden(false);
      playerMoviment.GetTransform().position = mouthPivo.position;

      playerMoviment._rigidbody.velocity = Vector3.zero;
      playerMoviment._rigidbody.AddForce(direction, ForceMode2D.Impulse);
      playerMoviment.DisableForSeconds(player.ignoreDamageDelay);
      player.IgnoreDamage(player.ignoreDamageDelay, true);
      Animator.Play("EnterFloor");
      this.player = null;
    }

    private void Stop()
    {
      MoveTo.enabled = false;
      Animator.Play("Idle");
    }
  }
}
