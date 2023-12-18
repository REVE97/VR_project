using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;


    public void NextStage()
    {
        // Change Stage
        if (stageIndex < Stages.Length-1)
        {
        Stages[stageIndex].SetActive(false);
        stageIndex++;
        Stages[stageIndex].SetActive(true);
        PlayerReposition();
        }
        else {
            // Game Clear

            // Player Control Lock
            Time.timeScale = 0;
            // Result UI
            Debug.Log("게임 클리어!");
            // Restart Button UI
        }
        
        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;        
    }

    public void HealthDown()
    {
        if(health > 1)
            health--;
        else {
            // Player Die Effect
            player.OnDie();
            
            // Result UI
            Debug.Log("죽었습니다!");
            
            // Retry Button UI
        }
    }
    
    void OntriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Health Down
            HealthDown();

            // Player Reposition
           if(health > 1)
           {
            PlayerReposition();
           }


        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0,0,-1);
        player.VelocityZero();
    }
    
    
}
