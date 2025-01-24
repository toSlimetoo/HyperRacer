using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameTestScript
{
    
    private CarController _carController;
    private GameObject _leftMoveButton;
    private GameObject _rightMoveButton;
    
    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        
        Time.timeScale = 10f;
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        yield return waitForSceneLoad();
        
        //필수 오브젝트 확인
        
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager Object is null");
        
        var startButton = GameObject.Find("Start Button");
        Assert.IsNotNull(startButton, "Start Button is null");
        
        startButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();

        _carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(_carController, "Car Controller is null");
        
        _leftMoveButton = GameObject.Find("LeftMoveButton");
        Assert.IsNotNull(_leftMoveButton, "Left Button is null");
        _rightMoveButton = GameObject.Find("RightMoveButton");
        Assert.IsNotNull(_rightMoveButton, "Right Button is null");
        
        Vector3 leftPosition = new Vector3(-1f, 0.2f, -5f);
        Vector3 rightPosition = new Vector3(1f, 0.2f, -5f);
        Vector3 centerPosition = new Vector3(0, 0.2f, -5f);
        
        float rayDistance = 5f;
        Vector3 rayDirection = Vector3.forward;
        
        float elapsedTime = 0f;
        float targetTime = 10f;
        
        
        while (gameManager.GameState==GameManager.State.Play)
        {

            
            
            if (Physics.Raycast(leftPosition, rayDirection, out RaycastHit hit, rayDistance, LayerMask.GetMask("Enemy")))
            {
                if (Mathf.Abs(hit.point.x - _carController.transform.position.x) <= 0.6f)
                {
                    Debug.Log("left");
                    DodgeCar(hit.point);
                }
            }
            else if (Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance, LayerMask.GetMask("Enemy")))
            {
                if (Mathf.Abs(hit.point.x - _carController.transform.position.x) <= 0.6f)
                {
                    Debug.Log("right");
                    DodgeCar(hit.point);
                }
            }
            else if(Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance, LayerMask.GetMask("Enemy")))
            {
                if (Mathf.Abs(hit.point.x - _carController.transform.position.x) <= 0.6f)
                {
                    Debug.Log("right");
                    DodgeCar(hit.point);
                }
            }
            else if (Physics.Raycast(leftPosition, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("left");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("right");
                MoveCar(hit.point);
            }
            else if(Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("center");
                MoveCar(hit.point);
            }
            else
            {
                Debug.Log("none");
                MoveButtonUp(_leftMoveButton);
                MoveButtonUp(_rightMoveButton);
                
            }
            
            
            
            
            
            Debug.DrawRay(leftPosition, rayDirection*rayDistance, Color.red);
            Debug.DrawRay(rightPosition, rayDirection*rayDistance, Color.green);
            Debug.DrawRay(centerPosition, rayDirection*rayDistance, Color.blue);
                
            elapsedTime += Time.deltaTime;
            
            yield return null;            
        }

        if (elapsedTime < targetTime)
        {
            Assert.Fail("Game Time is too short");
        }
        
        Time.timeScale = 1f;
    }

    private IEnumerator waitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex>0)
        {
            yield return null;
        }
        
    }

    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
        
    }
    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
        
    }

    private void MoveCar(Vector3 targetPosition)
    {
        if (Mathf.Abs(targetPosition.x - _carController.transform.position.x) < 0.1f)
        {
            MoveButtonUp(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
            return;
        }
        
        if(targetPosition.x <_carController.transform.position.x)
        {
            MoveButtonDown(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
            
            
        }
        else if(targetPosition.x > _carController.transform.position.x)
        {
            MoveButtonDown(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
            
        }
        else
        {
            MoveButtonUp(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
        }
        
    }

    private void DodgeCar(Vector3 targetPosition)
    {
        if (Mathf.Abs(targetPosition.x - _carController.transform.position.x) >= 0.6f)
        {
            MoveButtonUp(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
            return;
        }
        
        if(targetPosition.x >=1f)
        {
            MoveButtonDown(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
        else if (targetPosition.x <= -1f)
        {
            MoveButtonDown(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
                
        }
        else if (targetPosition.x >= 0f)
        {
            if (_carController.transform.position.x <= targetPosition.x)
            {
                MoveButtonDown(_leftMoveButton);
                MoveButtonUp(_rightMoveButton);
            }
            else
            {
                MoveButtonDown(_rightMoveButton);
                MoveButtonUp(_leftMoveButton);
                
            }
        }
        
            
            
    }
        
        
    
    
    
}
