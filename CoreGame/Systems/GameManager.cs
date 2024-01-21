using Godot;
using System;

namespace CoreGame.GameSystems;

public partial class GameManager : Node2D
{

    #region Viewport Scaling
    /// <summary>
    /// Spawn game play elements within this VP as the parent.
    /// </summary>
    [Export]
    private Viewport gameViewport;

    [Export]
    private SubViewportContainer gameVPContainer;

    private Vector2 scaling;

    private Vector2 rootVPSize;
    #endregion

    public override void _Ready()
    {
        ScaleGameplayViewportToParent();
    }

    private void ScaleGameplayViewportToParent()
    {
        rootVPSize = GetViewportRect().Size;
        scaling = rootVPSize / gameViewport.GetVisibleRect().Size;
        gameVPContainer.Scale = scaling;
    }

}
