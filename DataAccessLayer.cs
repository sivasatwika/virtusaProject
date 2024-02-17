using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using dotnetapp.Models;
using System.Security.Cryptography;
using System.Reflection.Metadata;

namespace dotnetapp
{
    public class DataAccessLayer
    {
        private string connectionString;

        SqlConnection conn = null;
        public DataAccessLayer()
        {
            connectionString = "Data Source=LAPTOP-88KF2HDM\\SQLEXPRESS; Initial Catalog= CustomisedNameBoard_GiftShop;Integrated security=true";
            conn = new SqlConnection(connectionString);
        }


        //AuthController
        public string isUserPresent(LoginModel lm)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@email", lm.email);
                    cmd.Parameters.AddWithValue("@password", lm.password);

                    // Check if the email and password match in the LoginModel table
                    cmd.CommandText = "SELECT COUNT(*) FROM LoginModel WHERE email = @email AND password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";
                    conn.Open();
                    int userCount = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (userCount > 0)
                    {
                        return "valid"; // Both email and password are valid
                    }
                    else
                    {
                        // Check if the email exists in the LoginModel table
                        cmd.CommandText = "SELECT COUNT(*) FROM LoginModel WHERE email = @email";
                        conn.Open();
                        int userEmailCount = (int)cmd.ExecuteScalar();
                        conn.Close();

                        // Check if the password exists in the LoginModel table
                        cmd.CommandText = "SELECT COUNT(*) FROM LoginModel WHERE password = @password";
                        conn.Open();
                        int userPasswordCount = (int)cmd.ExecuteScalar();
                        conn.Close();

                        if (userEmailCount == 0 && userPasswordCount == 0)
                        {
                            return "invalid"; // Both email and password are invalid
                        }
                        else if (userEmailCount == 0)
                        {
                            return "invalid_email"; // Email doesn't exist in the LoginModel table
                        }
                        else
                        {
                            return "invalid_password"; // Password is incorrect
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database access or query execution.
                return "error";
            }
        }


        public string isAdminPresent(LoginModel lm)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@email", lm.email);
                    cmd.Parameters.AddWithValue("@password", lm.password);

                    // Check if the email and password match in the LoginModel table
                    cmd.CommandText = "SELECT COUNT(*) FROM AdminModel WHERE email = @email AND password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";
                    conn.Open();
                    int userCount = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (userCount > 0)
                    {
                        return "valid"; // Both email and password are valid
                    }
                    else
                    {
                        // Check if the email exists in the LoginModel table
                        cmd.CommandText = "SELECT COUNT(*) FROM AdminModel WHERE email = @email";
                        conn.Open();
                        int userEmailCount = (int)cmd.ExecuteScalar();
                        conn.Close();

                        // Check if the password exists in the LoginModel table
                        cmd.CommandText = "SELECT COUNT(*) FROM AdminModel WHERE password = @password";
                        conn.Open();
                        int userPasswordCount = (int)cmd.ExecuteScalar();
                        conn.Close();

                        if (userEmailCount == 0 && userPasswordCount == 0)
                        {
                            return "invalid"; // Both email and password are invalid
                        }
                        else if (userEmailCount == 0)
                        {
                            return "invalid_email"; // Email doesn't exist in the LoginModel table
                        }
                        else
                        {
                            return "invalid_password"; // Password is incorrect
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database access or query execution.
                return "error";
            }
        }



        public string saveUser(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "LoginModel_SaveUser";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);

                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "User added";
                }
                else
                {
                    return "User not added";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string saveAdmin(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "AdminModel_SaveAdmin";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@mobileNumber", user.mobileNumber);
                cmd.Parameters.AddWithValue("@userRole", user.userRole);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "Admin added";
                }
                else
                {
                    return "Admin not added";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //User Controller
        public string addUser(UserModel user)
        {


            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_AddUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@mobileNumber", user.mobileNumber);
                cmd.Parameters.AddWithValue("@userRole", user.userRole);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();


                if (user.userRole == "user" || user.userRole == "User")
                {
                    string x = saveUser(user);
                    if (x == "User added")
                    {
                        return "User Added";
                    }
                    else
                    {
                        return "User not added";
                    }
                }
                else
                {
                    string x = saveAdmin(user);
                    if (x == "Admin added")
                    {
                        return "Admin Added";
                    }
                    else
                    {
                        return "Admin not added";
                    }
                }
            }
            catch (Exception ex)
            {
                return "user exists";
            }
        }

        public string deleteUser(string UserID)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_DeleteById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "User deleted";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string editUser(string UserID, UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_Update";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@mobileNumber", user.mobileNumber);
                cmd.Parameters.AddWithValue("@userRole", user.userRole);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "User updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public List<UserModel> getUser()
        {
            try
            {
                List<UserModel> umlist = new List<UserModel>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_GetByList";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = null;
                sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel um = new UserModel();
                    um.email = dr["email"].ToString();
                    um.password = dr["password"].ToString();
                    um.username = dr["username"].ToString();
                    um.mobileNumber = dr["mobileNumber"].ToString();
                    um.userRole = dr["userRole"].ToString();
                    umlist.Add(um);
                }
                return umlist;
            }
            catch (Exception ex)
            {
                return new List<UserModel>();
            }
        }

        public UserModel getUserByEmail(string email)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_GetUserbyEmail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", email);

                UserModel user = new UserModel();

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user.username = dr["userName"].ToString();
                        user.mobileNumber = dr["mobileNumber"].ToString();
                    }
                }
                conn.Close();
                return user;

            }
            catch
            {
                return null;
            }
        }

        public string change(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_updatePassword";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "Password Updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePassword(LoginModel login)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "LoginModel_UpdatePassword";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", login.email);
                cmd.Parameters.AddWithValue("@password", login.password);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    UserModel user = new UserModel();
                    user.email = login.email;
                    user.password = login.password;
                    return change(user);
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateMobileNumber(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_UpdateMobileNumber";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@mobileNumber", user.mobileNumber);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "Mobile Number Updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateusername(UserModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserModel_UpdateUsername";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@username", user.username);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "Username Updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Theme Controller
        public List<ThemeModel> GetAllThemes()
        {
            try
            {
                List<ThemeModel> themes = new List<ThemeModel>();
                SqlCommand cmd = new SqlCommand("ThemeModel_GetByList", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ThemeModel theme = new ThemeModel
                    {
                        themeId = Convert.ToInt32(dr["themeId"]),
                        themeName = dr["themeName"].ToString(),
                        themePrice = Convert.ToInt32(dr["themePrice"]),
                        themeDetails = dr["themeDetails"].ToString()

                    };

                    themes.Add(theme);
                }
                conn.Close();
                return themes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving gifts: " + ex.Message);
                return new List<ThemeModel>();
            }
        }


        [HttpGet]
        [Route("admin/selectTheme/{themeId}")]
        public ThemeModel getTheme(int themeId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                ThemeModel theme = new ThemeModel();
                cmd.CommandText = "ThemeModel_GetById";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@themeId", themeId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    theme.themeId = Convert.ToInt32(dr["themeId"]);
                    theme.themeName = dr["themeName"].ToString();
                    theme.themePrice = Convert.ToInt32(dr["themePrice"]);
                    theme.themeDetails = dr["themeDetails"].ToString();
                }

                dr.Close();
                conn.Close();

                return theme;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ThemeModel theme = new ThemeModel();
                return theme;
            }
        }

        public string addTheme(ThemeModel data)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "ThemeModel_Insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@themeName", data.themeName);
                cmd.Parameters.AddWithValue("@themeDetails", data.themeDetails);
                cmd.Parameters.AddWithValue("@themePrice", data.themePrice);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowaffect > 0)
                {
                    return ("Theme Added");
                }
                else
                {
                    return ("Theme Not Added");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteTheme(int themeId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "ThemeModel_DeleteById";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@themeId", themeId);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return ("Theme deleted");
                }
                else
                {
                    return ("Theme not deleted");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EditTheme(int themeId, ThemeModel data)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "ThemeModel_Update";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@themeId", themeId);
                cmd.Parameters.AddWithValue("@themeName", data.themeName);
                cmd.Parameters.AddWithValue("@themeDetails", data.themeDetails);
                cmd.Parameters.AddWithValue("@themePrice", data.themePrice);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return ("Theme edited");
                }
                else
                {
                    return ("Theme not edited");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        //user side theme selection and insertion

        public List<ThemeModel> GetThemes()
        {
            try
            {
                List<ThemeModel> themelist = new List<ThemeModel>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "ThemeModel_GetByList";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    ThemeModel th = new ThemeModel();
                    th.themeName = dr["themeName"].ToString();
                    th.themePrice = int.Parse(dr["themePrice"].ToString());
                    th.themeDetails = dr["themeDetails"].ToString();
                    themelist.Add(th);
                }
                return themelist;
            }
            catch (Exception ex)
            {
                return new List<ThemeModel>();
            }
        }


        public string selectTheme(ThemeModel data)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserThemeModel_Insert1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@themeName", data.themeName);
                cmd.Parameters.AddWithValue("@themeDetails", data.themeDetails);
                cmd.Parameters.AddWithValue("@themePrice", data.themePrice);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowaffect > 0)
                {
                    return ("Theme Selected");
                }
                else
                {
                    return ("Theme Not Selected");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Gift Controller
        public List<GiftModel> getAllGifts()
        {
            try
            {
                List<GiftModel> giftlist = new List<GiftModel>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GiftModel_GetAll";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GiftModel gift = new GiftModel();
                    gift.giftId = Convert.ToInt32(dr["giftId"]);
                    gift.giftName = dr["giftName"].ToString();
                    gift.giftPrice = int.Parse(dr["giftPrice"].ToString());
                    gift.GiftImageUrl = dr["GiftImageUrl"].ToString();
                    gift.giftQuantity = dr["giftQuantity"].ToString();
                    gift.giftDetails = dr["giftDetails"].ToString();

                    giftlist.Add(gift);
                }
                conn.Close();
                return (giftlist);
            }
            catch (Exception ex)
            {
                List<GiftModel> giftlist = new List<GiftModel>();
                return giftlist;
            }
        }


        public GiftModel getGift(int giftId)
        {
            try
            {
                GiftModel gf = new GiftModel();
                SqlCommand cmd = new SqlCommand("GiftModel_GetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@giftId", giftId);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    gf.giftName = dr["giftName"].ToString();
                    gf.giftPrice = Convert.ToInt32(dr["giftPrice"]);
                    gf.GiftImageUrl = dr["GiftImageUrl"].ToString();
                    gf.giftQuantity = dr["giftQuantity"].ToString();
                    gf.giftDetails = dr["giftDetails"].ToString();
                }

                dr.Close();
                conn.Close();

                return gf;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GiftModel gf = new GiftModel();
                return gf;
            }
        }


        public string addGift(GiftModel data)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GiftModel_Insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@giftName", data.giftName);
                cmd.Parameters.AddWithValue("@giftPrice", data.giftPrice);
                cmd.Parameters.AddWithValue("@GiftImageUrl", data.GiftImageUrl);
                cmd.Parameters.AddWithValue("@giftQuantity", data.giftQuantity);
                cmd.Parameters.AddWithValue("@giftDetails", data.giftDetails);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowaffect > 0)
                {
                    return ("Gift added");
                }
                else
                {
                    return ("Gift Not added");
                }
            }

            catch (Exception ex)
            {
                return (ex.Message);
            }
        }


        public string editGift(int giftid, GiftModel data)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("GiftModel_UpdateById", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@giftId", giftid);
                cmd.Parameters.AddWithValue("@giftName", data.giftName);
                cmd.Parameters.AddWithValue("@giftPrice", data.giftPrice);
                cmd.Parameters.AddWithValue("@GiftImageUrl", data.GiftImageUrl);
                cmd.Parameters.AddWithValue("@giftQuantity", data.giftQuantity);
                cmd.Parameters.AddWithValue("@giftDetails", data.giftDetails);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return ("Gift edited");
                }
                else
                {
                    return ("Gift not edited");
                }
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }


        public string DeleteGift(int giftid)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GiftModel_DeleteById";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@giftId", giftid);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return ("Gift Deleted");
                }
                else
                {
                    return ("Gift not Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        public string selectGift(GiftModel data)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UserGiftModel_Insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@giftName", data.giftName);
                cmd.Parameters.AddWithValue("@giftPrice", data.giftPrice);
                cmd.Parameters.AddWithValue("@GiftImageUrl", data.GiftImageUrl);
                cmd.Parameters.AddWithValue("@giftQuantity", data.giftQuantity);
                cmd.Parameters.AddWithValue("@giftDetails", data.giftDetails);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowaffect > 0)
                {
                    return ("Gift selected");
                }
                else
                {
                    return ("Gift Not selected");
                }
            }

            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        //Order Controller

        public string addOrdersCart(OrderModel order)
        {
            try
             {
                 // Check if the gift quantity is sufficient
                 SqlCommand cmdCheckGiftQuantity = new SqlCommand("SELECT giftQuantity FROM GiftModel WHERE giftId = @GiftId", conn);
                 cmdCheckGiftQuantity.Parameters.AddWithValue("@GiftId", order.giftModel.giftId);
                 conn.Open();
                 string availableQuantity = (string)cmdCheckGiftQuantity.ExecuteScalar();
                 conn.Close();

                 if (Convert.ToInt32(availableQuantity) >= Convert.ToInt32(order.orderQuantity))
                 {
                     // Update gift quantity in the database
                     SqlCommand cmdUpdateGiftQuantity = new SqlCommand("UPDATE GiftModel SET giftQuantity = giftQuantity  -  @orderQuantity  WHERE giftId =  @GIftId ", conn);
                     cmdUpdateGiftQuantity.Parameters.AddWithValue("@GiftId", order.giftModel.giftId);
                     cmdUpdateGiftQuantity.Parameters.AddWithValue("@orderQuantity", Convert.ToInt32(order.orderQuantity));
                     conn.Open();
                     int rowsAffected = cmdUpdateGiftQuantity.ExecuteNonQuery();
                     conn.Close();

                     if (rowsAffected > 0)
                     {
                         // Insert the order into OrdersCart table
                         SqlCommand cmdAddOrder = new SqlCommand("addOrdersCart", conn);
                         cmdAddOrder.CommandType = CommandType.StoredProcedure;
                         cmdAddOrder.Parameters.AddWithValue("@orderName", order.orderName);
                         cmdAddOrder.Parameters.AddWithValue("@orderDescription", order.orderDescription);
                         cmdAddOrder.Parameters.AddWithValue("@ThemeModel", GetThemeModelXml(order.themeModel));
                         cmdAddOrder.Parameters.AddWithValue("@GiftId", order.giftModel.giftId);
                         cmdAddOrder.Parameters.AddWithValue("@orderDate", order.orderDate);
                         cmdAddOrder.Parameters.AddWithValue("@orderPrice", order.orderPrice);
                         cmdAddOrder.Parameters.AddWithValue("@orderAddress", order.orderAddress);
                         cmdAddOrder.Parameters.AddWithValue("@orderPhone", order.orderPhone);
                         cmdAddOrder.Parameters.AddWithValue("@orderEmail", order.orderEmail);
                         cmdAddOrder.Parameters.AddWithValue("@orderQuantity", order.orderQuantity);
                         conn.Open();
                         int rowEffect = cmdAddOrder.ExecuteNonQuery();
                         conn.Close();

                         if (rowEffect > 0)
                         {
                             return "Order added";
                         }
                         else
                         {
                             return "Order not added";
                         }
                     }
                     else
                     {
                         return "Order not added";
                     }
                 }
                 else
                 {
                     return "Insufficient gift quantity";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message;
             }
         }
        
      

       /* public string addOrdersCart(OrderModel order)
        {

            try
            {
                SqlCommand cmd = new SqlCommand("addOrdersCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orderName", order.orderName);
                cmd.Parameters.AddWithValue("@orderDescription", order.orderDescription);
                cmd.Parameters.AddWithValue("@ThemeModel", GetThemeModelXml(order.themeModel));
                cmd.Parameters.AddWithValue("@GiftId", order.giftModel.giftId);
                cmd.Parameters.AddWithValue("@orderDate", order.orderDate);
                cmd.Parameters.AddWithValue("@orderPrice", order.orderPrice);
                cmd.Parameters.AddWithValue("@orderAddress", order.orderAddress);
                cmd.Parameters.AddWithValue("@orderPhone", order.orderPhone);
                cmd.Parameters.AddWithValue("@orderEmail", order.orderEmail);
                cmd.Parameters.AddWithValue("@orderQuantity", order.orderQuantity);
                conn.Open();
                int roweffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (roweffect > 0)
                {
                    return "Order added";
                }
                else
                {
                    return "Order not added";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }*/

        public string addOrders(string userEmail)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("addOrders", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userEmail", userEmail);
                conn.Open();
                int roweffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (roweffect > 0)
                {
                    cmd = new SqlCommand("DeletefromOrdersCart", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userEmail", userEmail);
                    conn.Open();
                    int roweffect1 = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (roweffect1 > 0)
                        return "Order added";
                    else
                        return "Order not added";
                }
                else
                    return "Order not added";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string GetThemeModelXml(List<ThemeModel> themeModels)
        {
            XElement xml = new XElement("ThemeModel",
            themeModels.Select(theme => new XElement("Theme",
                new XElement("ThemeName", theme.themeName),
                new XElement("ThemePrice", theme.themePrice)
            ))
        );

            return xml.ToString();
        }


        public IActionResult viewPlacedOrders(string userEmail)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT orderID, orderName, orderDescription, ThemeModel, GiftModel.giftName, GiftModel.giftPrice ,orderDate, orderPrice, orderAddress, orderPhone, orderEmail, orderQuantity FROM OrdersCart JOIN GiftModel ON OrdersCart.GiftId = GiftModel.giftId WHERE orderEmail = @userEmail", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userEmail", userEmail);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<Dictionary<string, object>> orders = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> order = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    order[col.ColumnName] = row[col];
                }
                orders.Add(order);
            }
            return new JsonResult(orders);


        }


        public List<OrderModel> viewOrders(string userEmail)
        {
            try
            {
                List<OrderModel> orderlist = new List<OrderModel>();
                SqlCommand cmd = new SqlCommand("getCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userEmail", userEmail);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    OrderModel or = new OrderModel();
                    or.orderId = Convert.ToInt32(dr["orderId"]);
                    or.orderPrice = Convert.ToInt32(dr["orderPrice"]);
                    or.orderName = dr["orderName"].ToString();
                    or.orderDescription = dr["orderDescription"].ToString();
                    or.orderEmail = dr["orderEmail"].ToString();
                    or.orderQuantity = dr["orderQuantity"].ToString();
                    or.orderDate = Convert.ToDateTime(dr["orderDate"]);
                    or.orderPhone = dr["orderPhone"].ToString();
                    or.orderAddress = dr["orderAddress"].ToString();
                    or.giftModel.giftId = Convert.ToInt32(dr["GiftId"]);
                    or.themeModel = GetThemeModelFromXml(dr["themeModel"].ToString());
                    orderlist.Add(or);
                }
                conn.Close();
                return orderlist;
            }
            catch(Exception ex)
            {
                List<OrderModel> orderlist = new List<OrderModel>();
                return orderlist;
            }

        }

        public OrderModel viewPlacedOrderBYEmail(int orderId)
        {
                OrderModel or = new OrderModel();
                SqlCommand cmd = new SqlCommand("getOrdersCart_ById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orderId", orderId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    or.orderId= Convert.ToInt32(dr["orderId"]);
                    or.orderPrice = Convert.ToInt32(dr["orderPrice"]);
                    or.orderName = dr["orderName"].ToString();
                    or.orderDescription = dr["orderDescription"].ToString();
                    or.orderEmail= dr["orderEmail"].ToString();
                    or.orderQuantity = dr["orderQuantity"].ToString();
                    or.orderDate = Convert.ToDateTime(dr["orderDate"]);
                    or.orderPhone = dr["orderPhone"].ToString();
                    or.orderAddress = dr["orderAddress"].ToString();
                    or.giftModel.giftId = Convert.ToInt32(dr["GiftId"]);
                    or.themeModel = GetThemeModelFromXml(dr["themeModel"].ToString());

                }
            return or;

        }
        private List<ThemeModel> GetThemeModelFromXml(string xml)
        {
            List<ThemeModel> themeModels = new List<ThemeModel>();
            XElement root = XElement.Parse(xml);

            foreach (XElement themeElement in root.Elements("Theme"))
            {
                ThemeModel themeModel = new ThemeModel();
                themeModel.themeName = themeElement.Element("ThemeName").Value;
                themeModel.themePrice = Convert.ToInt32(themeElement.Element("ThemePrice").Value);

                themeModels.Add(themeModel);
            }

            return themeModels;
        }



        public string editOrder(int orderID, OrderModel order)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UpdateOrdersCart";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@orderName", order.orderName);
                cmd.Parameters.AddWithValue("@orderDescription", order.orderDescription);
                cmd.Parameters.AddWithValue("@themeModel", GetThemeModelXml(order.themeModel));

                cmd.Parameters.AddWithValue("@orderDate", order.orderDate);

                cmd.Parameters.AddWithValue("@orderAddress", order.orderAddress);
                cmd.Parameters.AddWithValue("@orderPhone", order.orderPhone);
                cmd.Parameters.AddWithValue("@orderPrice", order.orderPrice);
                cmd.Parameters.AddWithValue("@orderQuantity", order.orderQuantity);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "order updated";
                }
                else
                {
                    return "order not updated";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deleteOrder(int orderID)
        {
            try
            {
                SqlCommand cmdCheckOrderQuantity = new SqlCommand("SELECT orderQuantity FROM OrdersCart WHERE orderID = @orderID", conn);
                cmdCheckOrderQuantity.Parameters.AddWithValue("@orderID", orderID);
                conn.Open();
                string availableQuantity = (string)cmdCheckOrderQuantity.ExecuteScalar();
                conn.Close();
                SqlCommand cmdCheckGiftId = new SqlCommand("SELECT GiftId FROM OrdersCart WHERE orderID = @orderID", conn);
                cmdCheckGiftId.Parameters.AddWithValue("@orderID", orderID);
                conn.Open();
                int gi = (int)cmdCheckGiftId.ExecuteScalar();
                conn.Close();
                SqlCommand cmdUpdateGift = new SqlCommand("UPDATE GiftModel SET giftQuantity = giftQuantity + @orderQuantity WHERE giftId = @GIftId", conn);
                cmdUpdateGift.Parameters.AddWithValue("@GiftId", gi);
                cmdUpdateGift.Parameters.AddWithValue("@orderQuantity", Convert.ToInt32(availableQuantity));
                conn.Open();
                int rowsAffected = cmdUpdateGift.ExecuteNonQuery();
                conn.Close();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DeleteOrdersCart";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "order deleted";
                }
                else
                {
                    return "order not deleted";
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /*public string deleteOrder(int orderID)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DeleteOrdersCart";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "order deleted";
                }
                else
                {
                    return "order not deleted";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        */
        public JsonResult viewOrder()
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("ViewOrders", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<Dictionary<string, object>> orders = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> order = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    order[col.ColumnName] = row[col];
                }
                orders.Add(order);
            }
            return new JsonResult(orders);


        }
        public string AdminDeleteOrder(int orderID)
        {
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DeleteOrders";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "order deleted";
                }
                else
                {
                    return "order not deleted";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public JsonResult MyOrders(string email)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("UserViewOrders", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<Dictionary<string, object>> orders = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> order = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    order[col.ColumnName] = row[col];
                }
                orders.Add(order);
            }
            return new JsonResult(orders);
        }

        //Review Controller

        public List<ReviewModel> GetReviews()
        {
            List<ReviewModel> reviews = new List<ReviewModel>();
            SqlDataReader sdr = null;
            SqlCommand cmd = new SqlCommand("Getreview", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read() == true)
            {
                ReviewModel review = new ReviewModel();
                review.orderId = Convert.ToInt32(sdr["orderId"]);
                review.name = sdr["name"].ToString();
                review.comments = sdr["comments"].ToString();

                reviews.Add(review);
            }
            conn.Close();
            return reviews;
        }

        public string Postreview(ReviewModel review)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Insertreview", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orderId", review.orderId);
                cmd.Parameters.AddWithValue("@name", review.name);
                cmd.Parameters.AddWithValue("@comments", review.comments);
                conn.Open();
                int rowaffect = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowaffect > 0)
                {
                    return "inserted sucessfully";
                }
                else
                {
                    return "inserted failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}