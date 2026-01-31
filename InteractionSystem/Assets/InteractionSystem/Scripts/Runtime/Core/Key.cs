using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    [CreateAssetMenu(fileName = "New Key", menuName = "InteractionSystem/Items/Key")]
    public class Key : ScriptableObject
    {
        public string keyName;
        public int keyId;
        public Sprite keySprite;
    }
}