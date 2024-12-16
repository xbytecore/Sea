using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Mirror;
public class ShipNetModel : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnSetIsLemeChanged))]
    public bool isLemeOn;

    [SyncVar(hook = nameof(OnSetIsAnchorChanged))]
    public bool isAncorOn;

    [SyncVar(hook = nameof(OnSetIsVelaChanged))]
    public bool isVelaOn;


    [HideInInspector]
    public UnityEvent<bool> OnVela = new UnityEvent<bool>();

    [HideInInspector]
    public UnityEvent<bool> OnAnchor = new UnityEvent<bool>();



    public override void OnStartClient()
    {
        Invoke("WaitToStart", 1);
    }
    public void WaitToStart()
    {
        OnVela.Invoke(isVelaOn);
    }


    [Command]
    public void CmdSetChangeVela(NetworkConnectionToClient conn)
    {
        // Altera a SyncVar no servidor
        isVelaOn = !isVelaOn; // Alterna o estado da vela
    }
    public void OnSetIsVelaChanged(bool old, bool neww)
    {
        OnVela.Invoke(neww);
    }


    [Command]
    public void CmdSetChangeLeme(NetworkConnectionToClient conn)
    {
        // Altera a SyncVar no servidor
        isLemeOn = !isLemeOn; // Alterna o estado da vela
    }
    public void OnSetIsLemeChanged(bool old, bool neww)
    {
      //  OnVela.Invoke(neww);
    }

    [Command]
    public void CmdSetChangeAnchor(NetworkConnectionToClient conn)
    {
        // Altera a SyncVar no servidor
        isAncorOn = !isAncorOn; // Alterna o estado da vela
    }
    public void OnSetIsAnchorChanged(bool old, bool neww)
    {
          OnAnchor.Invoke(neww);
    }
}
