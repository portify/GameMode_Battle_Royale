$MaxRoyaleStartMessage = 0;
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Dance!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Fight!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Go get 'em'!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Read a book!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Let the games begin!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Play hopscotch!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Last man standing!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Disprove general relativity!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Go!";
$RoyaleStartMessage[$RoyaleStartMessageMax++] = "Play!";

function MiniGameSO::battleRoyaleStartMessage(%this, %step)
{
    cancel(%this.battleRoyaleSchedule);

    %text = "<font:palatino linotype:48><color:aaaaaa>Welcome to\n";
    %time = 1;

    if (%step >= 1)
    {
        %text = %text @ "<font:impact:96><color:ff6666>BATTLE ROYALE\n";
        %time = 2;
    }

    if (%step >= 2)
    {
        %text = %text @ "<font:palatino linotype:32>\n\c6The round will start in 20 seconds";
        %time = 4;
    }

    %this.centerPrintAll(%text, %time + 1);

    if (%step >= 2)
        %this.battleRoyaleSchedule = %this.schedule(20000, "battleRoyaleStartCountdown", 5);
    else
        %this.battleRoyaleSchedule = %this.schedule(%time * 1000, "battleRoyaleStartMessage", %step + 1);
}

function MiniGameSO::battleRoyaleStartCountdown(%this, %n)
{
    cancel(%this.battleRoyaleSchedule);

    if (%n == 0)
    {
        %this.battleRoyaleProhibit = false;
        %this.battleRoyaleGas = $Sim::Time;

        %this.centerPrintAll("<font:palatino linotype:64><color:FFAAAA>" @ $RoyaleStartMessage[getRandom(1, $MaxRoyaleStartMessage)], 2);

        for (%i = 0; %i < %this.numMembers; %i++)
        {
            %client = %this.member[%i];
            %player = %client.player;

            if (isObject(%player))
                %player.giveBattleRoyaleLoadout();

            // for (%j = 0; %j < 5; %j++)
            // {
            //     %player.tool[%j] = HEGrenadeItem.getID();
            //     messageClient(%client, 'MsgItemPickup', '', %j, HEGrenadeItem.getID(), %j == 0);
            // }

            // %player.setTool(0, M24RifleItem);
            // %player.setTool(1, MagazineItem_M24A1);
            // %player.setTool(2, MagazineItem_M24A1);

            // %player.setTool(0, Colt1911Item);
            // %player.setTool(1, MagazineItem_45ACP_x7);
            // %player.setTool(2, MagazineItem_45ACP_x7);
        }

        return;
    }

    %this.centerPrintAll("<font:impact:" @ (64 + 16 * (5 - %n)) @ ">\c6" @ %n @ "\n", 2);
    %this.battleRoyaleSchedule = %this.schedule(1000, "battleRoyaleStartCountdown", %n - 1);
}

function Player::giveBattleRoyaleLoadout(%this)
{
    %variant = getRandom(1, 7);

    switch (%variant)
    {
        case 1:
            %this.setTool(0, Colt1911Item);
            %mags = getRandom(1, 4);
            for (%i = 0; %i < %mags; %i++)
                %this.setTool(%i + 1, MagazineItem_45ACP_x7);

        case 2:
            %this.setTool(0, RevolverItem);

        case 3:
            %this.setTool(0, Remington870Item);

        case 4:
            %this.setTool(0, M1GarandItem);
            %mags = getRandom(1, 3);
            for (%i = 0; %i < %mags; %i++)
                %this.setTool(%i + 1, MagazineItem_3006_x8);

        case 5:
            %this.setTool(0, M24RifleItem);
            %mags = getRandom(1, 2);
            for (%i = 0; %i < %mags; %i++)
                %this.setTool(%i + 1, MagazineItem_M24A1);

        case 6:
            %this.setTool(0, ThompsonItem);
            %mags = getRandom(1, 3);
            for (%i = 0; %i < %mags; %i++)
                %this.setTool(%i + 1, MagazineItem_45ACP_x20_SMG);

        case 7:
            %count = getRandom(1, 3);
            for (%i = 0; %i < %count; %i++)
                %this.setTool(%i, HEGrenadeItem);
    }
}

function Player::setTool(%this, %slot, %data)
{
    %this.tool[%slot] = %data.getID();

    if (isObject(%this.itemProps[%slot]))
        %this.itemProps[%slot].delete();

    if (isObject(%this.client))
    {
        messageClient(%this.client, 'MsgItemPickup', '', %slot, %data.getID());
    }
}

$MaxSpawnableItem = 0;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_45ACP_x7;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_M24A1;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_45ACP_x20_SMG;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_3006_x8;
// $SpawnableItem[$MaxSpawnableItem++] = MagazineItem_MicroUzi;
$SpawnableItem[$MaxSpawnableItem++] = HEGrenadeItem;
$SpawnableItem[$MaxSpawnableItem++] = Colt1911Item;
$SpawnableItem[$MaxSpawnableItem++] = ColtWalkerItem;
$SpawnableItem[$MaxSpawnableItem++] = ThompsonItem;
$SpawnableItem[$MaxSpawnableItem++] = M1GarandItem;
$SpawnableItem[$MaxSpawnableItem++] = M24RifleItem;
$SpawnableItem[$MaxSpawnableItem++] = Remington870Item;

package BattleRoyaleGame
{
    function MiniGameSO::reset(%this, %client)
    {
        if (%this.owner != 0)
            return Parent::reset(%this, %client);

        if (!isObject(%this.battleRoyaleCleanup))
            %this.battleRoyaleCleanup = new SimSet();

        %this.battleRoyaleCleanup.deleteAll();
        %this.battleRoyaleProhibit = true;
        %this.battleRoyaleGas = "";
        cancel(%this.battleRoyaleSchedule);

        %brickGroupCount = MainBrickGroup.getCount();
        %spawnNTName = "_royale_item_spawn";
        %spawnCount = 0;

        for (%i = 0; %i < %brickGroupCount; %i++)
        {
            %brickGroup = MainBrickGroup.getObject(%i);
            %brickCount = %brickGroup.NTObjectCount[%spawnNTName];

            for (%j = 0; %j < %brickCount; %j++)
            {
                %brick = %brickGroup.NTObject[%spawnNTName, %j];

                if (getRandom() > 0.82)
                {
                    %brick.setItem(0);
                    continue;
                }

                %brick.setItem($SpawnableItem[getRandom(1, $MaxSpawnableItem)]);
                %brick.item.static = false;
                %spawnCount++;
            }
        }

        Parent::reset(%this, %client);
        %this.battleRoyaleSchedule = %this.schedule(1500, "battleRoyaleStartMessage", 0);
    }

    function GameConnection::spawnPlayer(%this, %a, %b, %c)
    {
        Parent::spawnPlayer(%this, %a, %b, %c);

        if (isObject(%this.player) && isObject(%this.miniGame) && %this.miniGame.owner == 0)
        {
            %this.player.setShapeNameDistance(15);
        }
    }

    function Armor::onCollision(%this, %obj, %col, %a, %b, %c)
    {
        if (%obj.client.miniGame.battleRoyaleProhibit && %col.getClassName() $= "Item")
            return;

        Parent::onCollision(%this, %obj, %col, %a, %b, %c);
    }

    function Armor::damage(%this, %obj, %source, %position, %damage, %damageType)
    {
        if (%obj.client.miniGame.battleRoyaleProhibit)
            return;

        return Parent::damage(%this, %obj, %source, %position, %damage, %damageType);
    }

    function serverCmdDropTool(%client, %slot)
    {
        $DropToolMiniGame = %client.miniGame;
        Parent::serverCmdDropTool(%client, %slot);
        $DropToolMiniGame = "";
    }

    function ItemData::onAdd(%this, %obj)
    {
        if (isObject($DropToolMiniGame))
            %obj.miniGame = $DropToolMiniGame;

        Parent::onAdd(%this, %obj);
    }

    function Item::schedulePop(%this)
    {
        if (isObject(%this.miniGame.battleRoyaleCleanup))
        {
            %this.miniGame.battleRoyaleCleanup.add(%this);
            return;
        }

        Parent::schedulePop(%this);
    }

    function Armor::onDisabled(%this, %obj, %state)
    {
        if (isObject(%obj.client.miniGame.battleRoyaleCleanup))
        {
            %obj.client.miniGame.battleRoyaleCleanup.add(%obj);
            %obj.isBattleRoyaleBody = true;
        }

        Parent::onDisabled(%this, %obj, %state);
    }

    function Player::removeBody(%this)
    {
        if (!%this.isBattleRoyaleBody)
            Parent::removeBody(%this);
    }
};

activatePackage("BattleRoyaleGame");
