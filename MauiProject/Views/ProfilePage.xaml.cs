namespace MauiProject.Views;

using MauiProject.ViewModels;
using Communication = Microsoft.Maui.ApplicationModel.Communication;


public partial class ProfilePage : ContentPage
{
    IServiceProvider serviceProvider;

    public ProfilePage(ProfilePageViewModel pVM, IServiceProvider serviceProvider)
	{
		InitializeComponent();
        BindingContext = pVM;
        this.serviceProvider = serviceProvider;
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
                Console.WriteLine(ex.Message);
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
        if (Application.Current is not null)
            ((App)Application.Current).CurrentUser = null;
        if (Application.Current is not null)
            Application.Current.Windows[0].Page = serviceProvider.GetService<LoginPage>();
    }

    private void OnEditProfileClicked(object sender, EventArgs e)
    {
        ((ProfilePageViewModel)BindingContext).Update();
    }
}