using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Orb.GirlLike.Utility
{
  [RequireComponent(typeof(Image))]
  public class FadeManager : MonoBehaviour
  {
    public static FadeManager Current;
    [SerializeField] private float defaultSpeed;
#pragma warning disable CS0649
    [SerializeField] private Fade startFade;
#pragma warning restore CS0649

    public AnimationCurve fadeInCurve;
    public AnimationCurve fadeOffCurve;

    private Image image;
    private float targetAlpha;
    private float currentAlpha;
    private AnimationCurve currentCurve;

    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = Mathf.Abs(value); }

    private void Awake()
    {
      image = GetComponent<Image>();
      image.raycastTarget = false;
      currentAlpha = image.color.a;

      if (Current == null)
      {
        Current = this;
      }
      else
      {
        Destroy(this);
      }
    }

    private void Start()
    {
      if (startFade == Fade.Off)
      {
        SetAlpha(currentAlpha = 1);
        FadeOff(defaultSpeed);
      }
      else
      {
        SetAlpha(currentAlpha = 0);
        FadeIn(defaultSpeed);
      }
    }

    public void FadeIn(float speed)
    {
      FadeIn(speed, null);
    }

    public void FadeIn(float speed, UnityEvent onComplete)
    {
      currentCurve = fadeInCurve;
      StartFadeAnimation(speed, 1, onComplete);
    }

    public void FadeOff(float speed)
    {
      FadeOff(speed, null);
    }

    public void FadeOff(float speed, UnityEvent onComplete)
    {
      currentCurve = fadeOffCurve;
      StartFadeAnimation(speed, 0, onComplete);
    }

    private void StartFadeAnimation(float speed, float targetAlpha, UnityEvent onComplete)
    {
      this.targetAlpha = targetAlpha;
      speed = speed <= 0 ? defaultSpeed : speed;
      StopAllCoroutines();
      StartCoroutine(FadeAnimation(speed, onComplete));
    }

    private IEnumerator FadeAnimation(float speed, UnityEvent onComplete)
    {
      image.raycastTarget = true;
      while (currentAlpha != targetAlpha)
      {
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * speed);
        currentAlpha = Mathf.Clamp01(currentAlpha);
        UpdateImageAlpha();
        yield return null;
      }

      image.raycastTarget = false;
      onComplete?.Invoke();
    }

    private void UpdateImageAlpha()
    {
      var desiredAlpha = fadeInCurve.Evaluate(currentAlpha);
      SetAlpha(desiredAlpha);
    }

    private void SetAlpha(float alpha)
    {
      var color = image.color;
      color.a = alpha;
      image.color = color;
    }

    private void OnValidate()
    {
      DefaultSpeed = defaultSpeed;
    }

    public enum Fade
    {
      None, In, Off
    }
  }
}
