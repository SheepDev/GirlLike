using System.Collections;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Orb.GirlLike.Players
{
  public class PlayerHitPoint : HitPoint
  {
    public GameObject painel;
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

    [ContextMenu("Die")]
    public override void Die()
    {
      var player = Moviment.GetComponent<Player>();
      Moviment.GetComponent<PlayerInput>().Disable(true);
      player.DisableCombat(true);
      StartCoroutine(BackToMenu());
      onDie.Invoke();
    }

    public IEnumerator BackToMenu()
    {
      yield return new WaitForSeconds(1);
      FadeManager.Current.FadeIn(.8f);
      yield return new WaitForSeconds(2);
      painel.SetActive(true);
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
