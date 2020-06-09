namespace Orb.GirlLike.Players
{
  public class PlayerShield : PlayerActiveItem
  {
    public int shieldPoints;

    public override bool Use(Player player)
    {
      if (player.HitPoint.ShieldPoints >= shieldPoints) return false;

      player.HitPoint.SetShieldPoints(shieldPoints);
      return true;
    }
  }
}
