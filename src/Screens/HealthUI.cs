using Godot;
using System;

public class HealthUI : Control
{
  private int _health = 4;
  private int _maxHealth = 4;

  private Stats _plyayerStats;
  private TextureRect _heartEmpty;
  private TextureRect _heartFull;

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

      // Update heart(s) for max health
      if (_heartEmpty != null)
      {
        _heartEmpty.RectSize = new Vector2(
            (float)Health * 15,
          _heartEmpty.RectSize.y);
      }
    }
  }

  public void SetHealth(int value)
  {
    Health = value;

    // Update heart(s) for health
    if (_heartFull != null)
      _heartFull.RectSize = new Vector2(
          (float)Health * 15,
          _heartFull.RectSize.y);

  }

  public override void _Ready()
  {
    this._plyayerStats = GetNode<Stats>("/root/PlayerStats");
    this._heartEmpty = GetNode<TextureRect>("HeartEmpty");
    this._heartFull = GetNode<TextureRect>("HeartFull");

    this._health = _plyayerStats.Health;
    this._maxHealth = _plyayerStats.MaxHealth;
    _plyayerStats.Connect("HealthChanged", this, "SetHealth");
  }
}
