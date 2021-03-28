using Godot;
using System;

public class Grass : Node2D
{
  // Any area that overlaps with this Class will trigger this method
  public void OnHurtBoxAreaEntered(Area2D area)
  {
    CreateGrassEffect();
    QueueFree();
  }

  private void CreateGrassEffect()
  {
    PackedScene grassEffect = ResourceLoader.Load("res://src/Effects/GrassEffect.tscn") as PackedScene;
    Node2D grassEffectInstance = grassEffect.Instance() as Node2D;
    grassEffectInstance.GlobalPosition = this.GlobalPosition;

    Node2D world = GetTree().CurrentScene as Node2D;
    world.AddChild(grassEffectInstance);
  }
}
