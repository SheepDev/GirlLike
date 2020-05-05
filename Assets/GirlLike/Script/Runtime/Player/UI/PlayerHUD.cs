﻿using System.Collections.Generic;
using Orb.GirlLike.Combats;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Orb.GirlLike.Players.UI
{
  public class PlayerHUD : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] private Sprite profileSprite;

    [Header("Prefab")]
    [SerializeField] private Heart heartPrefab;

    [Header("Config")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform lifeBarRoot;
    [SerializeField] private Image profileIMG;
    [SerializeField] private TextMeshProUGUI coinText;
#pragma warning restore CS0649

    private HitPoint hitPoint;
    private List<Heart> hearts;
    private Player player;

    public Canvas Canvas { get => canvas; set => canvas = value; }

    private void Awake()
    {
      hearts = new List<Heart>();
      player = GetComponent<Player>();

      profileIMG.sprite = profileSprite;
    }

    private void Start()
    {
      hitPoint = player.HitPoint;
      UpdateLifeHUD();
      hitPoint.onUpdate.AddListener(UpdateLifeHUD);
      hitPoint.onDamage.AddListener(UpdateLife);
      hitPoint.onHeal.AddListener(UpdateLife);

      player.Bag.onCoinUpdate.AddListener((amount) => coinText.text = amount.ToString());
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

    private void UpdateLifeHUD()
    {
      hearts.Clear();
      DeleteChild(lifeBarRoot);

      var maxHitPoint = Mathf.CeilToInt(hitPoint.MaxHitPoint);
      for (int index = 0; index < maxHitPoint; index++)
      {
        var heart = Instantiate(heartPrefab, lifeBarRoot);
        hearts.Add(heart);
      }

      UpdateLife();
    }

    public void DeleteChild(Transform transform)
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
      {
        var child = transform.GetChild(i);
        Destroy(child.gameObject);
      }
    }
  }
}
