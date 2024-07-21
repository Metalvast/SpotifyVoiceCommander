namespace SpotifyVoiceCommander.Maui.Shared.Lib.ErrorOr;

public static class ErrorOrExt
{
    public static TValue Unwrap<TValue>(this ErrorOr<TValue> errorOr) =>
        errorOr.Value;

    public static ErrorOr<TValue> FailIf<TValue>(
        this ErrorOr<TValue> wrapper,
        Func<TValue, bool> onValue,
        Func<TValue, Error> errorBuilder)
    {
        if (wrapper.IsError)
        {
            return wrapper;
        }

        return onValue(wrapper.Value)
            ? errorBuilder(wrapper.Value)
            : wrapper;
    }

    public static async Task<ErrorOr<TValue>> FailIfAsync<TValue>(
        this ErrorOr<TValue> wrapper,
        Func<TValue, Task<bool>> onValue,
        Func<TValue, Task<Error>> errorBuilder)
    {
        if (wrapper.IsError)
        {
            return wrapper;
        }

        return await onValue(wrapper.Value).ConfigureAwait(false)
            ? await errorBuilder(wrapper.Value).ConfigureAwait(false)
            : wrapper;
    }

    public static async Task<ErrorOr<TValue>> FailIf<TValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<TValue, bool> onValue,
        Func<TValue, Error> errorBuilder)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.FailIf(onValue, errorBuilder);
    }

    public static async Task<ErrorOr<TValue>> FailIfAsync<TValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<TValue, Task<bool>> onValue,
        Func<TValue, Task<Error>> errorBuilder)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.FailIfAsync(onValue, errorBuilder);
    }
}
