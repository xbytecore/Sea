using UnityEngine;
using Mirror;
using System;
public class ShipNetController : NetworkBehaviour
{
    public NetClientController netClientController;

    public ShipNetView shipNetView;

    public GameObject projectilePrefab;

    public ShipNetModel shipNetModel;

  


    #region Start
    private void Start()
    {
        shipNetModel.OnVela.AddListener(shipNetView.SetChangeVela);
        shipNetModel.OnAnchor.AddListener(shipNetView.SetChangeAnchor);
    }
    private void OnDestroy()
    {
        shipNetModel.OnVela.RemoveAllListeners();
        shipNetModel.OnAnchor.RemoveAllListeners();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
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
    #endregion


    [Client]
    public void SetChangeSail()
    {
        NetworkConnectionToClient conn = netClientController.GetComponent<NetworkIdentity>().connectionToClient;

        shipNetModel.CmdSetChangeVela(conn);
    }

    [Client]
    public void SetChangeLeme()
    {
        NetworkConnectionToClient conn = netClientController.GetComponent<NetworkIdentity>().connectionToClient;

        shipNetModel.CmdSetChangeLeme(conn);
    }

    [Client]
    public void SetChangeAnchor()
    {
        NetworkConnectionToClient conn = netClientController.GetComponent<NetworkIdentity>().connectionToClient;

        shipNetModel.CmdSetChangeAnchor(conn);
    }

    [Client]
    public void SetChangeCannon(int id)
    {
        CmdShoot(id);
    }


    [Command]
    void CmdShoot(int id)
    {
        // Instancia o projétil no servidor
        GameObject projectile = Instantiate(projectilePrefab, shipNetView.cannon[id].transform.position, shipNetView.cannon[id].transform.rotation);

        // Spawna o projétil na rede
        NetworkServer.Spawn(projectile,connectionToClient);

        // Aplica a velocidade no projétil

    }

    
    public void SetChangeDamagedPart(GameObject go, int id)
    {
        CmdChangeDamagetPart(go,id);
    }
    [Command]
    void CmdChangeDamagetPart(GameObject go, int id)
    {
        NetworkConnectionToClient conn = go.GetComponent<NetworkIdentity>().connectionToClient;
        conn.identity.GetComponent<NetClientController>().netShipController.RpcChangeDamagedPartRed(id);

    }
    [ClientRpc]
    void RpcChangeDamagedPartRed(int id)
    {
        Debug.Log("aloo");
        shipNetView.damagedPart[id].GetComponent<SpriteRenderer>().color = Color.red;
    }

    [Client]
    public void SetChangeDamagedPartGreen(int id)
    {
        CmdChangeDamagedPartGreen(id);
    }
    [Command]
    public void CmdChangeDamagedPartGreen(int id)
    {
        RpcChangeDamagedPartWhite(id);
    }
    [ClientRpc] 
    public void RpcChangeDamagedPartWhite(int id)
    {
        shipNetView.damagedPart[id].GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void SetStartNewPlayer()
    {

    }
}














/*
  [Command]
  void CmdSetChangeSail(NetworkConnectionToClient sender = null)
  {
      shipNetModel.SetChangeVela();
  }
  [ClientRpc]
  void RpcSetChangeSail()
  {
      //Debug.Log("ClientRpc chamado no cliente!");


    //  shipNetModel.SetChangeVela();
  }

  */