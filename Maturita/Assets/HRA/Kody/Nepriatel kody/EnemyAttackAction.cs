using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{
    [CreateAssetMenu(menuName ="A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public bool canCombo;

        public EnemyAttackAction comoboAction;

        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 80;
        public float minimumAttackAngle = -80;

        public float minimumDistanceNeededToAttack = -5;
        public float maximumDistanceNeededToAttack = 5;


    }

}