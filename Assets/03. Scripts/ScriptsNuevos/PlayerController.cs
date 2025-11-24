using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject touch;
    [SerializeField] private GameObject shop;
    [SerializeField] private RectTransform shopButtonArea;

    void Update()
    {
        if (InputPlayer.Instance == null)
            return;

        if (InputPlayer.Instance.SwipeUp)
            Jump();

        if (InputPlayer.Instance.SwipeDown)
            Roll();

        if (InputPlayer.Instance.SwipeLeft)
            MoveLeft();

        if (InputPlayer.Instance.SwipeRight)
            MoveRight();

        if (InputPlayer.Instance.Touch)
        {
            if (IsTouchOn(shopButtonArea))
            {
                OpenShop();
            }
            else
            {
                BasicAttack();
            }
        }
    }

    private void Jump()
    {
        up.SetActive(true);
        StartCoroutine(DisableUp());
    }

    private void Roll()
    {
        down.SetActive(true);
        StartCoroutine(DisableDown());
    }

    private void BasicAttack()
    {
        touch.SetActive(true);
        StartCoroutine(DisableTouch());
    }

    private void MoveLeft()
    {
        left.SetActive(true);
        StartCoroutine(DisableLeft());
    }

    private void MoveRight()
    {
        right.SetActive(true);
        StartCoroutine(DisableRight());
    }

    private void OpenShop()
    {
        shop.SetActive(true);
    }

    private bool IsTouchOn(RectTransform rect)
    {
        if (Input.touchCount == 0)
            return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.GetTouch(0).position,
            null
        );
    }

    //IENUMERATORS

    private IEnumerator DisableUp()
    {
        yield return new WaitForSeconds(1f);
        up.SetActive(false);
    }

    private IEnumerator DisableDown()
    {
        yield return new WaitForSeconds(1f);
        down.SetActive(false);
    }

    private IEnumerator DisableLeft()
    {
        yield return new WaitForSeconds(1f);
        left.SetActive(false);
    }

    private IEnumerator DisableRight()
    {
        yield return new WaitForSeconds(1f);
        right.SetActive(false);
    }

    private IEnumerator DisableTouch()
    {
        yield return new WaitForSeconds(1f);
        touch.SetActive(false);
    }
}
