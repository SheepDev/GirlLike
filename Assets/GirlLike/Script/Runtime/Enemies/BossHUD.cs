using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.Combats;
using UnityEngine;

public class BossHUD : MonoBehaviour
{
  public GameObject prefab;
  public GameObject root;
  public HitPoint hitPoint;
  public List<GameObject> hearts;

  private void Awake()
  {
    StartCoroutine(ShowHeat());
    hitPoint.onDamage.AddListener(TakeDamage);
  }

  private void TakeDamage()
  {
    Build();
  }

  public void Build()
  {
    foreach (var heat in hearts)
    {
      Destroy(heat.gameObject);
    }
    hearts.Clear();

    for (int i = 0; i < hitPoint.CurrentHitPoint; i++)
    {
      var heat = Instantiate(prefab, root.transform);
      hearts.Add(heat);
    }
  }

  public IEnumerator ShowHeat()
  {
    for (int i = 0; i < hitPoint.MaxHitPoint; i++)
    {
      var heat = Instantiate(prefab, root.transform);
      hearts.Add(heat);
      yield return new WaitForSeconds(.2f);
    }
  }
}
