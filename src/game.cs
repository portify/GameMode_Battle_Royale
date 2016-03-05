$MaxRoyaleStartMessage = 0;
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Dance!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Fight!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Go get 'em'!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Read a book!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Let the games begin!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Play hopscotch!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Last man standing!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Disprove general relativity!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Go!";
$RoyaleStartMessage[$MaxRoyaleStartMessage++] = "Play!";

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
		%this.play2D(RoyaleRoundStartedSound);

		%this.battleRoyaleProhibit = false;
		%this.battleRoyaleGas = $Sim::Time;

		%this.centerPrintAll("<font:palatino linotype:64><color:FFAAAA>" @ $RoyaleStartMessage[getRandom(1, $MaxRoyaleStartMessage)], 2);

		for (%i = 0; %i < %this.numMembers; %i++)
		{
			%client = %this.member[%i];
			%player = %client.player;

			if (isObject(%player))
				%player.giveBattleRoyaleLoadout();
		}

		return;
	}

	%this.centerPrintAll("<font:impact:" @ (64 + 16 * (5 - %n)) @ ">\c6" @ %n @ "\n", 2);
	%this.battleRoyaleSchedule = %this.schedule(1000, "battleRoyaleStartCountdown", %n - 1);
}

function Player::giveBattleRoyaleLoadout(%this)
{
	%variant = getRandom(1, 8);

	switch (%variant)
	{
		case 1:
			%this.setTool(0, Colt1911Item);
			%mags = getRandom(1, 4);
			for (%i = 0; %i < %mags; %i++)
				%this.setTool(%i + 1, MagazineItem_45ACP_x7);

		case 2:
			%this.setTool(0, RevolverItem);
			%this.addBullets(Bullet357Item, getRandom(12,24));

		case 3:
			%this.setTool(0, Remington870Item);
			%this.addBullets(BulletBuckshotItem, getRandom(12,24));

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
			%this.setTool(0, MicroUziItem);
			%mags = getRandom(1, 3);
			for (%i = 0; %i < %mags; %i++)
				%this.setTool(%i + 1, MagazineItem_MicroUzi);

		case 8:
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
$SpawnableItem[$MaxSpawnableItem++] = 0; // no item
$SpawnableChance[$MaxSpawnableItem] = 30;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_45ACP_x7;
$SpawnableChance[$MaxSpawnableItem] = 12;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_M24A1;
$SpawnableChance[$MaxSpawnableItem] = 12;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_45ACP_x20_SMG;
$SpawnableChance[$MaxSpawnableItem] = 12;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_3006_x8;
$SpawnableChance[$MaxSpawnableItem] = 12;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_MicroUzi;
$SpawnableChance[$MaxSpawnableItem] = 12;
$SpawnableItem[$MaxSpawnableItem++] = HEGrenadeItem;
$SpawnableChance[$MaxSpawnableItem] = 8;
$SpawnableItem[$MaxSpawnableItem++] = MagazineItem_MicroUziExtended;
$SpawnableChance[$MaxSpawnableItem] = 8;
$SpawnableItem[$MaxSpawnableItem++] = Colt1911Item;
$SpawnableChance[$MaxSpawnableItem] = 9;
$SpawnableItem[$MaxSpawnableItem++] = RevolverItem;
$SpawnableChance[$MaxSpawnableItem] = 9;
$SpawnableItem[$MaxSpawnableItem++] = ColtWalkerItem;
$SpawnableChance[$MaxSpawnableItem] = 5;
$SpawnableItem[$MaxSpawnableItem++] = ThompsonItem;
$SpawnableChance[$MaxSpawnableItem] = 8;
$SpawnableItem[$MaxSpawnableItem++] = M1GarandItem;
$SpawnableChance[$MaxSpawnableItem] = 7;
$SpawnableItem[$MaxSpawnableItem++] = M24RifleItem;
$SpawnableChance[$MaxSpawnableItem] = 6;
$SpawnableItem[$MaxSpawnableItem++] = Remington870Item;
$SpawnableChance[$MaxSpawnableItem] = 8;
$SpawnableItem[$MaxSpawnableItem++] = MicroUziItem;
$SpawnableChance[$MaxSpawnableItem] = 9;
// $SpawnableItem[$MaxSpawnableItem++] = Bullet357Item;
// $SpawnableIsAmmo[$MaxSpawnableItem] = true;
// $SpawnableChance[$MaxSpawnableItem] = 12;
// $SpawnableItem[$MaxSpawnableItem++] = BulletBuckshotItem;
// $SpawnableIsAmmo[$MaxSpawnableItem] = true;
// $SpawnableChance[$MaxSpawnableItem] = 12;

function MiniGameSO::play2D(%this, %profile)
{
	for (%i = 0; %i < %this.numMembers; %i++)
		%this.member[%i].play2D(%profile);
}

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

		%spawnNTName = "_royale_item_spawn";
		%spawnChanceMax = 0;

		for (%i = 1; %i <= $MaxSpawnableItem; %i++)
		{
			%spawnChanceMax += $SpawnableChance[%i];
			// talk("spawnChanceInk[" @ $SpawnableItem[%i].getName() @ "] = " @ %spawnChanceMax);
			%spawnChanceInc[%i] = %spawnChanceMax;
		}

		// talk("spawnChanceMax = " @ %spawnChanceMax);

		%brickGroupCount = MainBrickGroup.getCount();

		for (%i = 0; %i < %brickGroupCount; %i++)
		{
			%brickGroup = MainBrickGroup.getObject(%i);
			%brickCount = %brickGroup.NTObjectCount[%spawnNTName];

			for (%j = 0; %j < %brickCount; %j++)
			{
				%brick = %brickGroup.NTObject[%spawnNTName, %j];
				%chance = getRandom() * %spawnChanceMax;

				for (%k = 1; %k <= $MaxSpawnableItem; %k++)
				{
					if (%chance < %spawnChanceInc[%k])
					{
						// if ($SpawnableIsAmmo[%k])
						// {
						// 	%max = getRandom(2,4);
						// 	for (%i=1;%i<=%max;%i++)
						// 	{
						// 		%item = new Item()
						// 		{
						// 			dataBlock = $SpawnableItem[%k];

						// 			position = %brick.getPosition();
						// 		};
						// 		%spread = 15;
						// 		%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
						// 		%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

						// 		%vector = "0 0 10";
						// 		%matrix = matrixCreateFromEuler(%spread);
						// 		%vel = matrixMulVector(%matrix, %vector);
						// 		%item.setVelocity(%vel);
						// 		%position = getWords(%item.getTransform(), 0, 2);
						// 		%item.setTransform(%position SPC eulerToAxis("0 0" SPC getRandom() * 360 - 180));
						// 	}
						// }
						// else
						// {
						%brick.setItem($SpawnableItem[%k]);
						%brick.item.static = false;
						// }
						break;
					}
				}
			}
		}

		Parent::reset(%this, %client);

		%this.play2D(RoyaleRoundStartingSound);
		%this.battleRoyaleSchedule = %this.schedule(1500, "battleRoyaleStartMessage", 0);
	}

	function MiniGameSO::checkLastManStanding(%this)
	{
		if (%this.owner != 0)
			return Parent::checkLastManStanding(%this);

		%alive = 0;
		%last = 0;

		for (%i = 0; %i < %this.numMembers; %i++)
		{
			%client = %this.member[%i];
			%player = %client.player;

			if (isObject(%player))
			{
				%alive++;
				%last = %client;
			}
		}

		if (%alive == 0)
		{
			%this.play2D(RoyaleRoundDrawSound);

			cancel(%this.battleRoyaleSchedule);
			%this.chatMessageAll('', "\c5It's a draw. Everyone is dead.");
			%this.scheduleReset(6000);
		}
		else if (%alive == 1 && !%this.battleRoyaleProhibit)
		{
			for (%i = 0; %i < %this.numMembers; %i++)
			{
				%client = %this.member[%i];

				if (%client != %last)
					%client.play2D(RoyaleRoundEndSound);
			}

			%last.play2D(RoyaleRoundWinSound);

			cancel(%this.battleRoyaleSchedule);
			%this.chatMessageAll('', "\c3" @ %last.getPlayerName() @ " \c5is the last man standing! They win the round.");
			%this.scheduleReset(8000);
		}

		return 0;
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
		if (%this.canPickup !$= "")
			%item.canPickup = %this.canPickup;

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
