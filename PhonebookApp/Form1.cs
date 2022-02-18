using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhonebookApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PhonebookEntities db = new PhonebookEntities();
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshContactList();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string pn = txtPhoneNum.Text;

            db.Contacts.Add(new Contact() { FirstName = fn, LastName = ln, PhoneNumber = pn });

            bool saved = db.SaveChanges() != 0;

            RefreshContactList();

            MessageBox.Show(saved ? "Contact Added." : "Error");

        }
        private void RefreshContactList()
        {
            listv.Items.Clear();
            PopulateList(db.Contacts.ToList());
        }
        private void PopulateList(List<Contact> list)
        {
            foreach (Contact c in list)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = c.ContactID.ToString();
                lvi.SubItems.Add(c.FirstName);
                lvi.SubItems.Add(c.LastName);
                lvi.SubItems.Add(c.PhoneNumber);

                listv.Items.Add(lvi);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = listv.SelectedItems[0];
            int id = Convert.ToInt32(lvi.Text);

            Contact contact = db.Contacts.Single(c => c.ContactID == id);

            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string pn = txtPhoneNum.Text;

            contact.FirstName = fn;
            contact.LastName = ln;
            contact.PhoneNumber = pn;

            db.SaveChanges();

            RefreshContactList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = listv.SelectedItems[0];
            int id = Convert.ToInt32(lvi.Text);

            db.Contacts.Remove(db.Contacts.Single(c => c.ContactID == id));
            db.SaveChanges();

            RefreshContactList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string str = txtSearch.Text;
            List<Contact> list = db.Contacts.Where(c => (c.FirstName + " " + c.LastName).Contains(str) || c.PhoneNumber.Contains(str)).ToList();

            listv.Items.Clear();
            PopulateList(list);
        }

    }
}
