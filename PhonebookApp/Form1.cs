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

            MessageBox.Show(saved ? "Contact Added." : "Error");
        }
        private void RefreshContactList()
        {
            listv.Items.Clear();
            PopulateList();
        }
        private void PopulateList()
        {
            foreach (Contact c in db.Contacts.ToList())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = c.ContactID.ToString();
                lvi.SubItems.Add(c.FirstName);
                lvi.SubItems.Add(c.LastName);
                lvi.SubItems.Add(c.PhoneNumber);

                listv.Items.Add(lvi);
            }
        }
        
    }
}
