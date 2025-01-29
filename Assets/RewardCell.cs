using UnityEngine;

public class RewardCell : MonoBehaviour
{
    public GameObject check;

    public GameObject choose;

    public GameObject lastBG;

    public int rewardNum;
    public PriceType priceType;

    public void Choosed()
    {
        check.SetActive(true);
        lastBG.SetActive(false);
    }

    public void UnChoosed()
    {
        check.SetActive(false);
        lastBG.SetActive(true);
    }

    public void ActiveCell()
    {
        choose.SetActive(true);
    }
    public void DisActiveCell()
    {
        choose.SetActive(false);
    }
}
