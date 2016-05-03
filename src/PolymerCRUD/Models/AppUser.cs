using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace PolymerCRUD.Models
{
    [Table("appuser")]
    public class AppUser : ModelBase<AppUser>
    {
        [Key]
        public int idappuser { get; set; } = 0;
        public int idappusertype { get; set; } = 4;
        public string email { get; set; } = "";
        private string _pass = "";
        public string name { get; set; } = "";
        public string surname { get; set; } = "";
        public bool active { get; set; } = true;

        private static string[] userTypeList
            = new string[] { "Global administrator",
                            "Deck administrator",
                            "Moderator",
                            "User" };

        public AppUser() : base("appuser")
        {
        }

        [Editable(false)]
        public override int id
        {
            get
            {
                return idappuser;
            }
            set
            {
                idappuser = value;
            }
        }

        [Editable(false)]
        public override string Description
        {
            get
            {
                return "User " + name + " " + surname;
            }
        }

        [Editable(false)]
        public string pass
        {
            get
            {
                return "";
            }
            set
            {
                _pass = value;
            }
        }

        public override void PostSave()
        {
            if (_pass != "")
            {
                //System.Security.Cryptography
                //string encrypted = "";

                dbConnection.Execute("UPDATE AppUser set pass='" + _pass.Replace("'", "''") + "' Where idappuser = " + idappuser);
            }
        }

        [Editable(false)]
        public string AppUserType
        {
            get
            {
                if (idappusertype == 0)
                {
                    return "";
                }

                return userTypeList[idappusertype - 1];
            }
        }

        [Editable(false)]
        public string AppUserActive
        {
            get
            {
                return (active ? "Active" : "NOT Active");
            }
        }
    }
}
