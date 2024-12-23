using Mirror;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public CombatModel combatModel;
    public CombatView combatView;
    public NetPlayerController netPlayerController;
    public HitHandler hitHandler;
    public AnimationHandler animationHandler;
    private void Start()
    {
        combatModel = GetComponent<CombatModel>();
        combatView = GetComponent<CombatView>();    
        netPlayerController = GetComponent<NetPlayerController>();

        combatModel.OnLifeChange.AddListener(combatView.lifeBarHandler.SetLifePercent);
    }
    private void OnDisable()
    {
        combatModel.OnLifeChange.RemoveListener(combatView.lifeBarHandler.SetLifePercent);
    }
    
    public void SetDamage(float senderDamage, GameObject receiverDamage)
    {
        if (combatModel.isServer)
        {
            combatModel.SetDamage(senderDamage, receiverDamage);
        }
        else
        {
            combatModel.SetCmdDamage(senderDamage, receiverDamage);
        }
       
    }
    
    
    public void Update()
    {
        if (!combatModel.isOwned) { return; }
        
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

        SetPerformAttack();

    }

    public void SetPerformAttack()
    {
        if (!combatModel.canAttack) { return; }

        SetAnimStart();
    }
    public void SetAnimStart()
    {
        combatModel.SetChangeCanAttack(false);

        animationHandler.SetAnimStart("attack");
    }
    public void SetAnimTrigger1()
    {
        hitHandler.gameObject.SetActive(true);
    }
    public void SetAnimEnd()
    {
        combatModel.SetChangeCanAttack(true);
        
    }

}
