namespace Orb.GirlLike.Players
{
  public class PlayerInvulnerableItem : PlayerActiveItem
  {
    public float invulnerableTime;

    public override bool Use(Player player)
    {
      player.HitPoint.IgnoreDamage(invulnerableTime, true);
      return true;
    }
  }
}
