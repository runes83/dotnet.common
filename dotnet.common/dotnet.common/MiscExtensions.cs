using System;
using System.Collections.Generic;
using System.Threading;

namespace dotnet.common
{
    /// <summary>
    ///     Misc extensions and helper methods
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        ///     Enables to do a action on elements in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Enables to do an operation and retry if speficied exception is thrown
        /// </summary>
        /// <typeparam name="T">Type of exception to retry for</typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">How many times to retry</param>
        /// <param name="sleepMillicsecond">How long delay between each retry</param>
        public static void Retry<T>(this Action action, int retryLimit = 4, int sleepMillicsecond = 0)
            where T : Exception
        {
            var retry = 0;
            var success = false;
            while (!success && retry <= retryLimit)
            {
                try
                {
                    action();
                    success = true;
                }
                catch (T t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
            }
        }

        /// <summary>
        ///     Enables to do an operation and retry if speficied exception is thrown
        /// </summary>
        /// <typeparam name="T">Type of exception to retry for</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">How many times to retry</param>
        /// <param name="sleepMillicsecond">How long delay between each retry</param>
        /// <returns>Result of the operation if success on type specified by R</returns>
        public static R Retry<T, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0)
            where T : Exception
        {
            var retry = 0;
            var success = false;
            var result = default(R);
            while (!success && retry <= retryLimit)
            {
                try
                {
                    result = action();
                    success = true;
                }
                catch (T t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
            }

            return result;
        }

        /// <summary>
        ///     Enables to do an operation and retry if speficied exception is thrown
        /// </summary>
        /// <typeparam name="T">Type of exception to retry for</typeparam>
        /// <typeparam name="T2">Type of exception to retry for</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">How many times to retry</param>
        /// <param name="sleepMillicsecond">How long delay between each retry</param>
        /// <returns>Result of the operation if success on type specified by R</returns>
        public static R Retry<T, T2, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0)
            where T : Exception where T2 : Exception
        {
            var retry = 0;
            var success = false;
            var result = default(R);
            while (!success && retry <= retryLimit)
            {
                try
                {
                    result = action();
                    success = true;
                }
                catch (T t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
                catch (T2 t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
            }

            return result;
        }

        /// <summary>
        ///     Enables to do an operation and retry if speficied exception is thrown
        /// </summary>
        /// <typeparam name="T">Type of exception to retry for</typeparam>
        /// <typeparam name="T2">Type of exception to retry for</typeparam>
        /// <typeparam name="T3">Type of exception to retry for</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">How many times to retry</param>
        /// <param name="sleepMillicsecond">How long delay between each retry</param>
        /// <returns>Result of the operation if success on type specified by R</returns>
        public static R Retry<T, T2, T3, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0)
            where T : Exception where T2 : Exception where T3 : Exception
        {
            var retry = 0;
            var success = false;
            var result = default(R);
            while (!success && retry <= retryLimit)
            {
                try
                {
                    result = action();
                    success = true;
                }
                catch (T t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
                catch (T2 t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
                catch (T3 t)
                {
                    if (!success && retry >= retryLimit)
                        throw t;

                    if (sleepMillicsecond > 0)
                        Thread.Sleep(sleepMillicsecond);
                    retry++;
                }
            }

            return result;
        }
    }
}