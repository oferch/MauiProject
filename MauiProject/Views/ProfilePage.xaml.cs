namespace MauiProject.Views;

using Communication = Microsoft.Maui.ApplicationModel.Communication;


public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();

    }

    private async void OnFetchContactClicked(object sender, EventArgs e)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.ContactsRead>();
        }

        if (status == PermissionStatus.Granted)
        {
            try
            {
                var contact = await Communication.Contacts.Default.PickContactAsync();

                if (contact == null)
                    return;

                string id = contact.Id;
                string namePrefix = contact.NamePrefix;
                string givenName = contact.GivenName;
                string middleName = contact.MiddleName;
                string familyName = contact.FamilyName;
                string nameSuffix = contact.NameSuffix;
                string displayName = contact.DisplayName;
                List<ContactPhone> phones = contact.Phones; // List of phone numbers
                List<ContactEmail> emails = contact.Emails; // List of email addresses
            }
            catch (Exception ex)
            {
                // Most likely permission denied
            }
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Permission Denied", "Unable to access contacts. Please grant permission and try again.", "OK");
        }

        

    }


    void OnLogoutClicked(object sender, TappedEventArgs e)
    {

    }
}