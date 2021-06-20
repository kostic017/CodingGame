using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Texture2D map;
    public TileSize tileSize;
    public float tileSpacing;
    public LevelObject[] levelObjects;

    internal Level Level { get; private set; }

    private LevelObject wallObj;

    private readonly Dictionary<Color, LevelObject> mappings = new Dictionary<Color, LevelObject>();

    void Awake()
    {
        Level = new Level(map.width, map.height);

        foreach (var levelObject in levelObjects)
        {
            mappings.Add(levelObject.mapColor, levelObject);
            if (levelObject.prefab.name == "Wall")
                wallObj = levelObject;
        }
 
        for (var r = 0; r < map.height; ++r)
        {
            for (var c = 0; c < map.width; ++c)
            {
                var levelObject = mappings[map.GetPixel(c, r)];

                if (levelObject.isTile)
                {
                    var obj = InstantiateTile(r, c, levelObject);
                    if (levelObject.prefab.name == "Spawner")
                        obj.GetComponent<Spawner>().SetPosition(r, c);
                    else if (levelObject.prefab.name == "Exit")
                        Level.SetExit(r, c);
                }
                else
                {
                    var wall = InstantiateTile(r, c, wallObj);
                    var position = GetTilePosition(r, c, wall.transform.position.y + levelObject.heightScale * 0.5f);
                    var obj = Instantiate(levelObject.prefab, position, Quaternion.identity);
                    if (levelObject.prefab.name == "Turret")
                        Level.Add(obj.GetComponent<Turret>());
                }
            }
        }
    }

    GameObject InstantiateTile(int r, int c, LevelObject levelObject)
    {
        var position = GetTilePosition(r, c, levelObject.heightScale * 0.5f);
        var obj = Instantiate(levelObject.prefab, position, Quaternion.identity);
        obj.transform.localScale = new Vector3(tileSize.length, tileSize.height * levelObject.heightScale, tileSize.length);
        Level.Tiles[r, c] = levelObject.prefab.name;
        return obj;
    }

    internal Vector3 GetTilePosition(int r, int c, float y)
    {
        return new Vector3(c * (tileSize.length + tileSpacing), y, r * (tileSize.width + tileSpacing));
    }
}
