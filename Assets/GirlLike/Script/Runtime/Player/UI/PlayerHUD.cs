using System.Collections.Generic;
using Orb.GirlLike.Combats;
using UnityEngine;
using UnityEngine.UI;

namespace Orb.GirlLike.Players.UI
{
  public class PlayerHUD : MonoBehaviour
  {
    public Sprite profileSprite;

    [Header("Prefab")]
    public Heart heartPrefab;

    [Header("Config")]
    public HitPoint hitPoint;
    public Image profileIMG;
    public Transform lifeBarRoot;

    private List<Heart> hearts;

    private void Awake()
    {
      profileIMG.sprite = profileSprite;

      hearts = new List<Heart>();
      var maxHitPoint = Mathf.CeilToInt(hitPoint.MaxHitPoint);

      for (int index = 0; index < maxHitPoint; index++)
      {
        var heart = Instantiate(heartPrefab, lifeBarRoot);
        hearts.Add(heart);
      }

      hitPoint.onDamage.AddListener(UpdateLife);
    }

    public void UpdateLife()
    {
      var currentLife = hitPoint.CurrentHitPoint - 1;

      for (int index = 0; index < hearts.Count; index++)
      {
        var heart = hearts[index];
        if (index <= currentLife)
          heart.On();
        else
          heart.Off();
      }
    }
  }
}
