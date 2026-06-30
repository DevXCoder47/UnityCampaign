using System.Collections;
using UnityEngine;
using Zenject;

namespace Weapons
{
    public class DroneGun : Weapon
    {
        [Inject(Id = "Target")] private Transform _target;

        public override void Reload()
        {
            if (isReloading) return;

            StartCoroutine(ReloadRoutine());
        }

        public override void Shoot()
        {
            if (!CanShoot())
                return;

            if (_target == null)
                return;

            SpawnMuzzleFlash();
            PlayShotSound();

            Vector3 direction =
                (_target.position - muzzlePosition.position).normalized;

            Vector3 right =
                Vector3.Cross(Vector3.up, direction).normalized;

            Vector3 up =
                Vector3.Cross(direction, right).normalized;

            Vector2 spread =
                Random.insideUnitCircle * weaponData.maxSpread;

            direction += right * spread.x;
            direction += up * spread.y;

            direction.Normalize();

            Vector3 hitPoint;

            if (Physics.Raycast(
                    muzzlePosition.position,
                    direction,
                    out RaycastHit hit,
                    weaponData.shootDistance,
                    weaponData.hitLayers))
            {
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(weaponData.damage);

                hitPoint = hit.point;
            }
            else
            {
                hitPoint =
                    muzzlePosition.position +
                    direction * weaponData.shootDistance;
            }

            SpawnTracer(hitPoint);

            currentAmmo--;
        }

        protected override void PlayShotSound()
        {
            _audioService.PlayEnemyShotSound(muzzlePosition.position);
        }

        protected override IEnumerator ReloadRoutine()
        {
            isReloading = true;
            yield return new WaitForSeconds(weaponData.reloadingTime);
            currentAmmo = weaponData.maxAmmo;
            isReloading = false;
        }
    }
}
