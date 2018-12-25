using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    class Program
    {

        /// <summary>
        /// Класс данных
        /// </summary>
        public class Worker
        {
            /// <summary>
            /// Ключ
            /// </summary>
            public int id_;

            /// <summary>
            /// Для группировки
            /// </summary>
            public string surname_;

            /// <summary>
            /// Значение
            /// </summary>
            public int idSection_;

            /// <summary>
            /// Конструктор
            /// </summary>
            public Worker(int id, string surname, int idSection)
            {
                this.id_ = id;
                this.surname_ = surname;
                this.idSection_ = idSection;
            }

            /// <summary>
            /// Приведение к строке
            /// </summary>
            public override string ToString()
            {
                return "(id=" + this.id_.ToString() + "; surname=" + this.surname_ + "; idSection=" + this.idSection_.ToString() + ")";
            }
        }

        /// <summary>
        /// Класс для сравнения данных
        /// </summary>
        public class WorkerEqualityComparer : IEqualityComparer<Worker>
        {

            public bool Equals(Worker x, Worker y)
            {
                bool Result = false;
                if (x.id_ == y.id_ && x.surname_ == y.surname_ && x.idSection_ == y.idSection_) Result = true;
                return Result;
            }

            public int GetHashCode(Worker obj)
            {
                return obj.id_;
            }
        }

        public class Section
        {
            /// <summary>
            /// Ключ
            /// </summary>
            public int id_;

            /// <summary>
            /// Для группировки
            /// </summary>
            public string name_;

            /// <summary>
            /// Конструктор
            /// </summary>
            public Section(int id, string name)
            {
                this.id_ = id;
                this.name_ = name;
            }

            /// <summary>
            /// Приведение к строке
            /// </summary>
            public override string ToString()
            {
                return "(id=" + this.id_.ToString() + "; name=" + this.name_ + ")";
            }
        }

        public class SectionEqualityComparer : IEqualityComparer<Section>
        {

            public bool Equals(Section x, Section y)
            {
                bool Result = false;
                if (x.id_ == y.id_ && x.name_ == y.name_) Result = true;
                return Result;
            }

            public int GetHashCode(Section obj)
            {
                return obj.id_;
            }
        }

        public class Staffer
        {
            /// <summary>
            /// Ключ
            /// </summary>
            public int id_;

            /// <summary>
            /// Для группировки
            /// </summary>
            public int idSection_;

            /// <summary>
            /// Конструктор
            /// </summary>
            public Staffer(int id, int idSection)
            {
                this.id_ = id;
                this.idSection_ = idSection;
            }

            /// <summary>
            /// Приведение к строке
            /// </summary>
            public override string ToString()
            {
                return "(id=" + this.id_.ToString() + "; idSection=" + this.idSection_ + ")";
            }
        }

        public class StafferEqualityComparer : IEqualityComparer<Staffer>
        {

            public bool Equals(Staffer x, Staffer y)
            {
                bool Result = false;
                if (x.id_ == y.id_ && x.idSection_ == y.idSection_) Result = true;
                return Result;
            }

            public int GetHashCode(Staffer obj)
            {
                return obj.id_;
            }
        }

        //Пример данных
        static List<Worker> workerList = new List<Worker>()
        {
            new Worker(1, "zubkov", 1),
            new Worker(2, "smirnov", 2),
            new Worker(3, "ivanov", 1),
            new Worker(4, "petrov", 2),
            new Worker(5, "ivanova", 2),
            new Worker(6, "petrova", 1),
            new Worker(7, "alexandrov", 3),
            new Worker(8, "alekseev", 1)
        };

        static List<Section> sectionList = new List<Section>()
        {
            new Section(1, "otdel1"),
            new Section(2, "otdel2"),
            new Section(3, "otdel3")
        };

        static List<Staffer> stafferList = new List<Staffer>()
        {
            new Staffer(1, 1),
            new Staffer(1, 3),
            new Staffer(2, 1),
            new Staffer(3, 2),
            new Staffer(3, 1)
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Вывод сотрудников с сортировкой по отделам");
            var q1 = from x in workerList
                     join y in sectionList on x.idSection_ equals y.id_ into temp
                     orderby x.idSection_ ascending, x.surname_ ascending
                     select new { tmpWorker = x, tmpSection = temp };
            foreach (var x in q1)
            {
                Console.Write(x.tmpWorker);
                foreach (var y in x.tmpSection)
                    Console.WriteLine(" section name: " + y.name_);
            }

            Console.WriteLine("\nВывод сотрудников, у которых фамилия начинается с А");
            var q2 = from x in workerList
                     where x.surname_[0] == 'a'
                     select x;
            foreach (var x in q2) Console.WriteLine(x);

            Console.WriteLine("\nВывод всех отделов и количество работающих сотрудников");
            var q3 = from x in sectionList
                     join y in workerList on x.id_ equals y.idSection_ into tmp
                     select new { tmpSection = x, countWorkers = tmp.Count() };
            foreach (var x in q3)
            {
                Console.WriteLine("{0}; count workers = {1}", x.tmpSection, x.countWorkers);
            }

            Console.WriteLine("\nСписок отделов, у которых фамилии всех сотрудников начинаются с а");
            var q4 = from x in sectionList
                     join y in workerList on x.id_ equals y.idSection_ into tmp
                     where tmp.All(y => y.surname_[0] == 'a')
                     select new { tmpSection = x, workers = tmp };
            foreach (var x in q4)
            {
                Console.WriteLine("{0}:", x.tmpSection);
                foreach (var y in x.workers)
                {
                    Console.WriteLine(y);
                }
            }

            Console.WriteLine("\nСписок отделов, у которых фамилия хотя бы одного сотрудника начинаются с а");
            var q5 = from x in sectionList
                     join y in workerList on x.id_ equals y.idSection_ into tmp
                     where tmp.Any(y => y.surname_[0] == 'a')
                     select new { tmpSection = x, workers = tmp };
            foreach (var x in q5)
            {
                Console.WriteLine("{0}:", x.tmpSection);
                foreach (var y in x.workers)
                {
                    Console.WriteLine(y);
                }
            }

            Console.WriteLine("\nСписок всех отделов и список всех сотрудников в каждом отделе");
            var q6 = from x in sectionList
                     join y in stafferList on x.id_ equals y.idSection_ into tmp
                     select new { tmpSection = x, tmpWorkers = tmp };
            foreach (var x in q6)
            {
                Console.WriteLine("otdel: {0}", x.tmpSection);
                foreach (var y in x.tmpWorkers)
                {
                    Console.WriteLine("staffer: {0}", y);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nСписок всех отделов и количество сотрудников в каждом отделе");
            var q7 = from x in sectionList
                     join y in stafferList on x.id_ equals y.idSection_ into tmp
                     select new { tmpSection = x, countWorkers = tmp.Count() };
            foreach (var x in q7)
            {
                Console.WriteLine("otdel: {0}; countWorkers = {1}", x.tmpSection, x.countWorkers);
            }

            Console.ReadKey();
        }
    }
}
