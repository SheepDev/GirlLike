using UnityEngine;

namespace Orb.GirlLike.Itens
{
  public class Item : MonoBehaviour
  {
    public Type type;
  }

  public enum Type
  {
    Passive, Active
  }
}
