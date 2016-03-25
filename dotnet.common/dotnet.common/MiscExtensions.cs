using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace dotnet.common
{
    public static class MiscExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
        public static void Retry<T>(this Action action, int retryLimit = 4, int sleepMillicsecond = 0) where T : Exception
        {
            int retry = 0;
            bool success = false;
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

        public static R Retry<T, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0) where T : Exception
        {
            int retry = 0;
            bool success = false;
            R result = default(R);
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

        public static R Retry<T, T2, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0) where T : Exception where T2 : Exception
        {
            int retry = 0;
            bool success = false;
            R result = default(R);
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

        public static R Retry<T, T2, T3, R>(this Func<R> action, int retryLimit = 4, int sleepMillicsecond = 0)
            where T : Exception where T2 : Exception where T3 : Exception
        {
            int retry = 0;
            bool success = false;
            R result = default(R);
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
