using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInchest;

        [Header("World Item ID")]
        [SerializeField] int itemPickUpID;
        [SerializeField] bool hasBeenLooted;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        protected override void Start()
        {
            base.Start();

            if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];
        }

        public override void Interact(PlayerManager playerManager)
        {

            if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
            }
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

                Vector3 rotationDirection = transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 + Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                playerManager.OpenChestInteraction(playerStandingPosition);
                animator.Play("Chest Open");

                StartCoroutine(SpawnItemChest());

                ItemPickUp weaponPickUp = itemSpawner.GetComponent<ItemPickUp>();

                if (weaponPickUp != null)
                {
                    weaponPickUp.weapon = itemInchest;
                }

        }

        private IEnumerator SpawnItemChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }

    }

}
