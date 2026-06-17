using Data;
using UnityEngine;

namespace Weapons {
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected Transform muzzlePosition;

        protected float CurrentSpread;

        private float _nextShotTime;

        protected virtual void Start()
        {
            CurrentSpread = weaponData.baseSpread;
        }

        protected virtual void Update()
        {
            RecoverSpread();
        }

        protected void IncreaseSpread()
        {
            CurrentSpread += weaponData.spreadIncrease;

            if (CurrentSpread > weaponData.maxSpread)
                CurrentSpread = weaponData.maxSpread;
        }

        protected void RecoverSpread()
        {
            CurrentSpread = Mathf.MoveTowards(
                CurrentSpread,
                weaponData.baseSpread,
                weaponData.spreadDecrease * Time.deltaTime);
        }

        public abstract void Shoot();

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

        protected void PlayShotSound()
        {
            if (weaponData.shotSound == null)
                return;

            AudioSource.PlayClipAtPoint(
                weaponData.shotSound,
                muzzlePosition.position);
        }

        protected bool CanShoot()
        {
            if (Time.time < _nextShotTime)
                return false;

            _nextShotTime = Time.time + 1f / weaponData.fireRate;
            return true;
        }
    }
}
