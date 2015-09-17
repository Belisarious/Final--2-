using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Final
{
    public partial class CheckOut : System.Web.UI.Page
    {
        //Sets up variables for reapeted use
        OleDbCommand cmd;
    
        OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
        OleDbDataAdapter adapt;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sends the user back to login page if they have not logged in
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void AddBt_Click(object sender, EventArgs e)
        {
            // Convert the quantity that the shopper inputs into an int that access can use
            int quan;
            int.TryParse(quantityBox.Text, out quan);

            //Grabing the quantity from the book that they selected in the drop down list
            OleDbConnection selectConn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
            string selectQry = "select Quantity from Books where BookID = @book";
            cmd = new OleDbCommand(selectQry, selectConn);
            cmd.Parameters.AddWithValue("@book", BookList.SelectedValue);
            selectConn.Open();
            OleDbDataReader SelectRdr = cmd.ExecuteReader();
            SelectRdr.Read();

            //Setting the quantity to an int to be used to compare
            string quantity = SelectRdr["Quantity"].ToString();
            int quanSet = Int16.Parse(quantity);
            // comparing to see if the quantity in stock is more than the quantity wanted by the shopper
            if (quanSet < quan)
            {
                //Telling the shopper that they have added to many
                CheckOutLB.Text = "You Added To Many";
            }
            else
            {
                //Adding the book the shopper and the number of books into the Cart table
                string insertQry = "insert into Cart (Book_ID, Username,Customer_Quantity)values(@uname, @pword, @quan)";
                cmd = new OleDbCommand(insertQry, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@uname", BookList.SelectedValue);
                cmd.Parameters.AddWithValue("@pword", Session["Username"].ToString());
                cmd.Parameters.AddWithValue("@quan", quan);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                cmd.ExecuteNonQuery();

                //Removing Books from the drop down list if they have already been added to the cart with the username
                BookList.Items.RemoveAt(BookList.SelectedIndex);
                conn.Close();
            }
            SelectRdr.Close();
            selectConn.Close();
        }

        protected void checkOutBt_Click(object sender, EventArgs e)
        {
            //Show all the items in the cart for the person logged in and books in stock that have a quantity greater than 0
            string selectNewQry = "select  BookName, Price, Customer_Quantity  from Cart, Books where Books.BookID = Cart.Book_ID and Cart.Username = @type  and Quantity > 0 ";
            cmd = new OleDbCommand(selectNewQry, conn);
            cmd.Parameters.AddWithValue("@type", Session["Username"].ToString());
            adapt = new OleDbDataAdapter(cmd);
            conn.Open();
            cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            CheckOutView.DataSource = ds.Tables[0];
            CheckOutView.DataBind();
            conn.Close();

            // Grab all the data from the cart and book tables that are for the person logged in and of the books in the cart for that person
            string nextSelectQry = "select Quantity, BookID, Price, Book_ID, Username, Customer_Quantity from Cart, Books where BookID = Book_ID and Username = @type";
            cmd = new OleDbCommand(nextSelectQry, conn);
            cmd.Parameters.AddWithValue("@uname", Session["Username"].ToString());

            conn.Open();
            OleDbDataReader selectRdr = cmd.ExecuteReader();
            //Keeping track of the total amount owed by the person logged in
            double total = 0;
            selectRdr.Read();
            Boolean check = false;
            int checking = 0;
            //Going through all the books the person has in the cart

            while (selectRdr.Read() && check == true)
            {

                //Grabing the cost of each book
                string cost = selectRdr["Price"].ToString();
                double numberCost = Double.Parse(cost);
                //Grabing the Quantity in stock and in cart for each book 
                string quantity = selectRdr["Quantity"].ToString();
                string custQuan = selectRdr["Customer_Quantity"].ToString();
                int quanSet = Int16.Parse(quantity);
                int custQuanSet = Int16.Parse(custQuan);
                //Checking to make sure that any books that the Quantity in stock is greater than what the person wants and if so then dosen't do anything
                while(selectRdr.Read() && check == true){
                if (quanSet < custQuanSet)
                {
                    check = false;

                }
                }

                if (check == true)
                {

                    checking = 1;
                    //If there is supply in stock greater than the person needs then it will update the amount - the amount wanted by the user and tally up the totoal owed
                    total = total + numberCost * custQuanSet;
                    quanSet = quanSet - custQuanSet;
                    OleDbConnection updateConn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
                    string updateQry = "update Books set Quantity = @quan where BookID = @book";
                    OleDbCommand updateCmd = new OleDbCommand(updateQry, updateConn);
                    updateCmd.Parameters.AddWithValue("@quan", quanSet);
                    updateCmd.Parameters.AddWithValue("@book", selectRdr["Book_ID"].ToString());
                    updateConn.Open();
                    updateCmd.ExecuteNonQuery();
                    updateConn.Close();
                    //Deletes the item from the cart once the total has been added for the item
                    OleDbConnection deleteConn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString);
                    string deleteQry = "delete from Cart where Username = @quan";
                    OleDbCommand deleteCmd = new OleDbCommand(deleteQry, deleteConn);
                    deleteCmd.Parameters.AddWithValue("@quan", Session["Username"].ToString());

                    deleteConn.Open();
                    deleteCmd.ExecuteNonQuery();
                    deleteConn.Close();
                    //Prints out the total
                    CheckOutLB.Text = "The Total is : " + total.ToString();

                }

            }
            // Makes sure that the person can not check out with out logging back in
            if (checking == 0)
            {

                CheckOutLB.Text = "You have an item with to much quanity";

                CheckOutBt.Visible = false;

            }

            selectRdr.Close();
            conn.Close();
        }

    }
}