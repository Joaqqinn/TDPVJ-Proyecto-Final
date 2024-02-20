using UnityEngine;

public class PlayerSlideState : PlayerAbilityState {
    public bool CanSlide { get; private set; }
    private bool isHolding;
    private bool slideInputStop;

    private float lastSlideTime;

    private Vector2 slideDirection;
    private Vector2 slideDirectionInput;
    private Vector2 lastAIPos;
    public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CanSlide = false;
        player.InputHandler.UseSlideInput();

        isHolding = true;
        slideDirection = Vector2.right * Movement.FacingDirection;

        Time.timeScale = playerData.slideHoldTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {

            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));


            if (isHolding)
            {
                slideDirectionInput = player.InputHandler.SlideDirectionInput;
                slideInputStop = player.InputHandler.SlideInputStop;

                if (slideDirectionInput != Vector2.zero)
                {
                    slideDirection = slideDirectionInput;
                    slideDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, slideDirection);

                if (slideInputStop || Time.unscaledTime >= startTime + playerData.slideMaxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    Movement?.CheckIfShouldFlip(Mathf.RoundToInt(slideDirection.x));
                    player.RB.drag = playerData.slideDrag;
                    Movement?.SetVelocity(playerData.slideVelocity, slideDirection);
                }
            }
            else
            {
                Movement?.SetVelocity(playerData.slideVelocity, slideDirection);

                if (Time.time >= startTime + playerData.slideTime)
                {
                    player.RB.drag = 0f;
                    isAbilityDone = true;
                    lastSlideTime = Time.time;
                }
            }
        }
    }
    public bool CheckIfCanSlide()
    {
        return CanSlide && Time.time >= lastSlideTime + playerData.slideCooldown;
    }

    public void ResetCanSlide() => CanSlide = true;
}
