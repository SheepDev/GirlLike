using System.Collections.Generic;
using UnityEngine;

namespace Orb.GirlLike.Combats
{
  public class CastDamage : MonoBehaviour
  {
    public ContactFilter2D filter2D;
    public Collider2D[] colliders;

    public void Cast(float damage)
    {
      Cast(damage, string.Empty);
    }

    public void PointCast(float damage, Vector3 position)
    {
      PointCast(damage, position, string.Empty);
    }

    public void Cast(float damage, string tag)
    {
      var takeDamages = GetOverlapBehavious();

      foreach (var takeDamage in takeDamages)
      {
        var isCast = string.IsNullOrEmpty(tag) || takeDamage.CompareTag(tag);
        if (isCast)
        {
          takeDamage.Damage(Mathf.Abs(damage));
        }
      }
    }

    public void PointCast(float damage, Vector3 pointDamage, string tag)
    {
      var takeDamages = GetOverlapBehavious();

      foreach (var takeDamage in takeDamages)
      {
        var isCast = string.IsNullOrEmpty(tag) || takeDamage.CompareTag(tag);
        if (isCast)
        {
          takeDamage.PointDamage(Mathf.Abs(damage), pointDamage);
        }
      }
    }

    private HashSet<TakeDamage> GetOverlapBehavious()
    {
      var takeDamages = new HashSet<TakeDamage>();
      var findColliders = new List<Collider2D>();

      foreach (var collider in colliders)
      {
        collider.OverlapCollider(filter2D, findColliders);
        FindTakeDamages(takeDamages, findColliders);
      }

      return takeDamages;
    }

    private void FindTakeDamages(HashSet<TakeDamage> takeDamages, List<Collider2D> colliders)
    {
      foreach (var collider in colliders)
      {
        var takeDamage = collider.GetComponent<TakeDamage>();
        var isValid = takeDamage != null;
        var isAdd = isValid && !takeDamages.Contains(takeDamage);

        if (isAdd)
        {
          takeDamages.Add(takeDamage);
        }
      }
    }
  }
}
