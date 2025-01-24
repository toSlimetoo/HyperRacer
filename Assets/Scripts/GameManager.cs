using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField]private GameObject carPrefab;
   [SerializeField]private GameObject roadPrefab;
   
   
   [SerializeField]private MoveButton leftMoveButton;
   [SerializeField]private MoveButton rightMoveButton;
   [SerializeField] private TMP_Text gasText;
   [SerializeField]private GameObject startPanelPrefab;
   [SerializeField]private GameObject endPanelPrefab;
   
   [SerializeField]private Transform canvasTransform;
   
   
   
   private CarController _carController;
   
   
   private Queue<GameObject> _roadPool = new Queue<GameObject>();
   private int _roadPoolSize = 3;
   
   
   private List<GameObject> _activeRoads = new List<GameObject>();

   private int _roadIndex;
   
   
   
   public enum State
   {
       Start, Play, End
   }
   
   public State GameState { get; private set; } = State.Start;
   
   
   public static GameManager _instance;

   public static GameManager Instance
   {
       get
       {
           if (_instance == null)
           {
               _instance = FindObjectOfType<GameManager>();
           }
           return _instance;
       }
       
   }


   private void Awake()
   {
       if (_instance != null && _instance != this)
       {
           Destroy(this.gameObject);
       }
       else
       {
           _instance = this;
       }
       Time.timeScale = 5f;
   }
   
   private void Start()
   {
      
      
      GameState = State.Start;
      

      ShowStartPanel();

   }

   private void Update()
   {

       switch (GameState)
       {
           case State.Start:
               break;
           case State.Play:
           
           foreach (var activeRoad in _activeRoads)
           {
               activeRoad.transform.Translate(Vector3.back * Time.deltaTime);
               
           }
           if(_carController != null) gasText.text=_carController.Gas.ToString();
               break;
           
           case State.End:
               break;
           
       }
       
   }


   private void StartGame()
   {

       _roadIndex = 0;
       InitializeRoadPool();

       SpawnRoad(Vector3.zero);
       
       
       
       _carController = Instantiate(carPrefab, new Vector3(0,0,-3f), Quaternion.identity).GetComponent<CarController>();

       leftMoveButton.OnMoveButtonDown += () => _carController.Move(-1f);
       
       rightMoveButton.OnMoveButtonDown += () => _carController.Move(1f);
       
       
       //게임 상태 play 변경
       GameState = State.Play;


   }

   public void EndGame()
   {
       GameState = State.End;
       
       Destroy(_carController.gameObject);

       foreach (var activeRoad in _activeRoads)
       {
           activeRoad.SetActive(false);
           
       }
       
       ShowEndPanel();
       
   }

   #region UI


   public void ShowStartPanel()
   {
       StartPanelController startPanelController =
           Instantiate(startPanelPrefab, canvasTransform).GetComponent<StartPanelController>();
       startPanelController.OnStartButtonClick += () =>
       {
            StartGame();
            Destroy(startPanelController.gameObject);
       };

   }

   private void ShowEndPanel()
   {
       StartPanelController endPanelController = Instantiate(endPanelPrefab, canvasTransform).GetComponent<StartPanelController>();
       endPanelController.OnStartButtonClick += () =>
       {
           
           Destroy(endPanelController.gameObject);
           ShowStartPanel();
       };
   }
   

   #endregion
   
   
   
   #region 도로 생성 및 관리

   private void InitializeRoadPool()
   {
       for (int i = 0; i < _roadPoolSize; i++)
       {
           GameObject road = Instantiate(roadPrefab);
           road.SetActive(false);
           _roadPool.Enqueue(road);
           
       }
       
   }

   
   public void SpawnRoad(Vector3 position)
   {
       GameObject road;
       
       
       if (_roadPool.Count > 0)
       {
           road = _roadPool.Dequeue();
           road.transform.position = position;
           road.SetActive(true);
           
           
       }
       else
       {
           road = Instantiate(carPrefab, position, Quaternion.identity);
           
       }

       if (_roadIndex > 0 && _roadIndex % 2 == 0)
       {
           road.GetComponent<RoadController>().SpawnGas();
           
       }
       
       _activeRoads.Add(road);
       _roadIndex++;


   }

   public void DestroyRoad(GameObject road)
   {
       road.SetActive(false);
       _activeRoads.Remove(road);
       _roadPool.Enqueue(road);
       
   }
   
   #endregion
}
