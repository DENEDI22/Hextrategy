using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public string playerTeam = "red";
    public int influensePoints;
    public int wood;
    public int food;
    public int minerals;
    [SerializeField]
    TileController[] tiles;

    private bool timeToMove = true;
    private bool timeToCollectResourses = true;
    private string GoingToBuild;
    private bool CanBuild = false;
    void Start()
    {
        
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


    public void GiveResourses(int influense, int Wood, int Food, int Minerals)
    {
        influensePoints = influensePoints - influense;
        wood = wood - Wood;
        food = food - Food;
        minerals = minerals - Minerals;
    }

    private void Move()
    {
        RandomiseArray(tiles);
        foreach (var tile in tiles)
        {
            if (tile.team == playerTeam)
            {

                if (tile.isFree)
                {
                    ChangeGoingToBuild(tile);
                    if (GoingToBuild != "nothing")
                    {
                        tile.AddBuilding(GoingToBuild, false);
                        timeToMove = true;
                        break;
                    }

                }
            }
            else
            {
                if (tile.needInfluenceToTakeOver <= influensePoints - 3)
                {
                    if (CheckTile(tile))
                    {
                        influensePoints -= tile.needInfluenceToTakeOver;
                        tile.needInfluenceToTakeOver += influensePoints / 2;
                        tile.ChangeOwner(Color.red, playerTeam);
                        timeToMove = true;
                        break;
                    }
                }
            }
        }
        timeToMove = true;

    }

    private void RandomiseArray(TileController[] arrayToSort)
    {
        TileController tmp;
        int a;
        for (int i = 0; i < arrayToSort.Length; i++)
        {
            a = Random.Range(0, arrayToSort.Length);
            tmp = arrayToSort[i];
            arrayToSort[i] = arrayToSort[a];
            arrayToSort[a] = tmp;
        }
    }


    private bool CheckTile(TileController tile)
    {
        foreach (var item in tile.neighbour)
        {
            if (item.team == playerTeam)
            {
                return true;
            }
        }
        return false;
    }

    private void ChangeGoingToBuild(TileController tile)
    {
        if (wood >= 50 && food >= 70 && minerals >= 50)
        {
            GiveResourses(0, 50, 70, 50);
            tile.needInfluenceToTakeOver += 40 + influensePoints / 3;
            GoingToBuild = "town";
        }
        else if (influensePoints < wood && influensePoints < food && influensePoints < minerals && tile.typeOfTile == "water" && wood >= 50)
        {
            GiveResourses(0, 50, 0, 0);
            GoingToBuild = "pier";
            tile.needInfluenceToTakeOver += 15 + influensePoints / 3;
        }
        else if (influensePoints < wood && influensePoints < food && influensePoints < minerals && influensePoints >= 100 && wood >= 50 && food >= 70 && minerals >= 50)
        {
            GiveResourses(75, 50, 70, 50);
            GoingToBuild = "town";
            tile.needInfluenceToTakeOver += 40 + influensePoints / 3;
        }
        else if (wood < influensePoints && wood < food && wood < minerals && tile.typeOfTile == "forest" && minerals >= 25 && food >= 45)
        {
            GiveResourses(0, 0, 45, 25);
            GoingToBuild = "sawmill";
            tile.needInfluenceToTakeOver += 15 + influensePoints / 3;
        }
        else if (food < influensePoints && food < wood && food < minerals && tile.typeOfTile != "water" && wood >= 60)
        {
            GiveResourses(0, 60, 0, 0);
            GoingToBuild = "farm";
            tile.needInfluenceToTakeOver += 10 + influensePoints / 3;
        }
        else if (minerals < influensePoints && minerals < food && minerals < wood && wood >= 20 && food >= 50)
        {
            GiveResourses(0, 20, 50, 0);
            GoingToBuild = "mine";
            tile.needInfluenceToTakeOver += 12 + influensePoints / 3;
        }
        else
        {
            GoingToBuild = "nothing";
        }
    }

    void FixedUpdate()
    {
        if (timeToMove && influensePoints >= 0 && food >= 0 && wood >= 0 && minerals >= 0)
        {
            Invoke("Move", 1);
            timeToMove = false;
        }
        if (timeToCollectResourses && influensePoints >= 0 && food >= 0 && wood >= 0 && minerals >= 0)
        {
            Invoke("CollectResourses", 2f);
            timeToCollectResourses = false;
        }
    }
}
