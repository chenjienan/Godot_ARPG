using Godot;
using System;

public class Stats : Node
{
  [Signal] public delegate void NoHealth();
  [Signal] public delegate void HealthChanged(int value);

  private int _health;
  private int _maxHealth;

  [Export]
  public int MaxHealth
  {
    get
    {
      return _maxHealth;
    }

    set
    {
      if (value > 0)
      {
        _maxHealth = value;
        _health = value;
      }
    }
  }

  public int Health
  {
    get
    {
      return _health;
    }

    set
    {
      // value is a placeholder for the value that is assigned to the property
      _health = value;
      EmitSignal("HealthChanged", _health);
      if (_health <= 0)
      {
        EmitSignal("NoHealth");
      }
    }
  }
}
