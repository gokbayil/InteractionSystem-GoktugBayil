using UnityEngine;

public class KeyCollectable : MonoBehaviour
{

    [SerializeField] private Key key;

    public void KeyPickup()
    {
        if(key != null)
        {

            
            gameObject.SetActive(false);
        }
    }
}
