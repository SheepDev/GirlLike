namespace Orb.GirlLike.Players
{
  public class PlayerPlusHeart : PlayerActiveItem
  {
    public int amount;

    public override bool Use(Player player)
    {
      var hitPoint = player.HitPoint;
      var maxHitPoint = hitPoint.MaxHitPoint;
      hitPoint.SetMaxHitPoint(maxHitPoint + amount);
      hitPoint.Heal(amount);
      return true;
    }
  }
}
