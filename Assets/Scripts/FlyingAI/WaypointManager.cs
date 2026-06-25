using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FlyingAI
{
    public class WaypointManager : MonoBehaviour
    {
        [SerializeField] private float _connectionRadius = 100f;
        [SerializeField] private LayerMask _obstacleMask;

        [Inject(Id = "Waypoint")] private List<Waypoint> _waypoints;

        private void Start()
        {
            BuildConnections();
        }

        private void BuildConnections()
        {
            foreach (var waypoint in _waypoints)
            {
                waypoint.ClearNeighbours();

                foreach (var other in _waypoints)
                {
                    if (other == waypoint)
                        continue;

                    float distance = Vector3.Distance(
                        waypoint.transform.position,
                        other.transform.position);

                    if (distance > _connectionRadius)
                        continue;

                    if (!Physics.Linecast(
                            waypoint.transform.position,
                            other.transform.position,
                            _obstacleMask))
                    {
                        waypoint.AddNeighbour(other);
                    }
                }
            }
        }

        public Waypoint GetClosest(Vector3 position)
        {
            Waypoint closest = null;
            float bestDistance = float.MaxValue;

            foreach (var waypoint in _waypoints)
            {
                float distance =
                    Vector3.Distance(
                        position,
                        waypoint.transform.position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = waypoint;
                }
            }

            return closest;
        }

        public Waypoint GetRandom()
        {
            return _waypoints[Random.Range(0, _waypoints.Count)];
        }
    }
}
