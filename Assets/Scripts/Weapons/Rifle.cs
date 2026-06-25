using Signals;
using UnityEngine;
using Zenject;
using DG.Tweening;
using System.Collections;

namespace Weapons
{
    public class Rifle : Weapon
    {
        [SerializeField] private Camera playerCamera;

        [SerializeField] private float reloadDownOffset = 0.5f;
        [SerializeField] private float reloadAnimDuration = 0.25f;

        [Inject] private SignalBus _signalBus;

        private Vector3 _startLocalPosition;
        private bool isActive = true;

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied()
        {
            isActive = false;

            StopAllCoroutines();
            DOTween.Kill(transform);

            isReloading = false;
        }

        protected override void Start()
        {
            base.Start();
            _signalBus.Fire(new CurrentAmmoChangedSignal() { CurrentAmmo = currentAmmo });

            _startLocalPosition = transform.localPosition;
        }

        public override void Reload()
        {
            StartCoroutine(ReloadRoutine());
            _signalBus.Fire(new CurrentAmmoChangedSignal() { CurrentAmmo = currentAmmo });
        }

        public override void Shoot()
        {
            if (!CanShoot() || !isActive)
                return;

            SpawnMuzzleFlash();
            PlayShotSound();

            int mask =
                weaponData.hitLayers &
                ~(1 << LayerMask.NameToLayer("Player"));

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Vector3 direction = ray.direction;

            Vector2 spread = Random.insideUnitCircle * currentSpread;

            direction += playerCamera.transform.right * spread.x;
            direction += playerCamera.transform.up * spread.y;

            direction.Normalize();

            Vector3 hitPoint;

            if (Physics.Raycast(ray.origin, direction, out RaycastHit hit,
                weaponData.shootDistance, weaponData.hitLayers))
            {
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(weaponData.damage);

                hitPoint = hit.point;
            }
            else
            {
                hitPoint = ray.origin + direction * weaponData.shootDistance;
            }

            SpawnTracer(hitPoint);

            IncreaseSpread();

            currentAmmo--;
            _signalBus.Fire(new CurrentAmmoChangedSignal() { CurrentAmmo = currentAmmo });
        }

        protected override IEnumerator ReloadRoutine()
        {
            isReloading = true;

            Vector3 downPosition =
                _startLocalPosition + Vector3.down * reloadDownOffset;

            yield return transform
                .DOLocalMove(downPosition, reloadAnimDuration)
                .SetEase(Ease.InQuad)
                .WaitForCompletion();

            if (!isActive) yield break;

            yield return new WaitForSeconds(weaponData.reloadingTime);

            if (!isActive) yield break;

            currentAmmo = weaponData.maxAmmo;
            _signalBus.Fire(new CurrentAmmoChangedSignal() { CurrentAmmo = currentAmmo });

            yield return transform
                .DOLocalMove(_startLocalPosition, reloadAnimDuration)
                .SetEase(Ease.OutQuad)
                .WaitForCompletion();

            isReloading = false;
        }
    }
}
