using UnityEngine;

public class SwipeTrailManager : MonoBehaviour
{
    public Camera cam;
    public GameObject trailPrefab;

    private GameObject currentTrail;

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 pos = cam.ScreenToWorldPoint(new Vector3(
                touch.position.x,
                touch.position.y,
                cam.nearClipPlane + 0.1f
            ));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    currentTrail = Instantiate(trailPrefab, pos, Quaternion.identity);
                    break;

                case TouchPhase.Moved:
                    if (currentTrail != null)
                        currentTrail.transform.position = pos;
                    break;

                case TouchPhase.Ended:
                    if (currentTrail != null)
                        Destroy(currentTrail, 0.2f);
                    break;
            }
        }

#endif
    }
}
