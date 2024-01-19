using System;
using Godot;

namespace CoreGame.Pathfinding;

/// <summary>
/// Class still under W.I.P, debug draw maybe broken
/// </summary>
public partial class PathDebugger : Control
{
    [Export]
    private PathGenerator pathGenerator;

    [Export]
    private Marker2D startPoint;

    [Export]
    private Marker2D endPoint;

    private Vector2[] requestedPath;

    public override void _Process(double delta)
    {
        DebugDrawNavPoints();
        if (requestedPath != null && requestedPath.Length > 1)
        {
            requestedPath = pathGenerator.GetPathFromTo(startPoint.GlobalPosition, endPoint.GlobalPosition);
        }
    }

    public override void _Draw()
    {
        DebugDrawNavPoints();

        if (requestedPath != null && requestedPath.Length > 0)
        {
            DrawPathLines(requestedPath);
        }
    }

    private void DrawPossibleConnectionTo(Vector2I t, int x, int y)
    {
        Vector2I d = new Vector2I(x, y);
        if (pathGenerator.IsValidNavPoint(d))
        {
            var points = pathGenerator.GetPathFromTo(t, d);
            if (points.Length > 1)
            {
                DrawPathLines(points);
            }
        }
    }

    private void DrawPathLines(Vector2[] points)
    {
        var start = points[0];
        start.X += pathGenerator.GetNavCellSize().X / 2;
        start.Y += pathGenerator.GetNavCellSize().Y / 2;
        var end = points[points.Length - 1];
        end.X += pathGenerator.GetNavCellSize().X / 2;
        end.Y += pathGenerator.GetNavCellSize().Y / 2;
        DrawLine(start, end, Colors.White, 1.0f);
    }

    private void DebugDrawNavPoints()
    {
        for (int x = 0; x < pathGenerator.GetNavRegionSize().X; x++)
        {
            for (int y = 0; y < pathGenerator.GetNavRegionSize().Y; y++)
            {
                var t = new Vector2I(x, y);
                var pos = pathGenerator.GetPointPosition(t);
                pos.X = pathGenerator.GetNavCellSize().X / 2;
                pos.Y = pathGenerator.GetNavCellSize().Y / 2;
                DrawCircle(pos, 4.0f, Colors.Wheat);
                DrawPossibleConnectionTo(t, x, y + 1);
                DrawPossibleConnectionTo(t, x + 1, y);
            }
        }
    }

    private void DrawFullPathFrom(Vector2[] path)
    {
        for (int i = 0; i < path.Length - 2; i++)
        {
            var start = path[i];
            start.X += pathGenerator.GetNavCellSize().X / 2;
            start.Y += pathGenerator.GetNavCellSize().Y / 2;
            var end = path[i + 1];
            end.X += pathGenerator.GetNavCellSize().X / 2;
            end.Y += pathGenerator.GetNavCellSize().Y / 2;

            DrawLine(start, end, Colors.CornflowerBlue, 3.0f);
        }
    }
}