using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.Controllers;
using Orb.GirlLike.Helper;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Players.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerBag : Bag
  {
#pragma warning disable CS0649
    [SerializeField] private int maxItens;
    [SerializeField] private ItemSlot itemPrefab;
    [SerializeField] private Transform itensHUD;
#pragma warning restore CS0649
    [Header("Events")]
    public IntEvent onCoinUpdate;
    public ItemEvent onAddPassiveItem;
    public ItemEvent onRemovePassiveItem;

    private List<ItemSlot> slots;
    private Player player;
    private int selectedIndex;
    private int coinAmount;

    private void Awake()
    {
      slots = new List<ItemSlot>();
      player = GetComponent<Player>();
      onCoinUpdate = new IntEvent();

      for (int i = 0; i < maxItens; i++)
      {
        var itemSlot = Instantiate(itemPrefab, itensHUD);
        slots.Add(itemSlot);
      }

      SetSelectedIndex(0);
    }

    public override void Add(Item item)
    {
      if (!HasItem(item) && HasCoin(item) && GetAvaliableSlot(out var slot))
      {
        if (item.NeedToBuy)
        {
          RemoveCoin(item.Price);
          item.HasBuy();
        }

        slot.Save(item);
        base.Add(item);

        if (item.type == Type.Passive)
        {
          onAddPassiveItem.Invoke(item);
        }
      }
    }

    public void UseItem(ActionState state)
    {
      if (state != ActionState.Down) return;
      var item = slots[selectedIndex].CurrentItem;
      if (item == null || item.type == Itens.Type.Passive) return;

      var activeItem = (PlayerActiveItem)item;
      if (activeItem.Use(player))
        DestroyItem(selectedIndex);
    }

    public void NextItem()
    {
      SetSelectedIndex(selectedIndex + 1);
    }

    public void PreviousItem()
    {
      SetSelectedIndex(selectedIndex - 1);
    }

    public void DestroyItem(int index)
    {
      var slot = slots[index];
      if (slot.Remove(out var item))
      {
        if (item.type == Type.Passive)
        {
          onRemovePassiveItem.Invoke(item);
        }

        Destroy(item.gameObject);
      }
    }

    public void StartDestroySelectedItem()
    {
      var slot = slots[selectedIndex];

      if (slot.HasItem)
        StartCoroutine(DestroyItem(selectedIndex, 3));
    }

    public void CancelDestroySelectedItem()
    {
      StopAllCoroutines();
    }

    private IEnumerator DestroyItem(int selectedIndex, float delay)
    {
      var time = 0f;

      while (time < delay)
      {
        yield return null;
        time += Time.deltaTime;
      }

      DestroyItem(selectedIndex);
    }

    public void DropItem(int index)
    {
      var slot = slots[index];
      if (slot.Remove(out var item))
      {
        item.transform.position = player.GetTransform().position;
        item.gameObject.SetActive(true);

        if (item.type == Type.Passive)
        {
          onRemovePassiveItem.Invoke(item);
        }
      }
    }

    private void SetSelectedIndex(int index)
    {
      CancelDestroySelectedItem();
      slots[selectedIndex].Select(false);
      selectedIndex = (int)Mathf.Repeat(index, slots.Count);
      slots[selectedIndex].Select(true);
    }

    public bool HasItem(Item item)
    {
      foreach (var slot in slots)
      {
        if (slot.HasItem && slot.CurrentItem.ID == item.ID)
          return true;
      }

      return false;
    }

    public bool HasCoin(Item item)
    {
      return !item.NeedToBuy || item.Price <= coinAmount;
    }

    public bool HasAvaliableSlot()
    {
      foreach (var slot in slots)
      {
        if (slot.IsAvailable)
        {
          return true;
        }
      }

      return false;
    }

    private bool GetAvaliableSlot(out ItemSlot itemSlot)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
      var coin = other.GetComponent<Coin>();
      if (coin != null)
      {
        AddCoin(coin.Amount);
        Destroy(coin.gameObject);
      }
    }

    public void AddCoin(int totalCoin)
    {
      SetCoin(coinAmount + Mathf.Abs(totalCoin));
    }

    public void RemoveCoin(int totalCoin)
    {
      SetCoin(coinAmount - Mathf.Abs(totalCoin));
    }

    private void SetCoin(int coin)
    {
      onCoinUpdate.Invoke(coinAmount = coin);
    }

    [System.Serializable]
    public class ItemEvent : UnityEvent<Item> { }
  }
}
