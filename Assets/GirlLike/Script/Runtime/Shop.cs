using System.Collections.Generic;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Players;
using UnityEngine;

namespace Orb.GirlLike
{
  public class Shop : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] private Transform bertharSpawnPoint;
    [SerializeField] private PlayerHealItem bertharPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Item[] items;
#pragma warning restore CS0649

    private void Start()
    {
      GenerateItems();
    }

    public void GenerateItems()
    {
      var itemSpawn = new HashSet<int>();

      foreach (var point in spawnPoints)
      {
        var itemPrefab = GetRandomItem(itemSpawn);
        var itemObject = Instantiate(itemPrefab, point.position, Quaternion.identity);
        itemObject.needToBuy = true;
      }
    }

    private Item GetRandomItem(HashSet<int> itemSpawn)
    {
      var randomIndex = 0;
      do
      {
        randomIndex = UnityEngine.Random.Range(0, items.Length);
      } while (itemSpawn.Contains(randomIndex));

      itemSpawn.Add(randomIndex);
      return items[randomIndex];
    }
  }
}
