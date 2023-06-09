using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using CRUD_Operations.General;
using System.DirectoryServices.ActiveDirectory;

namespace CRUD_Operations
{
    public partial class Form1 : Winform
    {
        public Form1()
        {
            InitializeComponent();
        }


        public int StudentID;

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Winform Get Record", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);

                    WinformRecordDataGridView.DataSource = dt;
                }
            }
        }


        private bool IsValid()
        {
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            PersonalNumer.Clear();
            Name.Clear();
            Gender.Clear();
            FamilierLanguage.Clear();
            Hobbies.Clear();
            

           

           SubmitButton.Text = "Submit";
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecordDataGridView.SelectedRows[0].Cells[0].Value);
            txtPersonalNumber.Text = PersonalNumberDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtName.Text = NameDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            checkGender.check = checkDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            txtFamilierLanguage.Text = FamilierLanguageDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            Hobbies.Text = HobbiesDataGridView.SelectedRows[0].Cells[5].Value.ToString();

            Submit.Text = "Submit";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchStudentByStudentName();
        }

        private void SearchStudentByStudentName()
        {
            using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Students_SearchByName", con))
                {
                    DataTable dt = new DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", txtSearch.Text);
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);

                    if (dt == null)
                    {
                        MessageBox.Show("No " + txtName.Text + " is found in the database, Please search with correct name", "Not Found");
                    }
                    else
                    {
                        StudentRecordDataGridView.DataSource = dt;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Update", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PersonalNumber", txtPersonalNumber.Text);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Gender", txtRollGender.Text);
                        cmd.Parameters.AddWithValue("@FamilierLanguage", txtFamilierLAnguage.Text);
                        cmd.Parameters.AddWithValue("@Hobbies", txtHobbies.Text);


                        if (con.State != ConnectionState.Open)
                            con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Student Information is successfully saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetRecord();
                ResetFormControls();
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Delete", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", thisID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Student is deleted from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please Select an student to delete", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchStudentByStudentName();
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == string.Empty)
                GetStudentsRecord();
        }
    }
}