using UnityEngine;
using UnityEngine.UI;
public class LifeBarHandler : MonoBehaviour
{
    public Image hudLifeBar;

    public void SetLifePercent(float currentLife, float maxLife)
    {
        hudLifeBar.fillAmount = currentLife / maxLife;
    }
}
