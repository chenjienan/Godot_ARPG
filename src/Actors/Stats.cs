using Godot;
using System;

public class Stats : Node
{
  [Signal] public delegate void NoHealth();
  private double _health;
  private double _maxHealth;

  [Export]
  public double MaxHealth
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

  public double Health
  {
    get
    {
      return _health;
    }

    set
    {
      // value is a placeholder for the value that is assigned to the property
      _health = value;
      if (_health <= 0)
      {
        EmitSignal("NoHealth");
      }
    }
  }
}
