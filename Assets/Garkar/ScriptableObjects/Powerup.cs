using UnityEngine;

[System.Serializable]
public enum PowerupType
{
  None = 0,
  Sniper,
  Shotgun
}

[CreateAssetMenu(fileName = "Powerup", menuName = "Scriptable Objects/Powerup")]
public class Powerup : ScriptableObject
{
  

  [SerializeField]
  public PowerupType type;
  [SerializeField]
  public GameObject prefab;
  [SerializeField]
  public float duration;
}
