using DatingCode.Core;
using System;

namespace DatingStorage.Storages
{
    public class StorageHelper
    {
        internal Maybe<T> GetQueryMaybe<T>(Func<T> unsafeGetValueFunc)
        {
            try
            {
                var value = unsafeGetValueFunc();
                var result = Maybe<T>.NewSuccess(value);
                return result;
            }
            catch (Exception ex)
            {
                var result = Maybe<T>.NewFailure(ex.ToString());
                return result;
            }
        }

        internal Result<T> GetQueryResult<T>(Func<T> unsafeGetValueFunc)
        {
            try
            {
                var value = unsafeGetValueFunc();
                var result = Result<T>.NewSuccess(value);
                return result;
            }
            catch (Exception ex)
            {
                var result = Result<T>.NewFailure(ex.ToString());
                return result;
            }
        }

        internal Result GetResult(Action unsafeAction)
        {
            try
            {
                unsafeAction();
                var result = Result.NewSuccess();
                return result;
            }
            catch (Exception ex)
            {
                var result = Result.NewFailure(ex.ToString());
                return result;
            }
        }
    }
}
