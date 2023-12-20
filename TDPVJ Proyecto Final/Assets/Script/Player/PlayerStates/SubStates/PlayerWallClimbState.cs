using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    //////////////////////////////////////////////////////////////
    ///


    // NO SE USA ESTE SCRIPT A MENOS QUE PONGAMOS ANIMACION DE SUBIR PAREDES
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Debug.Log("CLIMB");

        if (!isExitingState)
        {
            core.Movement.SetVelocityY(playerData.wallClimbVelocity);

            if (yInput != 1)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }

        
    }
}
