using UnityEngine;
using System.Collections.Generic;

public class KeyInventory : MonoBehaviour
{
    [SerializeField] private List<int> keyIds = new List<int>();

    public static KeyInventory Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //to keep inventory on scene change
        }
        else
            Destroy(gameObject);
    }

    public void AddKey(Key key)
    {
        if(!keyIds.Contains(key.keyId))
        {
            keyIds.Add(key.keyId);
            Debug.Log($"Key added: {key.keyName} (ID: {key.keyId})");
            UIManager.Instance.AddKeyToUI(key);
        }
    }

    public bool HasKey(Key key)
    {
        return keyIds.Contains(key.keyId);
    }
}
