using System.Collections.Generic;
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
    [SerializeField] private Heart shieldPrefab;

    [Header("Config")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform lifeBarRoot;
    [SerializeField] private Image profileIMG;
    [SerializeField] private Image minimapIMG;
    [SerializeField] private TextMeshProUGUI coinText;
#pragma warning restore CS0649

    private PlayerHitPoint hitPoint;
    private List<Heart> hearts;
    private List<Heart> shields;
    private Player player;

    public Canvas Canvas { get => canvas; set => canvas = value; }

    private void Awake()
    {
      hearts = new List<Heart>();
      shields = new List<Heart>();
      player = GetComponent<Player>();

      minimapIMG.sprite = profileIMG.sprite = profileSprite;
    }

    private void Start()
    {
      hitPoint = player.HitPoint;
      UpdateLifeHUD();
      hitPoint.onUpdate.AddListener(UpdateLifeHUD);
      hitPoint.onDamage.AddListener(UpdateLife);
      hitPoint.onHeal.AddListener(UpdateLife);
      hitPoint.onUpdateShield.AddListener(ShieldUpdate);

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
      ShieldUpdate();
    }

    public void DeleteChild(Transform transform)
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
      {
        var child = transform.GetChild(i);
        Destroy(child.gameObject);
      }
    }

    public void ShieldUpdate()
    {
      foreach (var shield in shields)
      {
        if (shield != null)
          Destroy(shield.gameObject);
      }

      shields.Clear();

      var shieldCount = player.HitPoint.ShieldPoints;

      for (int i = 0; i < shieldCount; i++)
      {
        var shield = Instantiate(shieldPrefab, lifeBarRoot);
        shields.Add(shield);
      }
    }
  }
}
