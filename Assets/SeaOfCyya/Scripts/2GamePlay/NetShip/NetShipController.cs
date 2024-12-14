using UnityEngine;
using Mirror;
using System;
public class NetShipController : NetworkBehaviour
{
    public NetClientController netClientController;

  

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

    }
}
