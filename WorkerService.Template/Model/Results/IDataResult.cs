using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.Model.Results
{
    //public interface IDataResult<out T> : IResult
    //{
    //    T Data { get; }
    //}

    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }

    public class Result : IResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public interface IDataResult<T>
    {
        T Data { get; set; }
        bool Success { get; set; }
        string Message { get; set; }
    }

    public class DataResult<T> : IDataResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
