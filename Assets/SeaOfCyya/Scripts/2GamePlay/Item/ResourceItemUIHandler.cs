using UnityEngine;
using UnityEngine.UI;

public class ResourceItemUIHandler : MonoBehaviour
{
    public int id;

    public Image iconImg;

    public ResourceItemModelScriptable itemModel = new ResourceItemModelScriptable();


    public void SetInfos(ResourceItemModel newItem,int newId)
    {
        id = newId;
        
        itemModel.quantity = newItem.quantity;
        itemModel.itemType = newItem.itemType;
        itemModel.id = newItem.id;

        SetIconImg();
    }
    public void SetIconImg()
    {
        iconImg.sprite = Resources.Load<Sprite>($"Inventory/{itemModel.itemType}");

    }
}
