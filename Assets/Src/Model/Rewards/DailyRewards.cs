using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewards
{
    public List<Weeks> weeks;

    public class Weeks
    {
        public int weekNumber { get; set; }
        public List<Days> days { get; set; }
    }

    public class Days
    {
        public int dayNumber { get; set; }
        public string rewardName { get; set; }
        public int value { get; set; }
    }
}