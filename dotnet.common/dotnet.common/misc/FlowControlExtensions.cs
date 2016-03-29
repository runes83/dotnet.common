using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace dotnet.common.misc
{
    //The MIT License(MIT)

    //Copyright(c) 2013 Vidar Kongsli

    //Permission is hereby granted, free of charge, to any person obtaining a copy of
    //this software and associated documentation files (the "Software"), to deal in
    //the Software without restriction, including without limitation the rights to
    //use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    //the Software, and to permit persons to whom the Software is furnished to do so,
    //subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all
    //copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
    //FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
    //COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
    //IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
    //CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


    public static class FlowControlExtensions
    {
        /// <summary>
        ///     Run action if the object have value, use to avoid null exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to run the action on</param>
        /// <param name="action">Action to perform</param>
        /// <param name="doContinue">If the object does not have value continue without throwing exception default true</param>
        [DebuggerStepThrough]
        public static void DoIfHasValue<T>(this T? obj, Action<T> action, bool doContinue = true) where T : struct
        {
            if (obj.HasValue)
            {
                action(obj.Value);
            }
            if (doContinue)
                return;
            ThrowInvalidOperationException<T, T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">Object to evaluate</param>
        /// <param name="func">Function to run</param>
        /// <param name="doContinue">If the object does not have value continue without throwing exception default true</param>
        /// <param name="defaultValue">Value to return if docontinue is true and the object does not have the value</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static TResult IfHasValue<T, TResult>(this T? obj, Func<T, TResult> func, bool doContinue = true,
            TResult defaultValue = default(TResult)) where T : struct
        {
            if (obj.HasValue)
            {
                return func(obj.Value);
            }
            return doContinue ? defaultValue : ThrowInvalidOperationException<T, TResult>();
        }

        /// <summary>
        ///     Run action if the object have value, use to avoid null exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to run the action on</param>
        /// <param name="func">Function to perform</param>
        /// <param name="doContinue">If the object does not have value continue without throwing exception default true</param>
        [DebuggerStepThrough]
        public static TResult IfNotNull<T, TResult>(this T obj, Expression<Func<T, TResult>> func,
            bool doContinue = true, TResult defaultValue = default(TResult)) where T : class
        {
            if (obj != null)
            {
                return func.Compile()(obj);
            }
            if (doContinue) return defaultValue;
            var parameterName = func.Parameters.First().Name;
            var parameterType = typeof (T).FullName;
            var visitor = new MemberNameCollector(parameterName);
            visitor.Visit(func.Body);
            throw new NullReferenceException(
                string.Format("Tried to reference .{0} on parameter with name '{1}' of type {2}, but it was null",
                    visitor.MemberName, parameterName, parameterType));
        }

        /// <summary>
        ///     Run action if the object have value, use to avoid null exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to run the action on</param>
        /// <param name="action">Action to perform</param>
        /// <param name="doContinue">If the object does not have value continue without throwing exception default true</param>
        [DebuggerStepThrough]
        public static void DoIfNotNull<T>(this T obj, Action<T> action, bool doContinue = true) where T : class
        {
            if (obj != null)
            {
                action(obj);
            }
            if (doContinue) return;
            throw new NullReferenceException(string.Format("Tried to reference an instance of {0}, but it was null",
                typeof (T).FullName));
        }

        [DebuggerStepThrough]
        private static TResult ThrowInvalidOperationException<T, TResult>()
        {
            throw new InvalidOperationException(
                string.Format("Tried to access value of nullable of {0}, but it had no value", typeof (T).FullName));
        }

        private class MemberNameCollector : ExpressionVisitor
        {
            private readonly string _parameterName;
            public string MemberName = "Unknown";

            public MemberNameCollector(string parameterName)
            {
                _parameterName = parameterName;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Object != null && node.Object.NodeType == ExpressionType.Parameter &&
                    _parameterName == node.Object.ToString())
                {
                    MemberName = node.Method.Name + "(...)";
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.NodeType == ExpressionType.MemberAccess && node.Expression.ToString() == _parameterName)
                {
                    MemberName = node.Member.Name;
                }
                return base.VisitMember(node);
            }
        }
    }
}