using Orb.GirlLike.Itens;

namespace Orb.GirlLike.Players
{
  public abstract class PlayerActiveItem : Item
  {
    public abstract bool Use(Player player);

    private void OnValidate()
    {
      type = Type.Active;
    }
  }
}
