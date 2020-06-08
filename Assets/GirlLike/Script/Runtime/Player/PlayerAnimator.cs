using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerAnimator : MonoBehaviour
  {
    [SerializeField] private float spawnDisableInputInSeconds;
    [SerializeField] private SpriteUtility spriteUtility;
    [SerializeField] private Animator spriteAnimator;

    public PlayerMovement PlayerMovement { get; private set; }
    public Animator Animator { get; private set; }

    private void Start()
    {
      var player = GetComponent<Player>();
      PlayerMovement = player.Movement;
      Animator = GetComponent<Animator>();

      PlayerMovement.onJump.AddListener(Jump);
      PlayerMovement.onSetDirection.AddListener(SetDirection);
      PlayerMovement.DisableForSeconds(spawnDisableInputInSeconds);

      var playerCombat = player.Combat;
      playerCombat.onDash.AddListener(() => Animator.SetTrigger("Dash"));

      Animator.Play("Spawn");
    }

    private void Update()
    {
      var isGround = PlayerMovement.InGround();
      Animator.SetBool("IsGround", isGround);

      if (!isGround)
      {
        var isFall = PlayerMovement.Velocity.y < 0;

        if (isFall)
        {
          Animator.Play("Jump_Down", 2);
        }
        else
        {
          Animator.Play("Jump_Up", 2);
        }
      }

      if (!PlayerMovement.IsDisable())
        Move(PlayerMovement.Velocity.magnitude);
    }

    private void Move(float direction)
    {
      if (direction != 0)
      {
        Animator.Play("Walk");
      }
      else
      {
        Animator.Play("Idle");
      }
    }

    private void Jump()
    {
      Animator.Play("Jump", 2);
    }

    public void TakeDamage()
    {
      spriteAnimator.Play("Damage");
    }

    public void Vunerable(bool isVunerable)
    {
      if (isVunerable)
        spriteAnimator.Play("Empty");
      else
        spriteAnimator.Play("Damage");
    }

    private void SetDirection(LookDirection direction)
    {
      spriteUtility.FlipX(direction == LookDirection.Left ? true : false);
    }
  }
}
