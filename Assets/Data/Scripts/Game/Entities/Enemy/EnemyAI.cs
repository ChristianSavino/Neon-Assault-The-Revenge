using UnityEngine;

namespace Keru.Scripts.Game.Entities.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
       

        /*[Header("Config")]
        private float decisionInterval = 0.3f;
        private float visionRange = 10f;
        private float shootRange = 7f;
        private float coverSearchRange = 5f;
        private int maxAmmo = 10;
        private bool hasUltimate = true;
        private bool hasSpecial = true;

        [Header("References")]
        private Transform _player;
        private Transform[] patrolPoints;
        private Transform[] coverPoints;
        private GameObject bulletPrefab;
        private Transform shootPoint;

        private float decisionTimer = 0f;
        private int currentAmmo;
        private int patrolIndex = 0;
        private Vector3 lastSeenPlayerPos;
        private bool isReloading = false;

        void Start()
        {
            currentAmmo = maxAmmo;
            if (_player == null)
            {
                _player = PlayerBase.Singleton.transform;
            }
                

            decisionTimer = Random.Range(0f, decisionInterval);
        }

        void Update()
        {
            decisionTimer += Time.deltaTime;

            if (decisionTimer >= decisionInterval)
            {
                decisionTimer = 0f;
                Think();
            }

            RotateTowardsPlayer();
        }

        void Think()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            bool playerVisible = CanSeePlayer();

            if (playerVisible)
            {
                lastSeenPlayerPos = _player.position;

                if (ShouldUseUltimate())
                {
                    UseUltimate();
                }
                //else if (health < 30f && HasCoverNearby())
                //{
                //    TakeCover();
                //}
                else if (hasSpecial && Random.value > 0.7f)
                {
                    UseSpecial();
                }
                else if (distanceToPlayer <= shootRange && currentAmmo > 0 && !isReloading)
                {
                    Shoot();
                }
                else if (currentAmmo == 0 && !isReloading)
                {
                    StartCoroutine(Reload());
                }
                else
                {
                    MoveTo(lastSeenPlayerPos);
                }
            }
            else
            {
                Patrol();
            }
        }

        void RotateTowardsPlayer()
        {
            if (!_player) return;

            Vector3 dir = (_player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        void Shoot()
        {
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            currentAmmo--;
        }

        IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(2f);
            currentAmmo = maxAmmo;
            isReloading = false;
        }

        void TakeCover()
        {
            Transform bestCover = FindNearestCover();
            if (bestCover != null)
                MoveTo(bestCover.position);
        }

        void UseSpecial()
        {
            hasSpecial = false;
        }

        void UseUltimate()
        {
            hasUltimate = false;
        }

        void Patrol()
        {
            if (patrolPoints.Length == 0) return;

            MoveTo(patrolPoints[patrolIndex].position);
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        void MoveTo(Vector3 targetPos)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 3f);
        }

        bool CanSeePlayer()
        {
            if (!_player) return false;
            float dist = Vector3.Distance(transform.position, _player.position);
            return dist <= visionRange;
        }

        bool ShouldUseUltimate()
        {
            return hasUltimate && /*health > 50f &&*/ /* Vector3.Distance(transform.position, _player.position) < 6f;
        }

        bool HasCoverNearby()
        {
            foreach (Transform cover in coverPoints)
            {
                if (Vector3.Distance(transform.position, cover.position) <= coverSearchRange)
                    return true;
            }
            return false;
        }

        Transform FindNearestCover()
        {
            Transform best = null;
            float minDist = Mathf.Infinity;

            foreach (Transform cover in coverPoints)
            {
                float dist = Vector3.Distance(transform.position, cover.position);
                if (dist < minDist)
                {
                    best = cover;
                    minDist = dist;
                }
            }

            return best;
        } */
    }
}
