using Godot;

namespace CoreGame.Pathfinding;
public partial class PathGenerator : Node
{
    [Export]
    private TileMap gameMap;

    [Export]
    private int navMeshLayer = 0;

    private AStarGrid2D _mapNavMesh;

    #region Godot Functions
    public override void _Ready()
    {
        _mapNavMesh = new();

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

    #endregion
}