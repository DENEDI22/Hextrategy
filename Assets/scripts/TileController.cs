﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{


    public string team = "neutral";
    public bool isCapital = false;
    public string typeOfTile = "forest";
    public GameObject flag;
    public bool isFree;
    public PlayerController player;

    [SerializeField]
    public TileController[] neighbour;

    [Header("Resourses from this tile")]
    public int givesIP;
    public int givesFood;
    public int givesWood;
    public int givesMinerals;

    public int needInfluenceToTakeOver = 10;

    int tmpGeneration;
    int buildingGeneration;
    bool canTakeOver;
    void Start()
    {
        if (isCapital)
        {
            if (team == "blue")
            {  
                flag.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            else if (team == "red")
            {
                flag.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        else
        {
            tmpGeneration = Random.Range(1, 7);
            buildingGeneration = Random.Range(1, 16);
            if (tmpGeneration == 1)
            {
                typeOfTile = "water";
                needInfluenceToTakeOver = 12;
                GetComponent<MeshRenderer>().material.color = Color.cyan;
                if (buildingGeneration < 5)
                {
                    AddBuilding("pier", false);
                    needInfluenceToTakeOver = 30;
                }
                else if (buildingGeneration == 15)
                {
                    AddBuilding("town", false);
                    needInfluenceToTakeOver = 50;
                }
            }
            else if (tmpGeneration == 6)
            {
                typeOfTile = "forest";
                needInfluenceToTakeOver = 12;
                GetComponent<MeshRenderer>().material.color = Color.green;
                if (buildingGeneration < 4)
                {
                    needInfluenceToTakeOver = 28;
                    AddBuilding("sawmill", false);
                }
                else if (buildingGeneration == 15)
                {
                    needInfluenceToTakeOver = 50;
                    AddBuilding("town", false);
                }
                else if (buildingGeneration < 7)
                {
                    needInfluenceToTakeOver = 15;
                    AddBuilding("farm", false);
                }
                else if (buildingGeneration < 9)
                {

                    AddBuilding("mine", false);
                }
            }
            else
            {
                typeOfTile = "field";
                GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green, Color.white, 0.3f);
                if (buildingGeneration == 15)
                {
                    needInfluenceToTakeOver = 50;
                    AddBuilding("town", false);
                }
                else if (buildingGeneration < 7)
                {
                    needInfluenceToTakeOver = 25;
                    AddBuilding("farm", false);
                }
                else if (buildingGeneration < 10)
                {
                    needInfluenceToTakeOver = 25;
                    AddBuilding("mine", false);
                }
            }
        }
    }



    private void OnMouseDown()
    {
        foreach (var item in neighbour)
        {
            if (item.team == player.playerTeam)
            {
                canTakeOver = true;
                break;
            }
        }

        if (canTakeOver && player.influensePoints >= needInfluenceToTakeOver && team != player.playerTeam)
        {
            player.GiveResourses(needInfluenceToTakeOver, 0, 0, 0);
            needInfluenceToTakeOver += player.influensePoints/2;
            ChangeOwner(Color.blue, player.playerTeam);
        }
        else
        {
            Debug.Log("you don't have enough influence, " + needInfluenceToTakeOver.ToString() + " needed");
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && team == "blue")
        {

            AddBuilding(player.GoingToBuild, true);
            player.GoingToBuild = null;
            Debug.Log("building....");
        }   
    }

    public void ChangeOwner(Color color, string Team)
    {
        flag.GetComponent<MeshRenderer>().material.color = color;
        team = Team;
    }

    void ChangeStats(int IP, int Food, int Wood, int Minerals)
    {
        givesIP = IP;
        givesFood = Food;
        givesWood = Wood;
        givesMinerals = Minerals;
    }

    public void AddBuilding(string building, bool isByPlayer)
    {
        if (isByPlayer && isFree)
        {
            if (building == "farm" && typeOfTile != "water" && player.wood >= 60)
            {
                GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
                player.GiveResourses(0, 60, 0, 0);
                isFree = false;
                ChangeStats(0, 3, 0, 0);
                Debug.Log("built");
            }
            else if (building == "mine" && player.wood >= 20 && player.food >= 50)
            {
                GetComponent<MeshRenderer>().material.color = Color.black;
                player.GiveResourses(0, 20, 50, 0);
                isFree = false;
                ChangeStats(1, -1, 0, 3);
            }
            else if (building == "town"  && player.wood >= 50 && player.food >= 70 && player.minerals >= 50)
            {
                GetComponent<MeshRenderer>().material.color = Color.gray;
                player.GiveResourses(0, 50, 70, 50);
                isFree = false;
                ChangeStats(15, -5, 0, 0);
            }
            else if (building == "pier" && typeOfTile == "water" && player.wood >= 50)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                player.GiveResourses(0, 50, 0, 0);
                isFree = false;
                ChangeStats(2, 1, 0, 0);
            }
            else if (building == "sawmill" && typeOfTile == "forest" && player.minerals >= 25 && player.food >= 45)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                player.GiveResourses(0,0,45,25);
                isFree = false;
                ChangeStats(1, 0, 5, -1);
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }
        else if(isFree)
        {
            if (building == "farm")
            {
                GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
                isFree = false;
                ChangeStats(0, 3, 0, 0);
            }
            else if (building == "mine")
            {
                GetComponent<MeshRenderer>().material.color = Color.black;
                isFree = false;
                ChangeStats(1, -1, 0, 3);
            }
            else if (building == "town")
            {
                GetComponent<MeshRenderer>().material.color = Color.gray;
                isFree = false;
                ChangeStats(15, -5, 0, 0);
            }
            else if (building == "pier")
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                isFree = false;
                ChangeStats(2, 1, 0, 0);
            }
            else if (building == "sawmill")
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                isFree = false;
                ChangeStats(1, 0, 5, -1);
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }


    }

    void Update()
    {
        
    }
}
