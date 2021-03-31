using Godot;
using System;

public class Effect : AnimatedSprite
{
  public override void _Ready()
  {
    Connect("animation_finished", this, "OnAnimatedSpriteAnimationFinished");
    Play("Animate");
  }

  public void OnAnimatedSpriteAnimationFinished()
  {
    QueueFree();
  }
}
