using System;

public static class GameRuntimeParameters
{
    public static float BurstCollateralSpread;
    public static float MaxTolerableSpread;
    public static float GossipSpread;

    public static float ColorFailSpread;
    public static float GreyFailSpread;
    
    public static int GameDay;

    public static void ResetGameParameters()
    {
        GameDay = 0;
        GossipSpread = 0;
    }
}
