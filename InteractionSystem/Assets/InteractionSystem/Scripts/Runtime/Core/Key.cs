using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Scriptable Objects/Key")]
public class Key : ScriptableObject
{
    public string keyName;
    public int keyId;
    public Sprite keySprite;
}
