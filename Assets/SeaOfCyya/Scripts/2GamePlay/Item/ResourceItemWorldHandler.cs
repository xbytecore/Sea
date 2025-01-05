using UnityEngine;
using CodeMonkey.Utils;
using Mirror;
public class ResourceItemWorldHandler : NetworkBehaviour
{
    public int id;
    public SpriteRenderer iconImg;
    public Rigidbody2D rb;
    public ResourceItemModel model = new ResourceItemModel();
    public Collider2D itemCollider;

    private void OnEnable()
    {
        itemCollider = GetComponent<Collider2D>();
        itemCollider.isTrigger = true;
        model.OnItemChange.AddListener(SetIconImg);
        model.OnDirChange.AddListener(SetRBEffect);

        if (!isServer) { return; }

        Invoke("SetDestroy", 30);
    }
    public void SetInfos(ResourceItemModelScriptable newItem, int newId)
    {
        id = newId;

        model.SetInfos(newItem.dir, newItem.id, newItem.itemType);

        
        
    }
    public void SetIconImg(ItemType itemType)
    {
        string resourcePath = $"Inventory/{itemType}";

        Sprite loadedSprite = Resources.Load<Sprite>(resourcePath);

        iconImg.sprite = loadedSprite;
     
    }
    public void SetRBEffect(Vector3 randomDir)
    {
        rb.AddForce(randomDir * 0.5f, ForceMode2D.Impulse);

        Invoke("SetRBStopEffect", 1);
    }

    public void SetRBStopEffect()
    {
        // Zera a velocidade linear
        rb.linearVelocity = Vector2.zero;

        // Opcional: Zera a velocidade angular (caso o Rigidbody2D esteja girando)
        rb.angularVelocity = 0f;
        itemCollider.isTrigger = false;
    }

    public void SetDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }
}
