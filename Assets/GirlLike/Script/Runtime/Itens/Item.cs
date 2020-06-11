using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Itens
{
  public class Item : MonoBehaviour
  {
    public int ID;
    public Type type;
    public bool needToBuy;
    [SerializeField] private int price;

    public bool NeedToBuy => needToBuy;
    public int Price => price;

    [HideInInspector] public UnityEvent onBuy;
    private Player player;
    private PlayerBag bag;
    private Animator iconAnimator;

    private void Awake()
    {
      player = GameMode.Current.GetPlayer();
      bag = player.Bag;
      enabled = false;
    }

    private void Update()
    {
      var isBlock = bag.HasItem(this) || !bag.HasCoin(this) || !bag.HasAvaliableSlot();

      if (isBlock)
      {
        GetIconAnimator().Play("Block");
      }
      else
      {
        GetIconAnimator().Play("Avaliable");
      }
    }

    private void OnEnable()
    {
      GetIconAnimator().gameObject.SetActive(true);
    }

    private void OnDisable()
    {
      GetIconAnimator().gameObject.SetActive(false);
    }

    public Animator GetIconAnimator()
    {
      if (iconAnimator == null)
        iconAnimator = GetComponentInChildren<Animator>(true);

      return iconAnimator;
    }

    public void HasBuy()
    {
      needToBuy = false;
      onBuy.Invoke();
    }

    public override bool Equals(object obj)
    {
      return obj is Item item &&
             ID == item.ID;
    }

    public override int GetHashCode()
    {
      int hashCode = 1189832430;
      hashCode = hashCode * -1521134295 + ID.GetHashCode();
      return hashCode;
    }
  }

  public enum Type
  {
    Passive, Active
  }
}
