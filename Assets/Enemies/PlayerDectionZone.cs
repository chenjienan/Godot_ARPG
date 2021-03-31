using Godot;

public class PlayerDectionZone : Area2D
{
  public Player Player { get; set; }

  public void OnPlayerDectionZoneBodyEntered(Node body)
  {
    Player = body as Player;
  }

  public void OnPlayerDectionZoneBodyExited(Node body)
  {
    Player = null;
  }

  public bool PlayerInZone()
  {
    return Player != null;
  }
}
