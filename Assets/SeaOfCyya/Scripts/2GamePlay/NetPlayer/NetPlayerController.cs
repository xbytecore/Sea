using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class NetPlayerController : NetworkBehaviour
{
    public NetClientController netClientController;
    

 //   public Inventory inventory;

    private void Start()
    {

    }
    public Vector3 GetPosition()
    {
        return transform.position; 
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // gameObject.SetActive(false);
        if (isOwned)
        {
            SetStartNewPlayer();
        }
        else
        {

        }
    }
    [TargetRpc]
    public void TargetAssignNetClientController(NetworkConnectionToClient conn, NetClientController newNetClientController)
    {
        netClientController = newNetClientController;
    }


    public void SetStartNewPlayer()
    {
        SetAssingCamera();
        ResourceInventoryController.instance.netPlayerController = this;
    }

    public void SetAssingCamera()
    {
        if (Camera.main != null)
        {
            // Define a Main Camera como filha deste GameObject
            Camera.main.GetComponent<CamHandler>().player = gameObject.transform;

            // Ajusta a posi��o relativa (opcional)

        }
        else
        {
            Debug.LogError("N�o h� uma Main Camera na cena!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOwned) { return; }


    }

  

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOwned) { return; }

        if (!Input.GetKeyDown(KeyCode.E)) { return; }

        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ShipVelaHandler>(out ShipVelaHandler velaHandler))
        {
            Debug.Log("vela");
            collision.gameObject.transform.root.GetComponent<ShipNetController>().SetChangeSail();
        }
        if (collision.GetComponent<Collider2D>().TryGetComponent<ShipLemeHandler>(out ShipLemeHandler lemeHandler))
        {
            ShipNetController shipNetController = collision.gameObject.transform.root.GetComponent<ShipNetController>();

            shipNetController.SetChangeLeme();

            Debug.Log("leme");
        }
        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ShipAncorHandler>(out ShipAncorHandler shipAncorHandler))
        {
            ShipNetController shipNetController = collision.gameObject.transform.root.GetComponent<ShipNetController>();

            shipNetController.SetChangeAnchor();

            Debug.Log("Anchor");
        }

        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ShipCannonHandler>(out ShipCannonHandler shipCannonHandler))
        {
            ShipNetController shipNetController = collision.gameObject.transform.root.GetComponent<ShipNetController>();


            shipNetController.SetChangeCannon(shipCannonHandler.id);

            Debug.Log("ShipCannonHandler");
        }
        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ShipDamagedPartHandler>(out ShipDamagedPartHandler shipDamagedHandler))
        {
            ShipNetController shipNetController = collision.gameObject.transform.root.GetComponent<ShipNetController>();

            shipNetController.SetChangeDamagedPartGreen(shipDamagedHandler.id);

            Debug.Log("shipDamagedHandler");
        }
        
        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ResourceItemWorldHandler>(out ResourceItemWorldHandler resourceItemWorldHandler))
        {
            Debug.Log("resource");

            ResourceInventoryController.instance.SpawnItemInSlot(resourceItemWorldHandler.model, 0,true);

            SetDestroyItemCmd(resourceItemWorldHandler.gameObject);

        }
        
    }

    [Command]
    public void SetCreateItemCmd(ResourceItemModelScriptable newItem, int id, Vector3 newPos)
    {
        ResourceInventoryController.instance.SetSpawnWorldItem(newItem, id, newPos);    
    }
    [Command]
    public void SetDestroyItemCmd(GameObject receiver)
    {
        ResourceItemWorldHandler serverReceiver = receiver.GetComponent<NetworkIdentity>().gameObject.GetComponent<ResourceItemWorldHandler>();

        serverReceiver.SetDestroy();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.TryGetComponent<ShipLemeHandler>(out ShipLemeHandler lemeHandler))
        {
            ShipNetController shipNetController = collision.gameObject.transform.root.GetComponent<ShipNetController>();

            if (!shipNetController.GetComponent<ShipNetModel>().isLemeOn) { return; }

            shipNetController.SetChangeLeme();


            Debug.Log("leme");
        }
    }
}
