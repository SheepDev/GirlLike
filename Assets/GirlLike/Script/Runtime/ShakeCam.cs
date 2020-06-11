using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Orb.GirlLike.Helper
{
  public class ShakeCam : MonoBehaviour
  {
    public CinemachineVirtualCamera cam;
    public float amplitudeGain;
    public float frequencyGain;

    private void Awake()
    {
      cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float time)
    {
      StopAllCoroutines();
      StartCoroutine(ShakeTime(time));
    }

    private IEnumerator ShakeTime(float time)
    {
      var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
      noise.m_AmplitudeGain = amplitudeGain;
      noise.m_FrequencyGain = frequencyGain;
      yield return new WaitForSeconds(time);
      noise.m_AmplitudeGain = 0;
      noise.m_FrequencyGain = 0;
    }
  }
}
