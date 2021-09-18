using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DartsGames
{
    public static class CoreExtensions
    {
        public static int RandomIndex<T>(this IList<T> coll) => UnityEngine.Random.Range(0, coll.Count);
        public static T RandomElement<T>(this IList<T> coll) => coll[coll.RandomIndex()];
        public static T LastOf<T>(this IList<T> coll) => coll[coll.Count - 1];

        public static bool IsNullOrEmpty<T>(this IList<T> coll) => coll == null || coll.Count == 0;

        public static T[] Add<T>(this T[] arr, T element)
        {
            var newArr = new T[arr.Length + 1];

            for (int i = 0; i < arr.Length; i++)
                newArr[i] = arr[i];

            newArr[arr.Length] = element;
            return newArr;
        }

        public static T FromEnd<T>(this IList<T> coll, int i) => coll[coll.Count - 1 - i];

        public static T DeepClone<T>(this T original) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("Object is not serializable!");
            }

            if (object.ReferenceEquals(original, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as T;
            }
        }

        #region Reflection
        /// <summary>
        /// Useful to get private members in anchestors up the hierarcy because they are invisible to normal reflection
        /// The default binding flags are found in DartsGames.Utils.ReflectionUtils.GetStandardFlags
        /// </summary>
        public static List<MethodInfo> GetAllMethodsInAncestors(this Type t, List<Type> stopCheckTypes = null)
        {
            var methods = new List<MethodInfo>();
            var types = new List<Type>();

            while (t != null && (stopCheckTypes == default || !stopCheckTypes.Contains(t)))
            {
                types.Add(t);
                t = t.BaseType;
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                var range = types[i].GetMethods(Utils.ReflectionUtils.GetStandardFlags());

                foreach (var m in range)
                {
                    var added = false;

                    for (int x = 0; x < methods.Count; x++)
                    {
                        if (methods[x].Name == m.Name)
                        {
                            methods[x] = m;
                            added = true;
                            break;
                        }
                    }

                    if (!added)
                        methods.Add(m);
                }
            }

            return methods;
        }

        public static List<FieldInfo> GetAllFieldsInAncestors(this Type t, List<Type> stopCheckTypes = null)
        {
            var fields = new List<FieldInfo>();
            var types = new List<Type>();


            while (t != null && (stopCheckTypes == default || !stopCheckTypes.Contains(t)))
            {
                types.Add(t);
                t = t.BaseType;
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                var range = types[i].GetFields(Utils.ReflectionUtils.GetStandardFlags());

                foreach (var f in range)
                {
                    var added = false;

                    for (int x = 0; x < fields.Count; x++)
                    {
                        if (fields[x].Name == f.Name)
                        {
                            fields[x] = f;
                            added = true;
                            break;
                        }
                    }

                    if (!added)
                        fields.Add(f);
                }
            }

            return fields;
        }

        public static FieldInfo GetFieldInAncestors(this Type t, string name, List<Type> stopCheckTypes = null)
        {
            FieldInfo field = null;

            while (t != null && (stopCheckTypes == default || !stopCheckTypes.Contains(t)))
            {
                field = t.GetField(name, Utils.ReflectionUtils.GetStandardFlags());

                if (field != null)
                    break;

                t = t.BaseType;
            }

            return field;
        }

        public static MethodInfo GetMethodInAncestors(this Type t, string name, List<Type> stopCheckTypes = null)
        {
            MethodInfo method = null;

            while (t != null && (stopCheckTypes == default || !stopCheckTypes.Contains(t)))
            {
                method = t.GetMethod(name, Utils.ReflectionUtils.GetStandardFlags());

                if (method != null)
                    break;

                t = t.BaseType;
            }

            return method;
        }
        #endregion
    }
}