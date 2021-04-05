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
            return sheet.Cells[1, 1, 1, sheet.Dimension.Columns].Select(item => item.Value.ToString()).ToArray();
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
                data.Add(sheet.Cells[r, 1, r, colCount].Select(item => item.Value?.ToString()).ToList());
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
            .Where(item => options.Any(opt => opt.ExcelField == item.Value.ToString()))
            .ToDictionary(item => item.Value.ToString(), item => item.End.Column);
            List<T> data = new();
            // 将excel 的数据转换为 对应实体F
            for (int r = 2; r <= rowCount; r++)
            {
                // 将单行数据转换为 表头:数据 的键值对
                var rowData = sheet.Cells[r, 1, r, colCount]
                .Where(item => header.Any(title => title.Value == item.End.Column))
                .Select(item => new KeyValuePair<string, string>(header.First(title => title.Value == item.End.Column).Key, item.Value?.ToString()))
                .ToDictionary(item => item.Key, item => item.Value);
                var obj = Activator.CreateInstance(typeof(T));
                // 根据对应传入的设置 为obj赋值
                foreach (var option in options)
                    option.Prop.SetValue(obj, option.Action == null ? rowData[option.ExcelField] : option.Action(rowData[option.ExcelField]));
                data.Add((T)obj);
            }
            return data;
        }
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

    public static class ExcelOptionAdd
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
    }

}
