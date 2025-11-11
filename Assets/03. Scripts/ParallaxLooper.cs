using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public Transform[] tiles;
    public float speed = 1f;

    [HideInInspector] public float spriteWidth;
}

public class ParallaxLooper : MonoBehaviour
{
    [Header("Capas")]
    public ParallaxLayer[] layers = new ParallaxLayer[7];

    [Header("Control")]
    public bool stopParallax = false;

    private void Start()
    {
        foreach (var layer in layers)
        {
            if (layer.tiles == null || layer.tiles.Length < 2)
            {
                continue;
            }

            SpriteRenderer sr = layer.tiles[0].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                layer.spriteWidth = sr.bounds.size.x;
            }
        }
    }

    private void Update()
    {
        if (stopParallax) return;

        foreach (var layer in layers)
        {
            if (layer.tiles == null || layer.tiles.Length < 2) continue;

            float move = layer.speed * Time.deltaTime;

            for (int i = 0; i < layer.tiles.Length; i++)
            {
                Transform tile = layer.tiles[i];
                tile.position += Vector3.left * move;

                if (tile.position.x <= -layer.spriteWidth)
                {
                    float rightMost = layer.tiles[0].position.x;
                    foreach (var t in layer.tiles)
                    {
                        if (t.position.x > rightMost) rightMost = t.position.x;
                    }

                    tile.position = new Vector3(rightMost + layer.spriteWidth, tile.position.y, tile.position.z);
                }
            }
        }
    }
}
