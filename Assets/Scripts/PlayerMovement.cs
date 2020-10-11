using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController playerCC;
    Vector3 playerMoveVec;
    public Vector2 touchStartPos, swipeDirection=new Vector2(0,0);
    float  attackTimer=0f;
    int currentLane=0, enemyTimer=0, currentAttack=0;
    bool isForMove=false;

    public GameObject[] enemies;
    
    
    private void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Began");
                    
                    touchStartPos = touch.position;
                    isForMove = touch.position.y <= Screen.height / 3;
                    break;
                case TouchPhase.Ended:
                    swipeDirection = touch.position - touchStartPos;                    
                    break;
            }
        }
        if(swipeDirection.magnitude>0f){            
            float dx = Mathf.Sign(swipeDirection.x); // 0 - координата не измнилась, -1 - уменьшилась (вниз или влево)
            float dy = Mathf.Sign(swipeDirection.y); //       1 увеличилась (вверх или вправо)
            if(isForMove){
                if(Mathf.Abs(currentLane+dx)<2) currentLane = currentLane+(int)dx;
                isForMove=false;
            }else{
            if(dy==1){
                currentAttack=(int)(dx)+2;
            }else
                currentAttack=2;
            attackTimer=20;
            }
            swipeDirection=new Vector2(0,0);
        }
        
        if(attackTimer>0f){
            attackTimer--;
        }else{
            currentAttack=0;
        }
        enemyTimer++;
        if (enemyTimer%200==0)
        {
            enemyTimer=0;
            
            Object.Instantiate(enemies[Random.Range(0,enemies.Length)],new Vector3(transform.position.x+40,transform.position.y,-1.5f+Random.Range(0,3)*1.5f), Quaternion.identity);
        }
        playerMoveVec=new Vector3(0,0,-(transform.position.z+1.5f*currentLane)/10);
        playerCC.Move(playerMoveVec);

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"+currentAttack))
            Destroy(other.gameObject);
        else
            Application.LoadLevel(Application.loadedLevel);
             

    }
}
