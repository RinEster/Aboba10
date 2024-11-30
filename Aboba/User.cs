using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Aboba
{

    //    static void Main()
    //    {
    //        User Alice = new User();
    //        Alice.id = 0;
    //        Alice.preferences = new int[] {1, 0, 0, 1};

    //        User Bob = new User();
    //        Bob.id = 1;
    //        Bob.preferences = new int[] { 0, 1, 1, 0 };

    //        User Candice = new User();
    //        Candice.id = 2;
    //        Candice.preferences = new int[] { 1, 1, 1, 0 };

    //        User David = new User();
    //        David.id = 3;
    //        David.preferences = new int[] { 1, 1, 1, 0 };

    //        var a= David.recommendUsers(David, new User[] { Alice, Bob, Candice, David});
    //        foreach (var item in a) {
    //            System.Console.WriteLine(item);
    //        }
    //    }
    //}



    public class User //минимальные требования для работы функции рекомендации, сам класс переделайте под БД
    {
        public string login;
        public string[] preferences;

        private static int SameAnswers(string[] a, string[] b)
        {
            int d = 0;
            for (int i = 0; i < a.Length; ++i)
            {   
                for (int j = 0; j < b.Length; ++j)
                {
                    if (a[i] == b[j]) d++; break;
                }
            }
            return d;
        }

        public string[] recommendUsers(User user, User[] users)
        {
            object[][] prefOfUsers = new object[users.Length][];
            int j = 0;
            foreach (User u in users)
            {
                if (u.login == user.login) continue;
                prefOfUsers[j] = new object[2] { SameAnswers(user.preferences, u.preferences), u.login };
                ++j;
            }

            object[][] prefOfUsers1 = new object[users.Length - 1][];

            for (int i = 0; i < users.Length - 1; ++i)
            {
                prefOfUsers1[i] = new object[2];
                prefOfUsers1[i][0] = (int) prefOfUsers[i][0];
                prefOfUsers1[i][1] = (string) prefOfUsers[i][1];
                //prefOfUsers1[i].Primary = prefOfUsers1[0];
                //prefOfUsers1[i].Secondary = prefOfUsers1[1];
            }

            var sorted = prefOfUsers1.OrderBy(p => p[0])
                 .ThenBy(p => p[1]);

            //.OrderBy(p => p.Primary);
            //List<int> prefOfUsers1 = prefOfUsers.OrderBy(prefOfUsers => prefOfUsers[0]);
            string[] prefOfUsers2 = new string[users.Length - 1];

            int k = 0;
            foreach (var u in sorted)
            {
                prefOfUsers2[k] = (string) u[1];
                ++k;
            }
            prefOfUsers2 = prefOfUsers2.Reverse().ToArray();
            return prefOfUsers2;
        }
    }
}

