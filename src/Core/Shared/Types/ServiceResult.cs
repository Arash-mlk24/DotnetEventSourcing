using DotnetEventSourcing.src.Core.Shared.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace DotnetEventSourcing.src.Core.Shared.Types;

public class AppError
{
    public int Code { get; set; }
    public string Message { get; set; }

    public KeyValueList<string, string> ExtraFields { get; set; }

}
public class ServiceResult
{
    public AppError Error { get; set; }
    public string Message { get; set; }
    public string ReferenceId { get; set; }
    public bool HasError => Error != null;

    public ServiceResult()
    {
        ReferenceId = Guid.NewGuid().ToLong().ToString();
    }
    public ServiceResult(string error) : this()
    {
        Error = new AppError() { Message = error };
    }
    public ServiceResult(string error, string message) : this(error)
    {
        Message = message;
    }

    public virtual ServiceResult SetMessage(string msg)
    {
        Message = msg;
        return this;
    }

    public virtual ServiceResult SetError(string err, int code = 500)
    {
        Error = new AppError { Message = err, Code = code };
        return this;
    }

    public virtual ActionResult OK()
    {
        return OK(this);
    }

    public virtual ActionResult Bad()
    {
        return Bad(this);
    }

    protected ActionResult OK(object data)
    {
        return new OkObjectResult(data);
    }

    protected ActionResult Bad(object data)
    {
        return new BadRequestObjectResult(this);
    }

    public Task<ServiceResult> ToAsync()
    => Task.FromResult(this);

    public ServiceResult<T> To<T>(T data = default)
    => From(this, data);


    public AppException ToException()
    => new AppException(this);



    public virtual Task<ActionResult> AsyncResult(bool ok = true)
    {
        var isOk = ok && !HasError;

        return Task.FromResult(isOk ? OK() : Bad());
    }


    public static ServiceResult Empty => new ServiceResult();
    public static ServiceResult<T> Create<T>(T data) => new ServiceResult<T>(data);
    public static ServiceResult<T> From<T>(ServiceResult sr, T data)
    {
        var res = new ServiceResult<T>(data);
        res.Message = sr.Message;
        res.Error = sr.Error;
        res.ReferenceId = sr.ReferenceId;
        return res;
    }


}

public class ServiceResult<T> : ServiceResult
{

    public ServiceResult(T data)
    {
        Result = data;
    }


    public T Result { get; set; }

    public override ActionResult Bad()
    {
        return Bad(this);
    }

    public override ActionResult OK()
    {
        return OK(this);
    }

    public override Task<ActionResult> AsyncResult(bool ok = true)
    {
        var isOk = ok && !HasError;
        return Task.FromResult(isOk ? OK() : Bad());
    }

    public new Task<ServiceResult<T>> ToAsync()
    => Task.FromResult(this);
    public static ServiceResult<T> From(ServiceResult sr, T data)
    {
        var res = new ServiceResult<T>(data);
        res.Message = sr.Message;
        res.Error = sr.Error;
        res.ReferenceId = sr.ReferenceId;
        return res;
    }

}