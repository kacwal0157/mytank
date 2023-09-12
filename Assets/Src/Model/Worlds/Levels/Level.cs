using System.Collections.Generic;

public class Level
{
    public class Coordinates
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Scene
    {
        public Coordinates arena { get; set; }
        public Player player { get; set; }
        public List<Enemy> enemies { get; set; }
    }

    public class Arena
    {
        public Scene scene { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public int worldNumber { get; set; }
        public int levelDifficulty { get; set; }
        public Stats stats { get; set; }
        public RewardsFirstTime rewardsFirstTime { get; set; }
        public Rewards rewards { get; set; }
    }

    public class Stats
    {
        public int energyCost { get; set; }
        public int unlockScore { get; set; }
    }

    public class RewardsFirstTime
    {
        public int energy { get; set; }
        public int gold { get; set; }
        public string item1 { get; set; }
        public string item2 { get; set; }
        public string item3 { get; set; }
    }

    public class Rewards
    {
        public int energy { get; set; }
        public int gold { get; set; }
    }

    public class Enemy
    {
        public int x { get; set; }
        public int y { get; set; }
        public int healthPoints { get; set; }
        public string enemyName { get; set; }
    }

    public class Player
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}