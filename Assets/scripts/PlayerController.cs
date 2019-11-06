using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;



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
    GameObject WinPanel;
    [SerializeField]
    GameObject buildingMenu;
    [SerializeField]
    TileController[] tiles;
    [HideInInspector]
    public string GoingToBuild;
    bool timeToCollectResourses = true;

    public Texture2D cursor;
    public Texture2D hammer;

    float movement;

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI IP;
    [SerializeField]
    TextMeshProUGUI woodUI;
    [SerializeField]
    TextMeshProUGUI foodUI;
    [SerializeField]
    TextMeshProUGUI mineralsUI;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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
        Cursor.SetCursor(hammer, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 1;
    }

    void ToggleTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Debug.Log("Time runned");
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Time stopped");
        }
    }
    public void Loos()
    {
        DefeatPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Win()
    {
        WinPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowMenu(GameObject menu)
    {
        ToggleTime();
        menu.SetActive(!buildingMenu.activeInHierarchy);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
    private void Update()
    {
        movement = Input.GetAxis("Horizontal");
    }
    void FixedUpdate()
    {


        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    ToggleTime();
        //    buildingMenu.SetActive(!buildingMenu.activeInHierarchy);

        //}
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
        IP.text = influensePoints.ToString();
        woodUI.text = wood.ToString();
        foodUI.text = food.ToString();
        mineralsUI.text = minerals.ToString();
        transform.RotateAround(Vector3.zero, Vector3.up, movement * Time.fixedDeltaTime * -600);
    }

}
