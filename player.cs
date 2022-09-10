using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Text
using UnityEngine.UI;
// restart
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{

    // 預設 private , public 變成可在 unity 上調整 , [SerializeField] 既是私人又能 unity 編輯
    [SerializeField] float moveSpeed1 = 5f;

    // 紀錄腳下階梯
    GameObject currentFloor;


    // HP
    [SerializeField] int HP;

    [SerializeField] GameObject HPbar;

    // score
    [SerializeField]Text ScoreText;
    
    int Score;
    float ScoreTime;

    // 偷懶變數
    Animator anim;
    SpriteRenderer render;
    // 重玩按鈕
    [SerializeField]GameObject ReplayButton;



    // 初始值
    void Start()
    {           
        HP = 10;
        Score = 0;
        ScoreTime = 0;
        //
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        // 取得輸入
        if(Input.GetKey(KeyCode.D)){

            // Time.deltaTime 解決不同電腦偵率問題 , transform.Translate 移動
            transform.Translate( moveSpeed1 * Time.deltaTime, 0, 0);
            // 左右轉時轉頭
            render.flipX = false;

            // 當移動時，觸發跑步動畫。
            anim.SetBool("Run",true);

        }

        else if(Input.GetKey(KeyCode.A)){

            transform.Translate( -moveSpeed1 * Time.deltaTime, 0, 0);
            render.flipX = true;

            anim.SetBool("Run",true);

        }

        //  不動時，變回不動動畫
        else{
            anim.SetBool("Run",false);
        }

        // 每次 updata 時加分
        UpdateScore();
    }


    // 偵測碰撞 , 和 update 一樣持續執行 , other 表示碰到的東西
    void OnCollisionEnter2D(Collision2D other) {
        
        //  other.gameObject.tag 表示碰到的東西 ,結合 unity 內的 tag

        if(other.gameObject.tag == "normal"){

            // 碰到法向量 (0,1)的梯才算踩上去
            if (other.contacts[0].normal == new Vector2(0f,1f)){

            Debug.Log("撞到 1 ");
            // 紀錄當前腳下
            currentFloor = other.gameObject;

            // 踩到 normal 加血
            ModifyHP( 1 );
            
            }

        }
        
        else if(other.gameObject.tag == "nails"){

            if (other.contacts[0].normal == new Vector2(0f,1f)){

            Debug.Log("撞到 2 ");
            // 紀錄當前腳下
            currentFloor = other.gameObject;

            ModifyHP( -3 );

            // 播放 audio source音效
            other.gameObject.GetComponent<AudioSource>().Play();

            }
            
        }

        else if (other.gameObject.tag == "Ceiling"){

            Debug.Log("撞到天花板");

            //撞到天花板可穿過當前腳下階梯
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;

            ModifyHP( -3 );
            
            other.gameObject.GetComponent<AudioSource>().Play();

        }

    }


    // 和 collision 同，只是判斷經過，判定掉出地圖
    void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.tag == "Death.Line"){

            Debug.Log("You Loss");
            Die();

        }

    }


    // HP dicrease  
    void ModifyHP(int num){

        HP += num;
        if(HP > 10){

            HP = 10;

        }
        else if (HP <= 0){

            HP = 0;
            Die();

        }
        UpdateHPbar();
    }


    void UpdateHPbar(){

                            //  HPbar子物件數量，即目前HP
        for (int i = 0; i < HPbar.transform.childCount ; i++){

            if(HP > i ){                                //  控制血格隱藏
                HPbar.transform.GetChild(i).gameObject.SetActive(true);
            }

            else{
                HPbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // 分數增加
    void UpdateScore(){

        ScoreTime +=Time.deltaTime;

        if(ScoreTime > 2f){

            //每過兩秒加一分
            Score++;
            ScoreTime = 0f;
                                        // 改 text 中文字
            ScoreText.text = "地下" + Score.ToString() + "層";


        }

    }

    void Die(){

            // 死亡音效，不必other.gameObject ，因為是以碰到為前提，是以生命減至零
            GetComponent<AudioSource>().Play();
            anim.SetBool("Dead",false);

            // 遊戲執行倍率
            Time.timeScale = 0f;
            
            // 顯示 button
            ReplayButton.SetActive(true);


    }


    // 重新開始
    public void Replay(){
        
        Time.timeScale = 1f;
        // 重新載入 SampleScene 場景
        SceneManager.LoadScene("SampleScene");
    }

}
