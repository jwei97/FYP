using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Helper
{
    public class ConstantManager
    {
        public class SecurityConstants
        {
            public static readonly IList<String> RoleType = new ReadOnlyCollection<string>
            (new List<String> {
                ACCOUNT_TYPE_STUDENT,
                ACCOUNT_TYPE_LECTURE,
                ACCOUNT_TYPE_ADMIN,
            });

            public const string ACCOUNT_TYPE_STUDENT = "Student";
            public const string ACCOUNT_TYPE_LECTURE = "Lecture";
            public const string ACCOUNT_TYPE_ADMIN = "Admin";

            public int CURRENT_LOGIN_ID;

        }


        public class DocumentStatusConstants
        {
            public static readonly IList<String> statusList = new ReadOnlyCollection<string>
            (new List<String> {
                PENDING,
                SUCCESS,
                CANCEL,
            });

            public const string PENDING = "Pending";
            public const string SUCCESS = "Success";
            public const string CANCEL = "Cancel";

        }

        public class HttpSessionName
        {
            public const string CURRENT_ID = "Id";
            public const string CURRENT_USERNAME = "Username";
            public const string CURRENT_PASSWORD = "Password";
            public const string CURRENT_NAME = "Name";
            public const string CURRENT_TYPE = "Type";
        }
    }
}
