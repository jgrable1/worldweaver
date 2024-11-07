using UnityEngine;

public class PlayerStatUI : MonoBehaviour
{
    public RectTransform RectTransformComponent;
    private int barWidth = 200; // Width of playerbar
    private int playerMaxStat = 10; // stat of the player
    public void updateStat(int currStat){
        float newBarWidth = currStat * barWidth / playerMaxStat;
        RectTransformComponent.sizeDelta = new Vector2(newBarWidth, RectTransformComponent.sizeDelta.y);
    }
    public void updateStat(float currStat){
        float newBarWidth = currStat * barWidth / playerMaxStat;
        RectTransformComponent.sizeDelta = new Vector2(newBarWidth, RectTransformComponent.sizeDelta.y);
    }
}
