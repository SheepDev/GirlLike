using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  [RequireComponent(typeof(ControllerMap))]
  public class Controller : MonoBehaviour
  {
#pragma warning disable CS0649
    [SerializeField] [HideInInspector] internal Button jump;
    [SerializeField] [HideInInspector] internal Button attack;
    [SerializeField] [HideInInspector] internal Button dash;
    [SerializeField] [HideInInspector] internal Axis horizontal;
#pragma warning restore CS0649

    public ControllerMap map { get; private set; }

    private void Awake()
    {
      map = GetComponent<ControllerMap>();
    }

    private void Update()
    {
      jump.CheckState(map.jumpKey);
      attack.CheckState(map.attackKey);
      dash.CheckState(map.dashKey);

      var info = map.horizontal;
      horizontal.CheckUpdate(info.name, info.isInvert);
    }
  }
}
