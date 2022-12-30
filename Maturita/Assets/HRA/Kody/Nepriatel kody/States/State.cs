using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager);
    }

}
