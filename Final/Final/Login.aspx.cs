using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Final
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Gets all the logins from the customer table
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
            string qry = "select Login from Customer";
            OleDbCommand cmd = new OleDbCommand(qry, conn);
            conn.Open();
            OleDbDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            //Checks the logins in the table against the login provided to see if there are any conflicts
            if (rdr["Login"].Equals(userTbx.Text))
            {
                loginTx.Text = "Username already used"; 
            }
            else
            {
                //Grabes the password given and converts it to MD5 and then to a string
                string typedPassword = createpassTbx.Text;
                byte[] pwd = Encoding.ASCII.GetBytes(typedPassword);
                MD5 m = MD5.Create();
                string hashPassword = BitConverter.ToString(m.ComputeHash(pwd)).Replace("-", "").ToLower();

                //Creates a username and password in the Customer table with the Username and converted Password given
                OleDbConnection conn1 = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
                string qry1 = "insert into Customer ([Login], [Password]) values(@uname, @pword)";
                OleDbCommand cmd1 = new OleDbCommand(qry1, conn1);
                conn1.Open();
                cmd1.Parameters.AddWithValue("@uname", createUserTbx.Text);
                cmd1.Parameters.AddWithValue("@pword", hashPassword);

                cmd1.ExecuteNonQuery();

                conn1.Close();
            }
            rdr.Close();
            conn.Close();
        }

        protected void loginBT_Click(object sender, EventArgs e)
        {
            //Gets the username and password given by user
            Session["Username"] = userTbx.Text;
            string typedPassword = passTbx.Text;

            //Encodes password
            byte[] pwd = Encoding.ASCII.GetBytes(typedPassword);
            MD5 m = MD5.Create();

            //Puts encoded password to a string
            string hashPassword = BitConverter.ToString(m.ComputeHash(pwd)).Replace("-", "").ToLower();

            //Gathers the login and passowrd where the login provided equals the login in the Customer table
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
            string qry = "select * from Customer where Login = @uname";
            OleDbCommand cmd = new OleDbCommand(qry, conn);
            cmd.Parameters.AddWithValue("@uname", Session["Username"].ToString());
            
            conn.Open();
            OleDbDataReader rdr = cmd.ExecuteReader();


            if (rdr.Read())
            {

                //Checks the password provided agianst the password in the table 
                string dbPassword = rdr["Password"].ToString();
                if (hashPassword.Equals(dbPassword))
                {
                    //If the passwords match then it redirects to the bookstore
                    Response.Redirect("WebForm2.aspx");
                    
                }
                else
                {
                    //If the password does not match it tells the user
                    loginTx.Text = "Wrong Password";
                }
            }
            rdr.Close();
            conn.Close();
                


        }

        

        
    }
}