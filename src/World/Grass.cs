using Godot;
using System;

public class Grass : Node2D
{
  private PackedScene _grassEffect;

  public override void _Ready()
  {
    _grassEffect = ResourceLoader.Load<PackedScene>("res://src/Effects/GrassEffect.tscn");
  }

  // Any area that overlaps with this Class will trigger this method
  public void OnHurtBoxAreaEntered(Area2D area)
  {
    CreateGrassEffect();
    QueueFree();
  }

  private void CreateGrassEffect()
  {
    Node2D grassEffectInstance = _grassEffect.Instance() as Node2D;
    grassEffectInstance.GlobalPosition = this.GlobalPosition;
    GetParent().AddChild(grassEffectInstance);
  }
}
