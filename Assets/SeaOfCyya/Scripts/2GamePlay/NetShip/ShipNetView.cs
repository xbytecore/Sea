using UnityEngine;
using System.Collections.Generic;

public class ShipNetView : MonoBehaviour
{
    public GameObject vela;
    public GameObject mastro;
    public GameObject leme;
    public GameObject anchor;

    public List<GameObject> cannon;
    public List<GameObject> damagedPart;

    private void Start()
    {
        for(int i = 0; i < cannon.Count; i++)
        {
            cannon[i].GetComponent<ShipCannonHandler>().id = i;
        }
        for (int i = 0; i < damagedPart.Count; i++)
        {
            damagedPart[i].GetComponent<ShipDamagedPartHandler>().id = i;
        }
    }
    public void SetChangeVela(bool isSailOn)
    {
        if (isSailOn)
        {
            vela.gameObject.SetActive(true);
            mastro.gameObject.SetActive(false);
        }
        else
        {
            vela.gameObject.SetActive(false);
            mastro.gameObject.SetActive(true);

        }
    }
    public void SetChangeAnchor(bool isAnchorOn)
    {
        if (isAnchorOn)
        {
            anchor.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            anchor.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
