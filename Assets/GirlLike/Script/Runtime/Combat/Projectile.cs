using System.Collections;
using Orb.GirlLike.AI;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Combats
{
  public class Projectile : OverlapBehaviour
  {
    public bool defaultFlip;
    [SerializeField] private float lifeTime;
    [SerializeField] private float delayToDestroy;
    [SerializeField] private Animator animator;

    private bool isDestroy;
    public MoveTo MoveTo { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      if (animator == null)
        animator = GetComponentInChildren<Animator>();

      MoveTo = GetComponent<MoveTo>();
      onOverlap.AddListener((c) => Stop());
    }

    private void Update()
    {
      Overlap();
    }

    public void SetDirection(bool isLeft)
    {
      if (isDestroy) return;

      var isFlip = isLeft ? defaultFlip : !defaultFlip;
      var scale = transform.localScale;
      scale.x *= isFlip ? 1 : -1;
      transform.localScale = scale;
      MoveTo.Direction(isLeft ? Vector3.left : Vector3.right);
    }

    public void SetDirection(Vector3 forward)
    {
      if (isDestroy) return;
      MoveTo.Direction(forward);

      var isLeft = forward.x < 0;
      var isFlip = isLeft ? defaultFlip : !defaultFlip;
      var scale = transform.localScale;
      scale.x *= isFlip ? 1 : -1;
      transform.localScale = scale;
    }

    private void OnEnable()
    {
      StartCoroutine(LifeTime(lifeTime));
    }

    public void Stop()
    {
      StopAllCoroutines();
      StartCoroutine(Destroy(delayToDestroy));
    }

    private IEnumerator LifeTime(float time)
    {
      yield return new WaitForSeconds(time);
      yield return Destroy(delayToDestroy);
    }

    private IEnumerator Destroy(float time)
    {
      _collider2D.enabled = false;
      MoveTo.enabled = false;
      isDestroy = true;
      animator?.Play("Destroy");
      yield return new WaitForSeconds(time);
      Destroy(gameObject);
    }

    public void Destroy()
    {
      StartCoroutine(Destroy(delayToDestroy));
    }
  }
}
