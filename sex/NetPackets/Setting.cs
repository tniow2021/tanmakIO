namespace sex.NetPackets
{
    public static class Setting
    {
        public static void NetPacketFormTabling()
        {
            var table = Root.root.NetPacketMinimumLengthTable;

            table.Register(Vector3Int.GetMinimumLength(), Vector3Int.typeNumber);
        }
    }
}
