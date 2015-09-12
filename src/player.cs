datablock PlayerData(BattleRoyalePlayer : PlayerStandardArmor)
{
    uiName = "Battle Royale Player";

    canJet = false;
    airControl = 0.2;
    mass = 120;

    maxForwardSpeed = 7;
	maxSideSpeed = 6;
	maxBackwardSpeed = 4;

	maxForwardCrouchSpeed = 3;
	maxSideCrouchSpeed = 3;
	maxBackwardCrouchSpeed = 2;
};

function BattleRoyalePlayer::onNewDataBlock(%this, %player)
{
    Parent::onNewDataBlock(%this, %player);

    if (!isEventPending(%player.battleRoyaleTick))
        %player.battleRoyaleTick();
}

function BattleRoyalePlayer::onImpact(%this, %player, %other, %velocity, %speed)
{
    %player.damage(0, %player.getHackPosition(), (%speed - 20) * 1.75, $DamageType::Fall);
}

function BattleRoyalePlayer::onTrigger(%this, %player, %slot, %state)
{
    Parent::onTrigger(%this, %player, %slot, %state);
}

function BattleRoyalePlayer::onDamage(%this, %obj, %a, %b)
{
    Parent::onDamage(%this, %obj, %a, %b);
}

function BattleRoyalePlayer::onDisabled(%this, %obj, %state)
{
    if (isObject(%obj.client))
    {
        for (%i = 0; %i < %this.maxTools; %i++)
            serverCmdDropTool(%obj.client, %i);

        // talk("onDisabled with client");
        commandToClient(%obj.client, 'SetVignette', false, "0 0 0 0");
    }

    Parent::onDisabled(%this, %obj, %state);
}

function BattleRoyalePlayer::damage(%this, %obj, %source, %position, %damage, %damageType)
{
    Parent::damage(%this, %obj, %source, %position, %damage, %damageType);
}

function Player::battleRoyaleTick(%this)
{
    if (%this.getState() $= "Dead" || %this.getDataBlock() != BattleRoyalePlayer.getID())
        return;

    %client = %this.client;
    %miniGame = %client.miniGame;

    %vignetteColor = "0 0 0 0.25";
    %vignetteColor = blendRGBA(%vignetteColor, "1 0 0" SPC %this.getDamagePercent());
    %vignetteMultiply = false;

    if (isObject(%miniGame) && !%miniGame.battleRoyaleProhibit)
    {
        %gasAlpha = 0;

        if (%miniGame.battleRoyaleGas !$= "")
        {
            %timePassed = $Sim::Time - %miniGame.battleRoyaleGas;
            %timeFactor = %timePassed / $RoyaleRoundTime;
            %targetFactor = 1 - %timeFactor;

            %distance = vectorDist(setWord(%this.getPosition(), 2, 0), $RoyaleMapCenter);
            %playerFactor = %distance / $RoyaleMapRadius;

            %gasAlpha = 1 - mClampF((%targetFactor - %playerFactor) / 0.1, 0, 1);

            if (%playerFactor >= %targetFactor)
            {
                // %client.bottomPrint("\c6you are being gassed! rip!", 1);

                if ($Sim::Time - %this.lastGasTime > 0.25)
                {
                    %this.lastGasTime = $Sim::Time;
                    %this.damage(%this, %this.getHackPosition(), 2, $DamageType::Suicide);
                }

                %vignetteMultiply = ($Sim::Time - mFloor($Sim::Time)) >= 0.5;
            }
            else
            {
                // if (%targetFactor - %playerFactor < 0.1)
                //     %client.bottomPrint("\c6you are almost being gassed! move closer to the center" @ " | gasAlpha = " @ %gasAlpha, 1);
                // else
                //     %client.bottomPrint("\c6distance to gas: " @ %targetFactor - %playerFactor @ " | gasAlpha = " @ %gasAlpha, 1);
            }
        }

        if (%gasAlpha > 0)
            %vignetteColor = blendRGBA(%vignetteColor, "0 1 0" SPC %gasAlpha);
    }

    if (isObject(%client))
        commandToClient(%client, 'SetVignette', %vignetteMultiply, %vignetteColor);

    %this.battleRoyaleTick = %this.scheduleNoQuota(50, "battleRoyaleTick");
}
