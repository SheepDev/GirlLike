using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerStatus : MonoBehaviour
  {
    [SerializeField] private float defaultSpeed;
    private float damageBonus;
    private float moveSpeedBonus;
    private float jumpForceBonus;
    private Player player;

    public float DefaultSpeed => defaultSpeed + moveSpeedBonus;
    public float JumpBonus => jumpForceBonus;

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
      damageBonus += playerItem.damageBonus;
      moveSpeedBonus += playerItem.moveSpeedBonus;
      jumpForceBonus += playerItem.jumpForceBonus;

      player.Movement.BackToDefaultSpeed();
    }

    private void RemoveBonus(Item item)
    {
      var playerItem = item as PlayerPassiveItem;
      damageBonus -= playerItem.damageBonus;
      moveSpeedBonus -= playerItem.moveSpeedBonus;
      jumpForceBonus -= playerItem.jumpForceBonus;

      player.Movement.BackToDefaultSpeed();
    }

    public float CalculeDamage(float baseDamage)
    {
      return baseDamage + damageBonus;
    }
  }
}
