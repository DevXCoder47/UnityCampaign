using UnityEngine;

namespace FlyingAI
{
    public class PathNode
    {
        public Waypoint Waypoint;

        public PathNode Parent;

        public float GCost;
        public float HCost;

        public float FCost => GCost + HCost;
    }
}
