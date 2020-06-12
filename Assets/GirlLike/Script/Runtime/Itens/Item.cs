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
    [SerializeField] [TextArea(5, 10)] private string description;
    [SerializeField] private int price;

    public bool NeedToBuy => needToBuy;
    public int Price => price;

    [HideInInspector] public UnityEvent onBuy;
    private Animator iconAnimator;
    private Player Player => GameMode.Current.GetPlayer();
    private PlayerBag Bag => Player.Bag;
    public string Description => description;

    private void Awake()
    {
      enabled = false;
    }

    private void Update()
    {
      var isBlock = Bag.HasItem(this) || !Bag.HasCoin(this) || !Bag.HasAvaliableSlot();

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
