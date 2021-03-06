﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiaSyuanLibrary.Helper
{
    public static class  CsvFileHelper
    {
        /// <summary>
        /// CSV Generator
        /// </summary>
        /// <param name="FilePath">target CSV path</param>
        /// <param name="dataCollections">Data Collections</param>
        /// <param name="encoding">encoding</param>
        /// <param name="genColumn">output data property name</param>
        /// <param name="append"></param>
        public static void CSVGenerator<T>( string FilePath, IList<T> dataCollections, Encoding encoding, bool genColumn =true, bool append = false)
        {
            using (var file = new StreamWriter(FilePath, append, encoding))
            {
                Type type = typeof(T);
                PropertyInfo[] propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //是否要輸出屬性名稱
                if (genColumn)
                {
                    file.WriteLineAsync(string.Join(",", propInfos.Select(s => s.Name)));
                }
                if (dataCollections != null && dataCollections.Count > 0)
                {
                    foreach (var item in dataCollections)
                    {
                        file.WriteLineAsync(string.Join(",", propInfos.Select(s => s.GetValue(item)+"\t")));
                    }
                }
            }
        }
    }
}
