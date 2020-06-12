using Orb.GirlLike;
using Orb.GirlLike.Players;
using UnityEngine;

public class Heal : MonoBehaviour
{
  public int maxHeal;
  public int price;
  public Sprite avaiable;
  public Sprite disable;
  public SpriteRenderer spriteRenderer;
  public int healCount;
  public Player Player => GameMode.Current.GetPlayer();
  public bool isClose => healCount >= maxHeal;
  public bool HasCoin => Player.Bag.CoinAmount >= price;
  public bool PlayerNeedHeal => Player.HitPoint.CurrentHitPoint < Player.HitPoint.MaxHitPoint;

  public void HealPlayer()
  {
    var hitpoint = Player.HitPoint;
    if (HasCoin && PlayerNeedHeal && !isClose)
    {
      Player.Bag.RemoveCoin(price);
      hitpoint.Heal(1);
      healCount++;
    }
  }

  private void Update()
  {
    var isAvaiable = HasCoin && PlayerNeedHeal && !isClose;
    spriteRenderer.sprite = isAvaiable ? avaiable : disable;
  }
}
