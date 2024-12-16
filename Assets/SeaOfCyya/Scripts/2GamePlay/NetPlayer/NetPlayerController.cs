using UnityEngine;
using Mirror;
public class NetPlayerController : NetworkBehaviour
{
    public NetClientController netClientController;

    private void Start()
    {

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
    }

    public void SetAssingCamera()
    {
        if (Camera.main != null)
        {
            // Define a Main Camera como filha deste GameObject
            Camera.main.transform.SetParent(transform);

            // Ajusta a posi��o relativa (opcional)
            Camera.main.transform.localPosition = new Vector3(0, 5, -15); // Posi��o relativa
            Camera.main.transform.localRotation = Quaternion.Euler(10, 0, 0); // Rota��o relativa
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
