using System.Collections;
using Orb.GirlLike.Combats;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerHitPoint : HitPoint
  {
    public float ignoreDamageDelay;
    public float disableInputInSeconds;

    public float forceDamage;
    public bool isIgnoreDamage;

    [SerializeField] private TakeDamage takeDamage;

    [Header("Event")]
    public UnityEvent onVunerable;

    public PlayerMovement Moviment { get; private set; }
    public PlayerCombatSystem Combat { get; private set; }
    public PlayerAnimator Animator { get; private set; }

    private void Awake()
    {
      Moviment = GetComponent<PlayerMovement>();
      Combat = GetComponent<PlayerCombatSystem>();
      Animator = GetComponent<PlayerAnimator>();
      takeDamage.onPointDamage.AddListener(ApplyDamage);
    }

    public override void ApplyDamage(PointDamageData data)
    {
      if (isIgnoreDamage) return;

      Moviment.DisableForSeconds(disableInputInSeconds);
      Animator.TakeDamage();

      var positionX = Moviment.GetTransform().position.x;
      var direction = Vector2.one.normalized;
      direction *= forceDamage;

      if (positionX < data.point.x)
        direction.x *= -1;

      Moviment._rigidbody.velocity = Vector2.zero;
      Moviment._rigidbody.AddForce(direction, ForceMode2D.Impulse);
      base.ApplyDamage(data);
      IgnoreDamage(ignoreDamageDelay, true);
    }

    public void IgnoreDamage(float seconds, bool isStopDamageAnimation = false)
    {
      StartCoroutine(IgnoreDamageInSeconds(seconds, isStopDamageAnimation));
    }

    private IEnumerator IgnoreDamageInSeconds(float seconds, bool isStopDamageAnimation)
    {
      isIgnoreDamage = true;
      Animator.Vunerable(false);
      yield return new WaitForSeconds(seconds);

      if (!Combat.isDash)
      {
        isIgnoreDamage = false;
        onVunerable.Invoke();
      }

      if (isStopDamageAnimation)
        Animator.Vunerable(true);
    }
  }
}
