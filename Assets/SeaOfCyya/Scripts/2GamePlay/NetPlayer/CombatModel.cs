using UnityEngine;
using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine.Events;
public class CombatModel : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnSetLifeChanged))]
    public float lifeCurrent;
    public float lifeMax;
    public float dmg;
    public bool canAttack;

    [HideInInspector]
    public UnityEvent<float,float> OnLifeChange = new UnityEvent<float, float>();
    public float GetDamage()
    {
        return dmg;
    }

    [Command]
    public void SetCmdDamage(float senderDamage, GameObject receiverDamage)
    {

        if(receiverDamage == null) { return; }

        NetworkConnectionToClient conn = receiverDamage.GetComponent<NetworkIdentity>().connectionToClient;
        
        if (conn == null) // mob
        {
            Debug.Log("conn.identity.name" + receiverDamage.GetComponent<NetworkIdentity>().name);

            CombatModel receivarCombatModel = receiverDamage.GetComponent<NetworkIdentity>().gameObject.GetComponent<CombatModel>();

            receivarCombatModel.SetApplyDamage(senderDamage);
        }
     
        else // player
        {
            CombatController receiverCombatController = conn.identity.GetComponent<NetClientController>().netPlayerController.GetComponent<CombatController>();
            
            receiverCombatController.combatModel.SetApplyDamage(senderDamage);
        }
    }


    public void SetDamage(float senderDamage, GameObject receiverDamage)
    {

        if (receiverDamage == null) { return; }

        NetworkConnectionToClient conn = receiverDamage.GetComponent<NetworkIdentity>().connectionToClient;

        if (conn == null) // mob
        {
            Debug.Log("conn.identity.name" + receiverDamage.GetComponent<NetworkIdentity>().name);

            CombatModel receivarCombatModel = receiverDamage.GetComponent<NetworkIdentity>().gameObject.GetComponent<CombatModel>();

            receivarCombatModel.SetApplyDamage(senderDamage);
        }

        else // player
        {
            CombatController receiverCombatController = conn.identity.GetComponent<NetClientController>().netPlayerController.GetComponent<CombatController>();

            receiverCombatController.combatModel.SetApplyDamage(senderDamage);
        }
    }
    public void OnSetLifeChanged(float old, float neww)
    {
        
        OnLifeChange.Invoke(neww, lifeMax);

        if(!isOwned) { return; }

        if(neww > 0) { return; }

        Camera.main.transform.SetParent(null);
    }
    public void SetApplyDamage(float otherDamage)
    {
        otherDamage = SetCalculateDamage(otherDamage);

        lifeCurrent -= otherDamage;

        if(lifeCurrent > 0) { return; }

        

        NetworkServer.Destroy(gameObject);


    }
    public float SetCalculateDamage(float otherDamage)
    {
        return otherDamage;
    }
    public void SetChangeCanAttack(bool canAttackTemp)
    {
        canAttack = canAttackTemp;
    }
}
