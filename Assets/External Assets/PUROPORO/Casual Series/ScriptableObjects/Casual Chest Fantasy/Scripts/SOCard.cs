using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUROPORO
{
    [CreateAssetMenu(fileName = "Card", menuName = "PUROPORO/Casual/Card")]
    [System.Serializable]
    public class SOCard : ScriptableObject
    {
        [SerializeField] private Rarity m_CardRarity;
        [SerializeField] public string m_CardText;
        [SerializeField] private Sprite m_CardImage;

        public Rarity GetRarity()
        {
            return m_CardRarity;
        }

        public string GetName()
        {
            return m_CardText;
        }

        public Sprite GetImage()
        {
            return m_CardImage;
        }
    }
}
