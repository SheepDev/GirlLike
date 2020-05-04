using System.Collections;
using Orb.GirlLike.AI;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Spear : OverlapBehaviour
  {
    public float lifeTime;
    public float timeToDestroy;
    [SerializeField] private MoveTo moveTo;

    protected override void Awake()
    {
      base.Awake();
      onOverlap.AddListener((c) => Stop());
    }

    private void Update()
    {
      Overlap();
    }

    public void SetDirection(bool isLeft)
    {
      var scale = transform.localScale;
      scale.x *= isLeft ? 1 : -1;
      transform.localScale = scale;
      moveTo.Direction(isLeft ? Vector3.left : Vector3.right);
    }

    private void OnEnable()
    {
      StartCoroutine(Destroy(lifeTime));
    }

    private void Stop()
    {
      _collider2D.enabled = false;
      moveTo.enabled = false;

      StopAllCoroutines();
      StartCoroutine(Destroy(timeToDestroy));
    }

    private IEnumerator Destroy(float time)
    {
      yield return new WaitForSeconds(time);
      Destroy(gameObject);
    }
  }
}
