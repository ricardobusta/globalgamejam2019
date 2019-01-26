namespace Game.Scripts

{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class AutoTile4 : AutoTileBase
    {
        public Sprite[] Sprites = new Sprite[16];

        private static readonly Dictionary<int, int> MaskMap = new Dictionary<int, int>()
        {
            {0, 10}, {1, 6}, {2, 9}, {3, 5}, {4, 11}, {5, 7}, {6, 8}, {7, 4}, {8, 14}, {9, 2}, {10, 13}, {11, 1},
            {12, 15}, {13, 3}, {14, 12}, {15, 0}
        };

        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    var offsetPosition = location + new Vector3Int(x, y, 0);
                    if (tilemap.GetTile(offsetPosition) == this)
                    {
                        tilemap.RefreshTile(offsetPosition);
                    }
                }
            }
        }

        public override void GetTileData(Vector3Int location, ITilemap tilemap,
            ref TileData tileData)
        {
            var mask = IsAutoTile(tilemap, location + Vector3Int.up) ? 0 : 1;
            mask += IsAutoTile(tilemap, location + Vector3Int.left) ? 0 : 2;
            mask += IsAutoTile(tilemap, location + Vector3Int.right) ? 0 : 4;
            mask += IsAutoTile(tilemap, location + Vector3Int.down) ? 0 : 8;

            mask = MaskMap[mask];

            if (mask >= Sprites.Length || mask < 0)
            {
                Debug.LogWarning("Not enough sprites!");
            }
            else
            {
                tileData.sprite = Sprites[mask];
                tileData.color = color;
                tileData.colliderType = colliderType;
                tileData.flags = flags;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/AutoTile4")]
        public static void CreateTile()
        {
            CreateTileAsset<AutoTile4>();
        }
#endif
    }
}