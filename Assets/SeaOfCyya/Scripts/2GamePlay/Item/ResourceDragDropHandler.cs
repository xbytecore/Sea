using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResourceDragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    public Transform actualParent;
    private Canvas canvas;
    private bool isDragging;
    public bool isSlot;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        if (TryGetComponent<ResourceSlotHandler>(out ResourceSlotHandler slot))
        {
            isSlot = true;
        }

        canvas = GetComponentInParent<Canvas>();
        originalParent = canvas.transform;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

  

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;

        if (draggedItem == null)
        {
            Debug.Log("Objeto arrastado é nulo.");
            isDragging = false;
            return;
        }

        if (isSlot) return;

        ResourceInventoryController.instance.actualItem = draggedItem;

        if (!draggedItem.CompareTag("Item"))
        {
            Debug.Log("Não é um item válido para arrastar.");
            isDragging = false;
            return;
        }

        isDragging = true;

      
        draggedItem.transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (isSlot) return;

        GameObject draggedItem = eventData.pointerDrag;

        if (draggedItem == null) return;

        draggedItem.transform.position = eventData.position;

        // Detecção manual de slots durante o arrasto
        DetectSlotUnderMouse();


    }

    private void DetectSlotUnderMouse()
    {
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            ResourceSlotHandler slot = result.gameObject.GetComponent<ResourceSlotHandler>();
            if (slot != null)
            {
                ResourceInventoryController.instance.actualSlot = slot.gameObject;
                
                return;
            }
        }

        ResourceInventoryController.instance.actualSlot = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (isSlot) return;

        GameObject draggedItem = eventData.pointerDrag;

        if (draggedItem == null) return;

        if (ResourceInventoryController.instance.actualSlot != null)
        {
            Transform newParent = ResourceInventoryController.instance.actualSlot.transform;
            ResourceSlotHandler newSlot = ResourceInventoryController.instance.actualSlot.GetComponent<ResourceSlotHandler>();

            if (actualParent != null)
            {
                ResourceSlotHandler oldSlot = actualParent.gameObject.GetComponent<ResourceSlotHandler>();
                oldSlot.resourceItemHandler = null;
            }
            if (newSlot != null && newSlot.resourceItemHandler == null)
            {
                draggedItem.transform.SetParent(newParent);
                newSlot.resourceItemHandler = gameObject.GetComponent<ResourceItemUIHandler>();


              

                actualParent = newParent; 
                Debug.Log("deuruin");
            }
            else
            {
                if (actualParent == null) { draggedItem.transform.SetParent(originalParent); }
                else { draggedItem.transform.SetParent(actualParent); }
            }
        }
        else
        {
            if (actualParent != null)
            {
                ResourceSlotHandler oldSlot = actualParent.gameObject.GetComponent<ResourceSlotHandler>();
                oldSlot.resourceItemHandler = null;
            }

            Vector3 newPosition = ResourceInventoryController.instance.netPlayerController.transform.position;

            ResourceItemUIHandler itemUIHandler = gameObject.GetComponent<ResourceItemUIHandler>();


            ResourceInventoryController.instance.netPlayerController.SetCreateItemCmd(itemUIHandler.itemModel, 1, newPosition);

            ResourceInventoryController.instance.actualItem = null;
            ResourceInventoryController.instance.actualSlot = null;

            Destroy(gameObject);



        
            // if(actualParent == null) { draggedItem.transform.SetParent(originalParent); }
            // else { draggedItem.transform.SetParent(actualParent); }
        }

        draggedItem.transform.localPosition = Vector3.zero;
    }
}
