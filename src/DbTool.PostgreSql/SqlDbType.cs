namespace DbTool.DbProvider.PostgreSql
{
    internal enum SqlDbType
    {
        Int2,
        SmallInt,
        Int4,
        Integer,
        BigInt,
        Int8,
        Decimal,
        Numeric,
        Real,
        Float4,
        Float8,
        Double,
        Serial2,
        SmallSerial,
        Serial4,
        Serial,
        Serial8,
        BigSerial,

        Money,

        Character,
        VarChar,
        Char,
        Text,

        Boolean,
        Bool,
        ByteA,

        Date,
        Time,
        TimeZ,
        Timestamp,
        TimestampZ,

        Uuid,
    }
}
