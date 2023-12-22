namespace ImageDiffFinder.Enums
{


    #region Language dictionary
    public static class MyDictionary
    {
        public static Dictionary<string, string> CoursePr = new Dictionary<string, string>
        {
            { "CourseID", "شناسه"},
            { "Title", "درس"},
            { "Credits", "واحد نظری"},
            { "DepartmentName", "دانشکده" },
            { "InstructorName", "استاد" },
            { "CreditsPractical", "واحد عملی" },
            { "Day"       , "روز تشکیل" },
            { "Time"       , "ساعت تشکیل" },
            { "ExamDate"        , "تاریخ امتحان" },
            { "ExamTime"        , "ساعت امتحان" },
            { "Capacity"        , "ظرفیت باقی مانده" },
            { "CapacityTotal"        , "ظرفیت" },
            { "Notes"           , "توضیحات" }

        };
        public static Dictionary<string, string> CourseEn = new Dictionary<string, string>
        {
            { "CourseID", "Course Id"},
            { "Title", "Title"},
            { "Credits", "Theoretical Credits"},
            { "DepartmentName", "Department" },
            { "InstructorName", "Instructor" },
            { "CreditsPractical", "Practical Credits"},
            { "Day"             , "Day"},
            { "Time"            , "Time"},
            { "ExamDate"        , "Exam Date"},
            { "ExamTime"        , "Exam Time"},
            { "Capacity"        , "Remained Capacity"},
            { "CapacityTotal"        , "Capacity"},
            { "Notes"           , "Notes"}
        };



        public static Dictionary<string, string> EnrollmentPr = new Dictionary<string, string>
        {
            { "ID"                  , "شناسه" },
            { "CourseID"            , "شناسه درس" },
            { "CourseTitle"         , "درس" },
            { "CourseDay"         , "روز تشکیل" },
            { "CourseTime"         , "ساعت تشکیل" },
            { "StudentID"           , "شماره دانشجویی" },
            { "StudentFullName"     , "دانشجو" },
            { "InstructorFullName"  , "استاد" },
            { "DepartmentName"      , "دانشکده" }

        };
        public static Dictionary<string, string> EnrollmentEn = new Dictionary<string, string>
        {
            { "ID"                  , "Id" },
            { "CourseID"            , "Course Id" },
            { "CourseTitle"         , "Course" },
            { "CourseDay"         , "Course Day" },
            { "CourseTime"         , "Course Time" },
            { "StudentID"           , "Student Id" },
            { "StudentFullName"     , "Student" },
            { "InstructorFullName"  , "Instructor" },
            { "DepartmentName"      , "Department" }
        };




        public static Dictionary<string, string> DepartmentEn = new Dictionary<string, string>
        {
            { "DepartmentID" , "Id" },
            { "Name"         , "Name" },
            { "Budget"       , "Budget" },
            { "StartDate"    , "Start date" },
            { "Courses"      , "Courses" },
            { "Enrollments"  , "Enrollments" },
            { "Instructors"  , "Instructors" }

        };
        public static Dictionary<string, string> DepartmentPr = new Dictionary<string, string>
        {
            { "DepartmentID"                  , "شناسه" },
            { "Name"            , "نام" },
            { "Budget"         , "بودجه" },
            { "StartDate"           , "تاریخ شروع" },
            { "Courses"     , "دروس" },
            { "Enrollments"  , "ثبت نام ها" },
            { "Instructors"      , "اساتید" }
        };









        public static Dictionary<string, string> InstructorEn = new Dictionary<string, string>
        {
            { "HireDate" , "Hire date" },
            { "Courses"         , "Courses" },
            { "Departments"       , "Departments" },
            { "Enrollments"    , "Enrollments" },
            { "OfficeAssignment"      , "Office" },

            // Credentials
            { "ID"                  , "Id" },
            { "UserName"      , "Username" },
            { "Password"      , "Password" },

            // Person
            { "LastName"      , "Last name" },
            { "FirstMidName"      , "First name" },
            { "FullName"      , "Name" },
            { "BirthDate"      , "Birthdate" },
            { "Email"      , "Email" },
        };
        public static Dictionary<string, string> InstructorPr = new Dictionary<string, string>
        {
            { "HireDate" , "تاریخ استخدام" },
            { "Courses"         , "دروس" },
            { "Departments"       , "دانشکده ها" },
            { "Enrollments"    , "ثبت نام ها" },
            { "OfficeAssignment"      , "دفتر" },

             // Credentials
            { "ID"                  , "شناسه" },
            { "UserName"      , "نام کاربری" },
            { "Password"      , "رمز عبور" },

            // Person
            { "LastName"      , "نام خانوادگی" },
            { "FirstMidName"      , "نام" },
            { "FullName"      , "نام" },
            { "BirthDate"      , "تولد" },
            { "Email"      , "ایمیل" },
        };




        public static Dictionary<string, string> StudentEn = new Dictionary<string, string>
        {
            { "EnrollmentDate" , "Enrollment date" },
            { "Enrollments"         , "Enrollments" },
            { "DepartmentName", "Department" },


            // Person
            { "ID"                  , "Student Id" },
            { "UserName"      , "Username" },
            { "Password"      , "Password" },

            // Credentials
            { "LastName"      , "Last name" },
            { "FirstMidName"      , "First name" },
            { "FullName"      , "Name" },
            { "BirthDate"      , "Birthdate" },
            { "Email"      , "Email" },
        };
        public static Dictionary<string, string> StudentPr = new Dictionary<string, string>
        {
            { "EnrollmentDate" , "تاریخ ثبت نام" },
            { "Enrollments"         , "دروس" },
            { "DepartmentName", "دانشکده" },

             // Person
            { "ID"                  , "شماره دانشجویی" },
            { "UserName"      , "نام کاربری" },
            { "Password"      , "رمز عبور" },

            // Credentials
            { "LastName"      , "نام خانوادگی" },
            { "FirstMidName"      , "نام" },
            { "FullName"      , "نام" },
            { "BirthDate"      , "تولد" },
            { "Email"      , "ایمیل" },
        };




        public static Dictionary<string, string> ManagerEn = new Dictionary<string, string>
        {
            { "HireDate" , "Hire date" },
            { "DepartmentName", "Department" },

            // Credentials
            { "ID"                  , "Id" },
            { "UserName"      , "Username" },
            { "Password"      , "Password" },

            // Person
            { "LastName"      , "Last name" },
            { "FirstMidName"      , "First name" },
            { "FullName"      , "Name" },
            { "BirthDate"      , "Birthdate" },
            { "Email"      , "Email" },
        };
        public static Dictionary<string, string> ManagerPr = new Dictionary<string, string>
        {
            { "HireDate" , "تاریخ استخدام" },
            { "DepartmentName", "دانشکده" },

             // Credentials
            { "ID"                  , "شناسه" },
            { "UserName"      , "نام کاربری" },
            { "Password"      , "رمز عبور" },

            // Person
            { "LastName"      , "نام خانوادگی" },
            { "FirstMidName"      , "نام" },
            { "FullName"      , "نام" },
            { "BirthDate"      , "تولد" },
            { "Email"      , "ایمیل" },
        };




        public static Dictionary<string, string> SignInEn = new Dictionary<string, string>
        {
            { "UserName"      , "Username" },
            { "Password"      , "Password" },
        };
        public static Dictionary<string, string> SignInPr = new Dictionary<string, string>
        {
            { "UserName"      , "نام کاربری" },
            { "Password"      , "رمز عبور" },
        };

    }
    #endregion
}

