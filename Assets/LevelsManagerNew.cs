using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManagerNew : MonoBehaviour
{
    [SerializeField] public Slider charter1Slider;
    [SerializeField] public Slider charter2Slider;

    public TMP_Text text1;
    public TMP_Text text2;

    public TMP_Text name1;
    public TMP_Text name2;

    public GameObject textUnavail;
    public GameObject picUnavail;
    public GameObject butUnavail;
    private void OnEnable()
    {
        charter1Slider.value = PrefsManager.GetUnlockedLevelCharter1();
        charter2Slider.value = PrefsManager.GetUnlockedLevelCharter2();

        text1.text = (PrefsManager.GetUnlockedLevelCharter1() * 25).ToString() + "%";
        text2.text = (PrefsManager.GetUnlockedLevelCharter2() * 25).ToString() + "%";

        var lev1 = PrefsManager.GetChosenLevelCharter1() + 1;
        if ( lev1 >4 ) lev1 = 4;
        var lev2 = PrefsManager.GetChosenLevelCharter2() + 1;
        if (lev2 > 4) lev2 =4;

        name1.text = "Charter 1-" + lev1 + "\n" + "Hidden Forest";
        name2.text = "Charter 2-" + lev2 + "\n" + "Frozen Island";

        if (PrefsManager.GetUnlockedLevelCharter1() >= 4)
        {
            picUnavail.SetActive(false);
            butUnavail.SetActive(false);
            textUnavail.SetActive(false);
        }
        else
        {
            picUnavail.SetActive(true);
            butUnavail.SetActive(true);
            textUnavail.SetActive(true);
        }
    }
}
