using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cheng.Streams
{
    /// <summary>
    /// 释放方法高度封装的流
    /// </summary>
    public unsafe abstract class HEStream : Stream
    {

        #region 释放
        /// <summary>
        /// 此实例的资源是否被释放
        /// </summary>
        protected bool IsDispose
        {
            get => p_IsDispose;
        }
        /// <summary>
        /// 重写此方法以释放非托管资源
        /// </summary>
        /// <remarks>该方法只会在释放资源时执行一次，用于清理或释放所有非托管资源</remarks>
        protected virtual void DisposeUnmanaged()
        {
        }
        /// <summary>
        /// 在调用<see cref="Dispose(bool)"/>方法使用true参数时执行一次
        /// </summary>
        protected virtual void DisposeCtor()
        {
        }

        #region 封装
        private bool p_IsDispose = false;
        /// <summary>
        /// 关闭当前流的所有非托管资源和链接句柄
        /// </summary>
        public sealed override void Close()
        {
            Dispose(true);
        }
        /// <summary>
        /// 释放或关闭所有非托管资源
        /// </summary>
        /// <param name="disposing">此参数用于是否调用<see cref="GC.SuppressFinalize(object)"/>以禁止析构函数，true调用，false不调用；若要在析构函数中释放非托管资源，请使用false参数</param>
        protected sealed override void Dispose(bool disposing)
        {
            if (!p_IsDispose)
            {
                p_IsDispose = true;

                if (disposing)
                {
                    GC.SuppressFinalize(this);
                    DisposeCtor();
                }
                DisposeUnmanaged();
            }
        }
        #endregion

        #endregion

    }
}
