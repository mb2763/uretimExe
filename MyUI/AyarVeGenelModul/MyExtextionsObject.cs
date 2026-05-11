using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace MyUI
{
    public static class MyExtentionObject
    {
        public static void MesajHata(this string mesaj)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void MesajBilgi(this string mesaj)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static bool MesajSor(this string mesaj)
        {
            DialogResult kontrol = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return kontrol == DialogResult.Yes ? true : false;
        }
       
        public static bool IsNullOrEmpty(this Guid? @this)
        {
            if (@this == null)
            {
                return true;
            }
            else if (@this == Guid.Empty)
            {
                return true;
            }

            return false;
        }
        
        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, string adi) where T : class
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }
                dataTable.Rows.Add(values);
            }
            dataTable.TableName = adi;
            return dataTable;
        }
        public static DataTable ToDataTable<T>(this List<T> list, string adi) where T : class
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }
                dataTable.Rows.Add(values);
            }
            dataTable.TableName = adi;
            return dataTable;
        }
        public static DataTable ToDataTable<T>(this T entity, string adi) where T : class
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(entity);
            }
            dataTable.Rows.Add(values);
            dataTable.TableName = adi;
            return dataTable;
        }
    }
}