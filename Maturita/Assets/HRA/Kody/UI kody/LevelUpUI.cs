using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confirmLevelUpButon;

        [Header("Player Level")]
        public int currentPlayerLevel;          //aky level sme pred levelovanim
        public int projectedPlayerLevel;        //mozny level aky budeme ak levelneme
        public Text currentPlayerLevelText;     //UI cislo aky sme teraz level
        public Text projectedPlayerLevelText;   //Ui cislo pre projektovany hracov level

        [Header("Souls Level")]
        public Text currentSoulsText;
        public Text soulsRequiredToLevelUpText;
        private int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health")]
        public Slider healthSlider;
        public Text currentHealthLevelText;
        public Text projectedHealthLevelText;

        [Header("Strength")]
        public Slider strengthSlider;
        public Text currentStrengthLevelText;
        public Text projectedStrengthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public Text currentStaminaLevelText;
        public Text projectedStaminaLevelText;

        [Header("Intelligence")]
        public Slider intelligenceSlider;
        public Text currentIntelligenceLevelText;
        public Text projectedIntelligenceLevelText;

        [Header("Mana")]
        public Slider manaSlider;
        public Text currentManaLevelText;
        public Text projectedManaLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public Text currentFaithLevelText;
        public Text projectedFaithLevelText;

        [Header("Dexterity")]
        public Slider dexteritySlider;
        public Text currentDexterityLevelText;
        public Text projectedDexterityLevelText;

        [Header("Poise")]
        public Slider poiseSlider;
        public Text currentPoiseLevelText;
        public Text projectedPoiseLevelText;


        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthlevel;
            healthSlider.minValue = playerManager.playerStatsManager.healthlevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthlevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthlevel.ToString();

            strengthSlider.value = playerManager.playerStatsManager.strenghtLevel;
            strengthSlider.minValue = playerManager.playerStatsManager.strenghtLevel;
            strengthSlider.maxValue = 99;
            currentStrengthLevelText.text = playerManager.playerStatsManager.strenghtLevel.ToString();
            projectedStrengthLevelText.text = playerManager.playerStatsManager.strenghtLevel.ToString();



            staminaSlider.value = playerManager.playerStatsManager.staminalevel;
            staminaSlider.minValue = playerManager.playerStatsManager.staminalevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminalevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminalevel.ToString();



            intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();



            manaSlider.value = playerManager.playerStatsManager.manaLevel;
            manaSlider.minValue = playerManager.playerStatsManager.manaLevel;
            manaSlider.maxValue = 99;
            currentManaLevelText.text = playerManager.playerStatsManager.manaLevel.ToString();
            projectedManaLevelText.text = playerManager.playerStatsManager.manaLevel.ToString();



            faithSlider.value = playerManager.playerStatsManager.faithLevel;
            faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();



            dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;
            currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();



            poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();

            currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager.playerStatsManager.healthlevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStatsManager.staminalevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
            playerManager.playerStatsManager.manaLevel = Mathf.RoundToInt(manaSlider.value);
            playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);
            playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.maxMana = playerManager.playerStatsManager.SetMaxManaFromManaLevel();

            playerManager.playerStatsManager.currentSoulCount = playerManager.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
            playerManager.uIManager.soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for(int i = 0; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) *  1.5f);
            }
        }

        private void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthlevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strenghtLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminalevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(manaSlider.value) - playerManager.playerStatsManager.manaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp)
            {
                confirmLevelUpButon.interactable = false;
            }
            else
            {
                confirmLevelUpButon.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateManaLevelSlider()
        {
            projectedManaLevelText.text = manaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }

}
