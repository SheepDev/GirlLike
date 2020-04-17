using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerAnimator : MonoBehaviour
  {
    [SerializeField] private SpriteUtility spriteUtility;
    [SerializeField] private Animator spriteAnimator;

    public PlayerMovement PlayerMovement { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
      PlayerMovement = GetComponent<PlayerMovement>();
      Animator = GetComponent<Animator>();

      PlayerMovement.onJump.AddListener(Jump);
      PlayerMovement.onUpdateMoveAxis.AddListener(Move);
      PlayerMovement.onSetDirection.AddListener(SetDirection);

      var playerCombat = GetComponent<PlayerCombatSystem>();
      playerCombat.onDash.AddListener(() => Animator.SetTrigger("Dash"));
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

    public void Vunerable()
    {
      spriteAnimator.Play("Empty");
    }

    private void SetDirection(LookDirection direction)
    {
      spriteUtility.FlipX(direction == LookDirection.Left ? true : false);
    }
  }
}
