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
   
}
