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

namespace Contacts_WinForms_Presentation_Layer
{
    public partial class frmListContacts : Form
    {
        public frmListContacts()
        {
            InitializeComponent();
        }

        private void _RefreshContacts()
        {
            dgvListAllContacts.DataSource = clsContact.GetAllContacts();
            cbSearchBy.SelectedIndex = 0;
            mtxtSearchBy.Clear();
        }

        private void frmListContacts_Load(object sender, EventArgs e)
        {
            _RefreshContacts();
            
        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            frmAddEditContact frm = new frmAddEditContact(-1);
            frm.ShowDialog();
            _RefreshContacts();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            frmAddEditContact frm = new frmAddEditContact((int)dgvListAllContacts.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshContacts();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete contact [" + dgvListAllContacts.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK )
            {
                if (clsContact.DeleteContact((int)dgvListAllContacts.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Contact Deleted Successfully!");
                    _RefreshContacts();
                }
                else
                    MessageBox.Show("Contact is not Deleted.");
            }
            
        }

        private void cbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSearchBy.SelectedIndex == 0)
            {
                mtxtSearchBy.Mask = "9999";
            }
            else
            {
                mtxtSearchBy.Mask = "????????????";
            }
            mtxtSearchBy.Clear();
            mtxtSearchBy.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (mtxtSearchBy.Text != "")
            {
                DataView ContactsDataView = clsContact.GetAllContacts().DefaultView;

                if (cbSearchBy.SelectedIndex == 0)
                {
                    ContactsDataView.RowFilter = "ContactID = " + mtxtSearchBy.Text;
                }
                else
                {
                    ContactsDataView.RowFilter = "FirstName = '" + mtxtSearchBy.Text + "'";
                }

                dgvListAllContacts.DataSource = ContactsDataView;
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreshContacts();
        }
    }
}
