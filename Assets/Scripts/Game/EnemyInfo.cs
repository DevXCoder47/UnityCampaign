using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EnemyType
    {
        BlueBot,
        GreenBot,
        RedBot,
        YellowTurret,
        PinkDrone
    }

    [Serializable]
    public class EnemyInfo
    {
        public EnemyType type;
        public List<Transform> spawnPoints;
    }
}
