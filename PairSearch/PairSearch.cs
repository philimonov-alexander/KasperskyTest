using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Task2
{
    public static class PairSearch<T> where T : struct
    {
        private static readonly Func<T, T, T> add;

        static PairSearch()
        {
            try
            {
                var left = Expression.Parameter(typeof(T), "left");
                var right = Expression.Parameter(typeof(T), "right");
                add = Expression.Lambda<Func<T, T, T>>(Expression.Add(left, right), left, right).Compile();
            }
            catch (InvalidOperationException) {}
        }

        /// <summary>
        /// Поиск пар элементов массива, сумма которых образует заданное значение
        /// </summary>
        /// <param name="array">Массив элементов</param>
        /// <param name="target">Заданное значение суммы для пары</param>
        /// <returns>Список пар элементов</returns>
        ///  <exception cref="System.InvalidOperationException">Тип элемента не поддерживает операцию сложения</exception>
        public static List<Tuple<T, T>> GetPairs(T[] array, T target) 
        {
            var returnedList = new List<Tuple<T, T>>();
            if (array == null)
                return returnedList;

            if (add == null)
                throw new InvalidOperationException("Операция сложения не применима для данного типа: " + typeof(T));

            var nullableArray = new Nullable<T>[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                nullableArray[i] = array[i];
            }

            for (var i = 0; i < array.Length - 1; i++)
            {
                if (nullableArray[i] == null)
                    continue;

                for (var j = i + 1; j < array.Length; j++)
                {
                    if (nullableArray[j] == null)
                        continue;

                    if (add(nullableArray[i].Value, nullableArray[j].Value).Equals(target))
                    {
                        returnedList.Add(new Tuple<T, T>(nullableArray[i].Value, nullableArray[j].Value));
                        nullableArray[i] = nullableArray[j] = null;
                        break;
                    }
                }
            }

            return returnedList;
        }
    }
}
