namespace Defra.Trade.Plants.SpecFlowBindings;

using System;

/// <summary>
/// A static class to store constants used in the specflow bindings project.
/// </summary>
public static class SpecflowBindingsConstants
{
    public const int ApiRetryAttempts = 12;

    public static TimeSpan WorkWorderWaitTime => TimeSpan.FromSeconds(90);

    public static TimeSpan GridWaitTime => TimeSpan.FromSeconds(30);

    public static TimeSpan DefaultWaitTime => TimeSpan.FromSeconds(90);

    public static int DefaultRetryCount => 30;

    public static int GridRetryInterval => 1;

    public static int DefaultRetryInterval => 5000;

    public static TimeSpan ApiSleepDuration => TimeSpan.FromSeconds(10);

    public static int ImportNotificationTaskCount = 4;

    public const string BaseUser = "a user";
    public const string WorkOrderAlias = "WorkOrder";
    public const int DefaultTaskCountOwnedByCITImportsTeam = 2;
}