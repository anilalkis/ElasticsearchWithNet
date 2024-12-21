using System.Net;

namespace Elasticsearch.API.Dtos
{
    public record ResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public HttpStatusCode Status { get; set; }
        public static ResponseDto<T> Succsess(T Data,HttpStatusCode Status)
        {
            return new ResponseDto<T> { Data = Data, Status = Status };
        }
        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode Status)
        {
            return new ResponseDto<T> {Errors = errors, Status = Status };
        }

        public static ResponseDto<T> Fail(string errors, HttpStatusCode Status)
        {
            return new ResponseDto<T> {Errors = new List<string> { errors }, Status = Status };
        }
    }
}
