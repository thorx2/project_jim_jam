using Godot;

namespace CoreGame.Pathfinding;
public partial class PathGenerator : Node
{
    [Export]
    private TileMap gameMap;

    [Export]
    private int navMeshLayer = 0;

    private AStarGrid2D _mapNavMesh;


    private static PathGenerator instance = null;

    public static PathGenerator GetPathGeneratorInstance
    {
        get => instance;
    }

    #region Godot Functions
    public override void _Ready()
    {
        instance = this;
        _mapNavMesh = new();
        SetupGameMap();
    }

    public void SetupGameMap(TileMap map)
    {
        gameMap = map;
        SetupGameMap();
    }

    private void SetupGameMap()
    {
        if (gameMap != null)
        {
            GenerateNavMeshData();
            var allTilePos = gameMap.GetUsedCells(navMeshLayer);
            foreach (var tile in allTilePos)
            {
                _mapNavMesh.SetPointSolid(tile, false);
            }
        }
    }

    #endregion

    #region Functional


    private void GenerateNavMeshData()
    {
        _mapNavMesh.CellSize = new Vector2(gameMap.TileSet.TileSize.X, gameMap.TileSet.TileSize.Y);
        _mapNavMesh.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
        _mapNavMesh.Region = gameMap.GetUsedRect();
        _mapNavMesh.Update();
        _mapNavMesh.FillSolidRegion(_mapNavMesh.Region, true);
    }

    #endregion

    #region Data Functions

    public Vector2 GetGlobalPositionOfTile(Vector2I pos)
    {
        return gameMap.ToGlobal(gameMap.MapToLocal(pos));
    }

    public Vector2[] GetPathFromTo(Vector2 start, Vector2 end)
    {
        return _mapNavMesh.GetPointPath(gameMap.LocalToMap(gameMap.ToLocal(start)), gameMap.LocalToMap(gameMap.ToLocal(end)));
    }

    public Vector2 GetPointPosition(Vector2I pos)
    {
        return _mapNavMesh.GetPointPosition(pos);
    }

    public Vector2 GetPointPositionCentered(Vector2I pos)
    {
        var point = _mapNavMesh.GetPointPosition(pos);
        point.X += GetNavCellSize().X / 2;
        point.Y += GetNavCellSize().Y / 2;
        return point;
    }

    public Vector2 GetNavRegionSize()
    {
        return _mapNavMesh.Region.Size;
    }

    public Vector2 GetNavCellSize()
    {
        return _mapNavMesh.CellSize;
    }

    public bool IsValidNavPoint(Vector2I pos)
    {
        return _mapNavMesh.Region.HasPoint(pos) && !_mapNavMesh.IsPointSolid(pos);
    }

    public Vector2I GetMapPointForPosition(Vector2 pos)
    {
        return gameMap.LocalToMap(pos);
    }

    public TileData GetTileData(int layer, Vector2I pos, bool useProxy = false)
    {
        return gameMap.GetCellTileData(layer, pos, useProxy);
    }

    #endregion
}