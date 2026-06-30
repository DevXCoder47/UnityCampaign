using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Weapon stats")]
        public float fireRate;
        public float damage;

        [Header("Raycast settings")]
        public float shootDistance;
        public LayerMask hitLayers;

        [Header("Tracer settings")]
        public LineRenderer tracerPrefab;
        public float tracerDuration = 1;

        [Header("Recoil settings")]
        public float baseSpread;
        public float maxSpread;
        public float spreadIncrease;
        public float spreadDecrease;

        [Header("Muzzle flash settings")]
        public GameObject muzzleFlashPrefab;
        public float muzzleFlashDuration;

        [Header("Ammo settings")]
        public int maxAmmo;
        public float reloadingTime;
    }
}
