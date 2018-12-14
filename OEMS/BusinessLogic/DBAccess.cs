using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OEMS
{
    public class DBAccess
    {
        // connection string
        private readonly string ConnectoionString = "Data Source=BPDESILVA;Initial Catalog=OEMS;Integrated Security=True;Pooling=False;User ID=sa;Password=123456";
        //validate login credentials 
        public bool ValidateLogin(string Username, string Password)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();//open connection
                //sql query
                string Query = "Select * from Accounts Where Username='"+Username+"' and Password = '"+Password+"'";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                //Read rows 
                while (reader.Read())
                {
                    //validate data
                    if (reader["Username"].ToString().Equals(Username) && reader["Password"].ToString().Equals(Password))
                    {
                        //Convert data to string
                        AccountSession.User.AccountId = Int32.Parse(reader["AccountId"].ToString());
                        AccountSession.User.Type = reader["Type"].ToString().ToCharArray()[0];
                        AccountSession.User.Username = reader["Username"].ToString();
                        AccountSession.User.FName = reader["FName"].ToString();
                        AccountSession.User.LName = reader["LName"].ToString();
                        if(AccountSession.User.Type == 'S')//Validate user check if student
                        {
                            //set data
                            var student = GetStudent(AccountSession.User.AccountId);
                            AccountSession.User.StudentId = student.StudentId;
                            AccountSession.User.Name = student.Name;
                            AccountSession.User.CourseId = student.CourseId;
                            AccountSession.Course = GetCourse(Int32.Parse(student.CourseId));
                            
                            AccountSession.AllCourses = GetCourses();
                            AccountSession.AllModules = GetModules();
                        }

                        return true;
                    }
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return false;
        }
        //validate user name
        public bool ValidateUsername(string Username)
        {
            //sql instance
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();//open connection
                //sql query
                string Query = "Select * from Accounts Where Username='" + Username + "'";
                SqlCommand command = new SqlCommand(Query, connection);//parse in query and data
                SqlDataReader reader = command.ExecuteReader();// execute commands on tables
                //loop till condition met
                while (reader.Read())
                {
                    //validate data
                    if (reader["Username"].ToString().Equals(Username))
                    {
                        return true;
                    }
                }

            }
            catch (Exception e)//throw exception
            {

            }
            finally
            {
                connection.Close();//close connection
            }

            return false;
        }



        #region Exams
        public List<ExamModel> GetExams(int StudentId)
        {
            var Exams = new List<ExamModel>();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT * FROM Exams WHERE StudentId =" + StudentId + "";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ExamModel exam = new ExamModel()
                    {
                        ExamId = Int32.Parse(reader["ExamId"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        ModuleId = Int32.Parse(reader["ModuleId"].ToString()),
                        Attempts = Int32.Parse(reader["Attempts"].ToString()),
                        Marks = Int32.Parse(reader["Marks"].ToString())

                    };
                    Exams.Add(exam);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return Exams;

        }



        public ExamModel GetExam(int StudentId, int ModuleId)
        {
            var Exam = new ExamModel();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT * FROM Exams WHERE StudentId =" + StudentId + " AND ModuleId=" + ModuleId + "";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Exam = new ExamModel()
                    {
                        ExamId = Int32.Parse(reader["ExamId"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        ModuleId = Int32.Parse(reader["ModuleId"].ToString()),
                        Attempts = Int32.Parse(reader["Attempts"].ToString()),
                        Marks = Int32.Parse(reader["Marks"].ToString())

                    };
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return Exam;

        }
        public void AddExam(int sid, int mid)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Exams (StudentId, ModuleId, Attempts, Marks) VALUES ('" + sid + "', '" + mid + "', '0', '0')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateExam(ExamModel exam)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Exams SET Attempts = '" + exam.Attempts + "', Marks = '" + exam.Marks + "' WHERE ExamId='" + exam.ExamId + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Students

        public int GetAvgMarks(int sid)
        {
            int avg = 0;
            int total = 0;
            int count = 0;
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();
                
                string Query = "SELECT * FROM Exams Where StudentId='"+sid+"' AND Attempts > 0";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    total += Int32.Parse(reader["Marks"].ToString());
                    count++;
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            if(count > 0)
            {
                avg = total / count;
            }
            return avg;
        }

        public UserModel GetStudent(int accId)
        {
            UserModel user = new UserModel();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Students.AccountId AS StudentAcc_id, Students.*  FROM Students LEFT JOIN Accounts ON Students.AccountID=Accounts.AccountID WHERE Students.AccountId =" + accId + "";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        CourseId = reader["CourseId"].ToString()
                    };

                    user = NewUser;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return user;

        }

        public List<UserModel> GetStudents()
        {
            var users = new List<UserModel>();

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Students.AccountId AS StudentAcc_id, Students.*  FROM Students LEFT JOIN Accounts ON Students.AccountID=Accounts.AccountID";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        StudentId = Int32.Parse(reader["StudentId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        CourseId = reader["CourseId"].ToString()
                    };

                    users.Add(NewUser);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return users;
            
        }

        public void CreateStudent(UserModel user)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Accounts (Username, Password, Type, FName, LName) output INSERTED.AccountId VALUES ('"+user.Username+ "','" + user.Password+ "','" + user.Type+ "','" + user.FName+ "','" + user.LName+ "')";
                SqlCommand command = new SqlCommand(Query, connection);
                int ID = (Int32)command.ExecuteScalar();

                user.Username = ID+"_"+user.FName;
                user.AccountId = ID;

                Query = "UPDATE Accounts SET Username = '"+user.Username+"' WHERE AccountId='"+ID+"'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "INSERT INTO Students (AccountId, Name, CourseId) VALUES ('" + user.AccountId + "','" + user.FName + " " + user.LName + "','" + user.CourseId + "')";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();



            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal void UpdateStudent(UserModel student)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Students SET Name='" + student.Name + "' WHERE AccountId = '" + student.AccountId + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "UPDATE Accounts SET Password='" + student.Password + "', FName='" + student.FName + "', LName='" + student.LName + "' WHERE AccountId = '" + student.AccountId + "'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Lecturers

        public List<UserModel> GetLecturers()
        {
            var users = new List<UserModel>();

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT Accounts.AccountId AS Account_id, Accounts.*, Lecturers.AccountId AS LecturerAcc_id, Lecturers.*  FROM Lecturers LEFT JOIN Accounts ON Lecturers.AccountID=Accounts.AccountID";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new UserModel
                    {
                        AccountId = Int32.Parse(reader["Account_id"].ToString()),
                        LecturerId = Int32.Parse(reader["LecturerId"].ToString()),
                        Type = reader["Type"].ToString().ToCharArray()[0],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FName = reader["FName"].ToString(),
                        LName = reader["LName"].ToString(),
                        Name = reader["Name"].ToString(),
                        Area = reader["Area"].ToString()
                    };

                    users.Add(NewUser);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return users;
        }

       

        public void CreateLecturer(UserModel user)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Accounts (Username, Password, Type, FName, LName) VALUES ('"+user.Username+ "','" + user.Password+ "','" + user.Type+ "','" + user.FName+ "','" + user.LName+ "')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "Select * from Accounts Where Username='" + user.Username + "'";
                command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Username"].ToString().Equals(user.Username))
                    {
                        user.AccountId = Int32.Parse(reader["AccountId"].ToString());
                    }
                }
                reader.Close();

                Query = "INSERT INTO Lecturers (AccountId, Name, Area) VALUES ('" + user.AccountId + "','" + user.FName + " " + user.LName + "','" + user.Area + "')";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal void UpdateLecturer(UserModel newuser)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Lecturers SET Name='" + newuser.Name + "', Area='" + newuser.Area + "' WHERE AccountId = '" + newuser.AccountId + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

                Query = "UPDATE Accounts SET Password='" + newuser.Password + "', FName='" + newuser.FName + "', LName='" + newuser.LName + "' WHERE AccountId = '" + newuser.AccountId + "'";
                command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Modules

        public List<ModuleModel> GetModules()
        {
            var modules = new List<ModuleModel>();

            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();
                string Query = "SELECT * FROM Modules";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var NewUser = new ModuleModel
                    {
                        ModuleID = Int32.Parse(reader["ModuleId"].ToString()),
                        Name = reader["Name"].ToString()
                    };

                    modules.Add(NewUser);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return modules;
        }

        public ModuleModel GetModule(int id)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);
            ModuleModel Module = new ModuleModel();
            try
            {
                connection.Open();
                string Query = "Select * from Modules Where ModuleId='" + id + "' ";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((Int32.Parse(reader["ModuleId"].ToString())) == id)
                    {
                        var temp = new ModuleModel()
                        {
                        ModuleID = Int32.Parse(reader["ModuleId"].ToString()),
                        Name = reader["Name"].ToString()
                        //LecturerID = Int32.Parse(reader["LecturerId"].ToString()),
                        //Lecturer = reader["Lecturer"].ToString()
                        };
                        Module = temp;
                    }
                }
                reader.Close();

                if (Module.ModuleID == id)
                {
                    Query = "Select * from Questions Where ModuleId='" + id + "' ";
                    command = new SqlCommand(Query, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if ((Int32.Parse(reader["ModuleId"].ToString())) == id)
                        {
                            var temp = new QuestionModel()
                            {
                                QuestionID = Int32.Parse(reader["QuestionId"].ToString()),
                                ModuleID = Int32.Parse(reader["ModuleId"].ToString()),
                                Question = reader["Question"].ToString(),
                                Answer = reader["Answer"].ToString(),
                                AlternateAnsOne = reader["OptionOne"].ToString(),
                                AlternateAnsTwo = reader["OptionTwo"].ToString(),
                                AlternateAnsThree = reader["OptionThree"].ToString()
                            };
                            Module.Questions.Add(temp);
                        }
                    }
                    reader.Close();
                    return Module;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public void CreateModule(ModuleModel module)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Modules (Name) VALUES ('" + module.Name + "')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Questions

        public void CreateQuestion(QuestionModel question)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "INSERT INTO Questions (ModuleId, Question, Answer, OptionOne, OptionTwo, OptionThree) VALUES " +
                    "('" + question.ModuleID + "','" + question.Question + "','" + question.Answer + "','" + question.AlternateAnsOne + "','" + question.AlternateAnsTwo + "','" + question.AlternateAnsThree + "')";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public QuestionModel GetQuestion(int id)
        {
            QuestionModel question = new QuestionModel();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();

                string Query = "Select * from Questions Where QuestionId='" + id + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((Int32.Parse(reader["QuestionId"].ToString())) == id)
                    {
                        var temp = new QuestionModel()
                        {
                            QuestionID = Int32.Parse(reader["QuestionId"].ToString()),
                            ModuleID = Int32.Parse(reader["ModuleId"].ToString()),
                            Question = reader["Question"].ToString(),
                            Answer = reader["Answer"].ToString(),
                            AlternateAnsOne = reader["OptionOne"].ToString(),
                            AlternateAnsTwo = reader["OptionTwo"].ToString(),
                            AlternateAnsThree = reader["OptionThree"].ToString()
                        };
                        question = temp;
                    }
                }
                reader.Close();
                
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return question;
        }

        internal void EditQuestion(QuestionModel question)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "UPDATE Questions SET ModuleId='"+question.ModuleID+ "', Question='" + question.Question + "', Answer='" + question.Answer + "', OptionOne='" + question.AlternateAnsOne + "', OptionTwo='" + question.AlternateAnsTwo + "'" +
                    ", OptionThree='" + question.AlternateAnsThree + "' WHERE QuestionId = '" + question.QuestionID + "'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal void DeleteQuestion(int qid)
        {
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                connection.Open();

                string Query = "DELETE FROM Questions WHERE QuestionId='"+qid+"'";
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        internal List<QuestionModel> GetRanQuestions(int moduleId)
        {
            List<QuestionModel> question = new List<QuestionModel>();
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                connection.Open();

                string Query = "Select TOP 15 * FROM Questions Where ModuleId='" + moduleId + "' ORDER BY NEWID()";
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((Int32.Parse(reader["ModuleId"].ToString())) == moduleId)
                    {
                        var temp = new QuestionModel()
                        {
                            QuestionID = Int32.Parse(reader["ModuleId"].ToString()),
                            ModuleID = Int32.Parse(reader["ModuleId"].ToString()),
                            Question = reader["Question"].ToString(),
                            Answer = reader["Answer"].ToString(),
                            AlternateAnsOne = reader["OptionOne"].ToString(),
                            AlternateAnsTwo = reader["OptionTwo"].ToString(),
                            AlternateAnsThree = reader["OptionThree"].ToString()
                        };
                        question.Add(temp);
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }

            return question;
        }
        #endregion

        #region Course
        //Create Course
        public void CreateCourse(CourseModel course)
        {
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                //Open Connection
                connection.Open();
                //SQl Query
                string Query = "INSERT INTO Courses (Name) VALUES ('" + course.Name + "')";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                command.ExecuteNonQuery();// Execute Command

            }
            //Get exception 
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }
        }
        //Get list of Courses
        public List<CourseModel> GetCourses()
        {
            //list instance
            var courses = new List<CourseModel>();
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                // Open Connection
                connection.Open();
                //SQL Query
                string Query = "SELECT * FROM Courses";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //convert integer to string
                    int courseId = Int32.Parse(reader["CourseId"].ToString());
                    //SQL Instance
                    SqlConnection countconn = new SqlConnection(ConnectoionString);
                    //Open Connection
                    countconn.Open();
                    //SQL Query
                    string CountQuery = "SELECT COUNT(*) FROM CourseModules WHERE CourseId="+ courseId + "";
                    //SQL Command
                    SqlCommand countcommand = new SqlCommand(CountQuery, countconn);
                    //Return first column of the first row
                    int count = (Int32)countcommand.ExecuteScalar();
                    //Close Connection
                    countconn.Close();
                    //Parse in values to a new instace
                    var NewUser = new CourseModel
                    {
                        CourseId = courseId,
                        Name = reader["Name"].ToString(),
                        ModulesCount = count
                    };
                    
                    courses.Add(NewUser);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }

            return courses;
        }
        //Get Course Details
        public CourseModel GetCourse(int id)
        {
            //Course Instance
            CourseModel course = new CourseModel();
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                // Open Connection
                connection.Open();
                //SQL Query
                string Query = "SELECT * FROM Courses WHERE CourseId ="+id+"";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                //Execute command
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Convert into string and validate data
                    if ((Int32.Parse(reader["CourseId"].ToString())) == id)
                    {
                        //Add data to new instance 
                        var user = new CourseModel
                        {
                            CourseId = id,
                            Name = reader["Name"].ToString()
                        };
                        //Get IDs
                        user.Modules = GetCourseModules(id);
                        //Count Modules
                        user.ModulesCount = user.Modules.Count();

                        course = user;
                    }
                }

            }
            //Get exception 
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }

            return course;
        }
        //Get List of Modules
        public List<ModuleModel> GetCourseModules(int id)
        {
            List<ModuleModel> modules = new List<ModuleModel>();
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);
            try
            {
                // Open Connection
                connection.Open();
                //SQL Query
                string Query = "SELECT * FROM CourseModules WHERE CourseId =" + id + "";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                //Execute command
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Get Modules and convert to string
                    modules.Add(GetModule(Int32.Parse(reader["ModuleId"].ToString())));
                }

            }
            //Get exception 
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }

            return modules;
        }
        //Add Course Modules
        public void AddCourseModules(int courseid, int moduleid)
        {
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                // Open Connection
                connection.Open();
                //SQL Query
                string Query = "INSERT INTO CourseModules (CourseId, ModuleId) VALUES ('" + courseid + "','" + moduleid + "')";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                //Execute command
                command.ExecuteNonQuery();

            }
            //Get exception 
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }
        }
        //Remove Course Modules
        public void RemoveCourseModules(int courseid, int moduleid)
        {
            //SQL Instance
            SqlConnection connection = new SqlConnection(ConnectoionString);

            try
            {
                // Open Connection
                connection.Open();
                //SQL Query
                string Query = "DELETE FROM CourseModules WHERE CourseId='"+courseid+"' AND ModuleId='"+moduleid+"'";
                //SQL Command
                SqlCommand command = new SqlCommand(Query, connection);
                //Execute command
                command.ExecuteNonQuery();

            }
            //Get exception 
            catch (Exception e)
            {

            }
            finally
            {
                //Close connection
                connection.Close();
            }
        }
        #endregion

    }
}