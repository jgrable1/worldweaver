using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoBehaviour
{
    public RectTransform RectTransformComponent;
    private int barWidth = 200; // Width of playerbar
    private int playerMaxStat = 10; // stat of the player
    public TextMeshProUGUI statAmount;
    public void updateStat(int currStat){
        float newBarWidth = currStat * barWidth / playerMaxStat;
        RectTransformComponent.sizeDelta = new Vector2(newBarWidth, RectTransformComponent.sizeDelta.y);
        statAmount.text = currStat + "/" + playerMaxStat;
    }
    public void updateStat(float currStat){
        float newBarWidth = currStat * barWidth / playerMaxStat;
        RectTransformComponent.sizeDelta = new Vector2(newBarWidth, RectTransformComponent.sizeDelta.y);
        statAmount.text = Mathf.Round(10*currStat) + "%";
    }
}
