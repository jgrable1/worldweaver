using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    public RectTransform RectTransformComponent;
    private int barMaxHP = 300; // Width of playerbar
    private int playerMaxHP = 10; // HP of the player
    public void updatePlayerHP(int currHP){
        float newHPBarWidth = currHP * barMaxHP / playerMaxHP;
        RectTransformComponent.sizeDelta = new Vector2(newHPBarWidth, RectTransformComponent.sizeDelta.y);
    }
}
