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
    public UnityEvent onHeal;
    public UnityEvent onDamage;
    public UnityEvent onUpdate;

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

    public virtual void Heal(float points)
    {
      if (IsDie) return;
      hitPoint = Mathf.Min(maxHitPoint, hitPoint + points);
      onHeal.Invoke();
    }

    public virtual void SetMaxHitPoint(float maxHitPoint)
    {
      maxHitPoint = Mathf.Abs(maxHitPoint);
      if (this.maxHitPoint == maxHitPoint) return;

      this.maxHitPoint = maxHitPoint;
      hitPoint = Mathf.Min(hitPoint, this.maxHitPoint);
      onUpdate.Invoke();
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
