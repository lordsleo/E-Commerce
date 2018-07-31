using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceInterface.Common
{
    /// <summary>
    /// 交换数据包(返回值)。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Package<T>
    {
        /// <summary>
        /// 是否成功。
        /// </summary>
        private bool _IsSuccess;
        /// <summary>
        /// 异常消息。 
        /// </summary>
        private string _Exception;
        /// <summary>
        /// 交换数据。
        /// </summary>
        private T _Value;

        /// <summary>
        /// 获取是否成功。
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                _IsSuccess = value;
            }
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        public string Exception
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
            }
        }

        /// <summary>
        /// 获取交换数据
        /// </summary>
        public T Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        /// <summary>
        /// 初始化交换数据包。
        /// </summary>
        /// <param name="success">是否成功。</param>
        /// <param name="exception">异常消息。</param>
        /// <param name="value">交换数据。</param>
        public Package(bool isSuccess, string exception, T value)
        {
            _IsSuccess = isSuccess;
            _Exception = exception;
            _Value = value;
        }
        /// <summary>
        /// 初始化交换数据包。
        /// </summary>
        public Package()
        {
            _IsSuccess = false;
            _Exception = null;
        }
    }
}