using System.Collections.Generic;
using Orb.GirlLike.Controllers;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Players.UI;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerBag : Bag
  {
#pragma warning disable CS0649
    [SerializeField] private int maxItens;
    [SerializeField] private ItemSlot itemPrefab;
    [SerializeField] private Transform itensHUD;
#pragma warning restore CS0649

    private List<ItemSlot> slots;
    private Player player;
    private int selectedIndex;

    private void Awake()
    {
      slots = new List<ItemSlot>();
      player = GetComponent<Player>();

      for (int i = 0; i < maxItens; i++)
      {
        var itemSlot = Instantiate(itemPrefab, itensHUD);
        slots.Add(itemSlot);
      }

      SetSelectedIndex(0);
    }

    public override void Add(Item item)
    {
      if (GetAvaliableSlot(out var slot))
      {
        slot.Save(item);
        base.Add(item);
      }
    }

    public void UseItem(ActionState state)
    {
      if (state != ActionState.Down) return;
      var item = slots[selectedIndex].CurrentItem;
      if (item == null || item.type == Type.Passive) return;
      var activeItem = (PlayerActiveItem)item;
      activeItem.Use(player);
    }

    public void NextItem(ActionState state)
    {
      if (state != ActionState.Down) return;
      SetSelectedIndex(selectedIndex + 1);
    }

    public void PreviousItem(ActionState state)
    {
      if (state != ActionState.Down) return;
      SetSelectedIndex(selectedIndex - 1);
    }

    private void SetSelectedIndex(int index)
    {
      slots[selectedIndex].Select(false);
      selectedIndex = (int)Mathf.Repeat(index, slots.Count);
      slots[selectedIndex].Select(true);
    }

    public bool GetAvaliableSlot(out ItemSlot itemSlot)
    {
      itemSlot = default;

      foreach (var slot in slots)
      {
        if (slot.IsAvailable)
        {
          itemSlot = slot;
          return true;
        }
      }

      return false;
    }
  }
}
