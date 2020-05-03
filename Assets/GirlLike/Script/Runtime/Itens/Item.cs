using UnityEngine;

namespace Orb.GirlLike.Itens
{
  public class Item : MonoBehaviour
  {
    public int ID;
    public Type type;
  }

  public enum Type
  {
    Passive, Active
  }
}
