namespace Orb.GirlLike.Settings
{
  [System.Serializable]
  public struct Tag
  {
    public Name name;

    public override string ToString()
    {
      return (name == Name.Any) ? string.Empty : name.ToString();
    }

    [System.Serializable]
    public enum Name
    {
      Any, Untagged, Respawn, Finish, EditorOnly,
      MainCamera, Player, GameController, Enemy
    }
  }
}
