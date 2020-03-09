using UnityEngine;

namespace Orb.GirlLike.Combats
{
  public class TriggerDamage : MonoBehaviour
  {
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
      var takeDamage = other.GetComponent<TakeDamage>();

      if (takeDamage != null)
      {
        takeDamage.PointDamage(damage, transform.position);
      }
    }
  }
}
