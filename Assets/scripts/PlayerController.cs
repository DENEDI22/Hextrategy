using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class PlayerController : MonoBehaviour
{
    public string playerTeam = "blue";
    [Header("Player's resourses")]
    public int influensePoints;
    public int wood;
    public int food;
    public int minerals;
    [Header("Other")]
    [SerializeField]
    GameObject DefeatPanel;
    [SerializeField]
    GameObject buildingMenu;
    [SerializeField]
    TileController[] tiles;
    [HideInInspector]
    public string GoingToBuild;
    bool timeToCollectResourses = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GiveResourses(int influense, int Wood, int Food, int Minerals)
    {
        influensePoints = influensePoints - influense;
        wood = wood - Wood;
        food = food - Food;
        minerals = minerals - Minerals;
    }
    
    void CollectResourses()
    {
        foreach (var item in tiles)
        {
            if (item.team == playerTeam)
            {
                GiveResourses(-item.givesIP, -item.givesWood, -item.givesFood, -item.givesMinerals);
            }
        }
        timeToCollectResourses = true;
    }

    public void ChangeGoingToBuild(string building)
    {
        GoingToBuild = building;
        buildingMenu.SetActive(false);
    }

    public void Loos()
    {
        DefeatPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildingMenu.SetActive(!buildingMenu.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GoingToBuild != null)
        {
            GoingToBuild = null;
        }
        if (timeToCollectResourses)
        {
            Invoke("CollectResourses", 2f);
            timeToCollectResourses = false;
        }
        if (influensePoints < 0 || wood < 0 || food < 0 || minerals < 0)
        {
            Loos();
        }
    }

}
