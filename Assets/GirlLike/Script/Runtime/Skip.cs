using Orb.GirlLike.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike
{
  public class Skip : MonoBehaviour
  {
    public bool isSkiped;
    public FadeTrigger trigger;
    public UnityEvent onSkip;

    private void Update()
    {
      if (!isSkiped && Input.GetKeyDown(KeyCode.Space))
      {
        SkipIntro();
      }
    }

    public void SkipIntro()
    {
      trigger.FadeIn();
      onSkip.Invoke();
      isSkiped = true;
    }
  }
}
