using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager Instance;

    [Header("Settings")]
    public float baseSpeed = 2f;
    public float stopSmoothness = 15f;

    [Header("Parallax Layers")]
    public List<ParallaxLayer> layers = new List<ParallaxLayer>();

    private bool stopping = false;
    private float currentSpeedMultiplier = 1f;

    [Header("Zonas")]
    public ParallaxZone zone1;
    public ParallaxZone zone2;
    public ParallaxZone zone3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeLayers();
        ApplyZone(zone1); // zona por defecto
    }

    private void Update()
    {
        UpdateParallax();
        SmoothStop();
    }

    private void InitializeLayers()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            float speedFactor = 1f - (i * 0.15f);
            speedFactor = Mathf.Clamp(speedFactor, 0.1f, 1f);

            layers[i].Initialize(speedFactor, baseSpeed);
        }
    }

    private void UpdateParallax()
    {
        foreach (var layer in layers)
            layer.Move(currentSpeedMultiplier);
    }

    private void SmoothStop()
    {
        if (!stopping) return;

        currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, 0f, Time.deltaTime * stopSmoothness);

        if (currentSpeedMultiplier < 0.01f)
        {
            currentSpeedMultiplier = 0f;
            stopping = false;
        }
    }

    public void StopParallax()
    {
        stopping = true;
    }

    public void RenaudeParallax()
    {
        stopping = false;
    }

    public void SwitchParallaxZone1() => ApplyZone(zone1);
    public void SwitchParallaxZone2() => ApplyZone(zone2);
    public void SwitchParallaxZone3() => ApplyZone(zone3);

    private void ApplyZone(ParallaxZone zone)
    {
        if (zone == null || zone.layerSprites == null || zone.layerSprites.Length == 0)
        {
            return;
        }

        if (zone.layerSprites.Length != layers.Count)
        {
            return;
        }

        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].SwapSprite(zone.layerSprites[i]);
        }
    }
}

[System.Serializable]
public class ParallaxZone
{
    [Tooltip("Un sprite por capa. Layer 0 usa sprite[0], Layer 1 usa sprite[1], etc")]
    public Sprite[] layerSprites;
}

[System.Serializable]
public class ParallaxLayer
{
    [Tooltip("Sprites repetidos (mínimo 2) que forman el loop del layer")]
    public Transform[] tileSprites;

    private float finalSpeed;
    private float spriteWidth;
    private Vector3 startPos;

    public void Initialize(float speedFactor, float baseSpeed)
    {
        finalSpeed = baseSpeed * speedFactor;

        if (tileSprites.Length < 2)
        {
            return;
        }

        startPos = tileSprites[0].position;

        SpriteRenderer sr = tileSprites[0].GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    public void Move(float multiplier)
    {
        float move = finalSpeed * multiplier * Time.deltaTime;

        foreach (Transform spr in tileSprites)
        {
            spr.Translate(Vector3.left * move);

            if (spr.position.x <= startPos.x - spriteWidth)
            {
                spr.position += new Vector3(spriteWidth * tileSprites.Length, 0f, 0f);
            }
        }
    }

    public void SwapSprite(Sprite newSprite)
    {
        foreach (Transform spr in tileSprites)
        {
            SpriteRenderer sr = spr.GetComponent<SpriteRenderer>();
            sr.sprite = newSprite;
        }
    }
}
