using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheng.Streams.Parsers.Serialization
{


    public class SerializeStreamParser : StreamParser
    {
        #region 构造


        #endregion

        #region 参数

        #region const
        private static readonly Type p_tsbyte = typeof(sbyte);
        private static readonly Type p_tbyte = typeof(byte);
        private static readonly Type p_tint16 = typeof(short);
        private static readonly Type p_tuint16 = typeof(ushort);
        private static readonly Type p_tint32 = typeof(int);
        private static readonly Type p_tuint32 = typeof(uint);
        private static readonly Type p_tint64 = typeof(long);
        private static readonly Type p_tuint64 = typeof(ulong);

        private static readonly Type p_tfloat = typeof(float);
        private static readonly Type p_tdouble = typeof(double);

        private static readonly Type p_tchar = typeof(char);
        private static readonly Type p_tbool = typeof(bool);

        private static readonly Type p_tdecimal = typeof(decimal);

        private static readonly Type p_tguid = typeof(Guid);
        private static readonly Type p_tDateTime = typeof(DateTime);
        private static readonly Type p_tTimeSpan = typeof(TimeSpan);

        private static readonly Type p_tstr = typeof(string);

        private static readonly Type p_tArray = typeof(string);
        #endregion

        #region par

        #endregion

        #region buffer

        #endregion

        #endregion

        #region 封装

        #region 判断



        #endregion

        #region 存储


        #endregion



        #endregion

        public override object ConverToObject(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void ConverToStream(object obj, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
