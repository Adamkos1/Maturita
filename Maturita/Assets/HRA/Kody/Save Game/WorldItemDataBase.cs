using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AH
{

    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public List<WeaponItem> weaponItems = new List<WeaponItem>();

        public List<RangedAmmoItem> rangedAmmoItems = new List<RangedAmmoItem>();

        public List<ConsumableItem> consumableItems = new List<ConsumableItem>();


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public WeaponItem GetWeaponItemByID(int weaponID)
        {
            return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
        }

        public RangedAmmoItem GetAmmoItemByID(int ammoID)
        {
            return rangedAmmoItems.FirstOrDefault(ammo => ammo.itemID == ammoID);
        }

        public ConsumableItem GetConsumableItemByID(int consumableID)
        {
            return consumableItems.FirstOrDefault(consumable => consumable.itemID == consumableID);
        }
    }
}
