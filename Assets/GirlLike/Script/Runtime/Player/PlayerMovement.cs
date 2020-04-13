using System;
using System.Collections;
using Gameplay.Effect;
using Orb.GirlLike.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class PlayerMovement : MonoBehaviour
  {
    public Parallax parallax;
    public float defaultSpeed;
    public float currentSpeed;

    public float jumpForce;
    public bool isBlockJump;
    private bool isDisable;
    private bool lockLookDirection;

    public MovimentSetting movimentSetting;
    [SerializeField] private LookDirection look;

    [Header("Collider Settings")]
#pragma warning disable CS0649
    [SerializeField] private PhysicsMaterial2D materialAir;
    [SerializeField] private PhysicsMaterial2D materialGround;
#pragma warning restore CS0649

    [Header("Events")]
    public UnityEvent onJump;
    public FloatEvent onUpdateMoveAxis;
    public DirectionEvent onSetDirection;

    [Header("Ground Settings")]
    public Vector2 boxSize;
    public float maxDistance;
    public LayerMask layerGround;

    private Transform cacheTransform;
    private Vector3 lastPosition;
    private float desiredMoveAxis;
    private float currentMoveAxis;

    public Vector2 Velocity => _rigidbody.velocity;
    public LookDirection Direction => look;
    public Rigidbody2D _rigidbody { get; private set; }

    private void Awake()
    {
      _rigidbody = GetComponent<Rigidbody2D>();
      SetDirection(look, true);

      currentSpeed = defaultSpeed;
      lastPosition = transform.position;
    }

    private void Update()
    {
      var inGround = InGround();
      _rigidbody.sharedMaterial = (inGround) ? materialGround : materialAir;
      ApplyParallax();

      if (IsDisable()) return;

      var velocity = currentMoveAxis * currentSpeed;
      SetXVelocity(velocity);
    }

    private void ApplyParallax()
    {
      var transform = GetTransform();
      if (transform.position != lastPosition)
      {
        var delta = lastPosition - transform.position;
        parallax.Move(delta.normalized, delta.magnitude);
        lastPosition = transform.position;
      }
    }

    public void Jump(ActionState state)
    {
      var isJump = !IsDisable()
        && !isBlockJump
        && state == ActionState.Down && InGround();

      if (!isJump) return;
      var velocity = _rigidbody.velocity;
      velocity.y = 0;
      _rigidbody.velocity = velocity;
      _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      onJump.Invoke();
    }

    public void SetMoveAxis(float value)
    {
      desiredMoveAxis = value;
      if (IsDisable()) return;

      SetCurrentMoveAxis(value);
      onUpdateMoveAxis.Invoke(value);
    }

    private void SetCurrentMoveAxis(float value)
    {
      currentMoveAxis = value;
      SetDirection(currentMoveAxis);
    }

    private void SetDirection(LookDirection direction, bool isForceUpdate = false)
    {
      if (lockLookDirection) return;

      var isUpdate = isForceUpdate || this.look != direction;

      if (isUpdate)
      {
        onSetDirection.Invoke(this.look = direction);
      }
    }

    private void SetDirection(float direction, bool isForceUpdate = false)
    {
      if (direction == 0) return;
      var look = direction < 0 ? LookDirection.Left : LookDirection.Right;
      SetDirection(look, isForceUpdate);
    }

    private void SetXVelocity(float value)
    {
      var velocity = _rigidbody.velocity;
      velocity.x = value;
      _rigidbody.velocity = velocity;
    }

    public bool InGround()
    {
      return IsGround(out var hit);
    }

    public bool IsGround(out RaycastHit2D hit)
    {
      var transform = GetTransform();
      var origin = transform.position;
      hit = Physics2D.BoxCast(origin, boxSize, 0, Vector2.down, maxDistance, layerGround);
      return hit.collider != null;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    public void LockLookDirection(bool isLock)
    {
      lockLookDirection = isLock;

      if (!lockLookDirection)
      {
        SetDirection(currentMoveAxis);
      }
    }

    public bool IsDisable() => !isActiveAndEnabled || isDisable;

    public void IsDisable(bool value)
    {
      isDisable = value;

      if (!isDisable)
      {
        SetCurrentMoveAxis(desiredMoveAxis);
      }
    }

    public void DisableForSeconds(float seconds)
    {
      IsDisable(true);
      StartCoroutine(Disable(seconds));
    }

    private IEnumerator Disable(float seconds)
    {
      yield return new WaitForSeconds(seconds);
      IsDisable(false);
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
  }

  [Serializable]
  public enum LookDirection
  {
    Left, Right
  }

  [Serializable]
  public enum MovimentSetting
  {
    Free, StopInGround, Stop
  }

  [Serializable]
  public class DirectionEvent : UnityEvent<LookDirection> { }

  [Serializable]
  public class FloatEvent : UnityEvent<float> { }
}
