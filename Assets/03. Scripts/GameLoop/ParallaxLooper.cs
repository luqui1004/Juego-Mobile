//using UnityEngine;

//[System.Serializable]
//public class ParallaxLayer
//{
//    public GameObject[] tiles;
//    public float speed = 1f;
//    public Sprite[] sprites;
//    public int multiplyer; 

//    [HideInInspector] public float spriteWidth;
//}

//public class ParallaxLooper : MonoBehaviour
//{
//    [SerializeField] private int Zone = 0;   // Zona actual (índice de sprite)

//    [Header("Capas")]
//    public ParallaxLayer[] layers;

//    [Header("Control")]
//    public bool stopParallax = false;

//    private float currentTime;

//    private void Start()
//    {

//        foreach (var layer in layers)
//        {
//            if (layer == null || layer.tiles == null || layer.tiles.Length < 2)
//                continue;

//            SpriteRenderer sr = layer.tiles[0].GetComponent<SpriteRenderer>();
//            if (sr != null)
//            {
//                layer.spriteWidths = sr.bounds.size.x * layer.multiplyer;
//            }
//        }
//    }


//    private void Update()
//    {
//        //currentTime += Time.deltaTime;
//        //if (currentTime >= 8)
//        //{
//        //    ChangeZone();
//        //    currentTime = 0;
//        //}
//        if (stopParallax) return;

//        foreach (var layer in layers)
//        {
//            if (layer == null || layer.tiles == null || layer.tiles.Length < 2)
//                continue;

//            float move = layer.speed * Time.deltaTime;

//            for (int i = 0; i < layer.tiles.Length; i++)
//            {
//                Transform tile = layer.tiles[i].transform;
//                tile.position += Vector3.left * move;

//                if (tile.position.x <= -layer.spriteWidths)
//                {
//                    float rightMost = layer.tiles[0].transform.position.x;
//                    foreach (var t in layer.tiles)
//                    {
//                        if (t.transform.position.x > rightMost)
//                            rightMost = t.transform.position.x;
//                    }


//                    tile.position = new Vector3(
//                        rightMost + layer.spriteWidths,
//                        tile.position.y,
//                        tile.position.z
//                    );

//                    ApplyZoneSprite(layer, tile);
//                }
//            }
//        }
//    }


//    private void ApplyZoneSprite(ParallaxLayer layer, Transform tile)
//    {
//        if (layer.sprites == null || layer.sprites.Length == 0)
//            return;

//        int safeIndex = Mathf.Clamp(Zone, 0, layer.sprites.Length - 1);

//        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
//        if (sr != null)
//        {
//            sr.sprite = layer.sprites[safeIndex];

//            layer.spriteWidths = sr.bounds.size.x * layer.multiplyer;
//        }
//    }

//    public void ChangeZone()
//    {
//        // Buscamos cuántas zonas máximas soportan TODAS las capas (mínimo length)
//        int minSprites = int.MaxValue;

//        foreach (var layer in layers)
//        {
//            if (layer == null || layer.sprites == null || layer.sprites.Length == 0)
//                continue;

//            if (layer.sprites.Length < minSprites)
//                minSprites = layer.sprites.Length;
//        }

//        if (minSprites == int.MaxValue)
//            return;

//        Zone++;
//        if (Zone >= minSprites)
//        {
//            Zone = minSprites - 1;
//        }

//        // Si querés loopear zonas:
//        // Zone = (Zone + 1) % minSprites;
//    }
//}
