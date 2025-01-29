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
    public class UILootBoxSystemCustom : MonoBehaviour
    {
        private LootBoxState m_State;
        private int m_TotalRange;

        [Header("LootBox")]
        [SerializeField] private LootBoxDemo m_LootBox;

        [Header("UI Elements")]
        [SerializeField] private CardCounter m_Counter;

        private int m_Count;

        [Header("Cards")]
        [SerializeField] private AnimateAToBUi m_CardAnimator;
        [SerializeField] private CardUI m_CardUI;
        [SerializeField] private GameObject m_CardUiGo;
        [SerializeField] private RectTransform m_AchievedCardsUI;
        [SerializeField] private SOCardsDB m_CardsDB; // ScriptableObject
        private ProbabilityRange[] m_ProbabilityRange;
        private CardUI[] m_CardUIs = new CardUI[0];
        private int[] m_AchievedCards;

        private void Start()
        {
            m_State = LootBoxState.Opening;

            m_CardAnimator = GetComponent<AnimateAToBUi>();

            m_TotalRange = 0;
            m_ProbabilityRange = new ProbabilityRange[m_CardsDB.GetCount()];

            /// This method calculates and lists the probability for each card.
            for (int i = 0; i < m_ProbabilityRange.Length; i++)
            {
                m_ProbabilityRange[i] = new ProbabilityRange();
                m_ProbabilityRange[i].m_Min = m_TotalRange;
                m_TotalRange += (int)m_CardsDB.GetCard(i).GetRarity();
                m_ProbabilityRange[i].m_Max = m_TotalRange;
            }

            m_Count = m_LootBox.ChestChangeGraphics();

            if (m_Count == 6 || m_Count == 3)
                m_AchievedCardsUI.GetComponent<GridLayoutGroup>().constraintCount = 3;
            else
                m_AchievedCardsUI.GetComponent<GridLayoutGroup>().constraintCount = 4;

           // m_Counter.gameObject.SetActive(true);
           // m_Counter.UpdateCounter(m_Count); 
            m_AchievedCards = new int[m_Count];

        }

       
        public void OnClick()
        {
            SpawnCard();
        }

        /// <summary>
        /// Spawn a new chest and reset a UI for the new opening event.
        /// </summary>
      
        /// <summary>
        /// Spawn a new card at random from the database and display it with animations.
        /// The function contains a simple lottery method that draws cards according to probabilities.
        /// The card probabilities are calculated in the Start function.
        /// The function also gives the command to stop drawing when the cards run out of the chest.
        /// </summary>
        [System.Obsolete]
        private void SpawnCard()
        {
            if (m_State != LootBoxState.Opening) return;

            int i = Random.Range(0, m_TotalRange);

            for (int j = 0; j < m_ProbabilityRange.Length; j++)
            {
                if (i >= m_ProbabilityRange[j].m_Min && i <= m_ProbabilityRange[j].m_Max)
                {
                    SOCard temp = m_CardsDB.GetCard(j);
                    m_AchievedCards[m_Count - 1] = j;
                    m_CardUI.SetCard(GetRarityColor(temp.GetRarity()), temp.GetImage(), temp.GetName());
                    m_LootBox.SetRarityColor(GetRarityColor(temp.GetRarity()));
                    break;
                }
            }

            m_LootBox.ChestQuickOpens();
            m_CardAnimator.StartAnimation(0);

            m_Count--;
            m_Counter.UpdateCounter(m_Count);

            if (m_Count <= 0)
            {
                m_LootBox.ChestEmpty();
                m_State = LootBoxState.Ending;
            }
        }

        /// <summary>
        /// Returns the color of rarity by rarity enum.
        /// </summary>
        /// <param name="r">Enum of Rarity</param>
        /// <returns>Color of Rarity (Color32)</returns>
        public Color GetRarityColor(Rarity r)
        {
            switch (r)
            {
                case Rarity.common:
                    return new Color32(188, 188, 188, 255);
                case Rarity.uncommon:
                    return new Color32(165, 226, 57, 255);
                case Rarity.rare:
                    return new Color32(74, 160, 241, 255);
                case Rarity.epic:
                    return new Color32(202, 67, 250, 255);
                case Rarity.legendary:
                    return new Color32(255, 225, 0, 255);
                default:
                    return new Color32(188, 188, 188, 255);
            }
        }
    }
}
