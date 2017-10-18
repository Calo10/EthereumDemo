using System;
using System.Linq;
using Realms;

namespace EthereumDemoApp.Models
{
    public class Member : RealmObject
    {

        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string contract { get; set; }

        public Member()
        {
            
        }

        public static bool saveContract(string contract, string email)
        {
            var realm = Realm.GetInstance();

            var allMembers = realm.All<Member>().Where(m => m.contract == contract);


            if(allMembers.Count() == 0){

                realm.Write(() =>
                {
                    Member myMember = realm.CreateObject("Member");
                    myMember.contract = contract;
                    myMember.email = email;
                });

                return true;

            }

            return false;

        }

        public static string GetContract(string email)
        {
            var realm = Realm.GetInstance();

            Member member = realm.All<Member>().Where(m => m.email == email).FirstOrDefault();

            return member.contract;
        }

        public static bool validateIfExist(string email)
        {

            var realm = Realm.GetInstance();

            Member member = realm.All<Member>().Where(m => m.email == email).FirstOrDefault();

            if(member != null){

                return true;
            }

            return false;
        }
    }
}
