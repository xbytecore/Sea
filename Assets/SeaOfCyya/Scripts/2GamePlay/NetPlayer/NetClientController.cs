using UnityEngine;
using Mirror;
public class NetClientController : NetworkBehaviour
{
    public NetPlayerController netPlayerController;
    public NetShipController   netShipController;
    public HudManager hudManager;

    public ClientInfo clientInfo = new ClientInfo();

    public bool isMatchClient;
    public bool isMyClient;



    public override void OnStartAuthority()
    {
        base.OnStartClient();
      

        if (!isOwned) { return; }

       // hudManager = FindObjectOfType<HudManager>();

       // hudManager.netClientController = this;

        // prencher client info
        /*
        ClientInfo userInfos = APIManager.instance.clientInfo;

        clientInfo.userID = userInfos.userID;
        clientInfo.userIndex = userInfos.userIndex;
        clientInfo.clientName = userInfos.clientName;
        clientInfo.queueType = userInfos.queueType;

        */
        Debug.Log("OnStartAuthority .....");

        SetClientInfosCmd(gameObject, clientInfo);

    }


    [TargetRpc]
    public void TargetAssignNetPlayerController(NetworkConnectionToClient conn, NetPlayerController newNetPlayerController)
    {
        netPlayerController = newNetPlayerController;
    }
    [TargetRpc]
    public void TargetAssignNetShipController(NetworkConnectionToClient conn, NetShipController newNetShipController)
    {
        netShipController = newNetShipController;
    }



    [Command]
    public void SetClientInfosCmd(GameObject clientServer, ClientInfo userInfos)
    {
        Debug.Log("SetClientInfosCmd(GameObject clientServer, ClientInfo userInfos)");

        clientInfo.userIndex = userInfos.userIndex;
        clientInfo.clientName = userInfos.clientName;
       // clientInfo.queueType = userInfos.queueType;

        NetworkConnectionToClient conn = clientServer.GetComponent<NetworkIdentity>().connectionToClient;

        
    }
}
