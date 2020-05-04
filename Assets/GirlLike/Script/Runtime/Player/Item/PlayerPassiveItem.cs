using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerPassiveItem : Item
  {
    [Header("Combat")]
    public float damageBonus;

    [Header("Movimenet")]
    public float moveSpeedBonus;
    public float jumpForceBonus;

    private void OnValidate()
    {
      type = Type.Passive;
    }
  }
}
