using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Utility
{
  public class FadeTrigger : MonoBehaviour
  {
    public bool isOverrideSpeed;
    [SerializeField] private float overrideSpeed;

    [Header("Events")]
    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOffComplete;

    public float OverrideSpeed { get => overrideSpeed; set => overrideSpeed = Mathf.Abs(value); }
    private float CurrentSpeed => isOverrideSpeed ? overrideSpeed : 0;

    public void FadeIn()
    {
      FadeManager.Current.FadeIn(CurrentSpeed, onFadeInComplete);
    }

    public void FadeOff()
    {
      FadeManager.Current.FadeOff(CurrentSpeed, onFadeOffComplete);
    }

    private void OnValidate()
    {
      OverrideSpeed = overrideSpeed;
    }
  }
}
