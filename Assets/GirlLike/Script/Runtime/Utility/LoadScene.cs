using UnityEngine;
using UnityEngine.SceneManagement;

namespace Orb.GirlLike.Utility
{
  public class LoadScene : MonoBehaviour
  {
    public void Load(int buildIndex)
    {
      Time.timeScale = 1;
      SceneManager.LoadScene(buildIndex);
    }
  }
}
