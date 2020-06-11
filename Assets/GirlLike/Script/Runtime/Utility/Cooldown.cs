using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Cooldown : MonoBehaviour
{
  public float seconds;
  public UnityEvent onComplete;

  private void OnEnable()
  {
    StartCoroutine(Count());
  }

  public IEnumerator Count()
  {
    yield return new WaitForSeconds(seconds);
    onComplete.Invoke();
  }

  public void ResetLevel()
  {
    SceneManager.LoadScene(0);
  }
}
