using Godot;

public class HurtBox : Area2D
{
  [Signal] public delegate void InvincibilityEnabled();
  [Signal] public delegate void InvincibilityDisabled();

  private PackedScene _hitEffect;
  private Timer _timer;

  private bool _isInvincible = false;

  public bool IsInvincible
  {
    get
    {
      return _isInvincible;
    }

    set
    {
      _isInvincible = value;

      // Send out the signal for other node(s)
      if (_isInvincible)
      {
        EmitSignal("InvincibilityEnabled");
      }
      else
      {
        EmitSignal("InvincibilityDisabled");
      }
    }
  }

  public override void _Ready()
  {
    _hitEffect = ResourceLoader.Load<PackedScene>("res://src/Effects/HitEffect.tscn");
    _timer = GetNode<Timer>("Timer");
  }

  public void StartInvincibility(float duration)
  {
    this.IsInvincible = true;

    // will trigger timeout event after "duration"
    _timer.Start(duration);
  }

  public void CreateHitEffect()
  {
    Node2D hitEffectInstance = _hitEffect.Instance() as Node2D;
    hitEffectInstance.GlobalPosition = this.GlobalPosition;

    Node2D world = GetTree().CurrentScene as Node2D;
    world.AddChild(hitEffectInstance);
  }

  // listener
  public void OnTimerTimeout()
  {
    this.IsInvincible = false;
  }

  // listener
  public void OnInvincibilityEnabled()
  {
    SetDeferred("monitorable", false);
  }

  // listener
  public void OnInvincibilityDisabled()
  {
    Monitorable = true;
  }
}
