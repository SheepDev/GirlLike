using UnityEngine;
using UnityEngine.SceneManagement;

namespace Orb.GirlLike.Utility
{
  public class LoadScene : MonoBehaviour
  {
    public void Load(int buildIndex)
    {
      SceneManager.LoadScene(buildIndex);
    }
  }
}
