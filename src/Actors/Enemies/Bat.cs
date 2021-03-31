using Godot;
using System;

public class Bat : KinematicBody2D
{
  [Export] public int Acceleration = 300;
  [Export] public int MaxSpeed = 50;
  [Export] public int Friction = 200;

  private PackedScene _enemyDeathEffect;

  private Vector2 _knockback = Vector2.Zero;
  private Vector2 _velocity = Vector2.Zero;

  private AnimatedSprite _animatedSprite;
  private PlayerDectionZone _playerDectionZone;
  private Stats _stats;
  private State _state = State.CHASE;

  public enum State
  {
    IDLE,
    WANDER,
    CHASE
  }

  public override void _Ready()
  {
    _enemyDeathEffect = ResourceLoader.Load<PackedScene>("res://src/Effects/EnemyDealthEffect.tscn");
    _playerDectionZone = GetNode<PlayerDectionZone>("PlayerDectionZone");
    _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    _stats = GetNode<Stats>("Stats");
    GD.Print($"Bat {this.Name} health: {_stats.Health}");
  }

  public override void _PhysicsProcess(float delta)
  {
    _knockback = _knockback.MoveToward(Vector2.Zero, delta * Friction);
    _knockback = MoveAndSlide(_knockback);

    switch (_state)
    {
      case State.IDLE:
        _velocity = _velocity.MoveToward(Vector2.Zero, Friction * delta);
        this.SeekPlayer();
        break;

      case State.WANDER:
        break;

      case State.CHASE:
        Player player = _playerDectionZone.Player;
        if (player != null)
        {
          // get the direction to the player
          // get the difference between the player and bat
          // scale to unit length with Normalized()
          Vector2 direction = (player.GlobalPosition - this.GlobalPosition).Normalized();
          _velocity = _velocity.MoveToward(direction * MaxSpeed, Acceleration * delta);
        }
        else
        {
          _state = State.IDLE;
        }

        // flip sprite
        _animatedSprite.FlipH = _velocity.x < 0;
        break;

      default:
        throw new Exception($"Illegal enum value {_state}");
    }

    // move the bat
    _velocity = MoveAndSlide(_velocity);
  }

  private void SeekPlayer()
  {
    if (_playerDectionZone.PlayerInZone())
    {
      _state = State.CHASE;
    }
  }

  public void OnHurtBoxAreaEntered(PlayerHitBox area)
  {
    _stats.Health -= area.Damage;
    _knockback = area.KnockBackVector * 100;
  }

  public void OnStatsNoHealth()
  {
    QueueFree();

    // Create an effect instance
    Node2D effectInstance = _enemyDeathEffect.Instance() as Node2D;
    effectInstance.GlobalPosition = this.GlobalPosition;
    GetParent().AddChild(effectInstance);
  }
}
