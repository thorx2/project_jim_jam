using Godot;

public partial class GameVPScaler : SubViewportContainer
{
	#region Viewport Scaling
	[Export]
	private Viewport gameViewport;

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
		Scale = scaling;
	}
}
