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
    [SerializeField] private GameObject settings;

    [SerializeField] private RectTransform shopButtonArea;
    [SerializeField] private RectTransform closeShopButtonArea;
    [SerializeField] private RectTransform purchaseDamageButton;
    [SerializeField] private RectTransform purchaseShieldButton;
    [SerializeField] private RectTransform purchaseHealthButton;

    [SerializeField] private RectTransform settingsButtonArea;
    [SerializeField] private RectTransform closeSettingsButtonArea;

    public bool isInCombat = false;
    bool isInShop = false;
    bool isInSettings = false;

    private void Start()
    {
        ScoreManager.Instance.ResetScore();
        ScoreManager.LastRunScore = 0;
    }

    void Update()
    {
        if (InputManager.Instance == null)
            return;

        if (!isInShop && !isInSettings)
        {
            if (!isInCombat)
            {
                if (InputManager.Instance.Touch)
                {
                    if (IsTouchOn(shopButtonArea))
                        OpenShop();
                    if (IsTouchOn(settingsButtonArea))
                        OpenSettings();
                    else
                        Jump();
                }
            }

            if (isInCombat)
            {
                if (InputManager.Instance.SwipeUp)
                    TryPerfect(ArrowSet.ArrowType.Up, AttackUp);

                if (InputManager.Instance.SwipeDown)
                    TryPerfect(ArrowSet.ArrowType.Down, AttackDown);

                if (InputManager.Instance.SwipeLeft)
                    TryPerfect(ArrowSet.ArrowType.Left, AttackLeft);

                if (InputManager.Instance.SwipeRight)
                    TryPerfect(ArrowSet.ArrowType.Right, AttackRight);
            }
        }

        if (isInShop)
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

        if (isInSettings)
        {
            if (InputManager.Instance.Touch)
            {
                if (IsTouchOn(closeSettingsButtonArea))
                    CloseSettings();
            }
        }
    }

    private void TryPerfect(ArrowSet.ArrowType swipeType, System.Action attackAction)
    {
        var closestCorrect = CombatController.Instance.GetClosestArrowInTarget(swipeType);

        if (closestCorrect != null)
        {
            //perfect
            attackAction.Invoke();
            CombatController.Instance.currentEnemy?.TakeDamage();

            closestCorrect.DestroyArrow();
        }
        else
        {
            CombatController.Instance.MissByWrongSwipe();
            ScoreManager.Instance.TakeDamage(CombatController.Instance.currentEnemy.Damage);
        }
    }


    private void AttackUp()
    {
        up.SetActive(true);
        StartCoroutine(DisableUp());
    }

    private void AttackDown()
    {
        down.SetActive(true);
        StartCoroutine(DisableDown());
    }

    private void AttackLeft()
    {
        left.SetActive(true);
        StartCoroutine(DisableLeft());
    }

    private void AttackRight()
    {
        right.SetActive(true);
        StartCoroutine(DisableRight());
    }

    private void Jump()
    {
        jump.SetActive(true);
        StartCoroutine(DisableTouch());
    }


    private void OpenShop()
    {
        PauseManager.Instance.PauseGame();
        shop.SetActive(true);
        isInShop = true;
    }

    private void CloseShop()
    {
        PauseManager.Instance.ResumeGame();
        shop.SetActive(false);
        isInShop = false;
    }

    private void OpenSettings()
    {
        PauseManager.Instance.PauseGame();
        settings.SetActive(true);
        isInSettings = true;
    }

    private void CloseSettings()
    {
        PauseManager.Instance.ResumeGame();
        settings.SetActive(false);
        isInSettings = false;
    }

    private void PurchaseDamage() => ShopManager.Instance.TryBuyDamage();
    private void PurchaseShield() => ShopManager.Instance.TryBuyShield();
    private void PurchaseHealth() => ShopManager.Instance.TryBuyHealth();


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
