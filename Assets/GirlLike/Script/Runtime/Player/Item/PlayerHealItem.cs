namespace Orb.GirlLike.Players
{
  public class PlayerHealItem : PlayerActiveItem
  {
    public int healPoint;

    public override void Use(Player player)
    {
      player.HitPoint.Heal(healPoint);
    }
  }
}
