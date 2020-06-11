using UnityEngine;

namespace Orb.GirlLike
{
  [ExecuteInEditMode]
  public class MaterialProperty : MonoBehaviour
  {
    public Material material;
    public float value;
    public Color color;

    private void Update()
    {
      material.SetFloat("_Range", value);
      material.SetColor("_ColorMask", color);
    }
  }
}
