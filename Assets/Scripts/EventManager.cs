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

public static class CycleEventManager
{
    public static event EventHandler<NewTickEvent> OnTick;
    public static event EventHandler<PreTickEvent> OnPreTick;
    public static event EventHandler<NewCycleEvent> OnCycle;

    private const int TICKS_PER_CYCLE = 12;

    private static int _tickCounter;
    private static int _cycleCounter;

    public static int CurrentTick => _tickCounter;
    public static int CurrentCycle => _cycleCounter;

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
    }
    private static void PreTick()
    {

        OnPreTick?.Invoke(null, new PreTickEvent
        {
            CurrentTick = _tickCounter,
            CurrentCycle = _cycleCounter,
            TicksPerCycle = TICKS_PER_CYCLE
        });
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