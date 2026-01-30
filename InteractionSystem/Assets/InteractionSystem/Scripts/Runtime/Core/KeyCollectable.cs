using UnityEngine;

public class KeyCollectable : MonoBehaviour
{

    [SerializeField] private Key key;
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] private AudioSource audioSource;

    public void KeyPickup()
    {
        if(key != null)
        {
            KeyInventory.Instance.AddKey(key);

            if(pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            
            gameObject.SetActive(false);
        }
    }
}
