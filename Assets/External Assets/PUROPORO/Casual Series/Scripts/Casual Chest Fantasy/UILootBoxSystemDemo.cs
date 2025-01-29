using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PUROPORO
{
    /// <summary>
    /// Loot Box Demo.
    ///
    /// The demo showcases yet another usage for the chest in games.
    /// This demo debuts the loot box event, which is a common occurrence in mobile games.
    ///
    /// This class is used to manage events, update the UI, draw cards, and spawn cards,
    /// among other things.
    ///
    /// The database, which is made possible by the use of ScriptableObjects,
    /// has a list of the various cards. For more information about these,
    /// please check https://docs.unity3d.com/Manual/class-ScriptableObject.html
    /// </summary>
    public class UILootBoxSystemDemo : MonoBehaviour
    {
        private LootBoxState m_State;
        private int m_TotalRange;

        [Header("LootBox")]
        [SerializeField] private LootBoxDemo m_LootBox;

        [Header("UI Elements")]
        [SerializeField] private CardCounter m_Counter;
        [SerializeField] private Animator m_Button;
        [SerializeField] private GameObject m_TextYouGot;
        [SerializeField] private GameObject m_TextPressContinue;
        [SerializeField] private GameObject m_TextWelcome;
        private int m_Count;

        [Header("Cards")]
        [SerializeField] private AnimateAToBUi m_CardAnimator;
        [SerializeField] private CardUI m_CardUI;
        [SerializeField] private GameObject m_CardUiGo;
        [SerializeField] private RectTransform m_AchievedCardsUI;
        // [SerializeField] private SOCardsDB m_CardsDB; // ScriptableObject
        [SerializeField] private List<SOCard> m_Card;
        private int CardNum = 0;
        private ProbabilityRange[] m_ProbabilityRange;
        private CardUI[] m_CardUIs = new CardUI[0];
        private int[] m_AchievedCards;

        [Header("Chest Effects")]
        [SerializeField] private ParticleSystem m_godRay;

        [SerializeField] private UIManager _UIManager;
        private List<GameObject> _shownCards = new List<GameObject>();


        [System.Obsolete]
        private void Start()
        {
           

            m_State = LootBoxState.Waiting;

            m_CardAnimator = GetComponent<AnimateAToBUi>();

            //m_TotalRange = 0;
            //m_ProbabilityRange = new ProbabilityRange[m_CardsDB.GetCount()];

            ///// This method calculates and lists the probability for each card.
            //for (int i = 0; i < m_ProbabilityRange.Length; i++)
            //{
            //    m_ProbabilityRange[i] = new ProbabilityRange();
            //    m_ProbabilityRange[i].m_Min = m_TotalRange;
            //    m_TotalRange += (int)m_CardsDB.GetCard(i).GetRarity();
            //    m_ProbabilityRange[i].m_Max = m_TotalRange;
            //}

            ShowIntro();
        }

        private void OnEnable()
        {
            ShowIntro();    
        }

        [System.Obsolete]
        public void OnClick()
        {
            switch (m_State)
            {
                case LootBoxState.Opening:
                    SpawnCard();
                    return;
                case LootBoxState.Ending:
                    ShowEnding();
                    return;
                case LootBoxState.AfterEnd:
                    AfterEnd();
                    return;
                case LootBoxState.Waiting:
                    //  SpawnChest();

                    return;
                default:
                    return;
            }
        }

            /// <summary>
            /// Spawn a new chest and reset a UI for the new opening event.
            /// </summary>
            [System.Obsolete]
        private void SpawnChest()
        {          
            if (m_CardUIs.Length > 0)
            {
                for (int i = 0; i < m_CardUIs.Length; i++)
                    Destroy(m_CardUIs[i].gameObject);
            }

            m_TextWelcome.SetActive(false);
            m_TextYouGot.SetActive(false);
            m_TextPressContinue.SetActive(false);
            m_State = LootBoxState.Opening;
        }

        /// <summary>
        /// Spawn a new card at random from the database and display it with animations.
        /// The function contains a simple lottery method that draws cards according to probabilities.
        /// The card probabilities are calculated in the Start function.
        /// The function also gives the command to stop drawing when the cards run out of the chest.
        /// </summary>
        [System.Obsolete]
        private void SpawnCard()
        {
            m_TextWelcome.SetActive(false);

            if (m_State != LootBoxState.Opening) return;
                                                            
                    SOCard temp = m_Card[CardNum];
            CardNum++;
                    //m_AchievedCards[m_Count - 1] = j;
                    m_CardUI.SetCard(GetRarityColor(temp.GetRarity()), temp.GetImage(), temp.GetName());
                    m_LootBox.SetRarityColor(GetRarityColor(temp.GetRarity()));


            m_LootBox.ChestQuickOpens();
            m_CardAnimator.StartAnimation(0);

            m_Count--;
            m_Counter.UpdateCounter(m_Count);


            if (m_Count <= 0)
            {
                m_LootBox.ChestEmpty();

                m_TextPressContinue.SetActive(true);
                m_State = LootBoxState.Ending;
            }
        }

        /// <summary>
        /// Shows Loot Box's start intro.
        /// </summary>
        [System.Obsolete]
        public void ShowIntro()
        {
            m_TextWelcome.SetActive(true);
            m_TextYouGot.SetActive(false);
            //m_TextPressContinue.SetActive(true);
            
            m_Count = m_LootBox.ChestChangeGraphics();

            if (m_Count == 6 || m_Count == 3)
                m_AchievedCardsUI.GetComponent<GridLayoutGroup>().constraintCount = 3;
            else
                m_AchievedCardsUI.GetComponent<GridLayoutGroup>().constraintCount = 4;

            m_Counter.gameObject.SetActive(true);
            m_Counter.UpdateCounter(m_Count);

            m_State = LootBoxState.Opening;
            m_AchievedCards = new int[m_Count];

            m_LootBox.ChestDrop();
        }

        /// <summary>
        /// When all the cards have been drawn you better call this function.
        /// The function disposes of the empty chest and displays all the drawn cards in (a) row(s).
        /// </summary>
        private void ShowEnding()
        {
            if (m_State != LootBoxState.Ending) return;

            m_LootBox.ChestDisappear();
            m_CardAnimator.ResetAnimation();

            m_CardUIs = new CardUI[m_AchievedCards.Length];

            for (int i = 0; i < CardNum; i++)
            {
                GameObject go = Instantiate(m_CardUiGo, m_AchievedCardsUI);
                SOCard temp = m_Card[i];
                go.GetComponent<CardUI>().SetCard(GetRarityColor(temp.GetRarity()), temp.GetImage(), temp.GetName());
                m_CardUIs[i] = go.GetComponent<CardUI>();
                _shownCards.Add(go);
            }

            for (int i = 0; i < m_CardUIs.Length; i++)
                m_CardUIs[i].GetComponent<AnimateScale>().StartAnimation((m_CardUIs.Length - i) * 0.1f);

            m_Counter.gameObject.SetActive(false);

            m_TextYouGot.SetActive(true);
            m_TextPressContinue.SetActive(true);
            m_State = LootBoxState.AfterEnd;
            m_godRay.Stop();

            m_Button.Play("ButtonFadeIn");

        }

        private void AfterEnd()
        {
            _UIManager.AfterChest();
            foreach (var card in _shownCards)
            {
                Destroy(card);
            }

            _shownCards.Clear();
            CardNum = 0;

        }

        /// <summary>
        /// Returns the color of rarity by rarity enum.
        /// </summary>
        /// <param name="r">Enum of Rarity</param>
        /// <returns>Color of Rarity (Color32)</returns>
        public Color GetRarityColor(Rarity r)
        {//temporary switched to transparent
            switch (r)
            {
                case Rarity.common:
                    return new Color32(188, 188, 188, 0);
                case Rarity.uncommon:
                    return new Color32(165, 226, 57, 0);
                case Rarity.rare:
                    return new Color32(74, 160, 241, 0);
                case Rarity.epic:
                    return new Color32(202, 67, 250, 0);
                case Rarity.legendary:
                    return new Color32(255, 225, 0, 0);
                default:
                    return new Color32(188, 188, 188, 0);
            }
        }
    }

    [System.Serializable]
    public class ProbabilityRange
    {
        public int m_Min;
        public int m_Max;
    }

    public enum Rarity
    {
        none = 100,
        common = 40,
        uncommon = 30,
        rare = 20,
        epic = 10,
        legendary = 5
    }

    public enum LootBoxState
    {
        Starting,
        Opening,
        Ending,
        AfterEnd,
        Waiting
    }
}
