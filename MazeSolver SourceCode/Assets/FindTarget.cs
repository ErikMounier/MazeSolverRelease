using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class FindTarget : MonoBehaviour {

    
    [SerializeField] protected GameObject Prefab;
    [SerializeField] protected GameObject PlaceCollider;
    [SerializeField] protected GameObject SecondCollider;
    [SerializeField] private float distanceToEndPoint;
    [SerializeField] private float eCost;
    [SerializeField] private Vector3 EndVector;

    float distance;
    float Dis_E;
    float Dis_E_Closest;
    GameObject CheckIfWall1;
    GameObject CheckIfWall2;
    GameObject CheckIfWall3;
    GameObject closestobject;

    Vector3 CreatedFromPosition;

    GameObject Target;

    PointHandeler PHandeler;

    Vector3 Front = new Vector3(-1, 0, 0);
    Vector3 left = new Vector3(0, 0, -1);
    Vector3 right = new Vector3(0, 0, 1);
    Vector3 Back = new Vector3(1, 0, 0);
    Vector3 FrontLeft = new Vector3(-1, 0, -1);
    Vector3 FrontRight = new Vector3(-1, 0, 1);
    Vector3 BackLeft = new Vector3(1, 0, -1);
    Vector3 BackRight = new Vector3(1, 0, 1);

    float closest = 100;
    //List<GameObject> ObjectList = new List<GameObject>();
    float TimeTillUpdate;
    bool once = true;
    bool OnceTwo = true;

    bool CollidedWithTarget = false;
    // Use this for initialization
    private void Awake()
    {

        

        //Target = Resources.Load("TARGET", typeof(GameObject)) as GameObject;
        Prefab = Resources.Load("Cube", typeof(GameObject)) as GameObject;
        PlaceCollider = Resources.Load("Collider", typeof(GameObject)) as GameObject;
        SecondCollider = Resources.Load("Collider 2", typeof(GameObject)) as GameObject;

    }

    void Start ()
    {
        Front += transform.position;
        left += transform.position;
        right += transform.position;
        Back += transform.position;
        FrontLeft += transform.position;
        FrontRight += transform.position;
        BackLeft += transform.position;
        BackRight += transform.position;

        GameObject o = GameObject.Find("ListHandeler");
        PHandeler = o.GetComponent<PointHandeler>();
       
        GameObject GO = GameObject.Find("TARGET");
        Target = GO;

       
        EndVector = GO.transform.position;
        PHandeler.PointList.Add(Instantiate(PlaceCollider, Front, transform.rotation, transform));
        PHandeler.PointList.Add(Instantiate(PlaceCollider, left, transform.rotation, transform));
        PHandeler.PointList.Add(Instantiate(PlaceCollider, right, transform.rotation, transform));
        PHandeler.PointList.Add(Instantiate(PlaceCollider, Back, transform.rotation, transform));
        //PHandeler.PointList.Add(Instantiate(PlaceCollider, FrontLeft, transform.rotation, transform));
        //PHandeler.PointList.Add(Instantiate(PlaceCollider, FrontRight, transform.rotation, transform));
        //PHandeler.PointList.Add(Instantiate(PlaceCollider, BackLeft, transform.rotation, transform));
        //PHandeler.PointList.Add(Instantiate(PlaceCollider, BackRight, transform.rotation, transform));
        PHandeler.TotalCheckers += 1;
        PHandeler.Checkers += 1;

    }
	
	// Update is called once per frame
	void Update ()
    {
       
        
         PHandeler.TimeThisRound += Time.deltaTime;

         


         TimeTillUpdate += Time.deltaTime;
         if (once == true && TimeTillUpdate >= 0.05f)
         {

             ClosestPoint();
             once = false;

         }
        
        
	}
    
    void ClosestPoint()
    {
        closest = 300;
        foreach (GameObject check in PHandeler.PointList) // check which point is closest
        {

            distance = check.GetComponent<DestroyPoint>().GiveTileCost();
            Dis_E = check.GetComponent<DestroyPoint>().GiveECost();
            if (distance < closest)
            {
                
                closest = distance;
                Dis_E_Closest = Dis_E;
                closestobject = check;
            }
            else if (distance == closest)
            {
                
                if (Dis_E > Dis_E_Closest)
                {
                    closest = distance;
                    Dis_E_Closest = Dis_E;
                    closestobject = check;
                }
            }
        }
        
        MakeNewChecker();
    }

    void MakeNewChecker()
    {

        if (closestobject != null) //closest point becomes new Checker
        {
            eCost = closestobject.transform.GetComponent<DestroyPoint>().GiveECost();
            distanceToEndPoint = closestobject.transform.GetComponent<DestroyPoint>().GiveTileCost();
            closestobject = Instantiate(Prefab.gameObject, closestobject.transform.position, closestobject.transform.rotation,closestobject.transform.parent);
            closestobject.name = "Cube";
            PHandeler.CheckerList.Add(closestobject);
            
            
            
            
            
            once = false;
            //Debug.Log(distance);
            //Debug.Log(PHandeler.PointList.Count + " Points in Checker");
              
        }
        

        DeleteStuff();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < PHandeler.PointList.Count; i++) //delete point if contact with Checker / Wall
        {
            if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Self")
            {
                if (PHandeler.PointList[i].transform.position == collision.transform.position) 
                {
                    
                    Destroy(PHandeler.PointList[i].gameObject);
                    PHandeler.PointList.Remove(PHandeler.PointList[i]);
                }
            }
            if (collision.gameObject.name == "TARGET")
            {
                if (PHandeler.PointList[i].transform.position == collision.transform.position)
                {
                    Debug.Log("Made Collision with the TARGET");
                    PHandeler.CalculateBestRoute(PHandeler.PointList[i]);
                    PHandeler.newchecktime = Mathf.RoundToInt(PHandeler.TimeThisRound);
                    PHandeler.TimeThisRound = 0;
                    PHandeler.MazesSolved += 1;
                    PHandeler.PointList.Clear();
                    PHandeler.Checkers = 0;
                    //FoundTarget = true;

                    break;
                }
            }
            
        }
    }

    private void DeleteStuff()
    {
        PHandeler.CheckerList.Add(gameObject);

        var rigid = transform.GetComponent<Rigidbody>();
        rigid.detectCollisions = false;

        var script = transform.GetComponent<FindTarget>();
        script.enabled = false;
    }

    public void ParentPosition(Vector3 pos)
    {
        CreatedFromPosition = pos;
    }



}
