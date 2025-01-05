using Mirror;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResourceItemModel : NetworkBehaviour
{
    public int id;

    [SyncVar(hook = nameof(SetOnItemChanged))]
    public ItemType itemType;
    [SyncVar]
    public int quantity;
    [SyncVar(hook = nameof(SetOnDirChanged))]
    public Vector3 dir;

    [HideInInspector]
    public UnityEvent<ItemType> OnItemChange = new UnityEvent<ItemType>();
    [HideInInspector]
    public UnityEvent<Vector3> OnDirChange = new UnityEvent<Vector3>();

    public override void OnStartClient()
    {
        Invoke("WaitToStart", 0.1f);
    }
    public void WaitToStart()
    {
        OnItemChange.Invoke(itemType);
        OnDirChange.Invoke(dir);
    }
    public void SetOnItemChanged(ItemType oldItemType, ItemType newItemType)
    {
        OnItemChange.Invoke(newItemType);
    }
    public void SetOnDirChanged(Vector3 oldDir, Vector3 newDir)
    {
        OnDirChange.Invoke(newDir);
    }
    public void SetUpdateQuantity(int amount)
    {
        quantity += amount;
    }
    public void SetInfos(Vector3 newDir,int newQuantity, ItemType newItemType)
    {

        dir = newDir;
        quantity = newQuantity;
        itemType = newItemType;
    }
    public void SetStartInfos()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2);

        // Calcula a direção no plano 2D
        Vector2 randomDir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        dir = randomDir;
        quantity = Random.RandomRange(0, 10);
        itemType = GetRandomItemType();
    }
    public  ItemType GetRandomItemType()
    {
        // Obtém todos os valores possíveis do enum ItemType
        ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));

        // Escolhe um valor aleatório dentro do array de ItemType
        return itemTypes[Random.Range(0, itemTypes.Length-1)];
    }
}
[System.Serializable]
public class ResourceItemModelScriptable
{
    public int id;
    public ItemType itemType;
    
    public int quantity;
    
    public Vector3 dir;

    public void SetInfos()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2);

        // Calcula a direção no plano 2D
        Vector2 randomDir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        dir = randomDir;
        quantity = Random.RandomRange(0, 10);
        itemType = GetRandomItemType();

    
    }
    public ItemType GetRandomItemType()
    {
        // Obtém todos os valores possíveis do enum ItemType
        ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));

        // Escolhe um valor aleatório dentro do array de ItemType
        return itemTypes[Random.Range(0, itemTypes.Length - 1)];
    }
}
public enum ItemType
{
    Sword,
    HealthPotion,
    ManaPotion,
    Medkit,
    Coin
}