﻿using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for CharityView.xaml
    /// </summary>
    public partial class frmCharityView : Window
    {
        private ICharityManager _charityManager;
        private Charity _charity;
        private bool inAddMode;
        private bool _inApplyMode;
        private List<User> _userList;
        private User _charityUser;

        public frmCharityView(int userID)
        {
            InitializeComponent();
        }

        public frmCharityView(ICharityManager charityManager)
        {
            InitializeComponent();
            this._charityManager = charityManager;
            inAddMode = true;
        }

        public frmCharityView(ICharityManager charityManager, int employeeID)
        {
            InitializeComponent();
            this._charityManager = charityManager;
            inAddMode = true;
            txtEmployeeID.Text = employeeID.ToString();
            txtEmployeeID.IsEnabled = false;
            lblStatus.Visibility = Visibility.Collapsed;
        }

        public frmCharityView(ICharityManager charityManager, DataObjects.Charity charity)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this._charityManager = charityManager;
            this._charity = charity;
            lblCharityIDVal.Content = charity.CharityID;
            lblCharityNameVal.Content = charity.CharityName;
            lblContactFirstNameVal.Content = charity.ContactFirstName;
            lblContactHoursVal.Content = charity.ContactHours;
            lblContactLastNameVal.Content = charity.ContactLastName;
            lblEmailVal.Content = charity.Email;
            lblEmployeeIDVal.Content = charity.EmployeeID;
            lblPhoneNumberVal.Content = charity.PhoneNumber;
            lblUserIDVal.Content = charity.UserID;
            lblStatusVal.Content = charity.Status;
            inAddMode = false;
        }

        /// <summary>
        /// Christian Lopez
        /// Created 2017/03/08
        /// 
        /// The constructor for a _charity user to apply for a _charity.
        /// </summary>
        /// <param name="charityUserId"></param>
        /// <param name="_charityManager"></param>
        public frmCharityView(User charityUser, ICharityManager charityManager)
        {
            InitializeComponent();
            _inApplyMode = true;
            _charityUser = charityUser;
            this._charityManager = charityManager;
            SetEditable();
            
        }

        public void SetEditable()
        {
            lblCharityNameVal.Visibility = Visibility.Collapsed;
            lblContactFirstNameVal.Visibility = Visibility.Collapsed;
            lblContactHoursVal.Visibility = Visibility.Collapsed;
            lblContactLastNameVal.Visibility = Visibility.Collapsed;
            lblEmailVal.Visibility = Visibility.Collapsed;
            lblEmployeeIDVal.Visibility = Visibility.Collapsed;
            lblPhoneNumberVal.Visibility = Visibility.Collapsed;
            lblUserIDVal.Visibility = Visibility.Collapsed;

            txtCharityName.Visibility = Visibility.Visible;
            txtContactFirstName.Visibility = Visibility.Visible;
            txtContactHours.Visibility = Visibility.Visible;
            txtContactLastName.Visibility = Visibility.Visible;
            txtEmail.Visibility = Visibility.Visible;
            txtEmployeeID.Visibility = Visibility.Visible;
            txtPhoneNumber.Visibility = Visibility.Visible;
            if (!_inApplyMode)
            {
                try
                {
                    _userList = (new UserManager()).RetrieveFullUserList();
                    cboUserID.ItemsSource = _userList;
                    cboUserID.Visibility = Visibility.Visible;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    ErrorAlert.ShowDatabaseError();
                }
            }

            
            btnPost.Visibility = Visibility.Visible;
            if (_inApplyMode)
            {
                lblUserIDVal.Visibility = Visibility.Visible;
                lblUserIDVal.Content = _charityUser.UserName;
                lblUserIDVal.IsEnabled = false;
                txtEmployeeID.Visibility = Visibility.Hidden;
                txtContactFirstName.Text = _charityUser.FirstName;
                txtContactLastName.Text = _charityUser.LastName;
                txtEmail.Text = _charityUser.EmailAddress;
                txtPhoneNumber.Text = _charityUser.Phone;
            }
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            int employeeIDParsed;
            int? employeeID = null;
            bool shouldPost = true;
            if (!_inApplyMode)
            {
                if (txtCharityName.Text.Equals(""))
                {
                    MessageBox.Show("Charity name cannot be blank");
                    txtCharityName.Focus();
                    return;
                }
                if (txtContactFirstName.Text.Equals(""))
                {
                    MessageBox.Show("First Name cannot be blank");
                    txtContactFirstName.Focus();
                    return;
                }
                if (txtContactLastName.Text.Equals(""))
                {
                    MessageBox.Show("Last Name cannot be blank");
                    txtContactLastName.Focus();
                    return;
                }
                if (txtPhoneNumber.Text.Equals(""))
                {
                    MessageBox.Show("Phone Number cannot be blank");
                    txtPhoneNumber.Focus();
                    return;
                }
                if (txtEmail.Text.Equals(""))
                {
                    MessageBox.Show("Email cannot be blank");
                    txtEmail.Focus();
                    return;
                }
                if (txtContactHours.Text.Equals(""))
                {
                    MessageBox.Show("Contact Hours cannot be blank");
                    txtContactHours.Focus();
                    return;
                }
                if (Int32.TryParse(txtEmployeeID.Text, out employeeIDParsed))
                {
                        employeeID = employeeIDParsed;
                }
                else
                {
                    MessageBox.Show("Employee ID needs an integer value");
                    txtEmployeeID.Focus();
                    return;
                }

                var charityAsEntered = new Charity()
                {
                    CharityName = txtCharityName.Text,
                    ContactFirstName = txtContactFirstName.Text,
                    ContactHours = txtContactHours.Text,
                    ContactLastName = txtContactLastName.Text,
                    Email = txtEmail.Text,
                    EmployeeID = employeeID,
                    PhoneNumber = txtPhoneNumber.Text,
                    UserID = _userList[cboUserID.SelectedIndex].UserId,
                    Status = "PENDING"
                };

                if (inAddMode)
                {
                    try
                    {
                        _charityManager.AddCharity(charityAsEntered);
                        MessageBox.Show("Charity Added");
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (2627 == ex.Number)
                        {
                            MessageBox.Show("Unique key constraint violated");
                        }
                        else
                        {
                            ErrorAlert.ShowDatabaseError();
                        }
                    }
                }
            }
            else if (_inApplyMode)
            {
                Charity charityAsEntered = new Charity()
                {
                    CharityName = txtCharityName.Text,
                    ContactFirstName = txtContactFirstName.Text,
                    ContactHours = txtContactHours.Text,
                    ContactLastName = txtContactLastName.Text,
                    Email = txtEmail.Text,
                    EmployeeID = null,
                    PhoneNumber = txtPhoneNumber.Text,
                    UserID = _charityUser.UserId
                };

                try
                {
                    if (_charityManager.AddCharityApplication(charityAsEntered))
                    {
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            
        }

        
    }
}