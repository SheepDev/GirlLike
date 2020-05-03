namespace Orb.GirlLike.Players
{
  public class PlayerHealItem : PlayerActiveItem
  {
    public int healPoint;

    public override bool Use(Player player)
    {
      var hitPoint = player.HitPoint;
      if (hitPoint.CurrentHitPoint == hitPoint.MaxHitPoint) return false;
      player.HitPoint.Heal(healPoint);
      return true;
    }
  }
}
