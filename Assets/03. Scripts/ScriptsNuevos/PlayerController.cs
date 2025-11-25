using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject shop;

    //buttons area
    [SerializeField] private RectTransform shopButtonArea;
    [SerializeField] private RectTransform closeShopButtonArea;
    [SerializeField] private RectTransform purchaseDamageButton;
    [SerializeField] private RectTransform purchaseShieldButton;
    [SerializeField] private RectTransform purchaseHealthButton;

    bool isInShop = false;

    void Update()
    {
        if (InputManager.Instance == null)
            return;

        if (!isInShop)
        {
            if (InputManager.Instance.SwipeUp)
                AttackUp();
            if (InputManager.Instance.SwipeDown)
                AttackDown();
            if (InputManager.Instance.SwipeLeft)
                AttackLeft();
            if (InputManager.Instance.SwipeRight)
                AttackRight();

            if (InputManager.Instance.Touch)
            {
                if (IsTouchOn(shopButtonArea))
                    OpenShop();
                else
                    Jump();
            }
        }
        else
        {
            if (InputManager.Instance.Touch)
            {
                if (IsTouchOn(closeShopButtonArea))
                    CloseShop();

                if (IsTouchOn(purchaseDamageButton))
                    PurchaseDamage();

                if (IsTouchOn(purchaseShieldButton))
                    PurchaseShield();

                if (IsTouchOn(purchaseHealthButton))
                    PurchaseHealth();
            }
        }
    }

    private void AttackUp()
    {
        up.SetActive(true);
        ScoreManager.Instance.AddScore(100);
        StartCoroutine(DisableUp());
    }

    private void AttackDown()
    {
        down.SetActive(true);
        ScoreManager.Instance.RemoveScore(100);
        StartCoroutine(DisableDown());
    }

    private void AttackLeft()
    {
        left.SetActive(true);
        ScoreManager.Instance.AddCoins(100);
        StartCoroutine(DisableLeft());
    }

    private void AttackRight()
    {
        right.SetActive(true);
        ScoreManager.Instance.RemoveCoins(100);
        StartCoroutine(DisableRight());
    }

    private void Jump()
    {
        jump.SetActive(true);
        StartCoroutine(DisableTouch());
    }

    private void OpenShop()
    {
        shop.SetActive(true);
        isInShop = true;
    }

    private void CloseShop()
    {
        shop.SetActive(false);
        isInShop = false;
    }

    private void PurchaseDamage()
    {
        ShopManager.Instance.TryBuyDamage();
    }

    private void PurchaseShield()
    {
        ShopManager.Instance.TryBuyShield();
    }

    private void PurchaseHealth()
    {
        ShopManager.Instance.TryBuyHealth();
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
        jump.SetActive(false);
    }
}
