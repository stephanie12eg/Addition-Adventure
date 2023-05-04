using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfDiamonds { get;set; }

    public UnityEvent<PlayerInventory> OnDiamondCollected;

    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }

    public void ResetOfDiamonds()
    {
        NumberOfDiamonds = 0; 
    }
}
