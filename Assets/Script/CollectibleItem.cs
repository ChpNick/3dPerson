using UnityEngine;
using System.Collections;
public class CollectibleItem : MonoBehaviour {
    
//    Введите имя этого элемента на панели Inspector.
    [SerializeField] private string itemName;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Item collected: " + itemName);
        
        Managers.Inventory.AddItem(name);
        
        Destroy(this.gameObject);
    }
}
