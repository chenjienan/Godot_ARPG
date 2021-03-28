using Godot;
using System;

public class GrassEffect : Node2D
{
  private AnimatedSprite animatedSprite;

  public override void _Ready()
  {
    animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    animatedSprite.Play("Animate");
  }

  public void OnAnimatedSpriteAnimationFinished()
  {
    QueueFree();
  }
}
