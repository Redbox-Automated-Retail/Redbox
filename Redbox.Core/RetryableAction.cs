using System;
using System.Threading;

namespace Redbox.Core
{
    public static class RetryableAction
    {
        static RetryableAction()
        {
            DefaultRethrowEvaluator = new CustomRethrowEvaluator((i, e) => i == 4);
            DefaultSleepEvaluator = new RandomExponentialSleepEvaluator(5);
        }

        private static IRethrowEvaluator DefaultRethrowEvaluator { get; }

        private static ISleepEvaluator DefaultSleepEvaluator { get; }

        public static void Execute(Action action)
        {
            Execute(action, DefaultRethrowEvaluator, DefaultSleepEvaluator);
        }

        public static void Execute(Action action, IRethrowEvaluator rethrowEvaluator)
        {
            Execute(action, rethrowEvaluator, DefaultSleepEvaluator);
        }

        public static void Execute(
            Action action,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }

        public static TResult Execute<TResult>(
            Func<TResult> function,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }

        public static TResult Execute<T1, TResult>(
            Func<T1, TResult> function,
            T1 argument1,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return function(argument1);
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }

        public static TResult Execute<T1, T2, TResult>(
            Func<T1, T2, TResult> function,
            T1 argument1,
            T2 argument2,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return function(argument1, argument2);
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }

        public static TResult Execute<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> function,
            T1 argument1,
            T2 argument2,
            T3 argument3,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return function(argument1, argument2, argument3);
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }

        public static TResult Execute<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> function,
            T1 argument1,
            T2 argument2,
            T3 argument3,
            T4 argument4,
            T5 argument5,
            IRethrowEvaluator rethrowEvaluator,
            ISleepEvaluator sleepEvaluator)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return function(argument1, argument2, argument3, argument4, argument5);
                }
                catch (Exception ex)
                {
                    if (rethrowEvaluator.Rethrow(retryCount, ex))
                        throw;
                    Thread.Sleep(sleepEvaluator.SleepMilliseconds(retryCount, ex));
                }

                ++retryCount;
            }
        }
    }
}