using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Controllers;
using Orb.GirlLike.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerCombatSystem : MonoBehaviour
  {
    public bool isBlock;
    private bool isAttack;
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

    [SerializeField] private List<AttackSetting> attackSettings;

    public bool IsAttack => isAttack;
    public PlayerMovement Movement { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }

    private void Awake()
    {
      Movement = GetComponent<PlayerMovement>();
      PlayerAnimator = GetComponent<PlayerAnimator>();
      var hitPoint = GetComponent<PlayerHitPoint>();

      onDash.AddListener(() => hitPoint.isIgnoreDamage = true);
      onDashFinish.AddListener(() => hitPoint.isIgnoreDamage = false);
    }

    public void Attack(ActionState state)
    {
      var inGround = Movement.InGround();
      if (!inGround || isBlock || state != ActionState.Down) return;

      if (isAttack)
      {
        isNextAttack = isAllowNextAttack;
      }
      else
      {
        Movement.LockLookDirection(true);
        Movement.isBlockJump = true;
        Movement.currentSpeed = Movement.defaultSpeed * .3f;
        PlayerAnimator.Animator.SetBool("IsAttack", isAttack = true);
      }
    }

    public void Dash(ActionState state)
    {
      if (state != ActionState.Down || isDash || isDashCountdown) return;

      onDash.Invoke();
      StartCoroutine(WhileDash());
    }

    private IEnumerator WhileDash()
    {
      Movement.IsDisable(true);
      isDash = true;
      isDashCountdown = true;
      yield return new WaitForSeconds(dashStartDelay);
      Movement._rigidbody.gravityScale = 0;
      Movement._rigidbody.velocity = Vector2.zero;

      var direction = Movement.Direction == LookDirection.Left ? Vector2.left : Vector2.right;
      direction *= dashForce;
      Movement._rigidbody.AddForce(direction, ForceMode2D.Impulse);

      yield return new WaitForSeconds(dashDuration);
      Movement._rigidbody.velocity = Vector2.zero;
      Movement._rigidbody.gravityScale = 1;
      Movement.IsDisable(false);
      isDash = false;
      onDashFinish.Invoke();

      yield return new WaitForSeconds(dashDelay);
      isDashCountdown = false;
    }

    private void ApplyDamage_AnimTrigger(string attackID)
    {
      var setting = attackSettings.Find(obj => obj.ID == attackID);

      if (setting == null)
      {
        Debug.LogWarningFormat("[{0}] Not find attack ID \"{1}\" in Settings", name, attackID);
        return;
      }

      setting.TriggerDamage(setting.baseDamage);
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
      Movement.isBlockJump = false;
      Movement.currentSpeed = Movement.defaultSpeed;

      isAllowNextAttack = isNextAttack = isAttack = false;
      PlayerAnimator.Animator.SetBool("IsAttack", isAttack);
    }

    [System.Serializable]
    public class AttackSetting
    {
      public string ID;
      public float baseDamage;
      public Tag tag;
      public CastDamage castDamage;

      public void TriggerDamage(float damage)
      {
        castDamage.Cast(damage, tag.ToString());
      }
    }
  }
}
