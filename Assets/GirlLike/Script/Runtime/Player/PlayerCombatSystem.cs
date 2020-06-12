using System;
using System.Collections;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Controllers;
using Orb.GirlLike.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Orb.GirlLike.Players
{
  public class PlayerCombatSystem : MonoBehaviour
  {
    [Header("Attack")]
    public bool isEnable;
    public float attackDelay;
    public bool hasSpecial;
    public float specialDelay;
    public float movementSpeedDuringAttack;
    public Image specialIcon;

    private bool isAttack;
    private bool isDelayAttack;
    private bool isDelaySpecial;
    private bool isAllowNextAttack;
    private bool isNextAttack;

    [Header("Dash")]
    public bool isDash;
    public bool isDashCountdown;
    public float dashDelay;
    public float dashStartDelay;
    public float dashDuration;
    public float dashForce;

    [Header("Event")]
    public UnityEvent onDash;
    public UnityEvent onDashFinish;

    [Header("Settings")]
    [SerializeField] private AttackSetting[] attackSettings;
    [SerializeField] private ProjectileSettings[] projectileSettings;
    private Player player;

    public bool IsAttack => isAttack;
    public PlayerMovement Movement { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }
    public PlayerStatus Status { get; private set; }

    private void Start()
    {
      player = GetComponent<Player>();
      Status = player.Status;
      Movement = player.Movement;
      PlayerAnimator = player.Animator;
      var hitPoint = player.HitPoint;

      Movement.onJump.AddListener(ResetAttack);
      onDash.AddListener(() => hitPoint.isIgnoreDamage = true);
      onDashFinish.AddListener(() => hitPoint.isIgnoreDamage = false);

      if (hasSpecial)
        specialIcon.gameObject.SetActive(true);
    }

    public void Attack(ActionState state)
    {
      var inGround = Movement.InGround();
      if (!inGround || !isEnable || isDelayAttack || state != ActionState.Down) return;

      if (isAttack)
      {
        isNextAttack = isAllowNextAttack;
      }
      else
      {
        Movement.LockLookDirection(true);
        Movement.currentSpeed = movementSpeedDuringAttack;
        PlayerAnimator.Animator.SetBool("IsAttack", isAttack = true);
      }
    }

    internal void Special(ActionState state)
    {
      var inGround = Movement.InGround();
      if (!hasSpecial || !inGround || !isEnable || isDelaySpecial || state != ActionState.Down) return;

      Movement.LockLookDirection(true);
      Movement.currentSpeed = movementSpeedDuringAttack;
      PlayerAnimator.Animator.SetTrigger("Special");
      isDelaySpecial = isAttack = true;
      specialIcon.color = new Color(.3f, .3f, .3f);
      StartCoroutine(DelaySpecial(specialDelay));
    }

    public void Dash(ActionState state)
    {
      if (!isEnable || isDash || isDashCountdown || state != ActionState.Down) return;

      onDash.Invoke();
      StartCoroutine(WhileDash());
    }

    private IEnumerator DelaySpecial(float delay)
    {
      yield return new WaitForSeconds(delay);
      isDelaySpecial = false;
      specialIcon.color = Color.white;
    }

    private IEnumerator WhileDash()
    {
      Movement.IsDisable(true);
      isDash = true;
      isDashCountdown = true;
      yield return new WaitForSeconds(dashStartDelay);
      var cacheGravity = Movement._rigidbody.gravityScale;
      Movement._rigidbody.gravityScale = 0;
      Movement._rigidbody.velocity = Vector2.zero;

      var direction = Movement.Direction == LookDirection.Left ? Vector2.left : Vector2.right;
      direction *= dashForce + player.Status.DashSpeedBonus;
      Movement._rigidbody.AddForce(direction, ForceMode2D.Impulse);

      yield return new WaitForSeconds(dashDuration);
      Movement._rigidbody.velocity = Vector2.zero;
      Movement._rigidbody.gravityScale = cacheGravity;
      Movement.IsDisable(false);
      isDash = false;
      onDashFinish.Invoke();

      yield return new WaitForSeconds(dashDelay);
      isDashCountdown = false;
    }

    private void ApplyDamage_AnimTrigger(int Id)
    {
      var attack = attackSettings[Id];
      var damage = Status.CalculeDamage(attack.baseDamage);
      attack.TriggerPointDamage(damage, transform.position);
    }

    private void SpawnProjectil(int id)
    {
      var settings = projectileSettings[id];
      var projectile = Instantiate(settings.prefab, settings.pivo.position, Quaternion.identity);
      projectile.SetDirection(Movement.Direction == LookDirection.Left);
    }

    private void AllowNextAttack()
    {
      isAllowNextAttack = true;
    }

    private void FinishAttack()
    {
      if (!isNextAttack)
      {
        ResetAttack();
      }

      isAllowNextAttack = isNextAttack = false;
    }

    private void ResetAttack()
    {
      Movement.LockLookDirection(false);
      Movement.BackToDefaultSpeed();

      isAllowNextAttack = isNextAttack = isAttack = false;
      PlayerAnimator.Animator.SetBool("IsAttack", isAttack);
      PlayerAnimator.Animator.Play("Empty", 1);
      StartCoroutine(DelayAttack(attackDelay));
    }

    private IEnumerator DelayAttack(float attackDelay)
    {
      isDelayAttack = true;
      yield return new WaitForSeconds(attackDelay);
      isDelayAttack = false;
    }

    [System.Serializable]
    public struct ProjectileSettings
    {
      public Transform pivo;
      public Projectile prefab;
    }

    [System.Serializable]
    public class AttackSetting
    {
      public float baseDamage;
      public Tag tag;
      public CastDamage castDamage;

      public void TriggerDamage(float damage)
      {
        castDamage.Cast(damage, tag.ToString());
      }

      public void TriggerPointDamage(float damage, Vector3 position)
      {
        castDamage.PointCast(damage, position, tag.ToString());
      }
    }
  }
}
