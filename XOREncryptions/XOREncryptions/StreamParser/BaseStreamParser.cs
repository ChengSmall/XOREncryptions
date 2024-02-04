using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace Cheng.Streams.Parsers
{
    /// <summary>
    /// 流数据解析器公共接口
    /// </summary>
    public interface IStreamParser
    {
        /// <summary>
        /// 读取流数据转化为对象
        /// </summary>
        /// <param name="stream">要读取的流数据</param>
        /// <returns>转化到的对象</returns>
        /// <exception cref="ArgumentNullException">流对象为null</exception>
        object ConverToObject(Stream stream);
        /// <summary>
        /// 将给定对象转化并写入流数据
        /// </summary>
        /// <param name="obj">要转化的对象</param>
        /// <param name="stream">要写入的数据流对象</param>
        /// <exception cref="ArgumentNullException">流对象为null</exception>
        void ConverToStream(object obj, Stream stream);
    }
    /// <summary>
    /// 实现流数据解析器的基类
    /// </summary>
    public abstract class StreamParser : IStreamParser
    {
        #region 派生
        /// <summary>
        /// 读取流数据转化为对象
        /// </summary>
        /// <param name="stream">要读取的流数据</param>
        /// <returns>转化到的对象</returns>
        /// <exception cref="ArgumentNullException">流对象为null</exception>
        public abstract object ConverToObject(Stream stream);
        /// <summary>
        /// 将给定对象转化并写入流数据
        /// </summary>
        /// <param name="obj">要转化的对象</param>
        /// <param name="stream">要写入的数据流对象</param>
        /// <exception cref="ArgumentNullException">流对象为null</exception>
        public abstract void ConverToStream(object obj, Stream stream);
        #endregion

        #region 功能
        /// <summary>
        /// 线程安全封装
        /// </summary>
        /// <param name="streamParser">要封装为线程安全的解析器</param>
        /// <returns>一个线程安全的流数据解析器</returns>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        public static IStreamParser AsnyhSafe(IStreamParser streamParser)
        {
            return new ThreadSafe(streamParser);
        }
        #endregion

        #region 结构

        class ThreadSafe : IStreamParser
        {
            public ThreadSafe(IStreamParser sp)
            {
                this.sp = sp ?? throw new ArgumentNullException();
            }

            IStreamParser sp;

            public object ConverToObject(Stream stream)
            {
                lock (sp) return sp.ConverToObject(stream);
            }

            public void ConverToStream(object obj, Stream stream)
            {
                lock (sp) sp.ConverToStream(obj, stream);
            }
        }

        #endregion
    }

    /// <summary>
    /// 数据流解析器异常基类
    /// </summary>
    public class StreamParserException : Exception
    {
        #region
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        public StreamParserException() : base("引发流数据解析器异常")
        {
        }
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        /// <param name="message">指定错误消息</param>
        public StreamParserException(string message) : base(message)
        {
        }
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        /// <param name="message">指定错误消息</param>
        /// <param name="exception">引发该异常的异常</param>
        public StreamParserException(string message, Exception exception) : base(message, exception)
        {
        }
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        /// <param name="type">引发异常的解析类型</param>
        public StreamParserException(Type type)
        {
            this.type = type;
        }
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        /// <param name="type">引发异常的解析类型</param>
        /// <param name="message">指定错误消息</param>
        public StreamParserException(Type type, string message) : base(message)
        {
            this.type = type;
        }
        /// <summary>
        /// 实例化一个解析器错误异常
        /// </summary>
        /// <param name="type">引发异常的解析类型</param>
        /// <param name="message">指定错误消息</param>
        /// <param name="exception">引发该异常的异常</param>
        public StreamParserException(Type type, string message, Exception exception) : base(message, exception)
        {
            this.type = type;
        }

        #endregion

        private Type type;
        /// <summary>
        /// 获取或设置引发异常的解析类型
        /// </summary>
        /// <value>引发异常的解析类型，若没有异常的解析类型则是一个null</value>
        public Type ExceptionType
        {
            get => type;
            set
            {
                type = value;
            }
        }

    }

}
