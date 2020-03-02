using Orb.GirlLike.Controllers;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerMovement : MonoBehaviour
  {
    public float speed;
    public float jumpForce;

    [Header("Collider Settings")]
#pragma warning disable CS0649
    [SerializeField] private PhysicsMaterial2D materialAir;
    [SerializeField] private PhysicsMaterial2D materialGround;
#pragma warning restore CS0649

    [Header("Ground Settings")]
    public Vector2 boxSize;
    public float maxDistance;
    public LayerMask layerGround;

    private float horizontalAxis;
    private Transform cacheTransform;

    public Rigidbody2D _rigidbody { get; private set; }

    private void Awake()
    {
      _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      SetXVelocity(horizontalAxis * speed);

      var inGround = IsGround(out var hit);
      _rigidbody.sharedMaterial = (inGround) ? materialGround : materialAir;
    }

    public void Jump(ActionState state)
    {
      var isJump = Input.GetKeyDown(KeyCode.Space) && IsGround(out var hit);
      if (!isJump) return;

      var velocity = _rigidbody.velocity;
      velocity.y = 0;
      _rigidbody.velocity = velocity;
      _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void SetHorizontal(float value)
    {
      horizontalAxis = value;
    }

    private void SetXVelocity(float value)
    {
      var velocity = _rigidbody.velocity;
      velocity.x = value;
      _rigidbody.velocity = velocity;
    }

    public bool IsGround(out RaycastHit2D hit)
    {
      var transform = GetTransform();
      var origin = transform.position;
      hit = Physics2D.BoxCast(origin, boxSize, 0, Vector2.down, maxDistance, layerGround);
      return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
      var transform = GetTransform();
      var origin = transform.position;

      var isGround = IsGround(out var hit);
      Gizmos.color = Color.white;
      Gizmos.DrawWireCube(origin, boxSize);

      if (isGround)
      {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(hit.centroid, boxSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, hit.centroid);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hit.point, hit.point + hit.normal);
      }
      else
      {
        var centroid = origin + Vector3.down * (maxDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centroid, boxSize);
        Gizmos.DrawLine(origin, centroid);
      }
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }
  }
}
