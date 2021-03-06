﻿using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  public class ControllerMap : MonoBehaviour
  {
    public AxisInfo horizontal;
    public AxisInfo scroll;
    public KeyCode jumpKey;
    public KeyCode attackKey;
    public KeyCode specialKey;
    public KeyCode dashKey;
    public KeyCode interactiveKey;
    public KeyCode useItemKey;
    public KeyCode removeItemKey;
    public KeyCode pauseGameKey;
  }

  [System.Serializable]
  public struct AxisInfo
  {
    public string name;
    public bool isInvert;
  }
}
