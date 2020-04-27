using Orb.GirlLike.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Combats
{
  public class TriggerDamage : MonoBehaviour
  {
    public Tag selectTag;
    public float damage;

    [Header("Event")]
    public UnityEvent onTrigger;

    private void OnTriggerStay2D(Collider2D other)
    {
      var takeDamage = other.GetComponent<TakeDamage>();
      if (takeDamage == null) return;

      var tagName = selectTag.ToString();
      if (string.IsNullOrEmpty(tagName) || takeDamage.CompareTag(tagName))
      {
        takeDamage.PointDamage(damage, transform.position);
        onTrigger.Invoke();
      }
    }
  }
}
