using UnityEngine;
using Mirror;

public class NetworkManagerCyya : NetworkManager
{
    public GameObject netPlayerPrefab;
    public GameObject netShipPrefab;


    private void Start()
    {
#if UNITY_STANDALONE_LINUX || PLATFORM_STANDALONE_LINUX || UNITY_STANDALONE_LINUX_API
        StartServer();

        return;
#endif
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Client Connected  ----- " + conn);

        base.OnServerAddPlayer(conn);

        SetSpawnClientGame(conn);
    }
    public void SetSpawnClientGame(NetworkConnectionToClient conn)
    {
        Debug.Log(" Set_Spawn_Car(NetworkConnectionToClient conn)");

        NetClientController netClientController = conn.identity.GetComponent<NetClientController>();



        ///

        GameObject netPlayerGO= Instantiate(netPlayerPrefab);

        NetPlayerController netPlayerController = netPlayerGO.GetComponent<NetPlayerController>();

        NetworkServer.Spawn(netPlayerGO, conn);


        netClientController.netPlayerController = netPlayerController;
        netPlayerController.netClientController = netClientController;


        netClientController.TargetAssignNetPlayerController(conn, netPlayerController);
        netPlayerController.TargetAssignNetClientController(conn, netClientController);
        ///

        ///

        GameObject netShipGO = Instantiate(netShipPrefab);

        NetShipController netShipController = netShipGO.GetComponent<NetShipController>();

        NetworkServer.Spawn(netShipGO, conn);

        netClientController.netShipController = netShipController;
        netShipController.netClientController = netClientController;


        netClientController.TargetAssignNetShipController(conn, netShipController);
        netShipController.TargetAssignNetClientController(conn, netClientController);

    }
}
