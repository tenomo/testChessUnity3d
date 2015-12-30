using System; 
using System.Reflection;
namespace Chess
{

    // Исходный код взят с сайта https://ru.wikipedia.org/wiki/Одиночка_(шаблон_проектирования)
    /// <summary>
    /// Generic Singleton<T> (потокобезопасный с использованием generic-класса и с отложенной инициализацией)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        /// Защищённый конструктор необходим для того, чтобы предотвратить создание экземпляра класса Singleton. 
        /// Он будет вызван из закрытого конструктора наследственного класса.
        protected Singleton() { }

        /// Фабрика используется для отложенной инициализации экземпляра класса
        private sealed class SingletonCreator<S> where S : class
        {
            //Используется Reflection для создания экземпляра класса без публичного конструктора
            private static readonly S instance = (S)typeof(S).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new Type[0],
                        new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance ()
            {
                 return instance; 
                
            }
        }
        public static T Instance
        {
            get { return Singleton<T>.SingletonCreator<T>.CreatorInstance(); } 
        }

    }
}
 
