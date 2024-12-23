using UnityEngine;

public class HitHandler : MonoBehaviour
{
    public CombatController combatController;

    private void Start()
    {
        combatController = transform.root.gameObject.GetComponent<CombatController>();

      
    }
    private void OnEnable()
    {
        Invoke("SetDisable", 0.5f);
    }
    public void SetDisable()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.root.TryGetComponent<CombatController>(out CombatController otherCombatController)) { return; }

        if (combatController.combatModel.isServer)
        {
            if (combatController.netPlayerController != null) { return; }

            if (combatController == otherCombatController) { return; }

            float myDamage = combatController.combatModel.GetDamage();

            combatController.SetDamage(myDamage, otherCombatController.gameObject);
        }

        else if (combatController.combatModel.isOwned)
        {

            if (combatController == otherCombatController) { return; }

            float myDamage = combatController.combatModel.GetDamage();

            combatController.SetDamage(myDamage, otherCombatController.gameObject);
        }

    }
}
