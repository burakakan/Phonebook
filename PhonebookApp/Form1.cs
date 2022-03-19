using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PhonebookApp
{
    public partial class Form1 : Form
    {
        string userId = "sa";
        string password = "123";

        PhonebookEntities db;
        public Form1()
        {
            InitializeComponent();

            EntityConnectionStringBuilder conStrBuilder = new EntityConnectionStringBuilder
            {
                Metadata = "res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = $"data source=.;initial catalog=Phonebook;persist security info=True;user id={userId};password={password};MultipleActiveResultSets=True;App=EntityFramework"
            };

            db = new PhonebookEntities(conStrBuilder.ConnectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshContactList();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string pn = txtPhoneNum.Text;

            if (fn == "" || ln == "" || pn == "")
            {
                MessageBox.Show("Fields cannot be left empty.");
                return;
            }

            db.Contacts.Add(new Contact() { FirstName = fn, LastName = ln, PhoneNumber = pn });

            bool saved = db.SaveChanges() != 0;

            RefreshContactList();
            ClearBoxes();
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
            if (listv.SelectedItems.Count < 1)
            {
                MessageBox.Show("Select the contact to update.");
                return;
            }
            ListViewItem lvi = listv.SelectedItems[0];
            int id = Convert.ToInt32(lvi.Text);

            Contact contact = db.Contacts.Single(c => c.ContactID == id);

            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string pn = txtPhoneNum.Text;

            if (fn == "" || ln == "" || pn == "")
            {
                MessageBox.Show("Fields cannot be left empty.");
                return;
            }
            contact.FirstName = fn;
            contact.LastName = ln;
            contact.PhoneNumber = pn;

            db.SaveChanges();

            RefreshContactList();

            ClearBoxes();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listv.SelectedItems.Count < 1)
            {
                MessageBox.Show("Select the contact to update.");
                return;
            }
            ListViewItem lvi = listv.SelectedItems[0];
            int id;
            try
            {
                id = Convert.ToInt32(lvi.Text);
            }
            catch
            {
                MessageBox.Show("Exception: Non-integer Id.");
                return;
            }

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

        private void ClearBoxes()
        {
            foreach (TextBoxBase tb in Controls.OfType<TextBoxBase>())
                tb.Clear();
        }
    }
}
