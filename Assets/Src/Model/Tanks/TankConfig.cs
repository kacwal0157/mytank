using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankConfig {

    public List<EnemyConfig> enemies;
    public List<PlayerConfig> players;
    public class PlayerConfig
    {
        public string name;
        public List<string> shootingModes;
        public List<string> bulletTypes;
    }

    public class EnemyConfig
    {
        public string name;
        public string shootingMode;
        public string bulletType;
    }

    
}
