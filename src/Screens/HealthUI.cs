using Godot;
using System;

public class HealthUI : Control
{
  private int _health = 4;
  private int _maxHealth = 4;
  private Stats _plyayerStats;
  private Label _label;

  [Export]
  public int Health
  {
    get
    {
      return _health;
    }
    set
    {
      _health = Mathf.Clamp(value, 0, _maxHealth);
    }
  }

  [Export]
  public int MaxHealth
  {
    get
    {
      return _maxHealth;
    }
    set
    {
      _maxHealth = Mathf.Max(value, 1);
    }
  }

  public void SetHealth(int value)
  {
    _health = Mathf.Clamp(value, 0, _maxHealth);
    if (_label != null)
    {
      _label.Text = $"HP = {_health}";
    }
  }

  public override void _Ready()
  {
    this._plyayerStats = GetNode<Stats>("/root/PlayerStats");
    this._label = GetNode<Label>("Label");
    this._health = _plyayerStats.Health;
    this._maxHealth = _plyayerStats.MaxHealth;
    _plyayerStats.Connect("HealthChanged", this, "SetHealth");
  }
}
