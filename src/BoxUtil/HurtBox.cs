using Godot;

public class HurtBox : Area2D
{
  [Export] public bool IsEnabled;
  private PackedScene _hitEffect;

  public override void _Ready()
  {
    _hitEffect = ResourceLoader.Load<PackedScene>("res://src/Effects/HitEffect.tscn");
  }

  public void OnHurtBoxAreaEntered(Area2D area)
  {
    if (IsEnabled)
    {
      Node2D hitEffectInstance = _hitEffect.Instance() as Node2D;
      hitEffectInstance.GlobalPosition = this.GlobalPosition;

      Node2D world = GetTree().CurrentScene as Node2D;
      world.AddChild(hitEffectInstance);
    }


  }
}
