using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ResourceInventoryController : MonoBehaviour
{
 
    public Transform slotContainer;

    public Transform itemSpawnPosition;
    public Transform mobSpawnPosition;

    public GameObject itemWorldPrefab;
    public GameObject itemUIPrefab;
    public GameObject slotPrefab;

    public List<ResourceItemModel> items = new List<ResourceItemModel>();

    public List<ResourceSlotHandler> slots = new List<ResourceSlotHandler>();


    public GameObject actualSlot;
    public GameObject actualItem;

    public static ResourceInventoryController instance;

    private int inventorySize = 24; // Número de slots no inventário

    public NetPlayerController netPlayerController;

    public void SetSpawnWorldItem(ResourceItemModelScriptable itemModel, int slotIndex, Vector3 position)
    {
        GameObject newItemWorldGO = Instantiate(itemWorldPrefab, position, itemWorldPrefab.transform.rotation);

        ResourceItemWorldHandler newResourceItemUI = newItemWorldGO.GetComponent<ResourceItemWorldHandler>();

        newResourceItemUI.SetInfos(itemModel, 1);

        NetworkServer.Spawn(newItemWorldGO);
    }
    void Start()
    {
        instance = this;
        // Inicializa os slots
        SetSpawnSlot();
    }
    public void SetSpawnSlot()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab, slotContainer);
            ResourceSlotHandler slotHandler = slotInstance.GetComponent<ResourceSlotHandler>();
            slotHandler.id = i;

            // Adiciona o slot à lista de slots
            slots.Add(slotHandler);
        }
    }
    public void SpawnItemInSlot(ResourceItemModel item, int slotIndex, bool spawnOnFirstFreeSlot = false)
    {
        if (spawnOnFirstFreeSlot)
        {

            for(int i = 0; i < slots.Count; i++)
            {
                ResourceSlotHandler slotHandler = slots[i].GetComponent<ResourceSlotHandler>();

                if(slotHandler.resourceItemHandler == null) { 
                    slotIndex = i;
                    break;
                }
            }
        }

        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError("Índice de slot inválido!");
            return;
        }

        ResourceSlotHandler targetSlot = slots[slotIndex];

        if (targetSlot.resourceItemHandler != null)
        {
            Debug.LogWarning($"O slot {slotIndex} já contém um item!");
            return;
        }

        // Instancia o item
        GameObject itemInstance = Instantiate(itemUIPrefab, targetSlot.itemContent);

        RectTransform itemRect = itemInstance.GetComponent<RectTransform>();
        itemRect.anchorMin = Vector2.zero;
        itemRect.anchorMax = Vector2.one;
        itemRect.anchoredPosition = Vector2.zero;
        itemRect.sizeDelta = Vector2.zero;

        ResourceItemUIHandler itemHandler = itemInstance.GetComponent<ResourceItemUIHandler>();

        // Configura os dados do item
        itemHandler.SetInfos(item, slotIndex);
        

        // Atribui o item ao slot
        targetSlot.resourceItemHandler = itemHandler;

        Debug.Log($"Item {item.itemType} adicionado ao slot {slotIndex}");
    }
    
    public void RemoveItemFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError("Índice de slot inválido!");
            return;
        }

        ResourceSlotHandler targetSlot = slots[slotIndex];

        if (targetSlot.resourceItemHandler == null)
        {
            Debug.LogWarning($"O slot {slotIndex} está vazio!");
            return;
        }

        // Remove o item do slot
        Destroy(targetSlot.resourceItemHandler.gameObject);
        targetSlot.resourceItemHandler = null;

        Debug.Log($"Item removido do slot {slotIndex}");
    }


    public void SetAddItem(ResourceItemModel item)
    {
        items.Add(item);
    }

    public void SetRemoveItem(ResourceItemModel item)
    {
        items.Remove(item);
    }
    public void SetAddSlot(ResourceSlotHandler slot)
    {
        slots.Add(slot);
    }

    public void SetRemoveSlot(ResourceSlotHandler slot)
    {
        slots.Remove(slot);
    }


   
}
