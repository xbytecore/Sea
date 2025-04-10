using UnityEngine;
using Mirror;

public class NetworkManagerCyya : NetworkManager
{
    public GameObject netPlayerPrefab;
    public GameObject netShipPrefab;
    public GameObject netMobPrefab;



    private void Start()
    {
#if UNITY_STANDALONE_LINUX || PLATFORM_STANDALONE_LINUX || UNITY_STANDALONE_LINUX_API
        StartServer();

        return;
#endif
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        InvokeRepeating("SpawnMob", 1,15);
    }
    public void SpawnItem()
    {
        if (FindObjectsOfType<ResourceItemWorldHandler>().Length > 10) { return; }


        Transform newPosition = ResourceInventoryController.instance.itemSpawnPosition;

        ResourceItemModelScriptable newResourceItemModel = new ResourceItemModelScriptable();

        newResourceItemModel.SetInfos();

        ResourceInventoryController.instance.SetSpawnWorldItem(newResourceItemModel,0, newPosition.position);

    }
    public void SpawnMob()
    {
        // Instancia o mob no servidor
        SpawnItem();


        if (FindObjectsOfType<CombatController>().Length > 10) { return; }

        Transform newPosition = ResourceInventoryController.instance.mobSpawnPosition;

        GameObject mob = Instantiate(netMobPrefab, newPosition.position, netMobPrefab.transform.rotation);

      
        // Spawna o mob na rede, sem dono
        NetworkServer.Spawn(mob);

     
        Debug.Log("Mob spawnado pelo servidor!");
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

        ShipNetController netShipController = netShipGO.GetComponent<ShipNetController>();

        NetworkServer.Spawn(netShipGO, conn);

        netClientController.netShipController = netShipController;
        netShipController.netClientController = netClientController;


        netClientController.TargetAssignNetShipController(conn, netShipController);
        netShipController.TargetAssignNetClientController(conn, netClientController);

    }
}
