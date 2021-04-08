using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using OfficeOpenXml;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class ExcelOperation
    {
        private readonly ICollection<string> ExcelHeader;
        private readonly ICollection<ICollection<string>> ExcelData;
        public ExcelOperation() { }
        public ExcelOperation(Stream excelStream)
        {
            ExcelHeader = GetExcelHeader(excelStream);
            ExcelData = GetExcelData(excelStream);
        }
        /// <summary>
        /// 获取表格header(仅限简单的单行表头)
        /// </summary>
        public static ICollection<string> GetExcelHeader(Stream excelStream)
        {
            using ExcelPackage pack = new(excelStream);
            var sheet = pack.Workbook.Worksheets[1];
            return sheet.Cells[1, 1, 1, sheet.Dimension.Columns].Select(item => item.Value?.ToString().Trim()).ToArray();
        }

        /// <summary>
        /// 获取表格的数据(仅限简单的单行表头)
        /// </summary>
        public static ICollection<ICollection<string>> GetExcelData(Stream excelStream)
        {
            using ExcelPackage pack = new(excelStream);
            var sheet = pack.Workbook.Worksheets[1];
            int rowCount = sheet.Dimension.Rows, colCount = sheet.Dimension.Columns;
            List<ICollection<string>> data = new();
            for (int r = 2; r <= rowCount; r++)
                data.Add(sheet.Cells[r, 1, r, colCount].Select(item => item.Value?.ToString().Trim()).ToList());
            return data;
        }

        /// <summary>
        /// 将表格数据转换为指定的数据实体(实际上下面的方法高度重合,主要为了Init)
        /// </summary>
        public static ICollection<T> ExcelToEntity<T>(Stream excelStream, ExcelImportOption<T> options)
        {
            using ExcelPackage pack = new(excelStream);

            // 合并 FieldOption 和 DefaultOption
            var propOptions = options.FieldOption.Concat(options.DefaultOption);

            var sheet = pack.Workbook.Worksheets[1];
            int rowCount = sheet.Dimension.Rows, colCount = sheet.Dimension.Columns;
            // 获取对应设置的 表头 以及其 column
            var header = sheet.Cells[1, 1, 1, sheet.Dimension.Columns]
            .Where(item => propOptions.Any(opt => opt.ExcelField == item.Value?.ToString().Trim()))
            .ToDictionary(item => item.Value?.ToString().Trim(), item => item.End.Column);
            List<T> data = new();
            // 将excel 的数据转换为 对应实体
            for (int r = 2; r <= rowCount; r++)
            {
                // 将单行数据转换为 表头:数据 的键值对
                var rowData = sheet.Cells[r, 1, r, colCount]
                .Where(item => header.Any(title => title.Value == item.End.Column))
                .Select(item => new KeyValuePair<string, string>(header.First(title => title.Value == item.End.Column).Key, item.Value?.ToString().Trim()))
                .ToDictionary(item => item.Key, item => item.Value);
                var obj = Activator.CreateInstance(typeof(T));
                // 根据对应传入的设置 为obj赋值
                foreach (var option in propOptions)
                {
                    if (!string.IsNullOrEmpty(option.ExcelField))
                    {
                        var value = rowData.ContainsKey(option.ExcelField) ? rowData[option.ExcelField] : string.Empty;
                        if (!string.IsNullOrEmpty(value))
                            option.Prop.SetValue(obj, option.Action == null ? value : option.Action(value));
                    }
                    else
                        option.Prop.SetValue(obj, option.Action == null ? null : option.Action(string.Empty));
                }
                if (options.Init != null)
                    obj = options.Init((T)obj);
                data.Add((T)obj);

            }
            return data;
        }

        /// <summary>
        /// 将表格数据转换为指定的数据实体
        /// </summary>
        public static ICollection<T> ExcelToEntity<T>(Stream excelStream, ICollection<ExcelCellOption<T>> options)
        {
            using ExcelPackage pack = new(excelStream);
            var sheet = pack.Workbook.Worksheets[1];
            int rowCount = sheet.Dimension.Rows, colCount = sheet.Dimension.Columns;
            // 获取对应设置的 表头 以及其 column
            var header = sheet.Cells[1, 1, 1, sheet.Dimension.Columns]
            .Where(item => options.Any(opt => opt.ExcelField == item.Value?.ToString().Trim()))
            .ToDictionary(item => item.Value?.ToString().Trim(), item => item.End.Column);
            List<T> data = new();
            // 将excel 的数据转换为 对应实体
            for (int r = 2; r <= rowCount; r++)
            {
                // 将单行数据转换为 表头:数据 的键值对
                var rowData = sheet.Cells[r, 1, r, colCount]
                .Where(item => header.Any(title => title.Value == item.End.Column))
                .Select(item => new KeyValuePair<string, string>(header.First(title => title.Value == item.End.Column).Key, item.Value?.ToString().Trim()))
                .ToDictionary(item => item.Key, item => item.Value);
                var obj = Activator.CreateInstance(typeof(T));
                // 根据对应传入的设置 为obj赋值
                foreach (var option in options)
                {
                    if (!string.IsNullOrEmpty(option.ExcelField))
                    {
                        var value = rowData.ContainsKey(option.ExcelField) ? rowData[option.ExcelField] : string.Empty;
                        if (!string.IsNullOrEmpty(value))
                            option.Prop.SetValue(obj, option.Action == null ? value : option.Action(value));
                    }
                    else
                        option.Prop.SetValue(obj, option.Action == null ? null : option.Action(string.Empty));
                }
                data.Add((T)obj);

            }
            return data;
        }
    }

    public class ExcelImportOption<T>
    {
        // 依据表头的设置
        public ICollection<ExcelCellOption<T>> FieldOption { get; set; }
        // 一些默认的初始化设置
        public ICollection<ExcelCellOption<T>> DefaultOption { get; set; }
        // 随便你怎么搞的设置
        public Func<T, T> Init { get; set; }
    }

    public class ExcelCellOption<T>
    {
        /// <summary>
        /// 对应excel中的header字段
        /// </summary>
        public string ExcelField { get; set; }
        /// <summary>
        /// 对应字段的属性(实际上包含PropName)
        /// </summary>
        public PropertyInfo Prop { get; set; }
        /// <summary>
        /// 就是一个看起来比较方便的标识
        /// </summary>
        public string PropName { get; set; }
        /// <summary>
        /// 转换 表格 数据的方法
        /// </summary>
        public Func<string, object> Action { get; set; }
        public static ICollection<ExcelCellOption<T>> GenExcelOption<E>(string field, Expression<Func<T, E>> prop, Func<string, object> action = null)
        {
            var member = prop.GetMember();
            return new List<ExcelCellOption<T>>{
                new ExcelCellOption<T>
                {
                    PropName = member.Name,
                    Prop = (PropertyInfo)member,
                    ExcelField = field,
                    Action = action
                }
            };
        }
    }

    /// <summary>
    /// 一些大概会方便的扩展方法
    /// </summary>
    public static class ExcelOptionExt
    {
        public static ICollection<ExcelCellOption<T>> Add<T, E>(this ICollection<ExcelCellOption<T>> origin, string field, Expression<Func<T, E>> prop, Func<string, object> action = null)
        {
            var member = prop.GetMember();
            origin.Add(new ExcelCellOption<T>
            {
                PropName = member.Name,
                Prop = (PropertyInfo)member,
                ExcelField = field,
                Action = action
            });
            return origin;
        }

        public static ICollection<ExcelCellOption<T>> Default<T, E>(this ICollection<ExcelCellOption<T>> origin, Expression<Func<T, E>> prop, object defaultValue = null)
        {
            var member = prop.GetMember();
            origin.Add(new ExcelCellOption<T>
            {
                PropName = member.Name,
                Prop = (PropertyInfo)member,
                ExcelField = string.Empty,
                Action = item => defaultValue
            });
            return origin;
        }

        public static ExcelImportOption<T> Add<T, E>(this ExcelImportOption<T> origin, string field, Expression<Func<T, E>> prop, Func<string, object> action = null)
        {
            var member = prop.GetMember();
            if (origin.FieldOption == null) origin.FieldOption = new List<ExcelCellOption<T>>();
            origin.FieldOption.Add(new ExcelCellOption<T>
            {
                PropName = member.Name,
                Prop = (PropertyInfo)member,
                ExcelField = field,
                Action = action
            });
            return origin;
        }
        public static ExcelImportOption<T> Default<T, E>(this ExcelImportOption<T> origin, Expression<Func<T, E>> prop, object defaultValue = null)
        {
            var member = prop.GetMember();
            if (origin.DefaultOption == null) origin.DefaultOption = new List<ExcelCellOption<T>>();
            origin.DefaultOption.Add(new ExcelCellOption<T>
            {
                PropName = member.Name,
                Prop = (PropertyInfo)member,
                ExcelField = string.Empty,
                Action = item => defaultValue
            });
            return origin;
        }
        public static ExcelImportOption<T> AddInit<T>(this ExcelImportOption<T> origin, Func<T, T> action = null)
        {
            if (origin.Init == null) origin.Init = action;
            return origin;
        }
    }
}
