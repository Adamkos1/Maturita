using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public EnemyBossManager enemyBoss;

        public int bossID;
        public bool bossFightIsActive;
        public bool bossHasBeenAwakened;
        public bool bossHasBeenDefeated;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyBoss = FindObjectOfType<EnemyBossManager>();
        }

        private void Start()
        {
            if(!WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled.ContainsKey(bossID) && !WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted.Add(bossID, false);
            }

            bossHasBeenDefeated = WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled[bossID];
            bossFightIsActive = WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted[bossID];
            bossHasBeenAwakened = WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted[bossID];
        }

        public void ActiveBossFight()
        {
            if(enemyBoss != null)
            {
                if (WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted.Remove(bossID);
                }

                WorldSaveGameManager.instance.currentCharacterSaveData.bossFightStarted.Add(bossID, true);

                bossFightIsActive = true;
                bossHasBeenAwakened = true;
                bossHealthBar.SetUIHealthBarToActive();

                foreach (var fogWall in fogWalls)
                {
                    fogWall.ActivateFogWall();
                }
            }
        }

        public void BossHasBeenDefeated()
        {
            if (WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled.Remove(bossID);
            }

            WorldSaveGameManager.instance.currentCharacterSaveData.bossHasBeenKilled.Add(bossID, true);

            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
                bossHealthBar.SetHealthBarToInactive();
            }
        }
    }

}
