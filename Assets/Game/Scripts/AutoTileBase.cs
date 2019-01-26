namespace Game.Scripts
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class AutoTileBase : Tile
    {
        public TileBase[] FriendTile;

        protected bool IsAutoTile(ITilemap tileMap, Vector3Int position)
        {
            var tile = tileMap.GetTile(position);
            return tile == this || FriendTile.Any(friendTile => friendTile == tile);
        }

#if UNITY_EDITOR
        protected static void CreateTileAsset<T>() where T : AutoTileBase
        {
            var className = typeof(T).Name;
            var path = EditorUtility.SaveFilePanelInProject("Save " + className, "New " + className, "Asset",
                "Save " + className, "Assets");
            if (path == "")
            {
                return;
            }

            AssetDatabase.CreateAsset(CreateInstance<T>(), path);
        }
#endif
    }
}