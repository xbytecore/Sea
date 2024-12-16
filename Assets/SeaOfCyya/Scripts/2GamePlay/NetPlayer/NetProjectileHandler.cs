using Mirror;
using Mirror.Examples.Tanks;
using UnityEngine;

public class NetProjectileHandler : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = -transform.right * 10f; // 10f � a velocidade do proj�til
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Exibe o nome do objeto colidido no console
       

        // Verifica se o objeto possui o componente ShipDamagedPartHandler
        if (other.gameObject.TryGetComponent<ShipDamagedPartHandler>(out ShipDamagedPartHandler damagedPart))
        {
            Debug.Log($"OnTriggerEnter2D: Colidiu com {other.gameObject.name}");
            if (!isOwned) return;

            ShipNetController otherShip = damagedPart.gameObject.transform.root.GetComponent<ShipNetController>();
        
            if (otherShip.isOwned) { return; }

            GameObject playerGameObject = NetworkClient.localPlayer.gameObject;

            // Ou, se você precisar de um componente específico:
            ShipNetController shipController = playerGameObject.GetComponent<NetClientController>().netShipController;
           
            GameObject go = otherShip.gameObject;
            shipController.SetChangeDamagedPart(go,damagedPart.id);

            CmdDestroy();

        }
    }
    [Command]
    public void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    
}
