using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class touchV2 : MonoBehaviour //, IPointerDownHandler
{
    public static int tapX;//where the double tap
    public static int tapY;//where the double tap
    public static int fuction;//第幾個手勢
    //0=double tap,1=left,2=right,3=down,4=clock,5=clockwise
    //-1=default
    public static bool isTrigger;//如果有新的手勢，trig會起來一個cycle

    Vector2 shift;

    int leftT;
    int rightT;
    int downT;
    int upT;
    int cnt;

    Vector2 leftMost;
    Vector2 rightMost;
    Vector2 downMost;
    Vector2 upMost;
    Vector2 start;
    Vector2 end;
    
    bool isTap;

    

    private void resetMost()
    {
        leftMost = new Vector2(1000000,0);
        rightMost = new Vector2(-1000000,0);
        upMost = new Vector2(0,-1000000);
        downMost = new Vector2(0,1000000);
        leftT = 0;
        rightT = 0;
        downT = 0;
        upT = 0;
        cnt = 0;
        fuction = -1;
       
    }
    void updateMost(Vector2 pos, int cnt)
    {
        if (leftMost.x > pos.x)
        {
            leftT = cnt;
            leftMost = pos;
        }
        if (rightMost.x < pos.x)
        {
            rightT = cnt;
            rightMost = pos;
        }
        if (upMost.y < pos.y)
        {
            upT = cnt;
            upMost = pos;
        }
        if (downMost.y > pos.y)
        {
            downT = cnt;
            downMost = pos;
        }
    }

    bool specialgesture()
    {
        if (leftT > downT && downT > rightT)//clock
        {
            if (start.y>leftMost.y && end.y> leftMost.y)
            {
                
                fuction = 4;

                return true;
            }
        }
        else if (rightT > downT && downT > leftT)
        {
            if (start.y > rightMost.y && end.y > rightMost.y)
            {
               
                fuction = 5;
                return true;
            }
        }
        return false;
    }


    private void Start()
    {
        isTap = false;
       
        resetMost();
    }
    /* public void OnPointerDown(PointerEventData eventData)
     {
         Debug.Log("one");
         Image.transform.position = eventData.position;
 ("one");
        Image.transform.position = eventData.position;
    }*/

    void tapControl()
    {
        if (Mathf.Abs(shift.x) <= 5f && Mathf.Abs(shift.y) <= 5f)//tap
        {
            int x = (int)Input.GetTouch(0).position.x / 72;
            int y = (int)Input.GetTouch(0).position.y / 72;
            if (isTap && x == tapX && y == tapY)//same point
            {
                
                fuction = 0;
                isTap = false;
            }
            else
            {
                isTap = true;
                tapX = x;
                tapY = y;
                
            }

        }//tap end
    }
    void slideControl()
    {
        if (Mathf.Abs(shift.y) > Mathf.Abs(shift.x))
        {
            if (shift.y <= -200f)
            {
                
                fuction = 3;
                isTrigger = true;

            }

        }
        else
        {
            if (shift.x >= 200.0f)
            {

                fuction = 2;
                isTrigger = true;
            }
            else if (shift.x <= -200f)
            {

                fuction = 1;
                isTrigger = true;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
        isTrigger = false;
        if (Input.touchCount >= 1)
        {
            
            cnt++;
            updateMost(Input.GetTouch(0).position, cnt);

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                start = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                end = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                shift = end - start;
                bool gest = specialgesture();
                if (!gest)
                {
                    slideControl();
                    tapControl();
                }
                else
                {
                    //有special gesture
                    isTrigger = true;
                }
                resetMost();
            }
        }//end touch count

       

    }//end update
}
