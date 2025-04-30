using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject winMenuFinal;
    [SerializeField] private Timer timerScript;
    [SerializeField] private WinLevel winLevelScript;
    private int numberEnemies;

    private void Start()
    {
        numberEnemies = GameObject.FindGameObjectsWithTag("Bot").Length;
        portal.SetActive(true);
    }

    private void Update()
    {
        numberEnemies = GameObject.FindGameObjectsWithTag("Bot").Length;

        if (numberEnemies == 0 && !portal.activeSelf)
        {
            portal.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && numberEnemies == 0)
        {
            Debug.Log("Ventana menu win");
            //5         5
            if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            {
                Debug.Log("5 mayor a 3");
                float finalTime = timerScript.StopTimer();
                winLevelScript.ShowFinalTime(finalTime);
                winMenu.SetActive(true);
                Debug.Log(SceneManager.sceneCountInBuildSettings + " " + SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                float finalTime = timerScript.StopTimer();
                winLevelScript.ShowFinalTime(finalTime);
                winMenuFinal.SetActive(true);
                Debug.Log(SceneManager.sceneCountInBuildSettings + " " + SceneManager.GetActiveScene().buildIndex);

            }
        }
        else
        {
            Debug.Log("workssss");
        }
    }
}
