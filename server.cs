$RoyaleMapX1 = -155;
$RoyaleMapX2 = 134;
$RoyaleMapY1 = -138;
$RoyaleMapY2 = 167;
$RoyaleMapCenter = ($RoyaleMapX1 + $RoyaleMapX2) / 2 SPC ($RoyaleMapY1 + $RoyaleMapY2) / 2;
$RoyaleMapRadius = mSqrt(mPow($RoyaleMapX2 - $RoyaleMapX1, 2) + mPow($RoyaleMapY2 - $RoyaleMapY1, 2)) / 2;

$RoyaleRoundTime = 300;

exec("./support/blendRGBA.cs");

exec("./src/player.cs");
exec("./src/game.cs");

function pingScoreTick()
{
    cancel($pingScoreTick);

    for (%i = 0; %i < ClientGroup.getCount(); %i++)
    {
        %client = ClientGroup.getObject(%i);
        %client.setScore(%client.getPing());
    }

    $pingScoreTick = schedule(500, 0, "pingScoreTick");
}

if (!isEventPending($pingScoreTick))
    pingScoreTick();
