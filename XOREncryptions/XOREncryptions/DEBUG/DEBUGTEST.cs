#if DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Cheng
{
    public unsafe static class DEBUGTEST
    {
        const string nullstr = "[Null]";
        const string emptystr = "[Empty]";
        #region print
        public static void print<T>(this T obj)
        {
            if (obj == null) Console.Write(nullstr);
            else Console.Write(obj);
        }
        public static void printl<T>(this T obj)
        {
            if (obj == null) Console.WriteLine(nullstr);
            else Console.WriteLine(obj);
        }

        public static string ToPtrStr(this IntPtr ptr)
        {
            if(sizeof(void*) == 4) return ((uint)ptr.ToPointer()).ToString("x").ToUpper();
            return ((ulong)ptr.ToPointer()).ToString("x").ToUpper();
        }

        public static void print(this IntPtr ptr)
        {
            ((ulong)ptr.ToPointer()).ToString("x").ToUpper().print();
        }
        public static void printl(this IntPtr ptr)
        {
            ((ulong)ptr.ToPointer()).ToString("x").ToUpper().printl();
        }
        public static void print(this UIntPtr ptr)
        {
            ((ulong)ptr.ToPointer()).ToString("x").ToUpper().print();
        }
        public static void printl(this UIntPtr ptr)
        {
            ((ulong)ptr.ToPointer()).ToString("x").ToUpper().printl();
        }

        static string defToStr<T>(T obj)
        {
            string str = obj?.ToString();
            if (str is null) return nullstr;
            if (str.Length == 0) return emptystr;
            return str;
        }

        public static string ToBin(this byte value)
        {
            const char c0 = '0';
            const char c1 = '1';
            char* cs = stackalloc char[9];
            cs[8] = '\0';

            for (int i = 0; i < 8; i++)
            {
                cs[7 - i] = ((value >> i) & 0b1) == 1 ? c1 : c0;
            }

            return new string(cs);
        }

        public static void ToBinptr(this byte value, char* cs)
        {
            const char c0 = '0';
            const char c1 = '1';

            for (int i = 0; i < 8; i++)
            {
                cs[7 - i] = ((value >> i) & 0b1) == 1 ? c1 : c0;
            }

        }
        public static string ToBinary<T>(this T value, string fen = " ") where T : unmanaged
        {
            int size = sizeof(T);
            StringBuilder sb = new StringBuilder(size * 8);
            char* csp = stackalloc char[8];
            byte* bp = (byte*)&value;

            for (int i = size - 1; i >= 0; i--)
            {
                bp[i].ToBinptr(csp);
                sb.Append(csp, 8);
                sb.Append(fen);
            }
            return sb.ToString();
        }

        public static string Foreach(this IEnumerable arr, int lineCount, string fen = " ", Func<object, string> toStr = null)
        {
            if (toStr is null) toStr = defToStr;
            if (arr is IList)
            {
                return foreachList((IList)arr, lineCount, fen, toStr);
            }
            if (arr is ICollection)
            {
                return foreachCol((ICollection)arr, lineCount, fen, toStr);
            }
            return foreachEnumator(arr.GetEnumerator(), lineCount, fen, toStr);
        }

        static string foreachList(IList arr, int lineCount, string fen, Func<object, string> toStr)
        {
            StringBuilder sb = new StringBuilder(arr.Count);

            for (int i = 0; i < arr.Count; i++)
            {
                if (i != 0 && i % lineCount == 0)
                {
                    sb.AppendLine();
                }

                sb.Append(toStr.Invoke(arr[i]) + fen);
            }

            return sb.ToString();
        }

        static string foreachCol(ICollection collect, int lineCount, string fen, Func<object, string> toStr)
        {
            StringBuilder sb = new StringBuilder();

            int length = collect.Count;

            IEnumerator enr = collect.GetEnumerator();
            for (int i = 0; enr.MoveNext(); i++)
            {
                if (i != 0 && i % lineCount == 0)
                {
                    sb.AppendLine();
                }
                sb.Append(toStr.Invoke(enr.Current) + fen);

            }


            return sb.ToString();
        }

        static string foreachEnumator(IEnumerator enumator, int lineCount, string fen, Func<object, string> toStr)
        {
            StringBuilder sb = new StringBuilder();

            int count = 0;
            object temp;

            while (enumator.MoveNext())
            {
                temp = enumator.Current;


                if (count % lineCount == 0)
                {
                    sb.AppendLine();
                }

                sb.Append(toStr.Invoke(temp) + fen);

                count++;
            }



            return sb.ToString();
        }

        public static void printl()
        {
            Console.WriteLine();
        }
        #endregion
        #region stream
        /// <summary>
        /// 打开一个只读类型的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileStream Open(this string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }
        /// <summary>
        /// 创建一个只写文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileStream Write(this string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.Write);
        }
        #endregion

        #region 反射

        public static string Fanshe<T>(this T obj)
        {
            return Fanshe((obj?.GetType()) ?? typeof(T), obj);
        }

        public static string Fanshe(this Type type, object obj = null)
        {
            //var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            //var funcs = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            StringBuilder sb = new StringBuilder(256);
            var mems = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            int length = mems.Length;
            MemberInfo mem;
            MemberTypes mt;
            sb.AppendLine("----------------------------------------------");
            sb.AppendLine("类型:" + type.FullName);
            sb.AppendLine();
            int end = length - 1;
            for (int i = 0; i < length; i++)
            {
                mem = mems[i];
                mt = mem.MemberType;
                sb.AppendLine("成员:" + mem.Name + " 成员类型:" + toMemberName(mt));
                
                sb.AppendLine(toStrMem(mem, mt, obj));
                if (i != end)
                {
                    sb.AppendLine("--------------------------------------");
                    sb.AppendLine();
                }
            }
            sb.AppendLine("----------------------------------------------");
            return sb.ToString();
        }

        static bool basefield(FieldInfo field)
        {
            Type t = field.FieldType;
            if (t.IsPrimitive) return true;
            if (t == typeof(decimal)) return true;
            if (t == typeof(string)) return true;
            if (t == typeof(object)) return true;


            return false;
        }

        static string toMemberName(MemberTypes type)
        {
            switch (type)
            {
                case MemberTypes.Constructor:
                    return "构造";
                case MemberTypes.Event:
                    return "事件";
                case MemberTypes.Field:
                    return "字段";
                case MemberTypes.Method:
                    return "方法";
                case MemberTypes.Property:
                    return "属性";
                case MemberTypes.TypeInfo:
                    return "类型";
                default:
                    return type.ToString();
            }
        }

        static string toStrMem(MemberInfo info, MemberTypes type, object obj)
        {
            const string space = " ";
            object fie;
            int i;
            if(type == MemberTypes.Field)
            {
                if (obj is null) return "";
                
                FieldInfo fi = (FieldInfo)info;
                
                if(basefield(fi))
                {
                    
                    return (fi.FieldType.FullName + ": " + fi.GetValue(obj).ToString());
                }
                fie = fi.GetValue(obj);
                return Fanshe(fie.GetType(), fie);
            }
            StringBuilder sb;
            if (type == MemberTypes.Method)
            {
                MethodInfo method = (MethodInfo)info;
                sb = new StringBuilder();

                sb.Append(method.ReturnType + space);
                sb.Append(method.Name + '(');

                var pars = method.GetParameters();
                for (i = 0; i < pars.Length; i++)
                {
                    sb.Append((pars[i].ParameterType.Name + pars[i].Name));
                    if (i != pars.Length - 1) sb.Append(',');
                }
                sb.Append(");");
                return sb.ToString();

            }
            if(type == MemberTypes.Constructor)
            {
                ConstructorInfo method = (ConstructorInfo)info;
                sb = new StringBuilder();

                sb.Append(method.DeclaringType.Name + '(');

                var pars = method.GetParameters();
                for (i = 0; i < pars.Length; i++)
                {
                    sb.Append((pars[i].ParameterType.Name + pars[i].Name));
                    if (i != pars.Length - 1) sb.Append(',');
                }
                sb.Append(");");
                return sb.ToString();
            }
            

            return info.ToString();
        }


        #endregion

    }
}
#endif
