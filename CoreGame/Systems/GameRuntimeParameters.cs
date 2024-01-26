using System;

public static class GameRuntimeParameters
{
    public static float GossipSpread;
    
    public static int GameDay;

    public static void ResetGameParameters()
    {
        GameDay = 0;
        GossipSpread = 0;
    }
}
