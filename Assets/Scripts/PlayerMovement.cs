using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rect rightPart = new Rect(0, 0, Screen.width / 2, Screen.height);
    private Rect leftPart = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height);
    public UnityEngine.UI.Text text;
    CharacterController playerCC;
    Vector3 playerMoveVec;
    public Vector2 touchStartPos, swipeDirection=new Vector2(0,0);
    float speed=0.1f, attackTimer=0f;
    int lanenumber=1, enemyTimer=0, currentattack=0;

    public GameObject[] enemies;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerCC = GetComponent<CharacterController>();
        playerMoveVec = new Vector3(speed,0,0);
        playerCC.Move(playerMoveVec);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Began");
                    
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    Debug.Log("Move");
                    
                    swipeDirection = touch.position - touchStartPos;
                    break;
                case TouchPhase.Ended:
                    swipeDirection = touch.position - touchStartPos;

                    if(swipeDirection.magnitude==0f){ //если клик
                        if(leftPart.Contains(touch.position)){
                            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Max(-1.5f, transform.position.z-1.5f));
                        }else if(rightPart.Contains(touch.position)){
                            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Min(1.5f, transform.position.z+1.5f));
                        }
                    }
                    
                    break;
            }
        }
        if(swipeDirection.magnitude>0f){
            float dx = Mathf.Sign(swipeDirection.x); // 0 - координата не измнилась, -1 - уменьшилась (вниз или влево)
            float dy = Mathf.Sign(swipeDirection.y); //       1 увеличилась (вверх или вправо)
            if(dy==1){
                currentattack=(int)(dx)+2;
                Debug.Log(currentattack);
                attackTimer=30;
                text.text=""+currentattack;
            }else if(dy==-1) {
                currentattack=2;
                attackTimer=100;
            }
            swipeDirection=new Vector2(0,0);
        }
        
        if(attackTimer>0f){
            attackTimer--;
        }else{
            currentattack=0;
        }
        enemyTimer++;
        if (enemyTimer%200==0)
        {
            enemyTimer=0;
            
            Object.Instantiate(enemies[Random.Range(0,enemies.Length)],new Vector3(transform.position.x+40,transform.position.y,-1.5f+Random.Range(0,3)*1.5f), Quaternion.identity);
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"+currentattack))
            Destroy(other.gameObject);
        else
            Application.LoadLevel(Application.loadedLevel);
             

    }
}
