using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerStatus : MonoBehaviour
  {
    [SerializeField] private float defaultSpeed;
    private Player player;

    public float DefaultSpeed => defaultSpeed + MoveSpeedBonus;
    public float DamageBonus { get; private set; }
    public float MoveSpeedBonus { get; private set; }
    public float JumpForceBonus { get; private set; }
    public float DashSpeedBonus { get; private set; }

    private void Start()
    {
      player = GetComponent<Player>();
      var bag = player.Bag;
      bag.onAddPassiveItem.AddListener(AddBonus);
      bag.onRemovePassiveItem.AddListener(RemoveBonus);
    }

    private void AddBonus(Item item)
    {
      var playerItem = item as PlayerPassiveItem;
      DamageBonus += playerItem.damageBonus;
      MoveSpeedBonus += playerItem.moveSpeedBonus;
      JumpForceBonus += playerItem.jumpForceBonus;
      DashSpeedBonus += playerItem.dashForceBonus;

      player.Movement.BackToDefaultSpeed();
    }

    private void RemoveBonus(Item item)
    {
      var playerItem = item as PlayerPassiveItem;
      DamageBonus -= playerItem.damageBonus;
      MoveSpeedBonus -= playerItem.moveSpeedBonus;
      JumpForceBonus -= playerItem.jumpForceBonus;
      DashSpeedBonus -= playerItem.dashForceBonus;

      player.Movement.BackToDefaultSpeed();
    }

    public float CalculeDamage(float baseDamage)
    {
      return baseDamage + DamageBonus;
    }
  }
}
