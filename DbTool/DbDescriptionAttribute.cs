using System;

namespace DbTool
{
    /// <summary>
    /// 列描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnDescriptionAttribute : Attribute
    {
        public ColumnDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }

    /// <summary>
    /// 表描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableDescriptionAttribute : Attribute
    {
        public TableDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }

    /// <summary>
    /// 数据库描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseDescriptionAttribute : Attribute
    {
        public DatabaseDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }
}