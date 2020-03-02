using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  [System.Serializable]
  public class Axis
  {
    public GetAxis onUpdate;
    [HideInInspector] public float value;

    internal void CheckUpdate(string name, bool isInvert = false)
    {
      var newValue = Input.GetAxis(name);
      if (newValue != value)
      {
        value = newValue;
        if (isInvert) newValue *= -1;
        onUpdate.Invoke(newValue);
      }
    }
  }

  public delegate void GetAxis(float value);
}