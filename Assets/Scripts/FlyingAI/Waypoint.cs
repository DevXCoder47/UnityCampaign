using System.Collections.Generic;
using UnityEngine;

namespace FlyingAI
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> neighbours = new();

        public IReadOnlyList<Waypoint> Neighbours => neighbours;

        public void AddNeighbour(Waypoint waypoint)
        {
            if (!neighbours.Contains(waypoint))
                neighbours.Add(waypoint);
        }

        public void ClearNeighbours()
        {
            neighbours.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.purple;

            foreach (var neighbour in neighbours)
            {
                if (neighbour != null)
                    Gizmos.DrawLine(transform.position,
                                   neighbour.transform.position);
            }

            Gizmos.DrawSphere(transform.position, 0.6f);
        }
    }
}
