﻿namespace CSM.Commands
{
    /// <summary>
    ///     This class contains a list of command IDs used in this mod,
    ///     please create a new ID here when you create a new command to ensure
    ///     no overlap. Ids range from 0-255
    /// </summary>
    public class CommandIds
    {
        public const byte ConnectionRequestCommand = 0;
        public const byte ConnectionResultCommand = 1;
        public const byte ChatMessageCommand = 2;

        // 3 - 49

        public const byte ClientConnectCommand = 50;
        public const byte ClientDisconnectCommand = 51;
        public const byte PlayerListCommand = 52;
        public const byte WorldInfoCommand = 53;
        public const byte FinishTransactionCommand = 54;

        // 54 - 99

        public const byte SpeedCommand = 100;
        public const byte PauseCommand = 101;
        public const byte MoneyCommand = 102;
        public const byte BuildingCreateCommand = 103;
        public const byte BuildingRemoveCommand = 104;
        public const byte BuildingRelocateCommand = 105;
        public const byte DemandDisplayedCommand = 106;
        public const byte TaxRateChangeCommand = 107;
        public const byte BudgetChangeCommand = 108;
        public const byte NodeCreateCommand = 109;
        public const byte NodeUpdateCommand = 110;
        public const byte NodeReleaseCommand = 111;
        public const byte SegmentCreateCommand = 112;
        public const byte SegmentReleaseCommand = 113;
        public const byte ZoneUpdateCommand = 114;
        public const byte UnlockAreaCommand = 115;
        public const byte TreeCreateCommand = 116;
        public const byte TreeMoveCommand = 117;
        public const byte TreeReleaseCommand = 118;
        public const byte DistrictCreateCommand = 119;
        public const byte DistrictAreaModifyCommand = 120;
        public const byte DistrictReleaseCommand = 121;
        public const byte DistrictPolicyCommand = 122;
        public const byte DistrictCityPolicyCommand = 123;
        public const byte DistrictPolicyUnsetCommand = 124;
        public const byte DistrictCityPolicyUnsetCommand = 125;
        public const byte CityPolicyCommand = 126;
        public const byte TransportCreateLineCommand = 127;
        public const byte TransportReleaseLineCommand = 128;
        public const byte TransportLineAddStopCommand = 129;
        public const byte TransportLineRemoveStopCommand = 130;
        public const byte TransportLineMoveStopCommand = 131;

        // 116 - 255
    }
}
