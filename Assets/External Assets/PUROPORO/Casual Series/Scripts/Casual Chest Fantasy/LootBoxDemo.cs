﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUROPORO
{
    /// <summary>
    /// Loot Box Demo.
    ///
    /// The demo showcases yet another usage for the chest in games.
    /// This demo debuts the loot box event, which is a common occurrence in mobile games.
    ///
    /// The animations and particle effects of the chest are managed in this class.
    ///
    /// Please, pay close attention to how the CrossFade animation method is used in the demo.
    /// Check https://docs.unity3d.com/ScriptReference/Animation.CrossFade.html
    ///
    /// The database, which is made possible by the use of ScriptableObjects,
    /// has a list of the various chests. For more information about these,
    /// please check https://docs.unity3d.com/Manual/class-ScriptableObject.html
    /// </summary>
    public class LootBoxDemo : MonoBehaviour
    {
        public bool IsURPDemo;
        public MaterialType MaterialType;

        [Header("Chest")]
        [SerializeField] private SOChestsDB m_ChestsDB; // ScriptableObject
        private SkinnedMeshRenderer m_ChestMeshRenderer;
        private bool m_IsEmpty;
        private bool m_IsGlows;

        private Material m_Material;
        private const string c_BaseMap = "_BaseMap";
        private const string c_MaskMap = "_MaskMap";
        private const string c_GlowingPower = "_VFXGlowingPower";

        // Members for CrossFade-animation method.
        // Check https://docs.unity3d.com/ScriptReference/Animation.CrossFade.html
        private Animator m_Anim;
        private int m_CurrentAnimState;
        private static readonly int c_AnimIdle = Animator.StringToHash("Idle");
        private static readonly int c_AnimOpens = Animator.StringToHash("Opens");
        private static readonly int c_AnimCloses = Animator.StringToHash("Closes");
        private static readonly int c_AnimDisappears = Animator.StringToHash("Disappears");
        private static readonly int c_AnimAppears = Animator.StringToHash("Appears");
        private static readonly int c_AnimDrops = Animator.StringToHash("Drops");
        private static readonly int c_AnimQuickOpens = Animator.StringToHash("Quick Opens");

        [Header("Particles")]
        [SerializeField] private ParticleSystem m_ParticlesSpawnChest;
        [SerializeField] private ParticleSystem m_ParticlesSpawnCard;
        [SerializeField] private ParticleSystem m_ParticlesDisappearChest;
        [SerializeField] private ParticleSystem m_FinalOpenChest1;
        [SerializeField] private ParticleSystem m_FinalOpenChest2;

        [SerializeField] private GameObject m_pressToCont;

        [SerializeField] private int _amountCards;
        [SerializeField] private GameObject _mesh;

        private Color32 m_RarityColor;



        private void Awake()
        {
            m_Anim = GetComponent<Animator>();


            m_ChestMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            m_Material = m_ChestMeshRenderer.materials[0];



            m_IsEmpty = false;
            m_IsGlows = true;
        }

        [System.Obsolete]
        private void Start()
        {

        }

        [System.Obsolete]
        public void OnDisable()
        {
            m_Material?.SetFloat(c_GlowingPower, 0);
            m_IsEmpty = false;
            m_ParticlesSpawnChest.startDelay = 0.3f;
            m_ParticlesSpawnChest.Play();
            transform.localScale /= 1000;
            _mesh.SetActive(true);
        }

        private void LateUpdate()
        {
           // if (m_IsGlows && !m_IsEmpty)
             //   m_Material.SetFloat(c_GlowingPower, Mathf.SmoothStep(0, 0.4f, Mathf.PingPong(Time.time, 1)));
        }

        /// <summary>
        /// Quickly hide the chest (with animation).
        /// </summary>
        public void ChestHide()
        {
            if (m_CurrentAnimState == c_AnimDisappears) return;

            m_Anim.CrossFade(c_AnimDisappears, 0, 0);
            m_CurrentAnimState = c_AnimDisappears;
        }

        /// <summary>
        /// Change the chest's look with textures from chest database (scriptable objects).
        /// </summary>
        /// <returns>An amount of Cards.</returns>
        public int ChestChangeGraphics()
        {
            //SOChest tempChest = m_ChestsDB.GetChest(-1);

            //m_ChestMeshRenderer.sharedMesh = tempChest.GetMesh();
            //m_ChestMeshRenderer.material = tempChest.GetMaterial(MaterialType, IsURPDemo);
            //m_ChestMeshRenderer.materials[0].SetTexture(c_BaseMap, tempChest.GetBaseTexture());
            //m_ChestMeshRenderer.materials[0].SetTexture(c_MaskMap, tempChest.GetMaskMap());

            //m_Material = m_ChestMeshRenderer.materials[0];

            return _amountCards;
        }

        /// <summary>
        /// This function makes the chest glows (on/off setting). Glow-animation is animated with the LateUpdate function.
        /// </summary>
        public void ChestGlows()
        {
            m_IsGlows = !m_IsGlows;

            if (!m_IsGlows)
                m_Material.SetFloat(c_GlowingPower, 0);
        }

        /// <summary>
        /// This function makes the chest appear empty when it is empty.
        /// </summary>
        public void ChestEmpty()
        {
            if (m_CurrentAnimState == c_AnimDisappears) return;
            if (m_CurrentAnimState == c_AnimOpens) return;
            if (m_IsEmpty) return;

            m_Anim.CrossFade(c_AnimOpens, 0, 0);
            m_CurrentAnimState = c_AnimOpens;

            m_IsEmpty = true;
            m_ParticlesSpawnCard.Play();
            m_FinalOpenChest1.Play();
            m_FinalOpenChest2.Play();
            //  StartCoroutine(FadeToWhite());
        }

        /// <summary>
        /// Fade to Black animation (coroutine).
        /// </summary>
        IEnumerator FadeToWhite()
        {
            for (float f = 0; f >= 0.1f; f = 0.01f)
            {
                m_Material.SetFloat(c_GlowingPower, f);
                yield return null;
            }
        }

        /// <summary>
        /// Animate the chest quickly to open and then closes it. Also sets the rarity color for the particles.
        /// </summary>
        [System.Obsolete]
        public void ChestQuickOpens()
        {
            if (m_CurrentAnimState == c_AnimDisappears) return;
            if (m_CurrentAnimState == c_AnimOpens) return;

            m_Anim.Play(c_AnimQuickOpens, 0, 0);
            m_CurrentAnimState = c_AnimQuickOpens;
            m_ParticlesSpawnCard.Play();
        }

        /// <summary>
        /// Dispose of the chest.
        /// </summary>
        public void ChestDisappear()
        {
            //if (m_CurrentAnimState == c_AnimDisappears) return;

            //m_Anim.CrossFade(c_AnimDisappears, 0.5f, 0);
            //m_CurrentAnimState = c_AnimDisappears;
            m_Anim.CrossFade(c_AnimCloses, 0, 0);
            m_CurrentAnimState = c_AnimCloses;

            _mesh.SetActive(false);

            m_ParticlesDisappearChest.Play();
            m_FinalOpenChest1.Stop();
            m_FinalOpenChest2.Stop();

            StopAllCoroutines();
        }

        /// <summary>
        /// Animate the chest to drop from the sky.
        /// </summary>
        [System.Obsolete]
        public void ChestDrop()
        {
            //if (m_CurrentAnimState != c_AnimDisappears) return;
            if (m_CurrentAnimState == c_AnimDrops) return;



            m_Anim.CrossFade(c_AnimDrops, 0, 0);

            m_CurrentAnimState = c_AnimDrops;
            transform.localScale *= 1000;

            m_Material?.SetFloat(c_GlowingPower, 0);
            m_IsEmpty = false;
            m_ParticlesSpawnChest.startDelay = 0.3f;
            m_ParticlesSpawnChest.Play();

        }

        /// <summary>
        /// Set the color of rarity used in some particle effects.
        /// </summary>
        /// <param name="c">Rarity color</param>
        public void SetRarityColor(Color32 c)
        {
            m_RarityColor = c;
        }
    }
}
