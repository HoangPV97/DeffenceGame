using UnityEngine;

public class IGEasyController : MonoBehaviour
{

    public Transform player; // Drag your player here
    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position
    private float angle;
    private float swipeDistanceX;
    private float swipeDistanceY;

    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
                    Debug.Log("Double tap..");
                }
                if (Input.GetTouch(i).tapCount == 1)
                {
                    Debug.Log("Single tap..");
                }
            }
        }

        foreach (Touch touch in Input.touches)
        {

            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
                swipeDistanceX = Mathf.Abs((lp.x - fp.x));
                swipeDistanceY = Mathf.Abs((lp.y - fp.y));
            }
            if (touch.phase == TouchPhase.Ended)
            {
                angle = Mathf.Atan2((lp.x - fp.x), (lp.y - fp.y)) * 57.2957795f;

                if (angle > 60 && angle < 120 && swipeDistanceX > 40)
                {
                    Debug.Log("right swipe...");
                    player.Rotate(0, 45, 0);
                }
                if (angle > 150 || angle < -150 && swipeDistanceY > 40)
                {
                    Debug.Log("down  swipe...");
                    player.position += new Vector3(0, -2, 0);
                }
                if (angle < -60 && angle > -120 && swipeDistanceX > 40)
                {
                    Debug.Log("left  swipe...");
                    player.Rotate(0, -45, 0);
                }
                if (angle > -30 && angle < 30 && swipeDistanceY > 40)
                {
                    Debug.Log("up  swipe...");
                    player.position += new Vector3(0, 2, 0);
                }
            }
        }
    }
}
