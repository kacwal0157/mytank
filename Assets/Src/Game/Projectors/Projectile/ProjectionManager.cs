using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionManager
{
    private Dictionary<int, ProjectileArc> allProjectileArc = new Dictionary<int, ProjectileArc>();
 

    public ProjectionManager(GameObject player, List<GameObject> enemies)
    {
        foreach(GameObject go in enemies)
        {
            allProjectileArc.Add(go.GetInstanceID(), go.GetComponentInChildren<ProjectileArc>());
        }
        allProjectileArc.Add(player.GetInstanceID(), player.GetComponentInChildren<ProjectileArc>());

        changeActiveUser(player);
    }

    public void changeActiveUser(GameObject activeUser)
    {
        foreach(int key in allProjectileArc.Keys)
        {
            if(key.Equals(activeUser.GetInstanceID()))
            {
                allProjectileArc[key].gameObject.SetActive(true);
            } 
            else
            {
                allProjectileArc[key].gameObject.SetActive(false);
            }
        }
    }
}

