using Godot;
using System;

public class Bat : KinematicBody2D
{
  private Vector2 knockback = Vector2.Zero;
  private Stats stats;

  public override void _Ready()
  {
    stats = GetNode<Stats>("Stats");
    GD.Print($"Bat {this.Name} health: {stats.Health}");
  }

  public override void _PhysicsProcess(float delta)
  {
    knockback = knockback.MoveToward(Vector2.Zero, delta * 200);
    knockback = MoveAndSlide(knockback);
  }

  public void OnHurtBoxAreaEntered(PlayerHitBox area)
  {
    stats.Health -= area.Damage;
    knockback = area.KnockBackVector * 100;
  }

  public void OnStatsNoHealth()
  {
    QueueFree();
  }
}
