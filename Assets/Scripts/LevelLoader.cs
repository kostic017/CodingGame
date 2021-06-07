using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Texture2D map;
    public TileSize tileSize;
    public float tileSpacing;
    public LevelObject[] levelObjects;

    internal static Level Level { get; private set; }

    private readonly Dictionary<Color, LevelObject> mappings = new Dictionary<Color, LevelObject>();

    void Awake()
    {
        Level = new Level(map.width, map.height);

        var l = map.width * (tileSize.length + tileSpacing);
        var w = map.height * (tileSize.width + tileSpacing);
        Camera.main.transform.position = new Vector3(l * 0.5f, Camera.main.transform.position.y, w * 0.25f);

        foreach (var levelObject in levelObjects)
            mappings.Add(levelObject.mapColor, levelObject);

        for (var r = 0; r < map.height; ++r)
        {
            for (var c = 0; c < map.width; ++c)
            {
                var levelObject = mappings[map.GetPixel(c, r)];
                Level.Grid[r, c] = levelObject;
                
                var position = GetTilePosition(r, c);
                position.y = levelObject.heightScale * 0.5f;

                var gameObject = Instantiate(levelObject.prefab, position, Quaternion.identity);
                
                gameObject.transform.localScale = new Vector3(tileSize.length, tileSize.height * levelObject.heightScale, tileSize.height);

                if (levelObject.prefab.name == "Spawner")
                {
                    var spawner = gameObject.GetComponent<Spawner>();
                    spawner.SetPosition(r, c);
                    spawner.SetLevelLoader(this);
                }
                else if (levelObject.prefab.name == "Exit")
                {
                    Level.Exits.Add(new Vector2Int(c, r));
                }
            }
        }
    }

    internal Vector3 GetTilePosition(int r, int c)
    {
        return new Vector3(c * (tileSize.length + tileSpacing), 0f, r * (tileSize.width + tileSpacing));
    }

}
