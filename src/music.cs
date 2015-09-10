datablock AudioDescription(AudioRoyaleMusic : AudioClose3d)
{
    is3D = false;
    volume = 1;
};

datablock AudioProfile(RoyaleRoundStartingSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/chooseteam.ogg";
    description = AudioRoyaleMusic;
    preload = true;
};

datablock AudioProfile(RoyaleRoundStartedSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/startround_01.ogg";
    description = AudioRoyaleMusic;
    preload = false;
};

datablock AudioProfile(RoyaleRoundEndSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/wonround.ogg";
    description = AudioRoyaleMusic;
    preload = true;
};

datablock AudioProfile(RoyaleRoundWinSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/roundmvpanthem_01.ogg";
    description = AudioRoyaleMusic;
    preload = true;
};

datablock AudioProfile(RoyaleRoundDrawSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/lostround.ogg";
    description = AudioRoyaleMusic;
    preload = true;
};

datablock AudioProfile(RoyaleDeathcamSound)
{
    fileName = "Add-Ons/GameMode_Battle_Royale/assets/music/sasha_01/deathcam.ogg";
    description = AudioRoyaleMusic;
    preload = true;
};
