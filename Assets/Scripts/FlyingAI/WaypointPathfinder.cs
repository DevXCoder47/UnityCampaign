using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlyingAI
{
    public class WaypointPathfinder
    {
        public List<Waypoint> FindPath(Waypoint start, Waypoint goal)
        {
            List<PathNode> open = new();
            HashSet<Waypoint> closed = new();

            open.Add(new PathNode()
            {
                Waypoint = start,
                GCost = 0,
                HCost = Distance(start, goal)
            });

            while (open.Count > 0)
            {
                PathNode current =
                    open.OrderBy(x => x.FCost).First();

                if (current.Waypoint == goal)
                    return ReconstructPath(current);

                open.Remove(current);
                closed.Add(current.Waypoint);

                foreach (Waypoint neighbour in current.Waypoint.Neighbours)
                {
                    if (closed.Contains(neighbour))
                        continue;

                    float gCost =
                        current.GCost +
                        Vector3.Distance(
                            current.Waypoint.transform.position,
                            neighbour.transform.position);

                    PathNode existing =
                        open.FirstOrDefault(
                            n => n.Waypoint == neighbour);

                    if (existing == null)
                    {
                        open.Add(new PathNode()
                        {
                            Waypoint = neighbour,
                            Parent = current,
                            GCost = gCost,
                            HCost = Distance(neighbour, goal)
                        });
                    }
                    else if (gCost < existing.GCost)
                    {
                        existing.Parent = current;
                        existing.GCost = gCost;
                    }
                }
            }

            return null;
        }

        private float Distance(Waypoint a, Waypoint b)
        {
            return Vector3.Distance(
                a.transform.position,
                b.transform.position);
        }

        private List<Waypoint> ReconstructPath(PathNode node)
        {
            List<Waypoint> path = new();

            while (node != null)
            {
                path.Add(node.Waypoint);
                node = node.Parent;
            }

            path.Reverse();

            return path;
        }
    }
}
