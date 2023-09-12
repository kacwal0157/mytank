using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worlds
{
    public List<WorldConfig> worldConfig { get; set; }
}

public class WorldConfig
{
    public int worldNumber { get; set; }
    public string name { get; set; }
    public int requiredLevel { get; set; }
}
