using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public static InputPlayer Instance;

    [Header("Swipe")]
    public float minSwipeDistance = 50f;

    public bool SwipeUp { get; private set; }
    public bool SwipeDown { get; private set; }
    public bool SwipeLeft { get; private set; }
    public bool SwipeRight { get; private set; }
    public bool Touch { get; private set; }

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS

        SwipeUp = SwipeDown = SwipeLeft = SwipeRight = Touch = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                // point a swipe
                case TouchPhase.Began:
                    startTouchPos = touch.position;
                    break;

                // point b swipe
                case TouchPhase.Ended:
                    endTouchPos = touch.position;
                    DetectSwipeOrTouch();
                    break;
            }
        }

#endif
    }

    private void DetectSwipeOrTouch()
    {
        Vector2 swipe = endTouchPos - startTouchPos;

        // tap
        if (swipe.magnitude < minSwipeDistance)
        {
            Touch = true;
            return;
        }

        // swipe
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            if (swipe.x > 0) SwipeRight = true;
            else SwipeLeft = true;
        }
        else
        {
            if (swipe.y > 0) SwipeUp = true;
            else SwipeDown = true;
        }
    }
}
