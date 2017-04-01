using BookStore.DAL;
using BookStore.Models; 
using System; 
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private UserGateway userGateway = new UserGateway();
        private BookStoreContext db = new BookStoreContext();

        public object WebSecurity { get; private set; } 

        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer 
            Session["Captcha" + prefix] = a + b;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x-r, y-r, r, r);
                    }
                }

                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewEmail()
        {
            return RedirectToAction("Email", "User");
        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != user.Captcha)
            {
                ModelState.AddModelError("Captcha", "Wrong value of sum, please try again.");
                return View("Index",user);
            }
            else {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (IsValidAdmin(user.UserName, user.Password))
                        {
                            //set form authentication to user true
                            FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                            var userObj = db.Users.Single(x => x.Username == user.UserName);

                            Session["userID"] = userObj.Id;
                            Session["currentCart"] = userObj.currentCart;
                            Session["username"] = userObj.Username;
                            Session["Role"] = userObj.Role;
                            Roles.CreateRole(userObj.Role);
                            Roles.AddUserToRole(userObj.Username, userObj.Role);

                            //redirect to admin page
                            return RedirectToAction("Index", "Home");
                        }
                        else if (IsValidUser(user.UserName, user.Password))
                        {
                            //set form authentication to user true
                            FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                            var userObj = db.Users.Single(x => x.Username == user.UserName);

                            Session["userID"] = userObj.Id;
                            Session["currentCart"] = userObj.currentCart;
                            Session["username"] = userObj.Username;
                            Session["Role"] = userObj.Role;
                            Roles.CreateRole(userObj.Role);
                            Roles.AddUserToRole(userObj.Username, userObj.Role);

                            //redirect to user page
                            return RedirectToAction("Index", "Item");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Login data is incorrect!");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");
                    }
                }
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            //set form authentication false
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return RedirectToAction("RegisterPage", "User");
        }

        public ActionResult AdminLayoutPage()
        {
            return RedirectToAction("AdminLayoutPage", "Shared");
        }

        public ActionResult ChangePassw()
        {
            return RedirectToAction("ChangePassword", "User");
        }

        public ActionResult ChangePassword()
        {
            UserChangePassword cp = new UserChangePassword();
            cp.uName = User.Identity.Name;

            return View(cp);
        }

        public ActionResult ChangeUserPassword(UserChangePassword cp)
        {
            cp.uName = User.Identity.Name;
            //check if old password is same as existing password in db (encode hash the input)
            bool result = checkOldPassword(cp); 
            
            while (result == true)
            {
                if (cp.uOPwd.Equals(cp.uPwd))
                {
                    ModelState.AddModelError("", "Password is similar, please try again!");
                    break;
                }
                //check if new password & confirm password is same
                else if (cp.uPwd.Equals(cp.confirmUserpwd))
                {
                    //update database new password where username is equal to cp.username
                    bool updateResult = updateNewPassword(cp);

                    if (updateResult)
                    {
                        ModelState.AddModelError("", "Password updated successfully!");
                    }
                    break;
                }
                else
                {
                    ModelState.AddModelError("", "Password & Confirm Password does not match!");
                    cp.uPwd = "";
                    cp.confirmUserpwd = "";
                    break;
                }
            }
            if (result == false){
                ModelState.AddModelError("", "Password is invalid, please try again!");
                cp.uOPwd = "";
                cp.uPwd = "";
                cp.confirmUserpwd = "";
            }

            return View("ChangePassword",cp);
        }

        public bool updateNewPassword(UserChangePassword cp)
        {
            bool result = false;
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                string _sql = @"Update [dbo].[Users] " +
                      @"SET [Password]= @p WHERE [Username] = @u";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar)) 
                    .Value = Helpers.SHA.Encode(cp.uPwd);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = cp.uName;
                cn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;

                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }


        public bool checkOldPassword(UserChangePassword cp)
        {
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                bool result = false;
                string _sql = @"SELECT * FROM [dbo].[Users] " +
                  @"WHERE [Username] = @u AND [Password] = @p";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                            .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                            .Value = cp.uName;
                cmd.Parameters
                            .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                            .Value = Helpers.SHA.Encode(cp.uOPwd);

                cn.Open();
                try
                {
                    //didnt read
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        result = true;
                        reader.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex) { }

                return result;
            }
        }
        //--------------------------AccountRegister----------------------------

        //GET: Register Page -- 
        public ActionResult RegisterPage()
        {
            return View();
        }

        public ActionResult Email()
        {
            return View();
        }

        public ActionResult RegisterAdminPage()
        {
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["userID"] != null)
            {
                return View();
            }

            else
            {
                return RedirectToAction("Login");
            }
        }

        //Checks whether Email exists or not using Entity Framework.
        //It sends json result either true or false.
        public bool checkMailExists(string email)
        {
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                bool result = false;
                string _sql = @"SELECT * FROM [dbo].[Users] " + @"WHERE [Email] = @u";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                            .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                            .Value = email;

                cn.Open();
                try
                {
                    //didnt read
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        result = true;
                        reader.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex) { }

                return result;
            }
        }



        public bool checkUserExists(string user)
        {
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                bool result = false;
                string _sql = @"SELECT * FROM [dbo].[Users] " + @"WHERE [Username] = @u";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                            .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                            .Value = user;

                cn.Open();
                try
                {
                    //didnt read
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        result = true;
                        reader.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex) { }

                return result;
            }
        }

        [HttpPost]
        public ActionResult Create(RegisterUser account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User reg;

                    if (account.uPremiumUser)
                    {
                        reg = new PremiumUser();
                    }
                    else {
                        reg = new BasicUser();
                    }

                    reg.Username = account.uName;
                    DateTime oDate = Convert.ToDateTime(account.uDOB);
                    reg.Birthdate = oDate;
                    reg.Password = Helpers.SHA.Encode(account.uPwd);

                    bool result = checkUserExists(account.uName);
                    bool resultemail = checkMailExists(account.uEmail);
                    if (result == true)
                    {
                        ModelState.AddModelError("", account.uName + " already taken.");
                    }
                    else
                        if (resultemail == true)
                        {
                            ModelState.AddModelError("", account.uEmail + " already taken.");
                        }
                    else
                        if (result && resultemail)
                        {
                            reg.Email = account.uEmail;
                            reg.Premiumuser = account.uPremiumUser;


                            if (account.uPremiumUser)
                            {
                                reg.Role = "PremiumUser";
                                userGateway.Insert(reg);
                            }
                            else
                            {
                                reg.Role = "BasicUser";
                                userGateway.Insert(reg);
                            }

                            ModelState.AddModelError("", account.uName + " successfully registered.");
                        }
                    }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }
            }
            return View("RegisterPage");
        }

        public ActionResult CreateAdmin(RegisterUser account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User reg = new Admin();
                    reg.Username = account.uName;
                    DateTime oDate = Convert.ToDateTime(account.uDOB);
                    reg.Birthdate = oDate;
                    reg.Password = Helpers.SHA.Encode(account.uPwd);

                    bool result = checkUserExists(account.uName);
                    if (result == true)
                    {
                        ModelState.AddModelError("", account.uName + " already taken.");
                    }
                    else
                    {
                        reg.Email = account.uEmail;
                        reg.Premiumuser = account.uPremiumUser;
                        reg.Role = "Admin";

                        userGateway.Insert(reg); 

                        ModelState.AddModelError("", account.uName + " successfully registered.");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.ToString();
                }
            }
            return View("RegisterAdminPage");
        }

        public bool IsValidAdmin(string _username, string _password)
        {
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                bool result = false;
                string _sql = @"SELECT [Username] FROM [dbo].[Users] " +
                      @"WHERE [Username] = @u AND [Password] = @p AND [Role]= @a";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = Helpers.SHA.Encode(_password);
                cmd.Parameters
                    .Add(new SqlParameter("@a", SqlDbType.NVarChar))
                    .Value = "Admin";
                cn.Open();
                try
                {
                    //didnt read
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        result = true;
                    }
                    else
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        result = false;
                    }
                }
                catch (Exception) { }

                return result;
            }
        }

        public bool IsValidUser(string _username, string _password)
        {
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                bool result = false;
                string _sql = @"SELECT [Username] FROM [dbo].[Users] " +
                      @"WHERE [Username] = @u AND [Password] = @p AND [Role] LIKE @a";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = Helpers.SHA.Encode(_password);
                cmd.Parameters
                    .Add(new SqlParameter("@a", SqlDbType.NVarChar))
                    .Value = "%User%";
                cn.Open();
                try
                {
                    //didnt read
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        result = true;
                    }
                    else
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        result = false;
                    }
                }
                catch (Exception) { }

                return result;
            }
        }

        //[HttpPost]
        public ActionResult SendEmail(User usr)
        {
            Execute(usr.Email);

            //if you want add update successful message
            return RedirectToAction("Index", "User");
        }

        public string GeneratePassword()
        {
            int lengthOfPassword = 5;
            string valid = "abcdefghijklmnozABCDEFGHIJKLMNOZ1234567890@!&";
            StringBuilder strB = new StringBuilder(100);
            Random random = new Random();
            while (0 < lengthOfPassword--)
            {
                strB.Append(valid[random.Next(valid.Length)]);
            }
            return strB.ToString();
        }

        public User checkEmailValid (String email)
        {
            User userObj = db.Users.Single(x => x.Email == email);
            return userObj; 
        }

        public void Execute(String usremail)
        {
            //check email valid to db
            User userProf = checkEmailValid(usremail);

            if (userProf != null)
            {
                //generate password & email
                String passw = GeneratePassword();
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                //set the email according to valid email from db
                mail.To.Add(userProf.Email);

                
                mail.From = new MailAddress("ict2106projectteam@gmail.com", "ICT2106 - Password Recovery", System.Text.Encoding.UTF8);
                mail.Subject = "ICT2106 - Password Recover for bookshoppee";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "Your new password is " + passw;

                //send email
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("ict2106projectteam@gmail.com", "SITICT2106");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }
                }
                finally
                {
                    //update db
                    userProf.Password = Helpers.SHA.Encode(passw);

                    UpdateUserProfile(userProf);
                }
            }
        }

        public bool UpdateUserProfile(User u)
        {
            bool result = false;
            using (var cn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStoreContext.mdf;Initial Catalog=BookStoreContext;Integrated Security=True"))
            {
                string _sql = @"Update [dbo].[Users] " +
                      @"SET [Password]= @p WHERE [Email] = @u";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = u.Password;
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = u.Email;
                cn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                    
                }
                catch (Exception ex) {
                    result = false;
                }
            }
            return result;
        }
 
    }
}