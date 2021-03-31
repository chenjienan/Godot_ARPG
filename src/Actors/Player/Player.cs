using Godot;
using System;

public class Player : KinematicBody2D
{

  [Export] public int MaxSpeed { get; set; } = 100;
  [Export] public int RollSpeed { get; set; } = 110;
  [Export] public int Acceleration { get; set; } = 400;
  [Export] public int Friction { get; set; } = 400;

  private Vector2 velocity = Vector2.Zero;
  private Vector2 rollVector = Vector2.Down;

  private AnimationPlayer _animationPlayer;
  private AnimationTree _animationTree;
  private AnimationNodeStateMachinePlayback _animationState;

  private PlayerHitBox _playerHitBox;
  private State _state = State.MOVE;

  public enum State
  {
    MOVE,
    ROLL,
    ATTACK
  }

  public override void _Ready()
  {
    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    _animationTree = GetNode<AnimationTree>("AnimationTree");

    // locate the path from root
    // Note: can use "copy node path": HitboxPivot/Hitbox
    _playerHitBox = GetNode<PlayerHitBox>("HitboxPivot/Hitbox");
    _playerHitBox.KnockBackVector = rollVector;  // same direction as rolling

    // Activate animation tree
    _animationTree.Active = true;
    _animationState = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
  }

  public override void _PhysicsProcess(float delta)
  {
    switch (_state)
    {
      case State.MOVE:
        MoveState(delta);
        break;

      case State.ATTACK:
        AttackState(delta);
        break;

      case State.ROLL:
        RollState(delta);
        break;

      default:
        throw new Exception($"Illegal enum value {_state}");
    }
  }

  private void MoveState(float delta)
  {
    Vector2 inputVector = Vector2.Zero;
    inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
    inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

    // Normalized: clips a vector in a circle
    inputVector = inputVector.Normalized();

    if (inputVector != Vector2.Zero)
    {
      // Set roll direction
      rollVector = inputVector;
      // Set knowback direction
      _playerHitBox.KnockBackVector = rollVector;

      // Manage animation: set blend position
      _animationTree.Set("parameters/Idle/blend_position", inputVector);
      _animationTree.Set("parameters/Run/blend_position", inputVector);
      _animationTree.Set("parameters/Attack/blend_position", inputVector);
      _animationTree.Set("parameters/Roll/blend_position", inputVector);

      // Run state
      _animationState.Travel("Run");
      velocity = velocity.MoveToward(inputVector * MaxSpeed, Acceleration * delta);
    }
    else
    {
      // Idle state
      _animationState.Travel("Idle");
      velocity = velocity.MoveToward(Vector2.Zero, Friction * delta);
    }

    velocity = MoveAndSlide(velocity);

    // Transit to attack state
    if (Input.IsActionJustPressed("attack"))
    {
      _state = State.ATTACK;
    }

    // Transit to roll state
    if (Input.IsActionJustPressed("roll"))
    {
      _state = State.ROLL;
    }
  }

  private void RollState(float delta)
  {
    velocity = rollVector * RollSpeed;
    _animationState.Travel("Roll");
    velocity = MoveAndSlide(velocity);
  }

  private void AttackState(float delta)
  {
    velocity = Vector2.Zero;
    _animationState.Travel("Attack");
  }


  // listener method
  public void AttackAnimationFinished()
  {
    _state = State.MOVE;
  }

  // listener method
  public void RollAnimationFinished()
  {
    velocity = velocity * (float)0.8;
    _state = State.MOVE;
  }
}