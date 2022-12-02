using System.Collections;

namespace Gymchair.Core
{
    public enum eContent
    {
        SUCCESS,
        ERROR_UNKNOWN = 1000
    }

    public class Result<T>
    {
        eContent code;
        string errorMessage;
        T data;

        public bool IsSuccess { get => code == eContent.SUCCESS; }
        public eContent Code { get => code; }
        public string ErrorMessage { get => errorMessage; }
        public T Data { get => data; }

        public Result(eContent code, string errorMessage, T data)
        {
            this.code = code;
            this.errorMessage = errorMessage;
            this.data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(eContent.SUCCESS, "", data);
        }

        public static Result<T> Error(eContent code = eContent.ERROR_UNKNOWN, string msg = "UNKNOWN")
        {
            return new Result<T>(code, msg, default(T));
        }

        public override string ToString()
        {
            if (!IsSuccess)
            {
                return $"Result [Failed code : {code.ToString()}, Error Message : {errorMessage}]";
            }

            string dataString = "";

            if (data is IList)
            {
                foreach (object d in data as IList)
                {
                    dataString += d + ", ";
                }
                if (dataString.Length > 2)
                {
                    dataString = dataString.Remove(dataString.Length - 2);
                }
            }
            else
            {
                dataString = data is object ? data.ToString() : "null";
            }
            return $"Result [SUCCESS data : {dataString}]";
        }
    }
}
