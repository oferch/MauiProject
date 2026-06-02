using MauiProject.Models;
using MauiProject.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace MauiProject.ViewModels
{
    public class ProfilePageViewModel : ObservableObject
    {
        public User user = new User() { Id = 0, FirstName = "", LastName = "", Password = "", UserName = "" };

        private string profileImage;
        private IDBStore db;
        public string ProfileImage { get => profileImage;
            set
            {
                if (profileImage != value)
                {
                    profileImage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirstName
        {
            get => user.FirstName;
            set
            {
                if (user.FirstName != value)
                {
                    user.FirstName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                }
            }
        } 
        public string LastName    
        {
            get => user.LastName;
            set
            {
                if (user.LastName != value)
                {
                    user.LastName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public ICommand ChangProfilePhotoCommand { get; }
        public ICommand GetContactCommand { get; }


        public ProfilePageViewModel(IDBStore db)
        {
            ProfileImage = "avatar.png";
            ChangProfilePhotoCommand = new Command(async () => await TakeProfilePhoto());
            GetContactCommand = new Command(async () => await FetchContact());
            this.db = db;
        }

        private async Task TakeProfilePhoto()
        {
            var cameraPermissionsRequest = await Permissions.RequestAsync<Permissions.Camera>();

            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // Save the file to local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    ProfileImage = localFilePath;
                }
            }
        }

        private async Task  FetchContact()
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
                    FirstName = contact.GivenName;
                    string middleName = contact.MiddleName;
                    LastName = contact.FamilyName;
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

    }
}
