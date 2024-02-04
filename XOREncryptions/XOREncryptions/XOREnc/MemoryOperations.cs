using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Cheng.Memorys
{
    /// <summary>
    /// 内存操作方法，此扩展没有做安全检查
    /// </summary>
    public unsafe static class MemoryOperation
    {
        #region 内存方法

        #region 内存拷贝

        #region 结构
        [StructLayout(LayoutKind.Sequential, Size = 32)]
        private struct byte32
        {
        }
        [StructLayout(LayoutKind.Sequential, Size = 128)]
        private struct byte128
        {
        }
        [StructLayout(LayoutKind.Sequential, Size = 1024)]
        private struct byte1024
        {
        }
        [StructLayout(LayoutKind.Sequential, Size = 8192)]
        private struct byte8192
        {
        }
        [StructLayout(LayoutKind.Sequential, Size = 1024 * 1024 * 4)]
        private struct mb4
        {
        }
        [StructLayout(LayoutKind.Sequential, Size = 1024 * 1024 * 128)]
        private struct mb128
        {
        }
        #endregion

        static void memoryCopyMB128(void* copy, void* to, int sizeMB128)
        {
            mb128* cp = (mb128*)copy;
            mb128* top = (mb128*)to;
            for (int i = 0; i < sizeMB128; i++)
            {
                top[i] = cp[i];
            }
        }
        static void memoryCopyMB4(void* copy, void* to, int sizeMB4)
        {
            mb4* cp = (mb4*)copy;
            mb4* top = (mb4*)to;
            for (int i = 0; i < sizeMB4; i++)
            {
                top[i] = cp[i];
            }
        }
        static void memoryCopy8192(void* copy, void* to, int size8192)
        {
            byte8192* cp = (byte8192*)copy;
            byte8192* top = (byte8192*)to;
            for (int i = 0; i < size8192; i++)
            {
                top[i] = cp[i];
            }
        }
        static void memroyCopy1024(void* copy, void* to, int size1024)
        {
            byte1024* cp = (byte1024*)copy;
            byte1024* top = (byte1024*)to;
            for (int i = 0; i < size1024; i++)
            {
                top[i] = cp[i];
            }
        }
        static void memroyCopy128(void* copy, void* to, int size128)
        {
            byte128* cp = (byte128*)copy;
            byte128* top = (byte128*)to;
            for (int i = 0; i < size128; i++)
            {
                top[i] = cp[i];
            }
        }
        static void memroyCopy32(void* copy, void* to, int size32)
        {
            byte32* cp = (byte32*)copy;
            byte32* top = (byte32*)to;
            for (int i = 0; i < size32; i++)
            {
                top[i] = cp[i];
            }
        }
        /// <summary>
        /// 将内存块拷贝到另一块内存当中
        /// </summary>
        /// <param name="copyMemory">要拷贝的内存块</param>
        /// <param name="toMemory">要拷贝到的内存位置</param>
        /// <param name="size">要拷贝的内存字节大小</param>
        /// <exception cref="ArgumentOutOfRangeException">拷贝的字节小于0</exception>
        public static void MemoryCopy(this IntPtr copyMemory, IntPtr toMemory, int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException("size");

            MemoryCopy((void*)copyMemory, (void*)toMemory, size);
        }

        /// <summary>
        /// 将内存块拷贝到另一块内存当中
        /// </summary>
        /// <param name="copyMemory">要拷贝的内存块</param>
        /// <param name="toMemory">要拷贝到的内存位置</param>
        /// <param name="size">要拷贝的内存字节大小</param>
        public static void MemoryCopy(void* copyMemory, void* toMemory, int size)
        {
            //Buffer.MemoryCopy(copyMemory, toMemory, size, size);

            if (size == 0) return;

            const int mb4 = 1024 * 1024 * 4;
            const int mb128 = 1024 * 1024 * 128;
            byte* copy = (byte*)copyMemory, to = (byte*)toMemory;

            //拷贝索引
            int copyByteIndex = 0;
            //倍率
            int sizeMagnitude;

            //大于32byte

            if (size > 128)
            {
                //大于128byte

                if (size > 1024)
                {

                    if (size > 8192)
                    {
                        //大于4MB

                        if (size > mb4)
                        {

                            if(size > mb128)
                            {

                                sizeMagnitude = size / mb128;

                                memoryCopyMB128(copy + copyByteIndex, to + copyByteIndex, sizeMagnitude);

                                copyByteIndex += sizeMagnitude * mb128;
                                size = size % mb128;

                            }

                            sizeMagnitude = size / mb4;

                            memoryCopyMB4(copy + copyByteIndex, to + copyByteIndex, sizeMagnitude);

                            copyByteIndex += sizeMagnitude * mb4;
                            size = size % mb4;
                        }

                        //大于8192byte
                        sizeMagnitude = size / 8192;

                        memoryCopy8192(copy + copyByteIndex, to + copyByteIndex, sizeMagnitude);

                        copyByteIndex += sizeMagnitude * 8192;
                        size = size % 8192;
                    }


                    //大于1024byte，小于等于8192
                    sizeMagnitude = size / 1024;
                    memroyCopy1024(copy + copyByteIndex, to + copyByteIndex, sizeMagnitude);

                    copyByteIndex += sizeMagnitude * 1024;
                    size = size % 1024;
                }

                //大于128byte，小于1024
                sizeMagnitude = size / 128;
                memroyCopy128(copy + copyByteIndex, to + copyByteIndex, sizeMagnitude);

                copyByteIndex += sizeMagnitude * 128;
                size = size % 128;
            }


            //剩余或小于128
            copy += copyByteIndex;
            to += copyByteIndex;

            int i;

            int size4 = size / 4;
            for (i = 0; i < size4; i++)
            {
                *(((int*)to) + i) = *(((int*)copy) + i);
            }

            size = size % 4;
            int offset4 = size4 * 4;
            copy += offset4;
            to += offset4;

            if (size != 0)
            {
                for (i = 0; i < size; i++)
                {
                    to[i] = copy[i];
                }
            }

        }

        /// <summary>
        /// 将两个内存块中的数据交换
        /// </summary>
        /// <param name="memory1">内存块1</param>
        /// <param name="memory2">内存块2</param>
        /// <param name="size">内存块字节大小</param>
        public static void MemorySwap(this IntPtr memory1, IntPtr memory2, int size)
        {
            if (size == 0) return;
            IntPtr temptr;
            if (size <= 64)
            {
                byte* temp = stackalloc byte[size];
                temptr = new IntPtr(temp);

                MemoryCopy(memory1, temptr, size);
                MemoryCopy(memory2, memory1, size);
                MemoryCopy(temptr, memory2, size);
                return;
            }

            byte[] buf = new byte[size];
            fixed (byte* bp = buf)
            {
                temptr = new IntPtr(bp);
                MemoryCopy(memory1, temptr, size);
                MemoryCopy(memory2, memory1, size);
                MemoryCopy(temptr, memory2, size);
            }

        }

        /// <summary>
        /// 交换集合当中两个元素的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <param name="index1">元素1位置索引</param>
        /// <param name="index2">元素2位置索引</param>
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            T t = list[index1];
            list[index1] = list[index2];
            list[index1] = t;
        }
        #endregion

        #region 字节数据
        /// <summary>
        /// 将非托管数据写入到字节数组
        /// </summary>
        /// <remarks>
        /// 此函数将非托管变量的内存转化到给定的字节数组当中，转化方式以此程序内的内存为基准
        /// </remarks>
        /// <typeparam name="T">要转化的类型</typeparam>
        /// <param name="value">要转化的数据</param>
        /// <param name="buffer">要转化到的字节数组，必须保证给定数组的写入长度大于或等于类型大小</param>
        /// <param name="index">要转化到的字节数组的起始位置</param>
        public static void ToByteArray<T>(this T value, byte[] buffer, int index) where T : unmanaged
        {
            fixed (byte* bp = buffer)
            {
                *((T*)(bp + index)) = value;
            }
        }
        /// <summary>
        /// 从字节数组的内存中获取指定类型的非托管数据
        /// </summary>
        /// <remarks>
        /// 从给定数组的位置，获取指定类型<typeparamref name="T"/>长度大小的字节，并以该类型返回数据
        /// </remarks>
        /// <typeparam name="T">转化的类型</typeparam>
        /// <param name="buffer">要转化的数组，必须保证给定数组的长度大于或等于类型大小</param>
        /// <param name="index">要转化的字节数组的起始位置</param>
        /// <returns>转化后的数据</returns>
        public static T ToStructure<T>(this byte[] buffer, int index) where T : unmanaged
        {
            fixed (byte* bp = buffer)
            {
                return (*((T*)(bp + index)));
            }
        }
        /// <summary>
        /// 将字符串内存数据直接作为字节数组返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="index">字符串要转化的的起始位置</param>
        /// <returns>给定字符串的内存字节；若字符串为null则直接返回null</returns>
        public static byte[] ToByteArray(this string str, int index)
        {
            if (str is null) return null;
            int size = str.Length * sizeof(char);
            byte[] buf = new byte[size];

            fixed (void* bp = buf, strp = str)
            {
                MemoryCopy(new IntPtr((((char*)strp) + index)), new IntPtr(bp), size);
            }
            return buf;
        }
        /// <summary>
        /// 将字符串内存数据写入为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">字符串要转化的的起始位置</param>
        /// <param name="buffer">写入到的字节数组</param>
        /// <param name="offset">字节数组的起始位置</param>
        public static void ToByteArray(this string str, int index, byte[] buffer, int offset)
        {
            if (str is null) throw new ArgumentNullException();
            int size = str.Length * sizeof(char);

            fixed (void* bp = buffer, strp = str)
            {
                MemoryCopy(new IntPtr((((char*)strp) + index)), new IntPtr(((byte*)bp) + offset), size);
            }
        }

        /// <summary>
        /// 将字节数组中的内存直接转化为字符串
        /// </summary>
        /// <param name="buffer">要转化的字节数组</param>
        /// <param name="index">要转化的字节数组起始位置</param>
        /// <param name="count">要转化到字符串的字符数</param>
        /// <returns>转化的字符串</returns>
        public static string ToStringBuffer(this byte[] buffer, int index, int count)
        {
            int length = buffer.Length;

            fixed(byte* p = buffer)
            {
                return new string((char*)(p + index), 0, count);
            }
        }
        /// <summary>
        /// 将字节数组中的内存直接转化为字符串
        /// </summary>
        /// <param name="buffer">要转化的字节数组</param>
        /// <param name="index">要转化的字节数组起始位置</param>
        /// <returns>转化的字符串</returns>
        public static string ToStringBuffer(this byte[] buffer, int index)
        {
            int length = buffer.Length;

            fixed (byte* p = buffer)
            {
                int count = (buffer.Length - index) / 2;
                return new string((char*)(p + index), 0, count);
            }
        }

        /// <summary>
        /// 将指定的非托管内存缓冲区写入到另一个非托管内存缓冲区当中
        /// </summary>
        /// <typeparam name="T">原缓冲区数组类型</typeparam>
        /// <typeparam name="TO">目标缓冲区数组类型</typeparam>
        /// <param name="buffer">原缓冲区</param>
        /// <param name="offset">原缓冲区要拷贝的字节偏移</param>
        /// <param name="toBuffer">要拷贝到的目标缓冲区</param>
        /// <param name="toOffset">要拷贝到的缓冲区的起始偏移</param>
        /// <param name="copyByteSize">要拷贝的字节数量</param>
        public static void CopyBufferArray<T, TO>(this T[] buffer, int offset, TO[] toBuffer, int toOffset, int copyByteSize) where T : unmanaged where TO : unmanaged
        {
            
            fixed (void* fi_buf = buffer, fi_toBuf = toBuffer)
            {

                MemoryCopy(((byte*)fi_buf) + offset, ((byte*)fi_toBuf) + toOffset, copyByteSize);

            }

        }

        /// <summary>
        /// 将非托管数据写入到字节数组
        /// </summary>
        /// <remarks>
        /// 此函数将非托管变量的内存转化到给定的字节数组当中，转化方式以此程序内的内存为基准
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要转化的数据</param>
        /// <param name="buffer">要转化到的字节数组，必须保证给定数组的写入长度大于或等于类型大小</param>
        /// <param name="index">要转化到的字节数组的起始位置</param>
        /// <param name="endIndex">转化完毕后根据起始索引向后推进到新位置的第一位索引，流数据位置模拟</param>
        public static void ToByteArray<T>(this T value, byte[] buffer, int index, out int endIndex) where T : unmanaged
        {
            endIndex = sizeof(T) + index;
            ToByteArray(value, buffer, index);
        }
        /// <summary>
        /// 从字节数组的内存中获取指定类型的非托管数据
        /// </summary>
        /// <remarks>
        /// 从给定数组的位置，获取指定类型<typeparamref name="T"/>长度大小的字节，并以该类型返回数据
        /// </remarks>
        /// <typeparam name="T">转化的类型</typeparam>
        /// <param name="buffer">要转化的数组，必须保证给定数组的长度大于或等于类型大小</param>
        /// <param name="index">要转化的字节数组的起始位置</param>
        /// <param name="endIndex">转化完毕后的索引位置，流数据位置模拟</param>
        /// <returns>转化后的数据</returns>
        public static T ToStructure<T>(this byte[] buffer, int index, out int endIndex) where T : unmanaged
        {
            endIndex = sizeof(T) + index;
            return ToStructure<T>(buffer, index);
        }

        #endregion

        #region 位域

        #region 常量

        const byte b0 = 0;
        const byte b1 = 0b1;
        const byte b2 = 0b10;
        const byte b3 = 0b100;
        const byte b4 = 0b1000;
        const byte b5 = 0b1000_0;
        const byte b6 = 0b1000_00;
        const byte b7 = 0b1000_000;
        const byte b8 = 0b1000_0000;

        const byte ball = 0b1111_1111;

        const byte sb1 = 0b1111_1110;
        const byte sb2 = 0b1111_1101;
        const byte sb3 = 0b1111_1011;
        const byte sb4 = 0b1111_0111;
        const byte sb5 = 0b1110_1111;
        const byte sb6 = 0b1101_1111;
        const byte sb7 = 0b1011_1111;
        const byte sb8 = 0b0111_1111;

        #endregion
        /// <summary>
        /// 获取单字节指定位域的值
        /// </summary>
        /// <param name="b">字节</param>
        /// <param name="index">位域索引，范围[0,7]</param>
        /// <returns>指定位域的值，1返回true，0返回false</returns>
        public static bool ByteBit(this byte b, int index)
        {
            switch (index)
            {
                case 0:
                    return (b & b1) == b1;
                case 1:
                    return (b & b2) == b2;
                case 2:
                    return (b & b3) == b3;
                case 3:
                    return (b & b4) == b4;
                case 4:
                    return (b & b5) == b5;
                case 5:
                    return (b & b6) == b6;
                case 6:
                    return (b & b7) == b7;
                case 7:
                    return (b & b8) == b8;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        /// <summary>
        /// 设置单字节指定位域的值
        /// </summary>
        /// <param name="b">字节引用</param>
        /// <param name="index">位域索引，范围[0,7]</param>
        /// <param name="value">设置到指定位域的值，true表示设置为1，false表示设置为0</param>
        public static void ByteBit(this ref byte b, int index, bool value)
        {
            switch (index)
            {
                case 0:
                    if (value) b &= b1;
                    else b &= sb1;
                    break;
                case 1:
                    if (value) b &= b2;
                    else b &= sb2;
                    break;
                case 2:
                    if (value) b &= b3;
                    else b &= sb3;
                    break;
                case 3:
                    if (value) b &= b4;
                    else b &= sb4;
                    break;
                case 4:
                    if (value) b &= b5;
                    else b &= sb5;
                    break;
                case 5:
                    if (value) b &= b6;
                    else b &= sb6;
                    break;
                case 6:
                    if (value) b &= b7;
                    else b &= sb7;
                    break;
                case 7:
                    if (value) b &= b8;
                    else b &= sb8;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        /// <summary>
        /// 获取指定非托管内存的某一字节引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">值</param>
        /// <param name="index">字节偏移</param>
        /// <returns>指定偏移的字节引用</returns>
        public static ref byte RefByte<T>(this ref T value, int index) where T : unmanaged
        {
            fixed (T* p = &value)
            {
                return ref *(((byte*)p) + index);
            }
        }
        /// <summary>
        /// 获取指定内存的某一字节引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptrAddress">内存所在地址</param>
        /// <param name="index">字节偏移</param>
        /// <returns>指定偏移的字节引用</returns>
        public static ref byte RefPtrByte(this IntPtr ptrAddress, int index)
        {
            return ref *(((byte*)ptrAddress) + index);
        }

        #endregion

        #region 大小端
        /// <summary>
        /// 判断当前程序运行环境是否为大端存储
        /// </summary>
        /// <returns>返回true表示大端存储，false表示小端存储</returns>
        public static bool IsBigEndian
        {
            get
            {
                ushort i = 1;
                return (*(byte*)&i) == 0;
            }
        }
        /// <summary>
        /// 进行大小端转化
        /// </summary>
        /// <param name="bytes">要转化的数据</param>
        /// <param name="re">转化到的数据</param>
        public static void StorageConversionByte(this byte[] bytes, byte[] re)
        {
            int length = bytes.Length;
            for (int i = 0, j = length - 1; i < length; i++, j--)
            {
                re[i] = bytes[j];
            }
        }
        /// <summary>
        /// 进行位域反转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConversionStorageBit(this byte value)
        {
            byte re = 0;

            re |= (value & b1) == 0 ? b0 : b8;
            re |= (value & b2) == 0 ? b0 : b7;
            re |= (value & b3) == 0 ? b0 : b6;
            re |= (value & b4) == 0 ? b0 : b5;

            re |= (value & b5) == 0 ? b0 : b4;
            re |= (value & b6) == 0 ? b0 : b3;
            re |= (value & b7) == 0 ? b0 : b2;
            re |= (value & b8) == 0 ? b0 : b1;

            return re;
        }
        /// <summary>
        /// 进行大小端和位域转化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="re"></param>
        public static void StorageConversionAll(this byte[] bytes, byte[] re)
        {
            int length = bytes.Length;
            for (int i = 0, j = length - 1; i < length; i++, j--)
            {
                re[i] = ConversionStorageBit(bytes[i]);
            }
        }
        /// <summary>
        /// 进行大小端转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要转化的数据</param>
        /// <returns>转化后的数据</returns>
        public static T StorageConversionByte<T>(this T value) where T : unmanaged
        {
            T temp;
            int length = sizeof(T);
            int end = sizeof(T) - 1;
            byte* firstp = (byte*)&value;
            byte* endp = (((byte*)&temp) + end);
            int i;

            for (i = 0; i < length; i++)
            {
                endp[end - i] = firstp[i];
            }

            return temp;
        }
        /// <summary>
        /// 进行大小端和位域转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要转化的数据</param>
        /// <returns>转化后的数据</returns>
        public static T StorageConversionAll<T>(this T value) where T : unmanaged
        {

            T temp;
            int length = sizeof(T);
            int end = sizeof(T) - 1;
            byte* firstp = (byte*)&value;
            byte* endp = (((byte*)&temp) + end);
            int i;
            for (i = 0; i < length; i++)
            {
                endp[end - i] = ConversionStorageBit(firstp[i]);
            }

            return temp;

        }

        #endregion

        #region 流数据
        /// <summary>
        /// 完整读取流数据的字节序列
        /// </summary>
        /// <param name="stream">读取的流</param>
        /// <param name="buffer">读取到的缓冲区</param>
        /// <param name="offset">缓冲区存放的起始索引</param>
        /// <param name="count">要读取的字节数量</param>
        /// <returns>实际读取的字节数量；若返回的值小于<paramref name="count"/>表示剩余字节数小于要读取的字节数，返回0表示流已到达结尾</returns>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        /// <exception cref="ArgumentException">给定参数超出范围</exception>
        /// <exception cref="ArgumentOutOfRangeException">给定参数超出范围</exception>
        /// <exception cref="IOException">IO错误</exception>
        /// <exception cref="NotSupportedException">不支持方法</exception>
        /// <exception cref="ObjectDisposedException">资源已释放</exception>
        public static int ReadBlock(this Stream stream, byte[] buffer, int offset, int count)
        {
            if(stream is null) throw new ArgumentNullException(nameof(stream));
            int index = offset;
            int rsize;
            int re = 0;
            while (count != 0)
            {
                rsize = stream.Read(buffer, index, count);
                if (rsize == 0) break;
                index += rsize;
                count -= rsize;
                re += rsize;
            }
            return re;
        }

        /// <summary>
        /// 将流数据读取并拷贝到另一个流当中
        /// </summary>
        /// <param name="stream">要读取的流</param>
        /// <param name="toStream">写入的流</param>
        /// <param name="buffer">流数据一次读写的缓冲区</param>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        /// <exception cref="ArgumentException">缓冲区长度为0</exception>
        /// <exception cref="NotSupportedException">流数据没有指定权限</exception>
        public static void CopyToStream(this Stream stream, Stream toStream, byte[] buffer)
        {
            if (stream is null || toStream is null || buffer is null) throw new ArgumentNullException();

            if (buffer.Length == 0) throw new ArgumentException("给定缓冲区长度为0");

            if ((!stream.CanRead) || (!toStream.CanWrite)) throw new NotSupportedException("流不支持读取或写入");

            copyToStream(stream, toStream, buffer, 0);
        }
        /// <summary>
        /// 将流数据读取并拷贝到另一个流当中
        /// </summary>
        /// <param name="stream">要读取的流</param>
        /// <param name="toStream">写入的流</param>
        /// <param name="buffer">流数据一次读写的缓冲区</param>
        /// <returns>一个枚举器，每次推进都会读写指定字节的数据，并把此次拷贝的字节量返回到枚举值当中</returns>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        /// <exception cref="ArgumentException">缓冲区长度为0</exception>
        /// <exception cref="NotSupportedException">流数据没有指定权限</exception>
        public static IEnumerable<int> CopyToStreamEnumator(this Stream stream, Stream toStream, byte[] buffer)
        {
            if (stream is null || toStream is null || buffer is null) throw new ArgumentNullException();

            if (buffer.Length == 0) throw new ArgumentException("给定缓冲区长度为0");

            if ((!stream.CanRead) || (!toStream.CanWrite)) throw new NotSupportedException("流不支持读取或写入");

            return copyToStreamEnr(stream, toStream, buffer, 0);
        }

        /// <summary>
        /// 将流数据读取并拷贝到另一个流当中
        /// </summary>
        /// <param name="stream">要读取的流</param>
        /// <param name="toStream">写入的流</param>
        /// <param name="buffer">流数据一次读写的缓冲区</param>
        /// <param name="maxBytes">指定最大拷贝字节量，0表示不指定最大字节量</param>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        /// <exception cref="ArgumentException">缓冲区长度为0</exception>
        /// <exception cref="NotSupportedException">流数据没有指定权限</exception>
        public static void CopyToStream(this Stream stream, Stream toStream, byte[] buffer, ulong maxBytes)
        {
            if (stream is null || toStream is null || buffer is null) throw new ArgumentNullException();

            if (buffer.Length == 0) throw new ArgumentException("给定缓冲区长度为0");

            if ((!stream.CanRead) || (!toStream.CanWrite)) throw new NotSupportedException("流不支持读取或写入");

            copyToStream(stream, toStream, buffer, maxBytes);
        }
        /// <summary>
        /// 将流数据读取并拷贝到另一个流当中
        /// </summary>
        /// <param name="stream">要读取的流</param>
        /// <param name="toStream">写入的流</param>
        /// <param name="buffer">流数据一次读写的缓冲区</param>
        /// <param name="maxBytes">指定最大拷贝字节量，0表示不指定最大字节量</param>
        /// <returns>一个枚举器，每次推进都会读写指定字节的数据，并把此次拷贝的字节量返回到枚举值当中</returns>
        /// <exception cref="ArgumentNullException">参数为null</exception>
        /// <exception cref="ArgumentException">缓冲区长度为0</exception>
        /// <exception cref="NotSupportedException">流数据没有指定权限</exception>
        public static IEnumerable<int> CopyToStreamEnumator(this Stream stream, Stream toStream, byte[] buffer, ulong maxBytes)
        {
            if (stream is null || toStream is null || buffer is null) throw new ArgumentNullException();

            if (buffer.Length == 0) throw new ArgumentException("给定缓冲区长度为0");

            if ((!stream.CanRead) || (!toStream.CanWrite)) throw new NotSupportedException("流不支持读取或写入");

            return copyToStreamEnr(stream, toStream, buffer, maxBytes);
        }

        static void copyToStream(Stream stream, Stream toStream, byte[] buffer, ulong maxBytes)
        {
            int length = buffer.Length;
            int rsize;
            int reas;
            ulong isReadSize;

            if(maxBytes == 0)
            {
                BeginLoop:
                rsize = stream.Read(buffer, 0, length);

                if (rsize == 0) return;

                toStream.Write(buffer, 0, rsize);

                goto BeginLoop;
            }

            isReadSize = 0;

            nBeginLoop:

            if (isReadSize == maxBytes) return;

            if ((isReadSize + (ulong)length) > maxBytes)
            {
                reas = (int)(maxBytes - isReadSize);
            }
            else reas = length;

            rsize = stream.Read(buffer, 0, reas);

            if (rsize == 0) return;

            isReadSize += (ulong)rsize;
            toStream.Write(buffer, 0, rsize);

            goto nBeginLoop;

        }

        static IEnumerable<int> copyToStreamEnr(Stream stream, Stream toStream, byte[] buffer, ulong maxBytes)
        {
            int length = buffer.Length;
            int rsize;
            int reas;
            ulong isReadSize;

            if (maxBytes == 0)
            {
                BeginLoop:
                rsize = stream.Read(buffer, 0, length);

                if (rsize == 0) yield break;

                toStream.Write(buffer, 0, rsize);
                yield return rsize;
                goto BeginLoop;
            }

            isReadSize = 0;

            nBeginLoop:

            if (isReadSize == maxBytes) yield break;

            if ((isReadSize + (ulong)length) > maxBytes)
            {
                reas = (int)(maxBytes - isReadSize);
            }
            else reas = length;

            rsize = stream.Read(buffer, 0, reas);

            if (rsize == 0) yield break;

            isReadSize += (ulong)rsize;
            toStream.Write(buffer, 0, rsize);
            yield return rsize;
            goto nBeginLoop;

        }
        #endregion

        #region 比较
        /// <summary>
        /// 比较两块内存中的数据是否相同
        /// </summary>
        /// <param name="ptr1">地址1</param>
        /// <param name="ptr2">地址2</param>
        /// <param name="length">比较的长度</param>
        /// <returns></returns>
        public static bool EqualsMemory(this IntPtr ptr1, IntPtr ptr2, int length)
        {
            long* xlp = (long*)ptr1, ylp = (long*)ptr2;
            if (xlp == ylp) return true;

            int i;
            int size8 = length / 8;
            int sf8 = length % 8;

            for (i = 0; i < size8; i++)
            {
                if ((*(xlp + i)) != (*(ylp + i))) return false;
            }

            if (sf8 != 0)
            {
                xlp += i;
                ylp += i;

                for (i = 0; i < sf8; i++)
                {
                    if ((*(((byte*)xlp) + i)) != (*(((byte*)ylp) + i))) return false;
                }

            }

            return true;
        }
        /// <summary>
        /// 比较两个字节数组的内存数据是否相等
        /// </summary>
        /// <param name="buffer1"></param>
        /// <param name="buffer2"></param>
        /// <returns>当两个字节数组的元素内容全部相同时，返回true，否则返回false；若两个参数全部是null，则返回true</returns>
        public static bool EqualsBytes(this byte[] buffer1, byte[] buffer2)
        {
            if (buffer1 == buffer2) return true;
            if (buffer1 is null || buffer2 is null) return false;

            int length = buffer1.Length;
            if (length != buffer2.Length) return false;

            fixed(byte* bp = buffer1, bp2 = buffer2)
            {
                return EqualsMemory(new IntPtr(bp), new IntPtr(bp2), length);
            }
        }


        #endregion

        #region 内存解引用
        /// <summary>
        /// 将指针解引用
        /// </summary>
        /// <typeparam name="T">表示解引用的类型</typeparam>
        /// <param name="ptrAddress">表示一个存储地址的指针变量</param>
        /// <returns>使用指针指向的地址访问内存并以<typeparamref name="T"/>类型变量返回值</returns>
        public static ref T PtrDef<T>(this IntPtr ptrAddress) where T : unmanaged
        {
            return ref (*(T*)ptrAddress);
        }

        /// <summary>
        /// 将指针解引用
        /// </summary>
        /// <typeparam name="R">表示一个指针的类型</typeparam>
        /// <typeparam name="T">表示解引用的类型</typeparam>
        /// <param name="ptrAddress">表示一个存储地址的指针变量</param>
        /// <returns>使用指针指向的地址访问内存并以<typeparamref name="T"/>类型变量返回值</returns>
        public static ref T PtrDef<R, T>(this R ptrAddress) where R : unmanaged where T : unmanaged
        {
            return ref *(*(T**)&ptrAddress);
        }
        /// <summary>
        /// 将指针解引用
        /// </summary>
        /// <typeparam name="R">表示一个指针的类型</typeparam>
        /// <typeparam name="T">表示解引用的类型</typeparam>
        /// <param name="ptrAddress">表示一个存储地址的指针变量</param>
        /// <param name="value">使用指针指向的地址访问内存并以<typeparamref name="T"/>类型变量赋值</param>
        public static void PtrDef<R, T>(this R ptrAddress, T value) where R : unmanaged where T : unmanaged
        {
            *(*(T**)&ptrAddress) = value;
        }

        #endregion

        #endregion

    }
}
