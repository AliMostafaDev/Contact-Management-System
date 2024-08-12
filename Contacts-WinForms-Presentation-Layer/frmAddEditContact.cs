using ContactsBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Contacts_WinForms_Presentation_Layer
{
    public partial class frmAddEditContact : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _ContactID;
        private clsContact _Contact;
        public frmAddEditContact(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;

            if (_ContactID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _LoadData() {

            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = 0;

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Contact";
                lblRemove.Visible = false;
                _Contact = new clsContact();
            }
            else
            {
                _Contact = clsContact.Find(_ContactID);
                if (_Contact == null)
                {
                    MessageBox.Show("This form will be closed because no Contact with this Id");
                    this.Close();
                    return;
                }
                lblMode.Text = "Edit Contact ID = " + _ContactID;
                txtContactID.Text = _Contact.ID.ToString();
                txtFirstName.Text = _Contact.FirstName;
                txtLastName.Text = _Contact.LastName;
                txtEmail.Text = _Contact.Email;
                txtPhone.Text = _Contact.Phone;
                txtAddress.Text = _Contact.Address;
                dtpDateOfBirth.Value = _Contact.DateOfBirth;

                if (_Contact.ImagePath != "")
                {
                    pictureBox1.Load(_Contact.ImagePath);
                }
                lblRemove.Visible = _Contact.ImagePath != "";
                cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Contact.CountryID).CountryName);
             }
        }

        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = clsCountry.Find(cbCountry.Text.ToString()).ID;

            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.Address = txtAddress.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = CountryID;

            if (pictureBox1.ImageLocation != null)
                _Contact.ImagePath = pictureBox1.ImageLocation;
            else
                _Contact.ImagePath = "";

            if (_Contact.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Done");
                _Mode = enMode.Update;
                lblMode.Text = "Edit Contact Id = " + _Contact.ID;
                txtContactID.Text = _Contact.ID.ToString();
                lblRemove.Visible = _Contact.ImagePath != "";
            }
                
            else
                MessageBox.Show("Error: Data Is Not Saved Successfully", "Error");

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.ImageLocation = null;
            lblRemove.Visible = false;
        }

        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
          
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                MessageBox.Show("Selected Image Is: " + selectedFilePath);
                pictureBox1.Load(selectedFilePath);
            }

        }

       
    }
}
