using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpRewards
{
    public List<LevelRewards> levels;
    public class LevelRewards
    {
        public int rewardsCount { get; set; }
        public List<Rewards> rewards { get; set; }
    }

    public class Rewards
    {
        public string rewardName { get; set; }
        public int value { get; set; }
    }
}
