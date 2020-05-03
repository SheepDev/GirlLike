using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class Coin : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] private int totalCoin;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
#pragma warning restore CS0649
    private Rigidbody2D rb2D;

    public int Amount => totalCoin;

    private void Awake()
    {
      rb2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyRandomForce()
    {
      var angle = Random.Range(minAngle, maxAngle);
      var force = Random.Range(minForce, maxForce);
      var direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
      rb2D.AddForce(direction * force, ForceMode2D.Impulse);
    }
  }
}
