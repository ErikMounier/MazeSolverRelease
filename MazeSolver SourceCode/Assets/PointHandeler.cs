using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PointHandeler : MonoBehaviour
{

    public List<GameObject> PointList;
    public List<GameObject> CheckerList;
    public int Checkers;
    public int TotalCheckers;
    public int MazesSolved;

    public float TimeThisRound;
    public int newchecktime;
    public int Shortesttime = 300;

    public GameObject GAfoundTarget;
    float TimeForEfficcientTile;
    public bool FoundTarget;

    bool Calculate;
    bool DoneBackTracking;

    bool ClearingOverlappingParts = true;
    float ClearingCooldown;

    bool EmergencyCleanup;
    float CleanupCooldown;

    float BackTrackTimer;

    int dupesRemoved;

    [SerializeField] private Text Points;
    [SerializeField] private Text Chekers;
    [SerializeField] private Text Mazes;
    [SerializeField] private Text CheckersTotal;
    [SerializeField] private Text ThisTime;
    [SerializeField] private Text FastTime;
    [SerializeField] private Text EmergencyText;
    [SerializeField] private Text DupesText;
    // Use this for initialization
    void Start()
    {
        PointList = new List<GameObject>();
        CheckerList = new List<GameObject>();
        if (GameObject.FindGameObjectsWithTag("ListHandeler").Length <= 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        
        GameObject canvas = GameObject.Find("Canvas");
        DontDestroyOnLoad(canvas);
        SceneManager.LoadScene(1);
        EmergencyText.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        DupesText.text = dupesRemoved.ToString() + " Dupes removed"; 

        if(Time.deltaTime >= 0.04f)
        {
            EmergencyCleanup = true;    
        }
        else if(EmergencyCleanup == true && CleanupCooldown >= 2f)
        {
            EmergencyCleanup = false;
            CleanupCooldown = 0;
            EmergencyText.enabled = false;
        }
        if(EmergencyCleanup == true)
        {
            CleanupCooldown += Time.deltaTime;
            EmergencyText.enabled = true;
        }

        ClearingCooldown += Time.deltaTime;
        if(ClearingCooldown >= 2f)
        {
            ClearingOverlappingParts = false;
        }

        if(newchecktime < Shortesttime && MazesSolved > 0)
        {
            Shortesttime = newchecktime;
        }
        
        Points.text = PointList.Count.ToString() + " Points";
        Chekers.text = Checkers + " Checkers";
        Mazes.text = MazesSolved.ToString() + " Mazes solved";
        CheckersTotal.text = TotalCheckers.ToString() + " Checkers total";
        ThisTime.text = TimeThisRound.ToString();
        FastTime.text = Shortesttime.ToString();


        //Debug.Log(PointList.Count + " points in PointsList");

        if (DoneBackTracking == true)
        {
            BackTrackTimer += Time.deltaTime;
        }
        if(BackTrackTimer >= 2)
        {
            DoneBackTracking = false;
            PointList.Clear();
            CheckerList.Clear();
            SceneManager.LoadScene(0);
            BackTrackTimer = 0;
        }

        for (int i = 0; i < PointList.Count; i++)
        {
            for (int j = i + 1; j < PointList.Count; j++)
            {
                if (PointList[i].transform.position == PointList[j].transform.position)
                {
                    Destroy(PointList[j].gameObject);
                    PointList.Remove(PointList[j]);
                }
            }
        }
        
        for (int i = 0; i < CheckerList.Count; i++)
        {
            if (CheckerList[i] != null)
            {
                for (int j = 0; j < PointList.Count; j++)
                {
                    if (CheckerList[i].transform.position == PointList[j].transform.position)
                    {
                        Debug.LogError("Removed overlapping point");
                        Destroy(PointList[j].gameObject);
                        PointList.Remove(PointList[j]);
                        ClearingOverlappingParts = true;
                        dupesRemoved += 1;
                    }
                }
            }
        }
        


        for (int i = 0; i < PointList.Count; i++) //delete point if contact with Checker / Wall
        {
            if (PointList[i].transform.position == new Vector3(37,0,-12))
            {
                //Debug.Log("Deleted a point at " + transform.position);
                Destroy(PointList[i].gameObject);
                PointList.Remove(PointList[i]);
            }
        }

        if (GAfoundTarget != null && GAfoundTarget.transform.parent != null && Calculate == true && ClearingOverlappingParts == false)
        {
            Debug.Log("Found target now calculating");
            TimeForEfficcientTile += Time.deltaTime;
            if (TimeForEfficcientTile >= 0.01f)
            {

                GameObject TempGA = GAfoundTarget.transform.parent.gameObject;
                if (GAfoundTarget.GetComponent<Renderer>() != null)
                {
                    GAfoundTarget.GetComponent<Renderer>().material.color = Color.black;
                    GAfoundTarget.GetComponent<Renderer>().material.color = Color.red;
                }

                GAfoundTarget = TempGA;
                TimeForEfficcientTile = 0;
            }
        }
        else if (GAfoundTarget != null && GAfoundTarget.transform.parent == null && ClearingOverlappingParts == false) 
        {
            //SceneManager.LoadScene(1);
            DoneBackTracking = true;
            Calculate = false;
            GAfoundTarget = null;
        }
        

    }
    public bool CleanupInProgress()
    {
        return EmergencyCleanup;
    }

    public void ClearList()
    {
        PointList.Clear();
    }

    public void CalculateBestRoute(GameObject usage)
    {
        GAfoundTarget = usage;
        Calculate = true;
    }

}
