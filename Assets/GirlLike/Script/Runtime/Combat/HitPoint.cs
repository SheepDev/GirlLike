using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Combats
{
  public class HitPoint : MonoBehaviour
  {
    [SerializeField] protected float hitPoint;
    [SerializeField] protected float maxHitPoint;

    [Header("Event")]
    public UnityEvent onDie;
    public UnityEvent onDamage;

    public bool IsDie => hitPoint <= 0;
    public float CurrentHitPoint => hitPoint;
    public float MaxHitPoint => maxHitPoint;

    public virtual void ApplyDamage(float damage)
    {
      hitPoint -= damage;
      onDamage.Invoke();

      if (IsDie)
        Die();
    }

    public virtual void ApplyDamage(PointDamageData data)
    {
      hitPoint -= data.damage;
      onDamage.Invoke();

      if (IsDie)
        Die();
    }

    public virtual void Die()
    {
      onDie.Invoke();
    }

    private void OnValidate()
    {
      hitPoint = Mathf.Min(hitPoint, maxHitPoint);
    }
  }
}
