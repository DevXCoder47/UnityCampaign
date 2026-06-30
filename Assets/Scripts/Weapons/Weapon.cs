using Data;
using Services;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Weapons {
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected Transform muzzlePosition;

        [Inject] protected IAudioService _audioService;

        protected float currentSpread;
        protected int currentAmmo;
        protected bool isReloading = false;

        private float _nextShotTime;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => weaponData.maxAmmo;
        public bool IsReloading => isReloading;

        protected virtual void Start()
        {
            currentSpread = weaponData.baseSpread;
            currentAmmo = weaponData.maxAmmo;
        }

        protected virtual void Update()
        {
            RecoverSpread();
        }

        protected void IncreaseSpread()
        {
            currentSpread += weaponData.spreadIncrease;

            if (currentSpread > weaponData.maxSpread)
                currentSpread = weaponData.maxSpread;
        }

        protected void RecoverSpread()
        {
            currentSpread = Mathf.MoveTowards(
                currentSpread,
                weaponData.baseSpread,
                weaponData.spreadDecrease * Time.deltaTime);
        }

        public abstract void Shoot();

        public abstract void Reload();

        protected abstract IEnumerator ReloadRoutine();
        
        protected void SpawnMuzzleFlash()
        {
            if (weaponData.muzzleFlashPrefab == null)
                return;
            

            GameObject flash = Instantiate(
                weaponData.muzzleFlashPrefab,
                muzzlePosition.position,
                muzzlePosition.rotation);

            Destroy(flash, weaponData.muzzleFlashDuration);
        }

        protected void SpawnTracer(Vector3 endPoint)
        {
            if (weaponData.tracerPrefab == null)
                return;
            

            LineRenderer tracer = Instantiate(
                weaponData.tracerPrefab);

            tracer.SetPosition(0, muzzlePosition.position);
            tracer.SetPosition(1, endPoint);

            Destroy(tracer.gameObject, weaponData.tracerDuration);
        }

        protected abstract void PlayShotSound();
       
        protected bool CanShoot()
        {
            if (Time.time < _nextShotTime)
                return false;

            _nextShotTime = Time.time + 1f / weaponData.fireRate;
            return true;
        }
    }
}
