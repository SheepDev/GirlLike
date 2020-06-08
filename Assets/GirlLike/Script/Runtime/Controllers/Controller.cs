using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  [RequireComponent(typeof(ControllerMap))]
  public class Controller : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] [HideInInspector] internal Button jump;
    [SerializeField] [HideInInspector] internal Button attack;
    [SerializeField] [HideInInspector] internal Button interactive;
    [SerializeField] [HideInInspector] internal Button removeItem;
    [SerializeField] [HideInInspector] internal Button useItem;
    [SerializeField] [HideInInspector] internal Button dash;
    [SerializeField] [HideInInspector] internal Button pauseGame;
    [SerializeField] [HideInInspector] internal Axis horizontal;
    [SerializeField] [HideInInspector] internal Axis scroll;
#pragma warning restore CS0649

    public ControllerMap map { get; private set; }

    private void Awake()
    {
      map = GetComponent<ControllerMap>();
    }

    private void Update()
    {
      pauseGame.CheckState(map.pauseGameKey);
      jump.CheckState(map.jumpKey);
      attack.CheckState(map.attackKey);
      dash.CheckState(map.dashKey);
      interactive.CheckState(map.interactiveKey);
      removeItem.CheckState(map.removeItemKey);
      useItem.CheckState(map.useItemKey);

      UpdateAxis(map.horizontal, horizontal);
      UpdateAxis(map.scroll, scroll);
    }

    private void UpdateAxis(AxisInfo info, Axis axis)
    {
      axis.CheckUpdate(info.name, info.isInvert);
    }
  }
}
