using UnityEngine;
using Zenject;

namespace Weapons
{
    public class Rifle : Weapon
    {
        [Inject] private Camera playerCamera;
        public override void Shoot()
        {
            if (!CanShoot())
                return;

            SpawnMuzzleFlash();
            PlayShotSound();

            int mask =
                weaponData.hitLayers &
                ~(1 << LayerMask.NameToLayer("Player"));

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Vector3 direction = ray.direction;

            Vector2 spread = Random.insideUnitCircle * CurrentSpread;

            direction += playerCamera.transform.right * spread.x;
            direction += playerCamera.transform.up * spread.y;

            direction.Normalize();

            Vector3 hitPoint;

            if (Physics.Raycast(ray.origin, direction, out RaycastHit hit,
                weaponData.shootDistance, weaponData.hitLayers))
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = ray.origin + direction * weaponData.shootDistance;
            }

            SpawnTracer(hitPoint);

            IncreaseSpread();
        }
    }
}
