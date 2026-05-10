using System;
using UnityEngine;

public class NewTickEvent : EventArgs
{
    public int CurrentTick { get; set; }
    public int CurrentCycle { get; set; }
    public int TicksPerCycle { get; set; }
}
public class PreTickEvent : EventArgs
{
    public int CurrentTick { get; set; }
    public int CurrentCycle { get; set; }
    public int TicksPerCycle { get; set; }
}
public class NewCycleEvent : EventArgs
{
    public int CurrentCycle { get; set; }
    public int TicksPerCycle { get; set; }
}
public class CarrierMoveTick : EventArgs
{

}
public class FightTickEvent : EventArgs
{

}
public class UpdateTick : EventArgs
{

}

public static class CycleEventManager
{
    //Order of events
    public static event EventHandler<PreTickEvent> OnPreTick;
    public static event EventHandler<NewTickEvent> OnTick;
    public static event EventHandler<CarrierMoveTick> CarrierMoveTick;
    public static event EventHandler<FightTickEvent> FightTick;
    public static event EventHandler<UpdateTick> UpdateTick;

    public static event EventHandler<NewCycleEvent> OnCycle;

    private static int TICKS_PER_CYCLE = GameInformation.cycleLength;

    private static int _tickCounter;
    private static int _cycleCounter;

    public static int CurrentTick => _tickCounter;
    public static int CurrentCycle => _cycleCounter;

    private static void PreTick()
    {

        OnPreTick?.Invoke(null, new PreTickEvent
        {
            CurrentTick = _tickCounter,
            CurrentCycle = _cycleCounter,
            TicksPerCycle = TICKS_PER_CYCLE
        });
    }


    public static void NewTick()
    {
        PreTick();
        _tickCounter++;

        if (_tickCounter % TICKS_PER_CYCLE == 0)
        {
            NewCycle();
        }

        
        OnTick?.Invoke(null, new NewTickEvent
        {
            CurrentTick = _tickCounter,
            CurrentCycle = _cycleCounter,
            TicksPerCycle = TICKS_PER_CYCLE
        });

        MoveTick();

        StartFightTick();

        UpdateUI();
    }


    private static void MoveTick()
    {

        CarrierMoveTick?.Invoke(null, new CarrierMoveTick { });
    }
    private static void StartFightTick()
    {
        FightTick?.Invoke(null, new FightTickEvent { });
    }

    private static void UpdateUI()
    {
        UpdateTick?.Invoke(null, new UpdateTick { });
    }
    public static void NewCycle()
    {
        _cycleCounter++;
        OnCycle?.Invoke(null, new NewCycleEvent
        {
            CurrentCycle = _cycleCounter,
            TicksPerCycle = TICKS_PER_CYCLE
        });
    }
}